using AssistentClient.Views;
using Commons.Items;
using DoctorCSharp.Model.Items;
using GalaSoft.MvvmLight.Command;
using Microsoft.VisualStudio.PlatformUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AssistentClient.ViewModels
{
    public class AssistentClientViewModel : INotifyPropertyChanged
    {
        static readonly HttpClient client = new HttpClient();
        public ObservableCollection<Patient> patients { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public static HomeView HomeView = new HomeView();
        public static RegisterView RegisterView = new RegisterView();
        private string _complaint { get; set; }

        public string Complaint
        {
            get
            {
                return _complaint;
            }
            set
            {
                _complaint = value;
                OnPropertyChanged("Complaint");
            }
        }
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
        public ICommand SwitchViewCommand { get; }
        public Patient selectedPatient { get; set; }
        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }
        public string Name { get; set; }
        public string Taj { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public AssistentClientViewModel()
        {
            currentView = HomeView;
            patients = new ObservableCollection<Patient>();
            AddCommand = new DelegateCommand(AddButton);
            Refresh = new DelegateCommand(FilterChanged);
            SendComplaintCommand = new DelegateCommand(SendButton);
            this.RegisterCommand = new DelegateCommand(Register);
            this.BackCommand = new RelayCommand<Window>(this.BackButton);
            SwitchViewCommand = new DelegateCommand(SwitchView);
            FilterChanged();
        }

        private UserControl _currentView;
        public UserControl currentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged("currentView");
            }
        }
        private void SwitchView()
        {
            if (currentView == RegisterView)
            {
                Filter = "";
                FilterChanged();
                currentView = HomeView;
            }
            else
            {
                currentView = RegisterView;
            }
        }
        private void AddButton()
        {
            SwitchView();
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
                        MessageBox.Show("Complaint is sent to doctor!");
                        Complaint = "";
                        FilterChanged();
                        selectedPatient = null;
                    }
                }
                catch (Exception ex)
                {
                    handleHttpExceptions(ex);
                }
            }
        }
        public async void FilterChanged()
        {
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync("http://localhost:52218/api/patient?filter=" + Filter);
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
        private async void Register()
        {
                var patient = new Patient();
                patient.name = Name;
                patient.phone = Phone;
                patient.taj = Taj;
                patient.address = Address;

                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("name", patient.name),
                new KeyValuePair<string, string>("taJ_nr",patient.taj),
                new KeyValuePair<string, string>("address",patient.address),
                new KeyValuePair<string, string>("phone",patient.phone)
            });

                var url = "http://localhost:52218/api/patient";
                using var client = new HttpClient();
                var response = await client.PostAsync(url, content);
                string result = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode)
                {
                    ParseAndShowErrorResponseFromServer(response);
                }
                else
                {
                    MessageBox.Show("New patient succesfully saved!");
                    Name = "";
                    Phone = "";
                    Address = "";
                    Taj = "";
                    SwitchView();
                }
            
        }
        private void BackButton(Window window)
        {
            SwitchView();
            FilterChanged();
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
            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                MessageBox.Show("Something went wrong!", "The server is confused ...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                jsonError jsonError = JsonConvert.DeserializeObject<jsonError>(responseBody);
                ShowErrorResponseFromServer(response, jsonError);
            }

        }

        private void ShowErrorResponseFromServer(HttpResponseMessage response, jsonError jsonError)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                MessageBox.Show(jsonError.message, "Invalid value", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("The requested resource can not be found", "Not found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
            }
        }
    }

}


