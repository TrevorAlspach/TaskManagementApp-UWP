using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace _4930_TaskManagementApp_UWP.ViewModels
{
    abstract public class ItemVM : DependencyObject
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name",
            typeof(string),
            typeof(ItemVM),
            new PropertyMetadata(null));

        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            "Description",
            typeof(string),
            typeof(ItemVM),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IsCompletedProperty = DependencyProperty.Register(
            "IsCompleted",
            typeof(bool),
            typeof(ItemVM),
            new PropertyMetadata(null));

        public static readonly DependencyProperty PriorityProperty = DependencyProperty.Register(
            "Priority",
            typeof(bool),
            typeof(ItemVM),
            new PropertyMetadata(null));

        public bool IsCompleted
        {
            get { return (bool)GetValue(IsCompletedProperty); }
            set { SetValue(IsCompletedProperty, value); }
        }
        public bool Priority
        {
            get { return (bool)GetValue(PriorityProperty); }
            set { SetValue(PriorityProperty, value); }
        }

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public Guid Id { get; set; }

        public abstract override string ToString();

    }
}
