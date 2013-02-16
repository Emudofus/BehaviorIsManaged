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
using System.ComponentModel;
using System.Reflection;
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
      Bot.CharacterSelected += Bot_CharacterSelected;
    }

    void Bot_CharacterSelected(Bot bot, Behaviors.Game.Actors.RolePlay.PlayedCharacter character)
    {
      Parent.Title = bot.Character.Name;
        }

        public Bot Bot
        {
            get;
            private set;
        }

        private LayoutContent m_parent;

        public LayoutContent Parent
        {
            get
            {
                return m_parent;
            }
            set
            {
                m_parent = value;
                Parent.Closing += OnClosing;
            }
        }

        protected override LayoutDocumentPane DocumentPane
        {
            get
            {
                return View.DocumentPane;
            }
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Bot.Dispose();
        }

        #region Handlers

        [MessageHandler(typeof(IdentificationSuccessMessage))]
        public void HandleIdentificationSuccessMessage(Bot bot, IdentificationSuccessMessage message)
        {
            Parent.Title = bot.ClientInformations.Nickname;
        }
        #endregion

        public void Dispose()
        {
            if (Bot != null)
      {
                Bot.Dispatcher.UnRegisterNonShared(this);
        Bot.CharacterSelected -= Bot_CharacterSelected;
      }

            UIManager.Instance.RemoveDocument(View);
        }
    }
}