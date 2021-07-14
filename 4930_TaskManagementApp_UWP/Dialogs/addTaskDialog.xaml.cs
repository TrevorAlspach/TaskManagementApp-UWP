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
using _4930_TaskManagementApp_UWP.ViewModels;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace _4930_TaskManagementApp_UWP.Dialogs
{
    public sealed partial class addTaskDialog : ContentDialog
    {
        static readonly DateTimeOffset DEFAULT_DATE = new DateTimeOffset(1600, 12, 31, 19, 0, 0, new TimeSpan(-5,0,0));

        private object listContext;
        public addTaskDialog(object context)
        {
            InitializeComponent();
            DataContext = new TaskVM();
            listContext = context;
        }

        private void DatePicker_SelectedDateChanged(DatePicker sender, DatePickerSelectedValueChangedEventArgs args)
        {
            var task = (DataContext as TaskVM);
            if (task.DeadlineTime != TimeSpan.Zero)
            {
                task.DeadlineDate = new DateTimeOffset(args.NewDate.Value.Year, args.NewDate.Value.Month, args.NewDate.Value.Day,
                    task.DeadlineTime.Hours, task.DeadlineTime.Minutes, task.DeadlineTime.Seconds, TimeSpan.Zero);
            }
            else
            {
                task.DeadlineDate = (DateTimeOffset)args.NewDate;
            }
                
        }
        private void TimePicker_SelectedTimeChanged(TimePicker sender, TimePickerSelectedValueChangedEventArgs args)
        {
            var task = (DataContext as TaskVM);
            if (task.DeadlineDate != null)
            {
                task.DeadlineDate = new DateTimeOffset(task.DeadlineDate.Year, task.DeadlineDate.Month, task.DeadlineDate.Day,
                    args.NewTime.Value.Hours, args.NewTime.Value.Minutes, args.NewTime.Value.Seconds, new TimeSpan(0));
                task.DeadlineTime = (TimeSpan)args.NewTime;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if ((DataContext as TaskVM).DeadlineDate != DEFAULT_DATE)
            {
                (listContext as MainViewModel).AddTask((DataContext as TaskVM));
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
