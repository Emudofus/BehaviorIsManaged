#region License GNU GPL
// ServerSelectorModelView.cs
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
using System.ComponentModel;
using BiM.Behaviors;
using BiM.Behaviors.Authentification;
using BiM.Behaviors.Frames;
using BiM.Core.Messages;
using BiM.Host.UI;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.ViewModels;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BasicPlugin.ServerSelection
{
    public static class ServerSelectionRegister
    {
        [MessageHandler(typeof(ServersListMessage))]
        public static void HandleServersListMessage(Bot bot, ServersListMessage message)
        {
            bot.AddFrame(new ServerSelectorModelView(bot));
        }
    }

    public class ServerSelectorModelView : Frame<ServerSelectorModelView>, IViewModel<ServerSelectionView>
    {
        public ServerSelectorModelView(Bot bot)
            : base(bot)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        object IViewModel.View
        {
            get { return View; }
            set { View = (ServerSelectionView)value; }
        }

        public ServerSelectionView View
        {
            get;
            set;
        }

        private DelegateCommand m_selectServer;

        public DelegateCommand SelectServerCommand
        {
            get
            {
                return m_selectServer ?? ( m_selectServer = new DelegateCommand(OnSelect, CanSelect) );
            }
        }

        private bool CanSelect(object parameter)
        {
            if (parameter == null)
                return false;

            var server = (ServersListEntry)parameter;
            return server.IsSelectable && 
                server.Status == ServerStatusEnum.ONLINE && 
                (server.Server.restrictedToLanguages.Count == 0 || server.Server.restrictedToLanguages.Contains(Bot.ClientInformations.Lang));
        }

        private void OnSelect(object parameter)
        {
            if (parameter == null || !CanSelect(parameter))
                return;

            var server = (ServersListEntry)parameter;
            Bot.AddMessage(() => Bot.SendToServer(new ServerSelectionMessage((short) server.Id)));
        }

        [MessageHandler(typeof (ServersListMessage))]
        public void HandleServersListMessage(Bot bot, ServersListMessage message)
        {
            View.Dispatcher.BeginInvoke(new Action(SelectServerCommand.RaiseCanExecuteChanged));
        }

        [MessageHandler(typeof (ServerStatusUpdateMessage))]
        public void HandleServerStatusUpdateMessage(Bot bot, ServerStatusUpdateMessage message)
        {
            View.Dispatcher.BeginInvoke(new Action(SelectServerCommand.RaiseCanExecuteChanged));
        }

        [MessageHandler(typeof (SelectedServerDataMessage))]
        public void HandleSelectedServerDataMessage(Bot bot, SelectedServerDataMessage message)
        {
            Bot.RemoveFrame(this);
        }

        public override void OnAttached()
        {
            base.OnAttached();

            var viewModel = Bot.GetViewModel();
            var layout = viewModel.AddDocument(this, () => new ServerSelectionView());
            layout.Title = "Servers";
            layout.CanClose = false;
        }

        public override void OnDetached()
        {
            base.OnDetached();

            var viewModel = Bot.GetViewModel();
            viewModel.RemoveDocument(View);
        }
    }
}