#region License GNU GPL
// UIManager.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AvalonDock.Layout;
using BiM.Core.Collections;
using BiM.Core.Reflection;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.ViewModels;
using BiM.Host.UI.Views;

namespace BiM.Host.UI
{
    public class UIManager : DockContainer<MainWindow>
    {
        private static readonly UIManager m_instance = new UIManager();

        public static UIManager Instance
        {
            get { return m_instance; }
        }


        public UIManager()
        {
            Application.Current.MainWindow.Initialized += OnInitialized;
        }

        private void OnInitialized(object sender, EventArgs e)
        {
            Task.Factory.StartNew(() =>
                              {
                                  Host.Initialize();
                                  Host.Start();
                              });
        }


        public BotViewModel GetBotViewModel(Behaviors.Bot bot)
        {
            lock (DocumentPane.Children)
            {
                var document = DocumentPane.Children.FirstOrDefault(x => x.Content is BotControl && ( x.Content as BotControl ).ViewModel.Bot == bot);

                return document != null ? ( document.Content as BotControl ).ViewModel : null;
            }
        }

        public BotViewModel[] GetBotsViewModel()
        {
            lock (DocumentPane.Children)
            {
                return DocumentPane.Children.Where(x => x.Content is BotControl).
                    Select(x => (x.Content as BotControl).ViewModel).ToArray();
            }
        }

        #region Busy
        public void SetBusy(bool toggle)
        {
            IsBusy = toggle;
        }

        public void SetBusyProgress(double current, double max)
        {
            IsBusyCounterDisplayed = Math.Abs(current - max) > 0.1;
            BusyCounter = current;
            BusyCounterMax = max;
        }

        public bool IsBusy
        {
            get;
            set;
        }

        public string BusyMessage
        {
            get;
            set;
        }

        public bool IsBusyCounterDisplayed
        {
            get;
            set;
        }

        public double BusyCounter
        {
            get;
            set;
        }

        public double BusyCounterMax
        {
            get;
            set;
        }

        #endregion

        protected override LayoutDocumentPane DocumentPane
        {
            get { return View.DocumentPane; }
        }
    }
}