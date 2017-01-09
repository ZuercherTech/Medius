using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Medius.Core.Controls;
using Medius.Core.Extensions;
using Medius.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(PageView), typeof(PageViewRenderer))]
namespace Medius.Droid.Controls
{
    public class PageViewRenderer : ViewRenderer<PageView, Android.Views.View>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<PageView> e)
        {
            base.OnElementChanged(e);
            ChangePage(e.NewElement?.Content);
        }

        private Page CurrentPage { get; set; }

        private bool NeedsLayout { get; set; }

        private void ChangePage(Page page)
        {
            //TODO handle current page
            if (page != null)
            {
                page.Parent = Element.GetParentPage();

                var renderer = Platform.GetRenderer(page);
                if (renderer == null)
                {
                    var newRenderer = Platform.CreateRenderer(page);
                    Platform.SetRenderer(page, newRenderer);
                    renderer = newRenderer;
                }

                NeedsLayout = true;
                SetNativeControl(renderer.ViewGroup);
                Invalidate();

                //TODO update the page
                CurrentPage = page;
            }
            else
            {
                //TODO - update the page
                CurrentPage = null;
            }

            if (CurrentPage == null)
            {
                var view = new Android.Views.View(Context);
                view.SetBackgroundColor(Element.BackgroundColor.ToAndroid());
                SetNativeControl(view);
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            if ((changed || NeedsLayout) && Control != null)
            {
                if (CurrentPage != null)
                {
                    CurrentPage.Layout(new Rectangle(0, 0, Element.Width, Element.Height));
                }

                var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
                var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

                Control.Measure(msw, msh);
                Control.Layout(0, 0, r, b);

                NeedsLayout = false;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == "Content")
            {
                ChangePage(Element?.Content);
            }
        }
    }
}