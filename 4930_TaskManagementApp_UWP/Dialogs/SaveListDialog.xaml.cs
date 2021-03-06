using _4930_TaskManagementApp_UWP.Services;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace _4930_TaskManagementApp_UWP.Dialogs
{
    public sealed partial class SaveListDialog : ContentDialog
    {
        private object listContext;
        LocalFolderAccess localfolder = LocalFolderAccess.GetInstance;

        public string nameToSave { get; set; }

        public SaveListDialog(object listContext)
        {
            this.InitializeComponent();
            DataContext = this;
            this.listContext = listContext;
        }

        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //await (listContext as MainViewModel).SaveList(nameToSave);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
