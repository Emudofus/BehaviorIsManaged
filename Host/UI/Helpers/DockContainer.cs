#region License GNU GPL
// DockContainer.cs
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
using System.Linq;
using System.Reflection;
using AvalonDock.Layout;
using BiM.Host.UI.ViewModels;
using BiM.Host.UI.Views;

namespace BiM.Host.UI.Helpers
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
                    {
                        if (View.Dispatcher.CheckAccess())
                        {
                            removed = DocumentPane.Children.Remove(child);
                        }
                        else
                        {
                            View.Dispatcher.Invoke(new Func<bool>(() => removed = DocumentPane.Children.Remove(child)));
                        }
                    }
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
        protected void FirePropertyChanged(string propertyName)
        {
          if (PropertyChanged != null)
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }


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