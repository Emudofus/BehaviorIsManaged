#region License GNU GPL
// BotViewModel.cs
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
        //private readonly ReadOnlyObservableCollectionMT<object> m_readOnlyDocuments;

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

        [MessageHandler(typeof (IdentificationMessage))]
        public void HandleIdentificationMessage(Bot bot, IdentificationMessage message)
        {
            Parent.Title = bot.ClientInformations.Login;
        }

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