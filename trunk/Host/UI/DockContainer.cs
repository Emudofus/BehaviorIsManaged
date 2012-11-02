using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using AvalonDock.Layout;
using BiM.Core.Collections;
using BiM.Host.UI.ViewModels;
using BiM.Host.UI.Views;

namespace BiM.Host.UI
{
    public abstract class DockContainer<T> : IViewModel<T>
        where T : IView
    {
        private Dictionary<object, Assembly> m_documentsAssembly = new Dictionary<object, Assembly>();

        public LayoutDocument AddDocument(IViewModel modelView, Func<IView> viewCreator)
        {
            LayoutDocument document = null;
            if (View.Dispatcher.CheckAccess())
            {
                document = AddDocumentInternal(modelView, viewCreator, Assembly.GetCallingAssembly());
            }
            else
            {
                var assembly = Assembly.GetCallingAssembly();
                View.Dispatcher.Invoke(new Func<LayoutDocument>(() => document = AddDocumentInternal(modelView, viewCreator, assembly)));
            }

            return document;
        }

        protected LayoutDocument AddDocumentInternal(IViewModel modelView, Func<IView> viewCreator, Assembly assembly)
        {
            IView view = viewCreator();
            var layout = new LayoutDocument()
            {
                Content = view
            };

            view.ViewModel = modelView; 
            modelView.View = view;

            if (modelView is IDocked)
            {
                ( modelView as IDocked ).Parent = layout;
            }

            if (view is IDocked)
            {
                ( view as IDocked ).Parent = layout;
            }

            DocumentPane.Children.Add(layout);

            if (!m_documentsAssembly.ContainsKey(view))
                m_documentsAssembly.Add(view, assembly);

            return layout;
        }

        public void RemoveDocumentsFrom(Assembly assembly)
        {
            var documentsToRemove = m_documentsAssembly.Where(x => x.Value == assembly).Select(x => x.Key).ToArray();

            foreach (var document in documentsToRemove)
            {
                RemoveDocument(document);
            }
        }

        public bool RemoveDocument(object document)
        {
            bool removed = false;
            lock (DocumentPane.Children)
            {
                foreach (var child in DocumentPane.Children.ToArray())
                {
                    if (child.Content == document)
                        removed = DocumentPane.Children.Remove(child);
                }
            }

            if (removed)
            {
                m_documentsAssembly.Remove(document);
            }

            return removed;
        }


        protected abstract LayoutDocumentPane DocumentPane
        {
            get;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        object IViewModel.View
        {
            get { return View; }
            set { View = (T)value; }
        }

        public T View
        {
            get;
            set;
        }
    }
}