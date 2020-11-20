using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.PlatformUI;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace AssistentClient.ViewModels
{
    public class RegisterWindowViewModel
    {
        public RegisterWindowViewModel()
        {
           // RegisterCommand = new DelegateCommand(Register);
            this.BackCommand = new RelayCommand<Window>(this.BackButton);

        }
        public ICommand RegisterCommand { get; }
        public ICommand BackCommand { get; }
        public string Name { get; set; }
        public string Taj { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public MainWindow m { get; set; }

        //private void Register() => RegisterButton();
        //private void Back() => BackButton();

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
