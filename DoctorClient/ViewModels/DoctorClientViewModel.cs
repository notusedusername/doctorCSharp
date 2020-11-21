using System;
using System.Collections.Generic;
using System.Windows.Input;
using DoctorClient.Views;
using System.Net.Http;
using DoctorCSharp.Model.Items;
using System.Collections.ObjectModel;
using System.Windows;
using Microsoft.VisualStudio.PlatformUI;
using Newtonsoft.Json;
using Commons.Items;
using System.ComponentModel;
using System.Windows.Controls;

namespace DoctorClient.ViewModels
{
    class DoctorClientViewModel : INotifyPropertyChanged
    {
        static readonly HttpClient client = new HttpClient();

        public static WaitingPatientList patientListView = new WaitingPatientList();

        public static DiagnosisView diagnosisView = new DiagnosisView();
        
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

        private ActiveComplaint _selectedComplaint;
        public ActiveComplaint selectedComplaint 
        {
            get
            {
                return _selectedComplaint;
            }
            set
            {
                _selectedComplaint = value;
                OnPropertyChanged("selectedComplaint");
            }
        }

        private Patient _patient;
        public Patient selectedPatientData 
        {
            get
            {
                return _patient;
            }
            set 
            {
                _patient = value;
                OnPropertyChanged("selectedPatientData");
            }
        }


        public ICommand SelectPatientCommand { get; }
        
        public ICommand RefreshWaitingPatientsCommand { get; }

        public ICommand PostDiagnosisCommand { get; }

        public ICommand UpdatePatientDataCommand { get; }

        public ICommand DeletePatientAllDataCommand { get; }

        public ICommand SwitchViewCommand { get; }

        public ObservableCollection<ActiveComplaint> patients { get; }

        public DoctorClientViewModel()
        {
            
            currentView = patientListView;
            patients = new ObservableCollection<ActiveComplaint>();
            SelectPatientCommand = new DelegateCommand(SelectPatient);
            PostDiagnosisCommand = new DelegateCommand(PostDiagnosis);
            UpdatePatientDataCommand = new DelegateCommand(UpdatePatient);
            DeletePatientAllDataCommand = new DelegateCommand(DeletePatient);
            RefreshWaitingPatientsCommand = new DelegateCommand(RefreshWaitingPatientList);
            SwitchViewCommand = new DelegateCommand(SwitchView);
            RefreshWaitingPatientList();
        }

        private async void RefreshWaitingPatientList()
        {
            HttpResponseMessage response;
            try
            {
               response = await client.GetAsync("http://localhost:52218/api/treatment/active");
            } catch(Exception e)
            {
                handleHttpExceptions(e);
                return;
            }
            
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                patients.Clear();
                JsonConvert.DeserializeObject<List<ActiveComplaint>>(responseBody).ForEach((item) => patients.Add(item));
            }
            else
            {
                ParseAndShowErrorResponseFromServer(response);
            }
        }

        private void SelectPatient()
        {
            if(selectedComplaint != null) 
            {
                GetPatientData(selectedComplaint.patient_id);
                SwitchView();
            }
            else
            {
                MessageBox.Show("Please select a patient from the list!", "No patient selected", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
        }

        private void PostDiagnosis()
        {
            MessageBox.Show("Diagnosis");
        }

        private void UpdatePatient()
        {
            MessageBox.Show("Update");
        }

        private void DeletePatient()
        {
            MessageBox.Show("Delete");
        }

        private void SwitchView()
        {
            if(currentView == diagnosisView)
            {
                selectedComplaint = null;
                currentView = patientListView;
            }
            else
            {
                currentView = diagnosisView;
            }
        }

        private async void GetPatientData(int id)
        {
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync("http://localhost:52218/api/patient/" + id);
            }
            catch (Exception e)
            {
                handleHttpExceptions(e);
                return;
            }

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                selectedPatientData = JsonConvert.DeserializeObject<Patient>(responseBody);
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
            if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("The requested resource can not be found", "Not found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                MessageBox.Show("Something went wrong!", "The server is confused ...", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
