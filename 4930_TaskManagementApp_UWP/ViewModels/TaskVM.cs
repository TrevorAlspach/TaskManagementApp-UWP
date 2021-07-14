using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace _4930_TaskManagementApp_UWP.ViewModels
{
    public class TaskVM : ItemVM
    {
        public static readonly DependencyProperty DeadlineDateProperty = DependencyProperty.Register(
           "DeadlineDate",
           typeof(DateTimeOffset),
           typeof(Task),
           new PropertyMetadata(null));

        public static readonly DependencyProperty DeadlineTimeProperty = DependencyProperty.Register(
           "DeadlineTime",
           typeof(TimeSpan),
           typeof(Task),
           new PropertyMetadata(null));

        public DateTimeOffset DeadlineDate
        {
            get { return (DateTimeOffset)GetValue(DeadlineDateProperty); }
            set { SetValue(DeadlineDateProperty, value); }
        }

        public TimeSpan DeadlineTime
        {
            get { return (TimeSpan)GetValue(DeadlineTimeProperty); }
            set { SetValue(DeadlineTimeProperty, value); }
        }


        public TaskVM()
        {
            DeadlineTime = new TimeSpan(0);
            IsCompleted = false;
            Priority = false;
        }


        public override string ToString()
        {
            return Name + "\tDeadline: " + DeadlineDate.ToString("f");
        }


    }
}
