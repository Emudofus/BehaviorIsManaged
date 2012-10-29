using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using AvalonDock.Layout;
using BiM.Core.Collections;
using BiM.Core.Messages;
using BiM.Protocol.Messages;
using Bot = BiM.Behaviors.Bot;

namespace BiM.Host.UI.ViewModels
{
    public class BotViewModel : PaneViewModel, IDisposable
    {
        private readonly ObservableCollectionMT<object> m_documents = new ObservableCollectionMT<object>();
        private readonly Dictionary<object, Assembly> m_documentsAssembly = new Dictionary<object, Assembly>();
        private readonly ReadOnlyObservableCollectionMT<object> m_readOnlyDocuments;

        public BotViewModel(Bot bot)
        {
            m_readOnlyDocuments = new ReadOnlyObservableCollectionMT<object>(m_documents);
            m_documents.CollectionChanged += OnDocumentsChanged;
            DocumentTemplateSelector = new DocumentTemplateSelector();
            DocumentStyleSelector = new DocumentStyleSelector();

            Bot = bot;
            Title = bot.ClientInformations.Login;
            bot.Dispatcher.RegisterNonShared(this);
        }

        public Behaviors.Bot Bot
        {
            get;
            private set;
        }

        #region Documents
        public ReadOnlyObservableCollection<object> Documents
        {
            get { return m_readOnlyDocuments; }
        }

        public DocumentTemplateSelector DocumentTemplateSelector
        {
            get;
            private set;
        }

        public DocumentStyleSelector DocumentStyleSelector
        {
            get;
            private set;
        }

        private void OnDocumentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (object item in e.OldItems)
                {
                    m_documentsAssembly.Remove(item);
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                m_documentsAssembly.Clear();
            }
        }

        public void AddDocument(object document)
        {
            m_documents.Add(document);

            if (!m_documentsAssembly.ContainsKey(document))
                m_documentsAssembly.Add(document, Assembly.GetCallingAssembly());
        }

        public void AddDocument(LayoutDocument document)
        {
            m_documents.Add(document);

            if (!m_documentsAssembly.ContainsKey(document))
                m_documentsAssembly.Add(document, Assembly.GetCallingAssembly());
        }

        public LayoutDocument AddDocument(object document, string title)
        {
            var layout = new LayoutDocument
                             {
                                 Content = document,
                                 Title = title,
                             };

            m_documents.Add(layout);

            if (!m_documentsAssembly.ContainsKey(document))
                m_documentsAssembly.Add(document, Assembly.GetCallingAssembly());

            return layout;
        }

        public LayoutDocument AddDocument(object document, string title, ImageSource icon)
        {
            var layout = new LayoutDocument
                             {
                                 Content = document,
                                 Title = title,
                                 IconSource = icon
                             };

            m_documents.Add(layout);

            if (!m_documentsAssembly.ContainsKey(document))
                m_documentsAssembly.Add(document, Assembly.GetCallingAssembly());

            return layout;
        }

        public void AddDocument(object document, DataTemplate template)
        {
            if (!DocumentTemplateSelector.HasTemplate(document))
            {
                DocumentTemplateSelector.AddTemplate(document, template);
            }

            m_documents.Add(document);

            if (!m_documentsAssembly.ContainsKey(document))
                m_documentsAssembly.Add(document, Assembly.GetCallingAssembly());
        }

        public void AddDocument(object document, Style style)
        {
            if (!DocumentStyleSelector.HasStyle(document))
            {
                DocumentStyleSelector.AddStyle(document, style);
            }

            m_documents.Add(document);

            if (!m_documentsAssembly.ContainsKey(document))
                m_documentsAssembly.Add(document, Assembly.GetCallingAssembly());
        }

        public void AddDocument(object document, Style style, DataTemplate template)
        {
            if (!DocumentStyleSelector.HasStyle(document))
            {
                DocumentStyleSelector.AddStyle(document, style);
            }

            if (!DocumentTemplateSelector.HasTemplate(document))
            {
                DocumentTemplateSelector.AddTemplate(document, template);
            }

            m_documents.Add(document);

            if (!m_documentsAssembly.ContainsKey(document))
                m_documentsAssembly.Add(document, Assembly.GetCallingAssembly());
        }

        public void RemoveDocumentsFrom(Assembly assembly)
        {
            object[] documentsToRemove = m_documentsAssembly.Where(x => x.Value == assembly).Select(x => x.Key).ToArray();

            foreach (object document in documentsToRemove)
            {
                RemoveDocument(document);
            }
        }

        public bool RemoveDocument(object document)
        {
            bool removed = m_documents.Remove(document);

            if (removed)
            {
                DocumentStyleSelector.RemoveStyle(document);
                DocumentTemplateSelector.RemoveTemplate(document);
            }

            return removed;
        }
        #endregion

        #region Handlers

        [MessageHandler(typeof (CharacterSelectedSuccessMessage))]
        public void HandleCharacterSelectedSuccessMessage(Bot bot, CharacterSelectedSuccessMessage message)
        {
            Title = bot.Character.Name;
        }

        #endregion

        public void Dispose()
        {
            if (Bot != null)
                Bot.Dispatcher.UnRegisterNonShared(this);

            UIManager.Instance.RemoveDocument(this);
        }
    }
}