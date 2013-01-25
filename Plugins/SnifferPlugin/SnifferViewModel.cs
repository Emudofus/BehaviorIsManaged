#region License GNU GPL

// SnifferViewmodel.cs
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Data;
using AvalonDock.Layout;
using BiM.Behaviors;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Messages;
using BiM.Core.Collections;
using BiM.Core.Config;
using BiM.Core.Messages;
using BiM.Core.Network;
using BiM.Host.UI;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.ViewModels;
using Microsoft.Win32;

namespace SnifferPlugin
{
    internal class SnifferViewModelRegister
    {
        [MessageHandler(typeof(BotAddedMessage))]
        public static void HandleBotAddedMessage(object dummy, BotAddedMessage message)
        {
            message.Bot.AddFrame(new SnifferViewModel(message.Bot));
        }
    }

    public class SnifferViewModel : Frame<SnifferViewModel>, IViewModel<SnifferView>
    {
        #region Recording On The Fly
        [Configurable("OnTheFlyFileName", "The name of the file used to record the messages on the fly. {0} is replaced by the name of the PC, {1} by the date.")]
        public static string OnTheFlyFileName = "{1} - {0}.csv";
        private void FlushOnTheFly(string lastMessage)
        {
            string filename = "OnTheFly/" + OnTheFlyFileName.Replace("{0}", Bot.Character == null ? "Bot" : Bot.Character.Name).Replace("{1}", DateTime.Today.ToString("yyMMdd"));
            try
            {
                Directory.CreateDirectory("OnTheFly");
                File.AppendAllText(filename, lastMessage);
            }
            catch(Exception ex)
            {
                Bot.Character.SendError("Can't append OnTheFly data in {0} : {1}", filename, ex.Message);
            }
        }

        #endregion

        [Configurable("NbMessagesToKeep", "This says how message to keep in memory. The older will be removed as new messages come over this quantity.")]
        public static int NbMessagesToKeep = 1000;

        [Configurable("DefaultPaused", "If true, the sniffer will be paused by default at start.")]
        public static bool DefaultPaused = false;

        [Configurable("DefaultRecordOnTheFly", "If true, the sniffer will record on the fly by default at start.")]
        public static bool DefaultRecordOnTheFly = true;

        private readonly ObservableCollectionMT<ObjectDumpNode> m_messages;
        private readonly ReadOnlyObservableCollectionMT<ObjectDumpNode> m_readOnlyMessages;
        private CollectionViewSource m_collectionViewSource;
        private string m_searchText;
        private bool m_seeProperties;
        private bool m_onTheFly;

        public SnifferViewModel(Bot bot)
            : base(bot)
        {
            m_messages = new ObservableCollectionMT<ObjectDumpNode>();
            m_readOnlyMessages = new ReadOnlyObservableCollectionMT<ObjectDumpNode>(m_messages);
            IsPaused = DefaultPaused;
            OnTheFly = DefaultRecordOnTheFly;
        }

        public ReadOnlyObservableCollection<ObjectDumpNode> Messages
        {
            get { return m_readOnlyMessages; }
        }

        public bool IsPaused
        {
            get;
            set;
        }

        public bool SeeProperties
        {
            get { return m_seeProperties; }
            set
            {
                m_seeProperties = value;
                RefreshVisibility();
            }
        }

        public bool OnTheFly
        {
            get { return m_onTheFly; }
            set
            {
                m_onTheFly = value;
            }
        }

        public string SearchText
        {
            get { return m_searchText; }
            set
            {
                m_searchText = value;
                if (m_collectionViewSource != null)
                    m_collectionViewSource.View.Refresh();
            }
        }

        #region IViewModel<SnifferView> Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        object IViewModel.View
        {
            get { return View; }
            set { View = (SnifferView)value; }
        }

        public SnifferView View
        {
            get;
            set;
        }

        #endregion

