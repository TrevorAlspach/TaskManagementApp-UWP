using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace _4930_TaskManagementApp_UWP.ViewModels
{
    public class AppointmentVM : ItemVM
    {
        public static readonly DependencyProperty StartTimeProperty = DependencyProperty.Register(
           "StartTime",
           typeof(DateTimeOffset),
           typeof(AppointmentVM),
           new PropertyMetadata(null));

        public static readonly DependencyProperty EndTimeProperty = DependencyProperty.Register(
           "EndTime",
           typeof(DateTimeOffset),
           typeof(AppointmentVM),
           new PropertyMetadata(null));

        public static readonly DependencyProperty atendeeToAddProperty = DependencyProperty.Register(
            "atendeeToAdd",
            typeof(string),
            typeof(AppointmentVM),
            new PropertyMetadata(null));

        public DateTimeOffset StartTime
        {
            get { return (DateTimeOffset)GetValue(StartTimeProperty); }
            set { SetValue(StartTimeProperty, value); }
        }
        public DateTimeOffset EndTime
        {
            get { return (DateTimeOffset)GetValue(EndTimeProperty); }
            set { SetValue(EndTimeProperty, value); }
        }
        public string atendeeToAdd
        {
            get { return (string)GetValue(atendeeToAddProperty); }
            set { SetValue(atendeeToAddProperty, value); }
        }

        public ObservableCollection<string> atendees { get; set; }

        public AppointmentVM()//Need to update constructor
        {
            IsCompleted = false;
            Priority = false;
            atendees = new ObservableCollection<string>();
        }

        public override string ToString()
        {
            return Name + $" on {StartTime.ToString("f")} - {EndTime.ToString("t")}";
        }
    }
}
