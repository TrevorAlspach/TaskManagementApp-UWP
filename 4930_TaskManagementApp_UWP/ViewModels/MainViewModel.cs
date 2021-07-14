using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Library.TaskManagement;
using _4930_TaskManagementApp_UWP.Services;
using Windows.UI.Xaml;

namespace _4930_TaskManagementApp_UWP.ViewModels
{
    class MainViewModel : DependencyObject, INotifyPropertyChanged
    {
        public static readonly DependencyProperty PageCountProperty = DependencyProperty.Register(
           "PageCount",
           typeof(int),
           typeof(MainViewModel),
           new PropertyMetadata(null));

        public static readonly DependencyProperty PageNumberProperty = DependencyProperty.Register(
           "PageNumber",
           typeof(int),
           typeof(MainViewModel),
           new PropertyMetadata(null));

        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }
        public int PageNumber
        {
            get { return (int)GetValue(PageNumberProperty); }
            set { SetValue(PageNumberProperty, value); }
        }
        private int currentIndex;                                           //index to keep track of which list in AllLists needs to be accessed
        public bool showCompleted { get; set; }                             //bound to checkbox, when false will exclude completed tasks

        public ItemVM SelectedTask { get; set; }
        public ObservableCollection<NamedList<ItemVM>> AllLists { get; set; } //Holds all currently loaded lists 
        public List<ItemVM> CurrentTaskList { get; set; }                     //populated with the ItemVMs of the currently selected list
        public ListNavigator<ItemVM> Navigator;                               //Reformatted navigator which returns observable collection<T> instead of dictionary
        public ObservableCollection<ItemVM> CurrentWindow { get; set; }       //The collection of ItemVMs which are currently displayed in view
        private LocalFolderAccess localfolder = LocalFolderAccess.GetInstance;

        public MainViewModel()
        {
            AllLists = new ObservableCollection<NamedList<ItemVM>>();
            CurrentTaskList = new List<ItemVM>();
            CurrentWindow = new ObservableCollection<ItemVM>();
            Navigator = new ListNavigator<ItemVM>(CurrentTaskList, CurrentWindow);
            CurrentWindow = Navigator.GoToFirstPage();
            PageNumber = 1;
            PageCount = 1;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddTask(ItemVM ItemVM)
        {
            try
            {
                CurrentTaskList.Add(ItemVM);
                AllLists.ElementAt(currentIndex).list.Add(ItemVM);
                Refresh();
            }
            catch(ArgumentOutOfRangeException) { }
        } //Adds task/appt to current view + AllLists

        public void Remove()
        {
            CurrentTaskList.Remove(SelectedTask);
            AllLists.ElementAt(currentIndex).list.Remove(SelectedTask);
            Refresh();
        }          //Removes task from current view + AllLists

        public void Complete()
        {
            SelectedTask.IsCompleted = true;
            if (!showCompleted)
            {
                RemoveCompleted();
            }
        }                //Completes currently selected task

        public void TogglePriority()
        {
            SelectedTask.Priority = !SelectedTask.Priority;
            Refresh();
            
        }          //toggles priority bool on selected task

        public void Refresh()
        {
            try
            {
                SortListByPriority();
                CurrentWindow = Navigator.GetCurrentPage();
                PageCount = calculateTotalPageCount();
            }
            catch (PageFaultException) { 
                Navigator.GoToFirstPage();
                PageCount -= 1;
                PageNumber -= 1;
            }
        }                 //Reloads current window, used when currenttasklist is modified or changed

        public void PreviousPage()
        {
            try
            {
                CurrentWindow = Navigator.GoBackward();
                PageNumber--;
            }
            catch(PageFaultException) {}
        }

        public void NextPage()
        {
            try
            {
                CurrentWindow = Navigator.GoForward();
                PageNumber++;
            }
            catch (PageFaultException) {}
        }

        public async System.Threading.Tasks.Task LoadList(string filename)
        {
            await localfolder.LoadListfromJSON(AllLists, filename);
            Refresh();
        }   //loads list from localfolder into AllLists

        public async System.Threading.Tasks.Task SaveList(string filename)
        {
           await localfolder.SaveListasJSON(CurrentTaskList, filename);
        }    //saves current list to localfolder as filename

        public int calculateTotalPageCount()
        {
            int taskCount = CurrentTaskList.Count;
            if (taskCount == 0)
            {
                return 1;
            }
            else if ((taskCount % 5) == 0)
            {
                return taskCount / 5;
            }
            else
            {
                return ((taskCount / 5) + 1);
            }
            
        }

        public void List_Changed(NamedList<ItemVM> newNamedList)
        {
            CurrentTaskList.Clear();
            CurrentWindow.Clear();
            
            if (newNamedList != null)
            {
                CurrentTaskList.AddRange(newNamedList.list);
                SortListByPriority();
                currentIndex = AllLists.IndexOf(newNamedList);

                if (!showCompleted)
                {
                    RemoveCompleted();
                }
            }
            Refresh();
        }               //Repopulates currenttasklist and currentwindow when selected list changed


        public void RemoveList(NamedList<ItemVM> selectedList)
        {
            AllLists.Remove(selectedList);
        }

        public void Search(string query)
        {
            CurrentTaskList.Clear();
            CurrentWindow.Clear();
            foreach (NamedList<ItemVM> namedlist in AllLists)
            {
                CurrentTaskList.AddRange((from ItemVM in namedlist.list
                                          where ItemVM.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase) || ItemVM.Description.Contains(query, StringComparison.InvariantCultureIgnoreCase)
                                          select ItemVM).ToList());
                //Add first linq query results to utility list to show

                var appointments = from ItemVM in namedlist.list where (ItemVM is AppointmentVM && !CurrentTaskList.Contains(ItemVM)) select ItemVM;
                //select those ItemVMs which are appointments and not already in the list

                foreach (AppointmentVM appointment in appointments)
                {
                    if ((from atendee in appointment.atendees where atendee.Contains(query, StringComparison.InvariantCultureIgnoreCase) select atendee).Any())
                        CurrentTaskList.Add(appointment);
                    //add those appointments where atendees list contains query string
                }
            }
            Refresh();
        }                                    //Queries every open list for ItemVMs that contain string, results populate in current window

        public void RemoveCompleted()
        {
            List<ItemVM> incompleteList = new List<ItemVM>();
            foreach(ItemVM ItemVM in CurrentTaskList)
            {
                if (!ItemVM.IsCompleted)
                {
                    incompleteList.Add(ItemVM);
                }
            }
            CurrentWindow.Clear();
            CurrentTaskList.Clear();
            CurrentTaskList.AddRange(incompleteList);
            Refresh();
        }                                        //Removes all completed tasks from current window + currenttasklist

        public void AddCompleted()
        {
            try
            {
                CurrentWindow.Clear();
                CurrentTaskList.Clear();

                CurrentTaskList.AddRange(AllLists.ElementAt(currentIndex).list);
                Refresh();
            } catch (ArgumentOutOfRangeException) { }
        }   //Returns completed tasks to currentwindow + currenttasklist

        private void SortListByPriority()
        {
            if (CurrentTaskList.Count != 0)
            {
                List<ItemVM> orderedList = CurrentTaskList.OrderByDescending(ItemVM => ItemVM.Priority).ToList();
                CurrentTaskList.Clear();
                CurrentTaskList.AddRange(orderedList);
            }
            
        }
    }
}