        private void OnMesssageDispatched(MessageDispatcher dispatcher, Message message)
        {
            if (IsPaused && !OnTheFly)
                return;

            var dumper = new TreeDumper(message);
            ObjectDumpNode tree = dumper.GetDumpTree();

            NetworkMessage networkMessage = message as NetworkMessage;
            if (networkMessage != null)
            {
                tree.TimeStamp = DateTime.Now;
                tree.Id = networkMessage.MessageId;
                tree.From = networkMessage.From;

                foreach (ObjectDumpNode child in tree.Childrens)
                {
                    if (m_seeProperties)
                    {
                        child.IsVisible = true;
                    }
                    else if (child.IsProperty)
                    {
                        child.IsVisible = false;
                    }
                }

                if (!IsPaused)
                {
                    m_messages.Add(tree);

                    #region Cleaning : avoid memory overflow on a long run
                    if (NbMessagesToKeep > 0 && m_messages.Count > NbMessagesToKeep)
                        m_messages.RemoveAt(0);
                    #endregion Cleaning : avoid memory overflow on a long run
                }
                #region Record On the fly
                if (OnTheFly)
                    FlushOnTheFly(tree.ExportToString(true));
                #endregion Record On the fly
            }
            else
                if (OnTheFly)
                {
                    if (message is InformationMessage)
                    {
                        #region Record On the fly
                        if (OnTheFly)
                            FlushOnTheFly((message as InformationMessage).Message+"\n");
                        #endregion Record On the fly
                    }
                }

        }


        private void FilterMethod(object sender, FilterEventArgs e)
        {
            e.Accepted = true;

            if (string.IsNullOrEmpty(SearchText))
                return;

            var node = e.Item as ObjectDumpNode;

            if (node == null)
                return;

            e.Accepted = node.Parent != null || node.Name.Contains(SearchText);
        }

        private void RefreshVisibility()
        {
            foreach (ObjectDumpNode node in m_messages)
            {
                foreach (ObjectDumpNode child in node.Childrens)
                {
                    if (m_seeProperties)
                    {
                        child.IsVisible = true;
                    }
                    else if (child.IsProperty)
                    {
                        child.IsVisible = false;
                    }
                }
            }
        }


        #region ExportCommand

        private DelegateCommand m_exportCommand;

        public DelegateCommand ExportCommand
        {
            get { return m_exportCommand ?? (m_exportCommand = new DelegateCommand(OnExport, CanExport)); }
        }

        private bool CanExport(object parameter)
        {
            return true;
        }

        private void OnExport(object parameter)
        {
            var dialog = new SaveFileDialog
            {
                FileName = string.Format("logs {0}", DateTime.Now.ToString("dd-MM-yy")),
                DefaultExt = ".log",
                Filter = "Log files (.log)|*.log|CSV files (.csv)|*.csv"
            };

            if (dialog.ShowDialog() == true)
            {
                var builder = new StringBuilder();
                bool exportCSV = Path.GetExtension(dialog.FileName) == ".csv";
                for (int i = 0; i < m_messages.Count; i++)
                {
                    builder.Append(m_messages[i].ExportToString(exportCSV));
                    builder.AppendLine();
                    if (!exportCSV) builder.AppendLine();
                }
                File.WriteAllText(dialog.FileName, builder.ToString());
            }
        }

        #endregion

        #region ClearCommand

        private DelegateCommand m_clearCommand;

        public DelegateCommand ClearCommand
        {
            get { return m_clearCommand ?? (m_clearCommand = new DelegateCommand(OnClear, CanClear)); }
        }

        private bool CanClear(object parameter)
        {
            return true;
        }

        private void OnClear(object parameter)
        {
            m_messages.Clear();
            RefreshVisibility();
        }

        #endregion


        #region PauseResumeCommand

        private DelegateCommand m_pauseResumeCommand;

        public DelegateCommand PauseResumeCommand
        {
            get { return m_pauseResumeCommand ?? (m_pauseResumeCommand = new DelegateCommand(OnPauseResume, CanPauseResume)); }
        }

        private bool CanPauseResume(object parameter)
        {
            return true;
        }

        private void OnPauseResume(object parameter)
        {
            IsPaused = !IsPaused;
        }

        #endregion

        public override void OnAttached()
        {
            base.OnAttached();

            Bot.Dispatcher.MessageDispatched += OnMesssageDispatched;

            BotViewModel viewModel = Bot.GetViewModel();
            LayoutDocument layout = viewModel.AddDocument(this, () => new SnifferView());
            layout.Title = "Sniffer";
            layout.CanClose = false;

            View.Dispatcher.Invoke(new Action(() => (m_collectionViewSource = View.Resources["Messages"] as CollectionViewSource).Filter += FilterMethod));
        }


        public override void OnDetached()
        {
            base.OnDetached();

            Bot.Dispatcher.MessageDispatched -= OnMesssageDispatched;
            BotViewModel viewModel = Bot.GetViewModel();
            if (viewModel != null)
                viewModel.RemoveDocument(View);
        }
    }
}