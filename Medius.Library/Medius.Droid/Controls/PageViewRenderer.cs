using System.ComponentModel;
using Android.Views;
using Medius.Core.Controls;
using Medius.Core.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly:ExportRenderer(typeof(PageView), typeof(Medius.Droid.Controls.PageViewRenderer))]
namespace Medius.Droid.Controls
{
    public class PageViewRenderer : ViewRenderer<PageView, Android.Views.View>
    {
        protected override void OnElementChanged(ElementChangedEventArgs<PageView> e)
        {
            base.OnElementChanged(e);
            ChangePage(e.NewElement?.Content);
        }

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

            var renderer = Platform.GetRenderer(page);
            if (renderer == null)
            {
                var newRenderer = Platform.CreateRenderer(page);
                Platform.SetRenderer(page, newRenderer);
                renderer = newRenderer;
            }
        }

        private void RemovePageRenderer()
        {
            SetNativeControl(new Android.Views.View(Context));
            (PageRenderer.ViewGroup.Parent as ViewGroup)?.RemoveView(PageRenderer.ViewGroup);

            NeedsLayout = true;
            Invalidate();
        }

        private void AddPageRenderer()
        {
            SetNativeControl(PageRenderer.ViewGroup);

            NeedsLayout = true;
            Invalidate();
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);
            if (changed || NeedsLayout)
            {
                var page = Element?.Content;
                page?.Layout(new Rectangle(0, 0, Element.Width, Element.Height));

                if (Control != null)
                {
                    var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
                    var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

                    Control.Measure(msw, msh);
                    Control.Layout(0, 0, r, b);
                }

                NeedsLayout = false;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(Element.Content))
            {
                ChangePage(Element?.Content);
            }
        }
    }
}