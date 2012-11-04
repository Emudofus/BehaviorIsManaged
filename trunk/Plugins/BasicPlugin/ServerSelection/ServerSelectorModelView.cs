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
        }

        public override void OnDetached()
        {
            base.OnDetached();

            var viewModel = Bot.GetViewModel();
            viewModel.RemoveDocument(View);
        }
    }
}