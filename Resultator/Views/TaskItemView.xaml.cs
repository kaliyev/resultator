using Resultator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Resultator.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TaskItemView : Page
    {
        public TaskItem task;
        public TaskSection tasksSection = new TaskSection();
        public int itemIndex;
        private bool selected = false;

        public TaskItemView()
        {
            this.InitializeComponent();
            selected = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            tasksSection = (TaskSection)e.Parameter;
            selected = false;
            if (AdaptiveApp.CurrentState.Name == "Phone")
            {
                if (selected)
                {
                    FirstColumn.Width = new GridLength(0);
                    SecondColumn.Width = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    FirstColumn.Width = new GridLength(1, GridUnitType.Star);
                    SecondColumn.Width = new GridLength(0);
                }
            }
            UpdateGroupedList();
        }

        private void UpdateGroupedList()
        {
            tasksSection.items = new ObservableCollection<TaskItem>(tasksSection.items.OrderBy(a => a.startTime));

            var groupedList =
            from item in tasksSection.items
            group item by item.startTime.Date.ToString("d") into res
            select res;

            groupLister.Source = groupedList;
        }

        private void taskListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            selected = true;
            if (AdaptiveApp.CurrentState.Name == "Phone")
            {
                if (selected)
                { 
                    FirstColumn.Width = new GridLength(0);
                    SecondColumn.Width = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    FirstColumn.Width = new GridLength(1, GridUnitType.Star);
                    SecondColumn.Width = new GridLength(0);
                }
            }
            
            task = (TaskItem)e.ClickedItem;
            ItemName.Text = task.name;
            ItemDescription.Text = task.description;
            ItemStartDate.Date = task.startTime.Date;
            ItemStartTime.Time = task.startTime.TimeOfDay;
            ItemEndDate.Date = task.endTime.Date;
            ItemEndTime.Time = task.endTime.TimeOfDay;
        }

        private void SaveItemButton_Click(object sender, RoutedEventArgs e)
        {
            saveProgress.Visibility = Visibility.Visible;
            task.name = ItemName.Text.Trim();
            task.description = ItemDescription.Text.Trim();
            task.startTime = ItemStartDate.Date.DateTime + ItemStartTime.Time;
            task.endTime = ItemEndDate.Date.DateTime + ItemEndTime.Time;
            App.tasksManager.saveToFile();
            UpdateGroupedList();
            SaveResult.Text = "Saved";
            saveProgress.Visibility = Visibility.Collapsed;
            if (AdaptiveApp.CurrentState.Name == "Phone")
            {
                FirstColumn.Width = new GridLength(1, GridUnitType.Star);
                SecondColumn.Width = new GridLength(0);
            }
        }
        
        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            tasksSection.items.RemoveAt(itemIndex);
            App.tasksManager.saveToFile();
        }

        private void AdaptiveApp_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
            VisualState state = (VisualState)e.NewState;
            if (state.Name == "Phone")
            {
                if (selected)
                {
                    FirstColumn.Width = new GridLength(0);
                    SecondColumn.Width = new GridLength(1, GridUnitType.Star);
                }
                else
                {
                    FirstColumn.Width = new GridLength(1, GridUnitType.Star); 
                    SecondColumn.Width = new GridLength(0);
                }
            }
        }

        private void DoneItemButton_Click(object sender, RoutedEventArgs e)
        {
            task.status = true;
            tasksSection.items.RemoveAt(itemIndex);
            App.tasksManager.saveToFile();
            UpdateGroupedList();
        }
    }
}
