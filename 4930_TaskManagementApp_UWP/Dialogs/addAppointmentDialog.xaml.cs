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
            var apptVM = (DataContext as AppointmentVM);

            apptVM.StartTime = (DateTimeOffset)args.NewDate;
            apptVM.EndTime = (DateTimeOffset)args.NewDate;
            
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
            var apptVM = (DataContext as AppointmentVM);
            apptVM.atendees.Add(apptVM.atendeeToAdd);
            apptVM.atendeeToAdd = "";
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var apptVM = (DataContext as AppointmentVM);
           
            apptVM.StartTime = new DateTimeOffset(apptVM.StartTime.Year, apptVM.StartTime.Month, apptVM.StartTime.Day,
            localStartTime.Hours, localStartTime.Minutes, localStartTime.Seconds, new TimeSpan(-4,0,0));

            apptVM.EndTime = new DateTimeOffset(apptVM.EndTime.Year, apptVM.EndTime.Month, apptVM.EndTime.Day,
            localEndTime.Hours, localEndTime.Minutes, localEndTime.Seconds, new TimeSpan(-4,0,0));

            if (apptVM.Name != null && apptVM.Description != null)
            {
                var MainVM = (listContext as MainViewModel);
                var mapper = new AutoMapper.Mapper(MainVM.Mapper.config);
                var appt = mapper.Map<AppointmentVM, Appointment>(DataContext as AppointmentVM);
                await MainVM.AddItem(appt);
            }
                
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
