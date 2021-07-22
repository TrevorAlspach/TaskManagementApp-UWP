using Library.TaskManagement;
using _4930_TaskManagementApp_UWP.ViewModels;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace _4930_TaskManagementApp_UWP.Dialogs
{
    public sealed partial class EditTaskDialog : ContentDialog
    {
        private object listContext;
        public EditTaskDialog(IList<ItemVM> TaskList, object context)
        {
            InitializeComponent();
            DataContext = (context as MainViewModel).SelectedTask;
            
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
            var MainVM = (listContext as MainViewModel);
            var mapper = new AutoMapper.Mapper(MainVM.Mapper.config);
            var task = mapper.Map<TaskVM, Task>(DataContext as TaskVM);
            MainVM.AddItem(task);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
