using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.PlatformUI;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using DoctorCSharp.Model.Items;
using System.Net.Http;
using Commons.Items;
using Newtonsoft.Json;

namespace AssistentClient.ViewModels
{
    public class RegisterWindowViewModel
    {
        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }
        public string Name { get; set; }
        public string Taj { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public MainWindow m { get; set; }
        public RegisterWindowViewModel()
        {
            this.RegisterCommand = new DelegateCommand(Register);
            this.BackCommand = new RelayCommand<Window>(this.BackButton);

        }
        private async void Register()
        {
            try
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
                    jsonError err = new jsonError();
                    err = JsonConvert.DeserializeObject<jsonError>(result);
                    MessageBox.Show(err.message);
                }
                else
                {
                    MessageBox.Show("New patient succesfully saved!");
                    Name = "";
                    Phone = "";
                    Address = "";
                    Taj = "";
                }
            }
            catch (Exceptions.InvalidInputException ex)
            {
                MessageBox.Show(ex.message);
            }
        }
        private void BackButton(Window window)
        {
            if (window != null)
            {
                m.Top = window.Top;
                m.Left = window.Left;
                m.Show();
                window.Close();
            }
        }
    }
}
