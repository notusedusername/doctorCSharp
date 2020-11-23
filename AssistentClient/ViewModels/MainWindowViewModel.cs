﻿using DoctorCSharp.Model.Items;
using Microsoft.VisualStudio.PlatformUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Commons.Items;
using System.ComponentModel;

namespace AssistentClient.ViewModels
{
    /*
     * <Grid Height="758" Width="1024" >
        <TextBox HorizontalAlignment="Left" FontSize="18" Text="{Binding Filter,UpdateSourceTrigger=PropertyChanged}"  VerticalAlignment="Top" Width="200" Height="30" Margin="32,32,0,0"/>
        <Button Content="Add" FontSize="18" Command="{Binding AddCommand }"
                CommandParameter="{Binding Path=., RelativeSource={RelativeSource AncestorType=Window}}"  HorizontalAlignment="Left" Width="100" Height="30" VerticalAlignment="Top" Margin="450,32,0,0"/>
        <Button FontSize="18" Content="Pick Up Complaint" Command="{Binding SendComplaintCommand}"  HorizontalAlignment="Right" VerticalAlignment="Bottom"  Width="233" Height="29" Margin="0,0,32,67"/>
        <DataGrid  CanUserResizeColumns="False" CanUserResizeRows="False" IsReadOnly="True" SelectedItem="{Binding selectedPatient}" ItemsSource="{Binding patients}" AutoGenerateColumns="True"  FontSize="18" Foreground="Black" BorderThickness="1"  Margin="32,90,0,0" Width="518"  HorizontalAlignment="Left" VerticalAlignment="Top" Height="601"/>

        <TextBox FontSize="18" HorizontalAlignment="Right" Margin="0,90,32,0" Text="{Binding Complaint}" TextWrapping="Wrap" VerticalAlignment="Top" Width="233" Height="537"/>
        <Label FontSize="18" Content="Complaint" HorizontalAlignment="Left" Margin="759,32,0,0" VerticalAlignment="Top" Height="40" Width="98"/>
    </Grid>
     * 
     * 
     * 
     * 
     * 
     * 
     * 
     * */
    class MainWindowViewModel
    {
        static readonly HttpClient client = new HttpClient();
        public event PropertyChangedEventHandler PropertyChanged;
        public string Complaint { get; set; }
        private string _filter;
        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                if (_filter == value)
                {
                    return;
                }

                _filter = value;
                OnPropertyChanged("Filter");
                FilterChanged();
            }
        }
        public ICommand AddCommand { get; }
        public ICommand SendComplaintCommand { get; }
        public ICommand Refresh { get; }
        public Patient selectedPatient { get; set; }
        public ObservableCollection<Patient> patients { get; set; }
        public  MainWindowViewModel(){
            patients = new ObservableCollection<Patient>();
            AddCommand = new DelegateCommand(AddButton);
            Refresh = new DelegateCommand(FilterChanged);
            SendComplaintCommand = new DelegateCommand(SendButton);
            FilterChanged();
        }
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        private void AddButton()
        {
            
        }
        private async void SendButton()
        {
            if (selectedPatient == null)
            {
                MessageBox.Show("Patient is not selected!");
            }
            else if (Complaint == null)
            {
                MessageBox.Show("Complaint is neccesary to send!");
            }
            else
            {
                try
                {
                    var complaint = new ActiveComplaint();
                    complaint.patient_id = selectedPatient.id;
                    complaint.complaint = Complaint;


                    var content = new FormUrlEncodedContent(new[]
                    {
                       new KeyValuePair<string, string>("complaint",complaint.complaint)
                       });

                    var url = "http://localhost:52218/api/treatment/active/" + complaint.patient_id.ToString();

                    using var client = new HttpClient();
                    var response = await client.PostAsync(url, content);
                    string result = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine(result);

                    if (!response.IsSuccessStatusCode)
                    {
                        ParseAndShowErrorResponseFromServer(response);
                    }
                    else
                    {
                        MessageBox.Show("Complaint is send to doctor!");
                        Complaint = "";
                    }
                }
                catch (Exceptions.InvalidInputException ex)
                {
                    MessageBox.Show(ex.message);
                }
            }
        }
        public async void FilterChanged()
        {
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync("http://localhost:52218/api/patient?filter="+Filter);
            }
            catch (Exception e)
            {
                handleHttpExceptions(e);
                return;
            }

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                patients.Clear();
                JsonConvert.DeserializeObject<List<Patient>>(responseBody).ForEach((item) => patients.Add(item));
            }
            else
            {
                ParseAndShowErrorResponseFromServer(response);
            }
        }
        private void handleHttpExceptions(Exception e)
        {
            if (e is InvalidOperationException || e is ArgumentNullException)
            {
                Console.WriteLine("There were some errors with the structure of the request", e);
                MessageBox.Show("There are problems with the request...", "Beat the developer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (e is HttpRequestException)
            {
                Console.WriteLine("Can not connect to the server!");
                MessageBox.Show("Can not connect to the server!", "No internet? :/", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private async void ParseAndShowErrorResponseFromServer(HttpResponseMessage response)
        {
            string responseBody = await response.Content.ReadAsStringAsync();
            jsonError jsonError = JsonConvert.DeserializeObject<jsonError>(responseBody);
            ShowErrorResponseFromServer(response, jsonError);

        }

        private void ShowErrorResponseFromServer(HttpResponseMessage response, jsonError jsonError)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show(jsonError.message, "Invalid value", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("The requested resource can not be found", "Not found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Something went wrong!", "The server is confused ...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}

/*
 * 
 * 
 *         <TextBox HorizontalAlignment="Center" Text="{Binding Filter,UpdateSourceTrigger=PropertyChanged}" TextWrapping="Wrap" VerticalAlignment="Top" Width="200" Height="18" Margin="0,37,0,0">

        </TextBox>
        <Button Content="Add" Command="{Binding AddCommand}" Grid.Column="2" Grid.Row="0" HorizontalAlignment="Center" Height="20" Width="100"/>
        <Button Content="Pick Up Complaint" Command="{Binding SendComplaintCommand}" Grid.Column="3" HorizontalAlignment="Center" Grid.Row="7" VerticalAlignment="Top" Width="170" Margin="0,12,0,0" Height="20"/>
        <DataGrid CanUserResizeColumns="False" CanUserResizeRows="False" IsReadOnly="True" SelectedItem="{Binding selectedPatient}" ItemsSource="{Binding patients}" AutoGenerateColumns="True" Background="Transparent" FontSize="18" Foreground="Black" BorderThickness="1" BorderBrush="White" Grid.ColumnSpan="2" Margin="37,10,2,22" Grid.RowSpan="7" Grid.Row="1">
        </DataGrid>
        <Label Content="Complaint" Grid.Column="3" HorizontalAlignment="Left" VerticalAlignment="Center" Height="26" Margin="46,0,0,0" Width="65"/>
        <TextBox Grid.Column="3" HorizontalAlignment="Center" Margin="0,22,0,0" Text="{Binding Complaint}" TextWrapping="Wrap" VerticalAlignment="Top" Width="170" Grid.RowSpan="7" Height="238" Grid.Row="1"/>
        <Button  Background="LightBlue" BorderBrush="White" BorderThickness="2" FontSize="18" Command="{Binding Refresh}"  Content="Refresh"  Grid.Column="1" Margin="99,37,66,140"/>


*/
