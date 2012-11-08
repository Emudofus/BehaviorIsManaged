#region License GNU GPL
// CharacterSelectionViewModel.cs
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
using System.Windows;
using System.Windows.Media;
using BiM.Behaviors;
using BiM.Behaviors.Authentification;
using BiM.Behaviors.Frames;
using BiM.Core.Cryptography;
using BiM.Core.Messages;
using BiM.Host.UI;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.ViewModels;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BasicPlugin.CharacterSelection
{
    public class CharacterSelectionRegister
    {
        [MessageHandler(typeof (CharactersListMessage))]
        public static void HandleCharactersListMessage(Bot bot, CharactersListMessage message)
        {
            bot.AddFrame(new CharacterSelectionViewModel(bot));
        }
    }

    public class CharacterSelectionViewModel : Frame<CharacterSelectionViewModel>, IViewModel<CharacterSelectionView>
    {
        public CharacterSelectionViewModel(Bot bot)
            : base(bot)
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        object IViewModel.View
        {
            get { return View; }
            set { View = (CharacterSelectionView)value; }
        }

        public CharacterSelectionView View
        {
            get;
            set;
        }


        #region SelectCharacterCommand

        private DelegateCommand m_selectCharacterCommand;

        public DelegateCommand SelectCharacterCommand
        {
            get { return m_selectCharacterCommand ?? (m_selectCharacterCommand = new DelegateCommand(OnSelectCharacter, CanSelectCharacter)); }
        }

        private bool CanSelectCharacter(object parameter)
        {
            return parameter is CharactersListEntry;
        }

        private void OnSelectCharacter(object parameter)
        {
            if (parameter == null || !CanSelectCharacter(parameter))
                return;

            Bot.SendToServer(new CharacterSelectionMessage(( (CharactersListEntry)parameter ).Id));
        }

        #endregion


        #region CreateCharacterCommand

        private CharacterCreationDialog m_characterCreationDialog;
        private CharacterCreationData m_characterCreationData;
        private DelegateCommand m_createCharacterCommand;

        public DelegateCommand CreateCharacterCommand
        {
            get { return m_createCharacterCommand ?? (m_createCharacterCommand = new DelegateCommand(OnCreateCharacter, CanCreateCharacter)); }
        }

        private bool CanCreateCharacter(object parameter)
        {
            return true; // Bot.ClientInformations.CanCreateNewCharacter; -> not used
        }

        private void OnCreateCharacter(object parameter)
        {
            if (m_characterCreationDialog != null && m_characterCreationDialog.Visibility == Visibility.Visible)
                m_characterCreationDialog.Close();

            m_characterCreationDialog = new CharacterCreationDialog();

            if (m_characterCreationData != null)
                m_characterCreationDialog.Data = m_characterCreationData;
            else
            {
                m_characterCreationDialog.Data.EnabledBreeds = Bot.ClientInformations.AvailableBreeds;
                m_characterCreationData = m_characterCreationDialog.Data;
            }

            if (m_characterCreationDialog.ShowDialog() == true)
            {
                int[] colors = new int[5];
                colors[0] = m_characterCreationData.Color1Used ? ColorToInt(m_characterCreationData.Color1) : -1;
                colors[1] = m_characterCreationData.Color2Used ? ColorToInt(m_characterCreationData.Color2) : -1;
                colors[2] = m_characterCreationData.Color3Used ? ColorToInt(m_characterCreationData.Color3) : -1;
                colors[3] = m_characterCreationData.Color4Used ? ColorToInt(m_characterCreationData.Color4) : -1;
                colors[4] = m_characterCreationData.Color5Used ? ColorToInt(m_characterCreationData.Color5) : -1;

                Bot.SendToServer(new CharacterCreationRequestMessage(m_characterCreationData.CharacterName, (sbyte)m_characterCreationData.Breed,
                    m_characterCreationData.Sex == SexTypeEnum.SEX_FEMALE, colors));
            }
        }

        private int ColorToInt(Color color)
        {
            return BitConverter.ToInt32(new byte[] { color.B, color.G, color.R, 0x00 }, 0);
        }

        #endregion


        #region DeleteCharacterCommand

        private DeletionDialog m_deletionDialog;
        private DelegateCommand m_deleteCharacterCommand;

        public DelegateCommand DeleteCharacterCommand
        {
            get { return m_deleteCharacterCommand ?? (m_deleteCharacterCommand = new DelegateCommand(OnDeleteCharacter, CanDeleteCharacter)); }
        }

        private bool CanDeleteCharacter(object parameter)
        {
            return parameter is CharactersListEntry;
        }

        private void OnDeleteCharacter(object parameter)
        {
            if (parameter == null || !CanDeleteCharacter(parameter))
                return;

            if (m_deletionDialog != null && m_deletionDialog.Visibility == Visibility.Visible)
                m_deletionDialog.Close();

            var character = (CharactersListEntry)parameter;
            if (character.Level >= 20)
            {
                m_deletionDialog = new DeletionDialog();
                m_deletionDialog.CharacterName = character.Name;
                m_deletionDialog.SecretQuestion = Bot.ClientInformations.SecretQuestion;

                if (m_deletionDialog.ShowDialog() == true)
                {
                    Bot.SendToServer(new CharacterDeletionRequestMessage(character.Id, Cryptography.GetMD5Hash(character.Id + "~" + m_deletionDialog.SecretAnswer)));
                }
            }
            else if (MessageService.ShowYesNoQuestion(View, string.Format("Are you sure you want to delete {0} ?", character.Name)))
            {
                Bot.SendToServer(new CharacterDeletionRequestMessage(character.Id, Cryptography.GetMD5Hash(character.Id + "~" + "000000000000000000")));
            }
        }

        #endregion

        [MessageHandler(typeof (CharacterSelectedSuccessMessage))]
        public void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            Bot.RemoveFrame(this);
        }

        [MessageHandler(typeof (CharactersListMessage))]
        public void HandleCharactersListMessage(Bot bot, CharactersListMessage message)
        {
            View.Dispatcher.BeginInvoke(new Action(() =>
                                       {
                                           SelectCharacterCommand.RaiseCanExecuteChanged();
                                           DeleteCharacterCommand.RaiseCanExecuteChanged();
                                       }));
        }

        [MessageHandler(typeof (CharacterDeletionErrorMessage))]
        public void HandleCharacterDeletionErrorMessage(Bot bot, CharacterDeletionErrorMessage message)
        {
            MessageService.ShowError(View, "Cannot delete the character : " + (CharacterDeletionErrorEnum)message.reason);
        }

        [MessageHandler(typeof (CharacterCreationResultMessage))]
        public void HandleCharacterCreationResultMessage(Bot bot, CharacterCreationResultMessage message)
        {
            var result = (CharacterCreationResultEnum)message.result;
            if (result != CharacterCreationResultEnum.OK)
            {
                MessageService.ShowError(View, "Cannot create the character : " + result);
            }
            else
            {
                m_characterCreationData = null;
            }
        }

        public override void OnAttached()
        {
            base.OnAttached();

            var botViewModel = Bot.GetViewModel();
            var layout = botViewModel.AddDocument(this, () => new CharacterSelectionView());
            layout.Title = "Characters";
            layout.CanClose = false;
        }

        public override void OnDetached()
        {
            base.OnDetached();

            var viewModel = Bot.GetViewModel();
            viewModel.RemoveDocument(View);

            if (m_deletionDialog != null && m_deletionDialog.Visibility == Visibility.Visible)
            {
                View.Dispatcher.Invoke(new Action(m_deletionDialog.Close));
                m_deletionDialog = null;
            }

            if (m_characterCreationDialog != null && m_characterCreationDialog.Visibility == Visibility.Visible)
            {
                View.Dispatcher.Invoke(new Action(m_characterCreationDialog.Close));
                m_characterCreationDialog = null;
            }
        }
    }
}