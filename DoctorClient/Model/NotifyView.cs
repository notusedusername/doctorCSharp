using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Controls;

namespace DoctorClient.Models
{
    class NotifyView : INotifyPropertyChanged
    {
        private static NotifyView currentView {get; set;}
        private NotifyView()
        {

        }

        public static NotifyView getInstance()
        {
            if(currentView == null)
            {
                currentView = new NotifyView();
            }
            return currentView;
        }

        private UserControl _view;

        public UserControl ShownView { 
            get
            {
                return this._view;
            }
            set
            {
                this._view = value;
                this.OnPropertyChanged("ShownView");
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
