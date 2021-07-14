using _4930_TaskManagementApp_UWP.ViewModels;
using Library.TaskManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace _4930_TaskManagementApp_UWP.Services
{
    

    public sealed class LocalFolderAccess
    {
        private static readonly LocalFolderAccess instance = new LocalFolderAccess();
        
        static LocalFolderAccess() { }
        private LocalFolderAccess() 
        {
            localFileNames = new List<string>();
        }

        public static LocalFolderAccess GetInstance
        {
            get
            {
                instance.getLocalFiles();
                return instance;
            }
        }
        public string selectedName { get; set; }
        public List<string> localFileNames { get; set; }
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        public async System.Threading.Tasks.Task SaveListasJSON(List<ItemVM> list, string filename)
        {
            string json = JsonConvert.SerializeObject(list, new JsonSerializerSettings 
            { 
                TypeNameHandling = TypeNameHandling.Auto
            });

            StorageFile file = await localFolder.CreateFileAsync($"{filename}.json",
                  CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, json);
        }

        public async System.Threading.Tasks.Task LoadListfromJSON(ObservableCollection<NamedList<ItemVM>> Namedlist, string filename)
        {
            try
            {
                StorageFile file = await localFolder.GetFileAsync($"{filename}.json");
                String listAsJson  = await FileIO.ReadTextAsync(file);
                var deserializedList = JsonConvert.DeserializeObject<List<ItemVM>>(listAsJson, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
                Namedlist.Add(new NamedList<ItemVM>($"{filename}", deserializedList));
                //list.Clear();
                //list.AddRange(deserializedList);
            }
            catch (FileNotFoundException) { }
            catch (IOException) { }
        }

        public async void getLocalFiles()
        {
            var storagefiles = await localFolder.GetFilesAsync();
            localFileNames.Clear();
            foreach(StorageFile file in storagefiles)
            {
                localFileNames.Add(file.DisplayName);
            }
            
        }
    }
}
