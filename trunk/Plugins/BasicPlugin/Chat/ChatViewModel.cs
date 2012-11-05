using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using AvalonDock.Layout;
using BiM.Behaviors;
using BiM.Behaviors.Data;
using BiM.Behaviors.Frames;
using BiM.Behaviors.Game.Actors;
using BiM.Behaviors.Game.Actors.RolePlay;
using BiM.Behaviors.Game.Chat;
using BiM.Core.Messages;
using BiM.Host.UI;
using BiM.Host.UI.Helpers;
using BiM.Host.UI.ViewModels;
using BiM.Protocol.Data;
using BiM.Protocol.Enums;
using BiM.Protocol.Messages;

namespace BasicPlugin.Chat
{
    public class ChatRegister
    {
        [MessageHandler(typeof (CharacterSelectedSuccessMessage))]
        public static void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            bot.AddFrame(new ChatViewModel(bot));
        }
    }

    public class ChatViewModel : Frame<ChatViewModel>, IViewModel<ChatView>
    {
        public const string MacroCounter = "##counter##";
        public const string MacroCharacter = "##character##";

        private static Dictionary<ChatActivableChannelsEnum, Color> m_colorsRules = new Dictionary<ChatActivableChannelsEnum, Color>()
        {
            {ChatActivableChannelsEnum.CHANNEL_GLOBAL, ColorFromHtmlCode("312D24")},
            {ChatActivableChannelsEnum.CHANNEL_TEAM, ColorFromHtmlCode("006699")},
            {ChatActivableChannelsEnum.CHANNEL_GUILD, ColorFromHtmlCode("663399")},
            {ChatActivableChannelsEnum.CHANNEL_ALIGN, ColorFromHtmlCode("BB6200")},
            {ChatActivableChannelsEnum.CHANNEL_PARTY, ColorFromHtmlCode("006699")},
            {ChatActivableChannelsEnum.CHANNEL_SALES, ColorFromHtmlCode("663300")},
            {ChatActivableChannelsEnum.CHANNEL_SEEK, ColorFromHtmlCode("737373")},
            {ChatActivableChannelsEnum.CHANNEL_NOOB , ColorFromHtmlCode("0000cc")},
            {ChatActivableChannelsEnum.CHANNEL_ADMIN, ColorFromHtmlCode("ff00ff")},
            {ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE, ColorFromHtmlCode("0066ff")},
            {ChatActivableChannelsEnum.PSEUDO_CHANNEL_INFO, ColorFromHtmlCode("009900")},
            {ChatActivableChannelsEnum.PSEUDO_CHANNEL_FIGHT_LOG, ColorFromHtmlCode("009900")},
            {ChatActivableChannelsEnum.CHANNEL_ADS, ColorFromHtmlCode("24a394")},
            {ChatActivableChannelsEnum.CHANNEL_ARENA, ColorFromHtmlCode("f16392")},
            {(ChatActivableChannelsEnum)666, ColorFromHtmlCode("FF0000")},
        };

        private Dictionary<ChatActivableChannelsEnum, string> m_channelsNames = new Dictionary<ChatActivableChannelsEnum, string>();

        private string m_wordFrom = DataProvider.Instance.Get<string>("ui.chat.from");
        private string m_wordTo = DataProvider.Instance.Get<string>("ui.chat.to");

        private int m_counter = 0;

        private ObservableCollection<FloodEntry> m_floodEntries;

        private bool m_isFloodEnabled;
        private ReadOnlyObservableCollection<FloodEntry> m_readOnlyFloodEntries;

        public ChatViewModel(Bot bot)
            : base(bot)
        {
            AvailableChannels = new[]
                                    {
                                        ChatActivableChannelsEnum.CHANNEL_GLOBAL,
                                        ChatActivableChannelsEnum.CHANNEL_TEAM,
                                        ChatActivableChannelsEnum.CHANNEL_GUILD,
                                        ChatActivableChannelsEnum.CHANNEL_ALIGN,
                                        ChatActivableChannelsEnum.CHANNEL_PARTY,
                                        ChatActivableChannelsEnum.CHANNEL_SALES,
                                        ChatActivableChannelsEnum.CHANNEL_SEEK,
                                        ChatActivableChannelsEnum.CHANNEL_NOOB,
                                        ChatActivableChannelsEnum.CHANNEL_ARENA,
                                        ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE,
                                    };

            var channels = Enum.GetValues(typeof(ChatActivableChannelsEnum));
            foreach (ChatActivableChannelsEnum channel in channels)
            {
                var data = DataProvider.Instance.Get<ChatChannel>((int)channel);
                m_channelsNames.Add(channel, DataProvider.Instance.Get<string>(data.nameId));
            }

            m_floodEntries = new ObservableCollection<FloodEntry>();
            m_readOnlyFloodEntries = new ReadOnlyObservableCollection<FloodEntry>(m_floodEntries);
        }

        public bool IsFloodEnabled
        {
            get { return m_isFloodEnabled; }
            set
            {
                m_isFloodEnabled = value;
                if (value)
                    StartFlood();
            }
        }

        public string TextToSend
        {
            get;
            set;
        }

        public ChatActivableChannelsEnum Channel
        {
            get;
            set;
        }

        public ChatActivableChannelsEnum[] AvailableChannels
        {
            get;
            set;
        }

        public string ReceiverName
        {
            get;
            set;
        }

        public FloodEntry SelectedEntry
        {
            get;
            set;
        }

        public ReadOnlyObservableCollection<FloodEntry> FloodEntries
        {
            get { return m_readOnlyFloodEntries; }
        }

        #region SendTextCommand

        private DelegateCommand m_sendTextCommand;

        public DelegateCommand SendTextCommand
        {
            get { return m_sendTextCommand ?? (m_sendTextCommand = new DelegateCommand(OnSendText, CanSendText)); }
        }

        private bool CanSendText(object parameter)
        {
            return true;
        }

        private void OnSendText(object parameter)
        {
            if (string.IsNullOrEmpty(TextToSend))
                return;

            if (Channel == ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE)
            {
                Bot.Character.SayTo(TextToSend, ReceiverName);
            }
            else
            {
                Bot.Character.Say(TextToSend, Channel);
            }
        }

        #endregion

        #region StartFloodCommand

        private DelegateCommand m_startFloodCommand;

        public DelegateCommand StartFloodCommand
        {
            get { return m_startFloodCommand ?? (m_startFloodCommand = new DelegateCommand(OnStartFlood, CanStartFlood)); }
        }

        private bool CanStartFlood(object parameter)
        {
            return !IsFloodEnabled;
        }

        private void OnStartFlood(object parameter)
        {
            IsFloodEnabled = true;
        }

        #endregion

        #region StopFloodCommand

        private DelegateCommand m_stopFloodCommand;

        public DelegateCommand StopFloodCommand
        {
            get { return m_stopFloodCommand ?? (m_stopFloodCommand = new DelegateCommand(OnStopFlood, CanStopFlood)); }
        }

        private bool CanStopFlood(object parameter)
        {
            return IsFloodEnabled;
        }

        private void OnStopFlood(object parameter)
        {
            IsFloodEnabled = false;
        }

        #endregion

        #region AddFloodEntryCommand

        private DelegateCommand m_addFloodEntryCommand;

        public DelegateCommand AddFloodEntryCommand
        {
            get { return m_addFloodEntryCommand ?? (m_addFloodEntryCommand = new DelegateCommand(OnAddFloodEntry, CanAddFloodEntry)); }
        }

        private bool CanAddFloodEntry(object parameter)
        {
            return !IsFloodEnabled;
        }

        private void OnAddFloodEntry(object parameter)
        {
            m_floodEntries.Add(new FloodEntry());
        }

        #endregion

        #region RemoveFloodEntryCommand

        private DelegateCommand m_removeFloodEntryCommand;

        public DelegateCommand RemoveFloodEntryCommand
        {
            get { return m_removeFloodEntryCommand ?? (m_removeFloodEntryCommand = new DelegateCommand(OnRemoveFloodEntry, CanRemoveFloodEntry)); }
        }

        private bool CanRemoveFloodEntry(object parameter)
        {
            return !IsFloodEnabled && SelectedEntry != null;
        }

        private void OnRemoveFloodEntry(object parameter)
        {
            m_floodEntries.Remove(SelectedEntry);
        }

        #endregion

        #region IViewModel<ChatView> Members

        object IViewModel.View
        {
            get { return View; }
            set { View = (ChatView) value; }
        }

        public ChatView View
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void StartFlood()
        {
            foreach (FloodEntry floodEntry in FloodEntries)
            {
                if (floodEntry.IsEnabled && floodEntry.UseTimer)
                {
                    FloodEntry entry = floodEntry;
                    floodEntry.LastSend = DateTime.Now;
                    Bot.CallDelayed(floodEntry.Timer*1000, () => OnTimerEnded(entry));
                }
            }
        }

        private void OnTimerEnded(FloodEntry entry)
        {
            if (!IsFloodEnabled)
                return;

            ExecuteEntry(entry, Bot.Character.Context.Actors.OfType<IPlayer>());

            if (IsFloodEnabled)
            {
                Bot.CallDelayed(entry.Timer*1000, () => OnTimerEnded(entry));
            }
        }

        [MessageHandler(typeof (GameRolePlayShowActorMessage))]
        public void HandleGameRolePlayShowActorMessage(Bot bot, GameRolePlayShowActorMessage message)
        {
            if (!IsFloodEnabled)
                return;

            var character = bot.Character.Context.GetContextActor(message.informations.contextualId) as IPlayer;

            if (character != null)
            {
                foreach (FloodEntry floodEntry in FloodEntries.Where(x => x.OnCharacterEnterMap))
                {
                    ExecuteEntry(floodEntry, new[] {character});
                }
            }
        }

        private void ExecuteEntry(FloodEntry entry, IEnumerable<IPlayer> receivers)
        {
            string text = entry.Text.Replace(MacroCounter, m_counter.ToString());

            foreach (FloodedChannel channel in entry.Channels.Where(x => x.IsEnabled))
            {
                if (channel.Channel != ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE)
                    Bot.Character.Say(text, channel.Channel);
                else
                {
                    foreach (Character character in receivers)
                    {
                        Bot.Character.SayTo(text.Replace(MacroCharacter, character.Name), character.Name);
                    }
                }

                m_counter++;
            }

            entry.LastSend = DateTime.Now;
        }

        #region Chat Handlers

        [MessageHandler(typeof (ChatMessageServer))]
        public void HandleChatMessageServer(Bot bot, ChatMessageServer message)
        {
            if (message.Channel == ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE)
            {
                AppendChatText(string.Format("[{0}] ({1}) {2} [{3}] : {4}\n", DateTime.Now.ToString("hh:mm"),
                    GetChannelName(message.Channel), m_wordFrom, message.ReceiverName, message.Content), GetChannelColor(message.Channel));
            }
            else
            {
                AppendChatText(string.Format("[{0}] ({1}) [{2}] : {3}\n", message.SentTime.ToString("hh:mm"),
                    GetChannelName(message.Channel), message.SenderName, message.Content), GetChannelColor(message.Channel));
            }
        }

        [MessageHandler(typeof (ChatMessageClient))]
        public void HandleChatMessageClient(Bot bot, ChatMessageClient message)
        {
            if (message.Channel == ChatActivableChannelsEnum.PSEUDO_CHANNEL_PRIVATE)
            {
                AppendChatText(string.Format("[{0}] ({1}) {2} [{3}] : {4}\n", DateTime.Now.ToString("hh:mm"),
                    GetChannelName(message.Channel), m_wordTo, message.ReceiverName, message.Content), GetChannelColor(message.Channel));
            }
        }

        [MessageHandler(typeof (TextInformationMessage))]
        public void HandleTextInformationMessage(Bot bot, TextInformationMessage message)
        {
            var data = DataProvider.Instance.Get<InfoMessage>(message.msgType * 10000 + message.msgId);
            var text = DataProvider.Instance.Get<string>(data.textId);

            // todo : params
            for (int i = 0; i < message.parameters.Length; i++)
            {
                var parameter = message.parameters[i];
                text = text.Replace("%" + (i + 1), parameter);
            }

            ChatActivableChannelsEnum channel;
            var type = (TextInformationTypeEnum)message.msgType;
            if (type == TextInformationTypeEnum.TEXT_INFORMATION_ERROR)
            {
                channel = (ChatActivableChannelsEnum)666;
            }
            else if (type == TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE)
            {
                channel = ChatActivableChannelsEnum.PSEUDO_CHANNEL_INFO;
            }
            else if (type == TextInformationTypeEnum.TEXT_INFORMATION_PVP)
            {
                channel = ChatActivableChannelsEnum.CHANNEL_ALIGN;
            }
            else if (type == TextInformationTypeEnum.TEXT_INFORMATION_FIGHT)
            {
                channel = ChatActivableChannelsEnum.PSEUDO_CHANNEL_FIGHT_LOG;
            }
            else
            {
                channel = ChatActivableChannelsEnum.PSEUDO_CHANNEL_INFO;
            }

            AppendChatText(string.Format("[{0}] {1}\n", DateTime.Now.ToString("hh:mm"), text), GetChannelColor(channel));
        }

        public void AppendChatText(string message, Color color)
        {
            View.Dispatcher.BeginInvoke(new Action(() => View.AppendText(message, color)));
        }

        private string GetChannelName(ChatActivableChannelsEnum channel)
        {
            if (!m_channelsNames.ContainsKey(channel))
                return "?";

            return m_channelsNames[channel];
        }

        private Color GetChannelColor(ChatActivableChannelsEnum channel)
        {
            if (!m_colorsRules.ContainsKey(channel))
                return Colors.Black;

            return m_colorsRules[channel];
        }

        #endregion

        public override void OnAttached()
        {
            base.OnAttached();

            var settings = Bot.Settings.GetEntry<BasicPluginSettings>();

            m_floodEntries = new ObservableCollection<FloodEntry>(settings.FloodEntries);
            m_readOnlyFloodEntries = new ReadOnlyObservableCollection<FloodEntry>(m_floodEntries);

            BotViewModel viewModel = Bot.GetViewModel();
            LayoutDocument layout = viewModel.AddDocument(this, () => new ChatView());
            layout.Title = "Chat";
        }

        public override void OnDetached()
        {
            base.OnDetached();

            var settings = Bot.Settings.GetEntry<BasicPluginSettings>();

            settings.FloodEntries = FloodEntries.ToArray();

            BotViewModel viewModel = Bot.GetViewModel();
            viewModel.RemoveDocument(View);
        }

        private static Color ColorFromHtmlCode(string htmlColor)
        {
            var colorInt = int.Parse(htmlColor.Replace("#",""), NumberStyles.HexNumber);

            return Color.FromRgb((byte)(colorInt >> 16 & 255), (byte)(colorInt >> 8 & 255), (byte)(colorInt & 255));
        }
    }
}