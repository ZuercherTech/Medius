using System;
using System.ComponentModel;
using System.Linq;
using Android.App;
using Android.Views;
using Android.Views.Animations;
using Medius.Core.Controls;
using Medius.Core.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(PageView), typeof(Medius.Droid.Controls.PageViewRenderer))]
namespace Medius.Droid.Controls
{
    public class PageViewRenderer : ViewRenderer<PageView, Android.Views.View>
    {
        private bool NeedsLayout { get; set; }

        private PageRenderer _pageRenderer;
        private PageRenderer PageRenderer
        {
            get { return _pageRenderer; }
            set
            {
                if (_pageRenderer != null)
                {
                    RemovePageRenderer();
                }

                _pageRenderer = value;

                if (_pageRenderer != null)
                {
                    AddPageRenderer();
                }
            }
        }

        private void ChangePage(Page page)
        {
            if (page == null)
            {
                PageRenderer = null;
                return;
            }

            var parentPage = Element.GetParentPage();
            page.Parent = parentPage;

            if (Platform.GetRenderer(page) == null)
            {
                Platform.SetRenderer(page, Platform.CreateRenderer(page));
            }

            PageRenderer = Platform.GetRenderer(page) as PageRenderer;
        }

        private void RemovePageRenderer()
        {
            PageRenderer.RemoveFromParent();

            NeedsLayout = true;
            Invalidate();
        }

        private void AddPageRenderer()
        {
            ViewGroup parentView = null;
            if (!Element.IsFloating)
            {
                PageRenderer.ViewGroup.Layout(0, 0, Width, Height);

                parentView = this;
            }
            else
            {
                PageRenderer.ViewGroup.Visibility = ViewStates.Visible;

                var dimensions = GetViewDimensions(false);
                PageRenderer.ViewGroup.LayoutParameters = new ViewGroup.LayoutParams((int) dimensions.Width,
                    (int) dimensions.Height);
                PageRenderer.ViewGroup.TranslationX = (int) dimensions.X;
                PageRenderer.ViewGroup.TranslationY = (int) dimensions.Y;
                
                parentView = (Context as Activity).Window.DecorView as ViewGroup;
                
                var metrics = this.Resources.DisplayMetrics;
                Element.Content.Layout(new Rectangle(0, 0, 
                    dimensions.Width / metrics.Density, dimensions.Height / metrics.Density));
            }

            parentView.AddView(PageRenderer);
            
            NeedsLayout = true;
            Invalidate();
        }

        private Rectangle GetViewDimensions(bool show)
        {
            var metrics = this.Resources.DisplayMetrics;
            var mainDimensions = new Rectangle(0, 0,
                metrics.WidthPixels, metrics.HeightPixels);

            var dimensions = mainDimensions;
            if (Element.WidthRequest > 0)
            {
                dimensions.Width = Element.WidthRequest * metrics.Density;
            }
            if (Element.HeightRequest > 0)
            {
                dimensions.Height = Element.HeightRequest * metrics.Density;
            }

            switch (Element.AttachTo)
            {
                case PanelAttachPoint.Left:
                    dimensions.X = (show) ? 0 : -dimensions.Width;
                    break;
                case PanelAttachPoint.Top:
                    dimensions.Y = (show) ? 0 : -dimensions.Height;
                    break;
                case PanelAttachPoint.Right:
                    dimensions.X = (show) ? mainDimensions.Width - dimensions.Width : mainDimensions.Width;
                    break;
                case PanelAttachPoint.Bottom:
                    dimensions.Y = (show) ? mainDimensions.Height - dimensions.Height : mainDimensions.Height;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return dimensions;
        }

        private void SetShown(bool shouldShow)
        {
            if (PageRenderer?.ViewGroup == null) return;

            var dimensions = GetViewDimensions(shouldShow);
            PageRenderer.ViewGroup.Animate()
                .SetDuration(250)
                .SetInterpolator(new DecelerateInterpolator())
                .TranslationX((float) dimensions.X)
                .TranslationY((float) dimensions.Y)
                .Start();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            if (changed || NeedsLayout)
            {
                var page = Element?.Content;
                page?.Layout(new Rectangle(0, 0, Element.Width, Element.Height));

                if (PageRenderer?.ViewGroup != null)
                {
                    var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
                    var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

                    PageRenderer.Measure(msw, msh);
                    PageRenderer.Layout(0, 0, r, b);
                }

                NeedsLayout = false;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(Element.Content) || e.PropertyName == "Renderer")
            {
                Device.BeginInvokeOnMainThread(() => ChangePage(Element?.Content));
            }
            else if (e.PropertyName == nameof(Element.Shown))
            {
                Device.BeginInvokeOnMainThread(() => SetShown(Element.Shown));
            }
        }
    }
}