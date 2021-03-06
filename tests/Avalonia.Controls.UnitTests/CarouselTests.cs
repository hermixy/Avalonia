// Copyright (c) The Avalonia Project. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Linq;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using Xunit;

namespace Avalonia.Controls.UnitTests
{
    public class CarouselTests
    {
        [Fact]
        public void First_Item_Should_Be_Selected_By_Default()
        {
            var target = new Carousel
            {
                Template = new FuncControlTemplate<Carousel>(CreateTemplate),
                Items = new[]
                {
                    "Foo",
                    "Bar"
                }
            };

            target.ApplyTemplate();

            Assert.Equal(0, target.SelectedIndex);
            Assert.Equal("Foo", target.SelectedItem);
        }

        [Fact]
        public void LogicalChild_Should_Be_Selected_Item()
        {
            var target = new Carousel
            {
                Template = new FuncControlTemplate<Carousel>(CreateTemplate),
                Items = new[]
                {
                    "Foo",
                    "Bar"
                }
            };

            target.ApplyTemplate();
            target.Presenter.ApplyTemplate();

            Assert.Equal(1, target.GetLogicalChildren().Count());

            var child = target.GetLogicalChildren().Single();
            Assert.IsType<ContentPresenter>(child);
            Assert.Equal("Foo", ((ContentPresenter)child).Content);
        }

        [Fact]
        public void Should_Remove_NonCurrent_Page_When_IsVirtualized_True()
        {
            var target = new Carousel
            {
                Template = new FuncControlTemplate<Carousel>(CreateTemplate),
                Items = new[] { "foo", "bar" },
                IsVirtualized = true,
                SelectedIndex = 0,
            };

            target.ApplyTemplate();
            target.Presenter.ApplyTemplate();

            Assert.Equal(1, target.ItemContainerGenerator.Containers.Count());
            target.SelectedIndex = 1;
            Assert.Equal(1, target.ItemContainerGenerator.Containers.Count());
        }

        [Fact]
        public void Should_Not_Remove_NonCurrent_Page_When_IsVirtualized_False()
        {
            var target = new Carousel
            {
                Template = new FuncControlTemplate<Carousel>(CreateTemplate),
                Items = new[] { "foo", "bar" },
                IsVirtualized = false,
                SelectedIndex = 0,
            };

            target.ApplyTemplate();
            target.Presenter.ApplyTemplate();

            Assert.Equal(1, target.ItemContainerGenerator.Containers.Count());
            target.SelectedIndex = 1;
            Assert.Equal(2, target.ItemContainerGenerator.Containers.Count());
        }

        private Control CreateTemplate(Carousel control)
        {
            return new CarouselPresenter
            {
                Name = "PART_ItemsPresenter",
                [~CarouselPresenter.IsVirtualizedProperty] = control[~Carousel.IsVirtualizedProperty],
                [~CarouselPresenter.ItemsProperty] = control[~Carousel.ItemsProperty],
                [~CarouselPresenter.ItemsPanelProperty] = control[~Carousel.ItemsPanelProperty],
                [~CarouselPresenter.SelectedIndexProperty] = control[~Carousel.SelectedIndexProperty],
                [~CarouselPresenter.TransitionProperty] = control[~Carousel.TransitionProperty],
            };
        }
    }
}
