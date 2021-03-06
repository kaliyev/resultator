﻿using Resultator.Model;
using Resultator.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Resultator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public TaskManager taskManager;
        public MainPage()
        {
            this.InitializeComponent();
            taskManager = App.tasksManager;
            ContentFrame.Navigate(typeof(TaskItemView), App.tasksManager.GetAllItems());
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            TaskSplitView.IsPaneOpen = !TaskSplitView.IsPaneOpen;
        }

        private void taskTypeListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ContentFrame.Navigate(typeof(TaskItemView), e.ClickedItem);
            if (AdaptiveApp.CurrentState.Name != "PC") TaskSplitView.IsPaneOpen = false;
        }

        private void TaskerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskSectionAddButton.IsSelected)
            {
                ContentFrame.Navigate(typeof(TaskItemView), App.tasksManager.GetAllItems());
                TaskSplitView.IsPaneOpen = false;
                TaskerListBox.SelectedItem = null;
            }            
        }

        private void SectionsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TaskSplitView.IsPaneOpen = true;
        }

        private void AddSectionBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(TaskSectionCreate));
            TaskSplitView.IsPaneOpen = false;
        }

        private void AddTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(TaskItemCreate));
            TaskSplitView.IsPaneOpen = false;
        }
    }
}
