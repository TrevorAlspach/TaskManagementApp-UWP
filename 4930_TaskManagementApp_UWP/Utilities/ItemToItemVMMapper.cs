using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _4930_TaskManagementApp_UWP.ViewModels;
using AutoMapper;
using Library.TaskManagement;

namespace _4930_TaskManagementApp_UWP.Utilities
{
    public class ItemToItemVMMapper
    {
        public MapperConfiguration config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Item, ItemVM>()
            .Include<Library.TaskManagement.Task, TaskVM>()
            .Include<Appointment, AppointmentVM>()
            .ReverseMap();
            cfg.CreateMap<Library.TaskManagement.Task, TaskVM>()
            .ReverseMap();
            cfg.CreateMap<Appointment, AppointmentVM>()
            .ReverseMap();
        });

    }
}
