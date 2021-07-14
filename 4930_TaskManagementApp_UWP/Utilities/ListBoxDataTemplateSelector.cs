using _4930_TaskManagementApp_UWP.ViewModels;
using Library.TaskManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace _4930_TaskManagementApp_UWP.Services
{
    public class ListBoxDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TaskTemplate { get; set; }
        public DataTemplate AppointmentTemplate { get; set; }       //Selector will be used to initialize the data templates 

        protected override DataTemplate 
            SelectTemplateCore(object item, DependencyObject container)
        {
            AppointmentVM listItem = item as AppointmentVM;

            if (listItem != null)
            {
                return AppointmentTemplate;
            }
            else
            {
                return TaskTemplate;
            }
        }
    }
}
