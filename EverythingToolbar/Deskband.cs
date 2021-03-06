﻿using EverythingToolbar;
using NLog;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace CSDeskBand
{
	[ComVisible(true)]
    [Guid("9d39b79c-e03c-4757-b1b6-ecce843748f3")]
    [CSDeskBandRegistration(Name = "Everything Toolbar")]
    public class Deskband : CSDeskBandWpf
    {
        private static ToolbarControl toolbarControl;
        protected override UIElement UIElement => toolbarControl;

        public Deskband()
        {
            try
            {
                toolbarControl = new ToolbarControl();

                Options.MinHorizontalSize = new Size(18, 30);
                Options.MinVerticalSize = new Size(30, 40);
                TaskbarInfo.TaskbarEdgeChanged += OnTaskbarEdgeChanged;

                SearchResultsPopup.taskbarEdge = TaskbarInfo.Edge;
            }
            catch (Exception e)
			{
                ToolbarLogger.GetLogger("EverythingToolbar").Error(e, "Unhandled exception");
                string exceptionContent = e.Message + "\n\n" + e.StackTrace;
                if (MessageBox.Show(exceptionContent + "\n\nDo you want to copy the exception content to clipboard?",
                    "Unhandled exception occured",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Error) == MessageBoxResult.Yes)
				{
                    Clipboard.SetText(exceptionContent);
				}
			}
        }

        private void OnTaskbarEdgeChanged(object sender, TaskbarEdgeChangedEventArgs e)
        {
            SearchResultsPopup.taskbarEdge = e.Edge;
        }

        protected override void DeskbandOnClosed()
        {
            base.DeskbandOnClosed();
            toolbarControl.Destroy();
            toolbarControl = null;
        }
    }
}
