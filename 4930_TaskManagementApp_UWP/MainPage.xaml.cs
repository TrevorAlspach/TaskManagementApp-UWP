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
using _4930_TaskManagementApp_UWP.ViewModels;
using _4930_TaskManagementApp_UWP.Dialogs;
using Library.TaskManagement;
using Windows.UI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace _4930_TaskManagementApp_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Item_Selected_Changed(object sender, SelectionChangedEventArgs e)
        {
            var itemListBox = (sender as ListBox);
            ItemVM unselectedItem;
            if (e.RemovedItems.Count != 0)
            {
                if (e.RemovedItems.FirstOrDefault() is TaskVM)
                {
                    unselectedItem = (e.RemovedItems.First() as TaskVM);
                    var unselectedBox = itemListBox.ContainerFromItem(unselectedItem) as ListBoxItem;
                    if (unselectedBox != null)
                    {
                        unselectedBox.ContentTemplate = (DataTemplate)this.Resources["unselected_task"];
                    }
                }
                else if (e.RemovedItems.FirstOrDefault() is AppointmentVM)
                {
                    unselectedItem = (e.RemovedItems.First() as AppointmentVM);
                    var unselectedBox = itemListBox.ContainerFromItem(unselectedItem) as ListBoxItem;
                    if (unselectedBox != null)
                    {
                        unselectedBox.ContentTemplate = (DataTemplate)this.Resources["unselected_appointment"];
                    }
                }
            }

            var selectedBox = itemListBox.ContainerFromItem(itemListBox.SelectedItem) as ListBoxItem;
            if (itemListBox.SelectedItem is TaskVM)
            {
                selectedBox.ContentTemplate = (DataTemplate)this.Resources["selected_task"];
            }
            else if (itemListBox.SelectedItem is AppointmentVM)
            {
                selectedBox.ContentTemplate = (DataTemplate)this.Resources["selected_appointment"];
            }
        } //changes the data template for newly selected item and unselected item

        private void List_Selected_Changed(object sender, SelectionChangedEventArgs e)
        {
            var listView = (sender as ListView);
            Guid newListGuid = Guid.Empty;
            if (listView.SelectedItem != null)
            {
                if ((DataContext as MainViewModel).AllLists.TryGetValue((string)listView.SelectedItem, out Guid value))
                {
                    newListGuid = value;
                }
            }
           
            if (newListGuid != Guid.Empty){
                (DataContext as MainViewModel).List_Changed(newListGuid);
            }
            
        }
        private void List_Click(object sender, ItemClickEventArgs e)
        {
           /* var listView = (sender as ListView);
            Guid newListGuid = Guid.Empty;

            if ((DataContext as MainViewModel).AllLists.TryGetValue((string)listView.SelectedItem, out Guid value))
            {
                newListGuid = value;
            }
            if (newListGuid != Guid.Empty)
            {
                (DataContext as MainViewModel).List_Changed(newListGuid);
            }*/

        }

        private async void AddTask_Click(object sender, RoutedEventArgs e)
        {
            var taskDiag = new addTaskDialog(DataContext);
            await taskDiag.ShowAsync();
        }

        private async void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            var apptDiag = new addAppointmentDialog(DataContext);
            await apptDiag.ShowAsync();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).RemoveItem();
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            if ((DataContext as MainViewModel).SelectedTask is TaskVM)
            {
                var editTaskDiag = new EditTaskDialog((DataContext as MainViewModel).CurrentTaskList.list, DataContext);
                await editTaskDiag.ShowAsync();
            }
            else if ((DataContext as MainViewModel).SelectedTask is AppointmentVM)
            {
                var editApptDiag = new EditAppointmentDialog((DataContext as MainViewModel).CurrentTaskList.list, DataContext);
                await editApptDiag.ShowAsync();
            }
        }

        private void Complete_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Complete();
        }

        private void Priority_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).TogglePriority();
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).PreviousPage();
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).NextPage();
        }

        private async void LoadList_Click(object sender, RoutedEventArgs e)
        {
            var loadDialog = new LoadListDialog(DataContext);
            await loadDialog.ShowAsync();
        }

        private async void SaveList_Click(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveListDialog(DataContext);
            await saveDialog.ShowAsync();
        }

        private async void CreateList_Click(object sender, RoutedEventArgs e)
        {
            var createListDialog = new CreateListDialog(DataContext);
            await createListDialog.ShowAsync();
        }

        private void RemoveList_Click(object sender, RoutedEventArgs e)
        {
            var LVAllLists = (this.FindName("LVAllLists") as ListView);
            var selectedName = LVAllLists.SelectedItem as string;
            //var selectedList = (KeyValuePair<string, Guid>)LVAllLists.SelectedItem;
            var ListId = (DataContext as MainViewModel).AllLists.GetValueOrDefault(selectedName);
            (DataContext as MainViewModel).RemoveList(new KeyValuePair<string, Guid>(selectedName, ListId));
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var textbox = this.FindName("queryTextBox") as TextBox;
            var query = textbox.Text;
            (DataContext as MainViewModel).Search(query);
        }

        private void ShowCompleted_Check(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Refresh();
        }

        private void ShowCompleted_Uncheck(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Refresh();
        }
    }
}
