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
using Library.TaskManagement;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace _4930_TaskManagementApp_UWP.Dialogs
{
    public sealed partial class CreateListDialog : ContentDialog
    {
        private object listContext;
        public string newListName { get; set; }

        public CreateListDialog(object listContext)
        {
            this.InitializeComponent();
            DataContext = this;
            this.listContext = listContext;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            (listContext as MainViewModel).AllLists.Add(new NamedList<ItemVM>(newListName));
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
