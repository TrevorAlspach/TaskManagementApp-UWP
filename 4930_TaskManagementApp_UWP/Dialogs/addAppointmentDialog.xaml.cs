 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Library.TaskManagement;
using System.Collections.ObjectModel;
using _4930_TaskManagementApp_UWP.ViewModels;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace _4930_TaskManagementApp_UWP.Dialogs
{
    public sealed partial class addAppointmentDialog : ContentDialog
    {
        public TimeSpan localStartTime { get; set; }
        public TimeSpan localEndTime { get; set; }
        private object listContext;

        public addAppointmentDialog(object context)
        {
            this.InitializeComponent();
            DataContext = new AppointmentVM();
            listContext = context;
        }

        private void DatePicker_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            var appt = (DataContext as AppointmentVM);

            appt.StartTime = (DateTimeOffset)args.NewDate;
            appt.EndTime = (DateTimeOffset)args.NewDate;
            
        }

        private void TimePicker_StartTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
        {
            localStartTime = (TimeSpan)args.NewTime;
        }

        private void TimePicker_EndTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
        {
            localEndTime = (TimeSpan)args.NewTime;
        }

        private void Atendee_Click(object sender, RoutedEventArgs e)
        {
            var appt = (DataContext as AppointmentVM);
            appt.atendees.Add(appt.atendeeToAdd);
            appt.atendeeToAdd = "";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var appt = (DataContext as AppointmentVM);
           
            appt.StartTime = new DateTimeOffset(appt.StartTime.Year, appt.StartTime.Month, appt.StartTime.Day,
            localStartTime.Hours, localStartTime.Minutes, localStartTime.Seconds, new TimeSpan(0));

            appt.EndTime = new DateTimeOffset(appt.EndTime.Year, appt.EndTime.Month, appt.EndTime.Day,
            localEndTime.Hours, localEndTime.Minutes, localEndTime.Seconds, new TimeSpan(0));

            if (appt.Name != null && appt.Description != null)
                (listContext as MainViewModel).AddTask((DataContext as AppointmentVM));
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
