using Library.TaskManagement;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _4930_TaskManagementApp_UWP.WebServices
{
    class TaskManagementAPIService
    {
        private static string baseURL = "http://localhost/TaskManagementAPI/";

        public static async Task<IDictionary<string, Guid>> LoadAllLists()
        {
            var handler = new WebRequestHandler();
            var lists = JsonConvert.DeserializeObject<IDictionary<string, Guid>>(await handler.Get($"{baseURL}TaskLists/AllListIDs"));
            return lists;
        }

        public async System.Threading.Tasks.Task AddTask(Library.TaskManagement.Task task, Guid ListId)
        {
            var handler = new WebRequestHandler();
            await handler.Post($"{baseURL}TaskLists/{ListId}/AddOrUpdateTask", task);
        }

        public async System.Threading.Tasks.Task AddAppointment(Appointment appointment, Guid ListId)
        {
            var handler = new WebRequestHandler();
            await handler.Post($"{baseURL}TaskLists/{ListId}/AddOrUpdateAppointment", appointment);
        }

        public async System.Threading.Tasks.Task RemoveItem(Guid itemId, Guid ListId)
        {
            var handler = new WebRequestHandler();
            await handler.Post($"{baseURL}TaskLists/{ListId}/RemoveItem", itemId);
        }

        public async Task<NamedList<Item>> GetList(Guid ListId)
        {
            var handler = new WebRequestHandler();
            var list = JsonConvert.DeserializeObject<NamedList<Item>>(await handler.Get($"{baseURL}TaskLists/{ListId}"), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return list;
        }

        public async Task<NamedList<Item>> GetListIncomplete(Guid ListId)
        {
            var handler = new WebRequestHandler();
            var list = JsonConvert.DeserializeObject<NamedList<Item>>(await handler.Get($"{baseURL}TaskLists/{ListId}/Incomplete"), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return list;
        }

        public async System.Threading.Tasks.Task AddList(string name)
        {
            var handler = new WebRequestHandler();
            await handler.Post($"{baseURL}TaskLists/AddList", name);
        }

        public async System.Threading.Tasks.Task RemoveList(Guid ListId)
        {
            var handler = new WebRequestHandler();
            await handler.Post($"{baseURL}TaskLists/DeleteList", ListId);
        }

        public async System.Threading.Tasks.Task<List<Item>> Search(string query)
        {
            var handler = new WebRequestHandler();
            var list = JsonConvert.DeserializeObject<List<Item>>(await handler.Get($"{baseURL}TaskLists/Search/{query}"), new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
            return list;
        }
    }
}
