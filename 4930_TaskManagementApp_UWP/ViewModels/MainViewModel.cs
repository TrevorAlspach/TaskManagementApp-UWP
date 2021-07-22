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
using _4930_TaskManagementApp_UWP.WebServices;
using NSwag.Collections;
using AutoMapper;
using _4930_TaskManagementApp_UWP.Utilities;

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
        public bool showCompleted { get; set; }                             //bound to checkbox, when false will exclude completed tasks

        public ItemVM SelectedTask { get; set; }
        public ObservableDictionary<string, Guid> AllLists { get; set; }
        public NamedList<ItemVM> CurrentTaskList { get; set; }                     //populated with the ItemVMs of the currently selected list
        public ListNavigator<ItemVM> Navigator;                               //Reformatted navigator which returns observable collection<T> instead of dictionary
        public ObservableCollection<ItemVM> CurrentWindow { get; set; }       //The collection of ItemVMs which are currently displayed in view
        //private LocalFolderAccess localfolder = LocalFolderAccess.GetInstance;
        public ItemToItemVMMapper Mapper = new ItemToItemVMMapper();
        Mapper mapper { get; set; }
        private TaskManagementAPIService taskAPI = new TaskManagementAPIService();

        public MainViewModel()
        {
            AllLists = new ObservableDictionary<string, Guid>();
            CurrentTaskList = new NamedList<ItemVM>();
            CurrentWindow = new ObservableCollection<ItemVM>();
            Navigator = new ListNavigator<ItemVM>(CurrentTaskList, CurrentWindow);
            CurrentWindow = Navigator.GoToFirstPage();
            mapper = new Mapper(Mapper.config);
            PageNumber = 1;
            PageCount = 1;
            InitializeLists();
        }

        public async void InitializeLists()
        {
            AllLists.Clear();
            var LoadedLists = await TaskManagementAPIService.LoadAllLists();
            AllLists.AddRange(LoadedLists);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async System.Threading.Tasks.Task AddItem(Item item)
        {
            if (item is Library.TaskManagement.Task)
            {
                await taskAPI.AddTask(item as Library.TaskManagement.Task, CurrentTaskList.Id);
            }
            if (item is Appointment)
            {
                await taskAPI.AddAppointment(item as Appointment, CurrentTaskList.Id);
            }
             Refresh();
        }

        public async void RemoveItem()
        {
            var GuidToDelete = SelectedTask.Id;
            await taskAPI.RemoveItem(GuidToDelete, CurrentTaskList.Id);
            Refresh();
        }

        public void Complete()
        {
            SelectedTask.IsCompleted = true;
            AddItem(mapper.Map<ItemVM, Item>(SelectedTask));
        }                //Completes currently selected task

        public async void TogglePriority()
        {
            SelectedTask.Priority = !SelectedTask.Priority;
            await AddItem(mapper.Map<ItemVM, Item>(SelectedTask));
            Refresh();
        }          //toggles priority bool on selected task

        public async void Refresh()
        {
            NamedList<Item> list;
            if (showCompleted)
            {
                list = await taskAPI.GetList(CurrentTaskList.Id);
            } else
            {
                list = await taskAPI.GetListIncomplete(CurrentTaskList.Id);
            }
            CurrentWindow.Clear();
            CurrentTaskList.list.Clear();

            
            var itemVMs = mapper.Map<List<Item>, List<ItemVM>>(list.list);
            CurrentTaskList.list.AddRange(itemVMs);
            try
            {
                CurrentWindow = Navigator.GetCurrentPage();
                PageCount = calculateTotalPageCount();
            }
            catch (PageFaultException) { 
                Navigator.GoToFirstPage();
                PageCount -= 1;
                PageNumber = 1;
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

        /*public System.Threading.Tasks.Task LoadList(string filename)
        {
            await localfolder.LoadListfromJSON(AllLists, filename);
            Refresh();
        }   //loads list from localfolder into AllLists

        public async System.Threading.Tasks.Task SaveList(string filename)
        {
           await localfolder.SaveListasJSON(CurrentTaskList, filename);
        }    //saves current list to localfolder as filename*/

        public int calculateTotalPageCount()
        {
            int taskCount = CurrentTaskList.list.Count;
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

        public async void List_Changed(Guid newListGuid)
        {
           if (newListGuid != Guid.Empty)
            {
                CurrentTaskList.list.Clear();
                CurrentWindow.Clear();
                NamedList<Item> list;
                if (showCompleted)
                {
                    list = await taskAPI.GetList(newListGuid);
                }
                else
                {
                    list = await taskAPI.GetListIncomplete(newListGuid);
                }
                var mapper = new Mapper(Mapper.config);
                var itemVMs = mapper.Map<List<Item>, List<ItemVM>>(list.list);
                CurrentTaskList.list.AddRange(itemVMs);
                CurrentTaskList.Id = list.Id;
                CurrentTaskList.name = list.name;
                Refresh();
            }
           
        }               //Repopulates currenttasklist and currentwindow when selected list changed

        public async void AddList(string name)
        {
            await taskAPI.AddList(name);
            InitializeLists();
        }

        public async void RemoveList(KeyValuePair<string, Guid> listToRemove)
        {
            if (listToRemove.Value != Guid.Empty)
            {
                await taskAPI.RemoveList(listToRemove.Value);
                AllLists.Remove(listToRemove.Key);
            }
            CurrentWindow.Clear();
            InitializeLists();
        }

        public async void Search(string query)
        {
            var searchResults = await taskAPI.Search(query);
            CurrentTaskList.list.Clear();
            CurrentWindow.Clear();
            var mapper = new Mapper(Mapper.config);
            var itemVMs = mapper.Map<List<Item>, List<ItemVM>>(searchResults);
            CurrentTaskList.list.AddRange(itemVMs);
            CurrentWindow = Navigator.GetCurrentPage();
            PageCount = calculateTotalPageCount();
        }                                    //Queries every open list for ItemVMs that contain string, results populate in current window
    }
}
