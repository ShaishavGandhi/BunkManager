using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Bunk_Manager.ViewModels
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _id;
        /// <summary>
        /// Sample ViewModel property; this property is used to identify the object.
        /// </summary>
        /// <returns></returns>
        public string ID
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("ID");
                }
            }
        }

        private string _subname;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string SubName
        {
            get
            {
                return _subname;
            }
            set
            {
                if (value != _subname)
                {
                    _subname = value;
                    NotifyPropertyChanged("SubName");
                }
            }
        }

        private int _total;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public int Total
        {
            get
            {
                return _total;
            }
            set
            {
                if (value != _total)
                {
                    _total = value;
                    NotifyPropertyChanged("Total");
                }
            }
        }

        private int _bunked;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public int Bunked
        {
            get
            {
                return _bunked;
            }
            set
            {
                if (value != _bunked)
                {
                    _bunked = value;
                    NotifyPropertyChanged("Bunked");
                }
            }
        }

        private float _attendance;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public float Attendance
        {
            get
            {
                return _attendance;
            }
            set
            {
                if (value != _attendance)
                {
                    _attendance = value;
                    NotifyPropertyChanged("Attendance");
                }
            }
        }



        private string _backcolor;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string BackColor
        {
            get
            {
                return _backcolor;
            }
            set
            {
                if (value != _backcolor)
                {
                    _backcolor = value;
                    NotifyPropertyChanged("BackColor");
                }
            }
        }

        private int _minimum;
        public int Minimum
        {
            get
            {
                return _minimum;
            }
            set
            {
                if (value != _minimum)
                {
                    _minimum = value;
                    NotifyPropertyChanged("Minimum");
                }
            }
        }

        private double _safebunk;
        public double SafeBunk
        {
            get
            {
                return _safebunk;
            }
            set
            {
                if (value != _safebunk)
                {
                    _safebunk = value;
                    NotifyPropertyChanged("SafeBunk");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}