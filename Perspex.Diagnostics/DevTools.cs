﻿// -----------------------------------------------------------------------
// <copyright file="DevTools.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Diagnostics
{
    using Perspex.Controls;
    using System.Reactive.Linq;
    using Perspex.Diagnostics.ViewModels;

    public class DevTools : Decorator
    {
        public static readonly PerspexProperty<Control> RootProperty =
            PerspexProperty.Register<DevTools, Control>("Root");

        public DevTools()
        {
            this.Content = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions
                {
                    new ColumnDefinition(1, GridUnitType.Star),
                    new ColumnDefinition(3, GridUnitType.Star),
                },
                Children = new Controls
                {
                    new TreeView
                    {
                        DataTemplates = new DataTemplates
                        {
                            new TreeDataTemplate<VisualTreeNode>(GetHeader, x => x.Children),
                        },
                        [!TreeView.ItemsProperty] = this[!DevTools.RootProperty].Select(x =>
                        {
                            if (x != null)
                            {
                                return new[] { new VisualTreeNode((IVisual)x) };
                            }
                            else
                            {
                                return null;
                            }
                        }),
                    }
                }
            };
        }

        public Control Root
        {
            get { return this.GetValue(RootProperty); }
            set { this.SetValue(RootProperty, value); }
        }

        private static Control GetHeader(VisualTreeNode node)
        {
            TextBlock result = new TextBlock();
            result.Text = node.Type;

            //if (control != null && control.TemplatedParent != null)
            //{
            //    result.FontStyle = Media.FontStyle.Italic;
            //}

            return result;
        }
    }
}