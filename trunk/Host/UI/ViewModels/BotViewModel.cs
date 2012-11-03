using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using AvalonDock.Layout;
using BiM.Core.Collections;
using BiM.Core.Messages;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.Views;
using BiM.Protocol.Messages;
using Bot = BiM.Behaviors.Bot;

namespace BiM.Host.UI.ViewModels
{
    public class BotViewModel : DockContainer<BotControl>, IDisposable, IDocked
    {
        private readonly ObservableCollectionMT<object> m_documents = new ObservableCollectionMT<object>();
        private readonly Dictionary<object, Assembly> m_documentsAssembly = new Dictionary<object, Assembly>();
        private readonly ReadOnlyObservableCollectionMT<object> m_readOnlyDocuments;

        public BotViewModel(Bot bot)
        {
            Bot = bot;

            bot.Dispatcher.RegisterNonShared(this);
        }

        public Bot Bot
        {
            get;
            private set;
        }

        protected override LayoutDocumentPane DocumentPane
        {
            get
            {
                return View.DocumentPane;
            }
        }

        #region Handlers

        [MessageHandler(typeof (CharacterSelectedSuccessMessage))]
        public void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            Parent.Title = bot.Character.Name;
        }

        #endregion

        public void Dispose()
        {
            if (Bot != null)
                Bot.Dispatcher.UnRegisterNonShared(this);

            UIManager.Instance.RemoveDocument(View);
        }

        public LayoutContent Parent
        {
            get;
            set;
        }
    }
}