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
using System.Threading;
using DoctorClient.Model;
using System.Threading.Tasks;
using System.Configuration;

namespace DoctorClient.ViewModels
{
    class DoctorClientViewModel : INotifyPropertyChanged
    {
        private readonly string serverUrl = ConfigurationManager.AppSettings["serverUrl"];
        
        static readonly HttpClient client = new HttpClient();

        private string _headerMessage;

        public string HeaderMessage
        {
            get
            {
                return _headerMessage;
            }
            set
            {
                if(_headerMessage != value)
                {
                    _headerMessage = value;
                    OnPropertyChanged("HeaderMessage");
                }
            }
        }
        
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
        private string _diagnosis { get; set; }

        public string Diagnosis
        {
            get
            {
                return _diagnosis;
            }
            set
            {
                _diagnosis = value;
                OnPropertyChanged("Diagnosis");
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
            HeaderMessage = "Loading waiting patients...";
            patients = new ObservableCollection<ActiveComplaint>();
            SelectPatientCommand = new DelegateCommand(SelectPatient);
            PostDiagnosisCommand = new DelegateCommand(PutDiagnosis);
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
               response = await client.GetAsync(serverUrl + "api/treatment/active");
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
                if(patients.Count == 0)
                {
                    HeaderMessage = HeaderMessages.NO_PATIENT;
                    await Task.Delay(5000);
                    RefreshWaitingPatientList();
                }
                else
                {
                    HeaderMessage = HeaderMessages.SELECT_PATIENT;
                }
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

        private async void PutDiagnosis()
        {
            HttpResponseMessage response;
            HttpContent content = new FormUrlEncodedContent(new[] { 
                new KeyValuePair<string, string>("diagnosis", Diagnosis) 
            });
            try
            {
                response = await client.PutAsync(serverUrl + "api/treatment/active/" + selectedPatientData.id, content);
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
                MessageBox.Show(JsonConvert.DeserializeObject<jsonError>(responseBody).message, "Diagnosis", MessageBoxButton.OK, MessageBoxImage.Information);
                SwitchView();
            }
            else
            {
                ParseAndShowErrorResponseFromServer(response);
            }
        }

        private async void UpdatePatient()
        {
            if (isConfirmed("This action will override the patient data, are you sure?", "Override patient data", MessageBoxImage.Question))
            {
                HttpResponseMessage response;
                HttpContent content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("name", selectedPatientData.name),
                new KeyValuePair<string, string>("address", selectedPatientData.address),
                new KeyValuePair<string, string>("TAJ_nr", selectedPatientData.taj),
                new KeyValuePair<string, string>("phone", selectedPatientData.phone),
            });
                try
                {
                    response = await client.PutAsync(serverUrl + "api/patient/" + selectedPatientData.id, content);
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
                    MessageBox.Show(JsonConvert.DeserializeObject<jsonError>(responseBody).message, "Update patient", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ParseAndShowErrorResponseFromServer(response);
                }
            }
            
        }


        private async void DeletePatient()
        {
            if (isConfirmed("This action will delete ALL patient data (including the threatments). This action can not be undone. Are you sure?", "Delete patient", MessageBoxImage.Warning))
            {
                HttpResponseMessage response;
                try
                {
                    response = await client.DeleteAsync(serverUrl + "api/patient/" + selectedPatientData.id);
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
                    MessageBox.Show(JsonConvert.DeserializeObject<jsonError>(responseBody).message, "Delete patient", MessageBoxButton.OK, MessageBoxImage.Information);
                    SwitchView();
                }
                else
                {
                    ParseAndShowErrorResponseFromServer(response);
                }
            }
        }

        private void SwitchView()
        {
            if(currentView == diagnosisView)
            {
                selectedComplaint = null;
                RefreshWaitingPatientList();
                currentView = patientListView;
            }
            else
            {
                currentView = diagnosisView;
                Diagnosis = "";
            }
        }

        private async void GetPatientData(int id)
        {
            HttpResponseMessage response;
            try
            {
                response = await client.GetAsync(serverUrl + "api/patient/" + id);
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
                SwitchView();
            }
        }

        private bool isConfirmed(string message, string title, MessageBoxImage image)
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, image) == MessageBoxResult.Yes;
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
            if(response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
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
            else if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                MessageBox.Show("The requested resource can not be found", "Not found", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
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
