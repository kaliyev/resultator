using Resultator.Model;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Resultator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskItemCreate : Page
    {
        public TaskManager taskManager;
        public TaskItemCreate()
        {
            this.InitializeComponent();
            taskManager = App.tasksManager;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            taskManager = App.tasksManager;
            
        }

        private void SaveItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(ItemName.Text) && !String.IsNullOrEmpty(ItemDescription.Text) 
                && comboBox.SelectedIndex > -1)
            {
                TaskItem saveItem = new TaskItem(ItemName.Text, ItemDescription.Text);
                saveItem.startTime = ItemStartDate.Date.DateTime + ItemStartTime.Time;
                saveItem.endTime = ItemEndDate.Date.DateTime + ItemEndTime.Time;
                App.tasksManager.sections[comboBox.SelectedIndex].AddItem(saveItem);
                App.tasksManager.saveToFile();
                saveItem = null;
                this.Frame.Navigate(typeof(TaskItemView), App.tasksManager.sections[comboBox.SelectedIndex]);
            }
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            ItemName.Text = "";
            ItemDescription.Text = "";
        }

    }
}
