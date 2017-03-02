using System;
using System.ComponentModel;
using CoreGraphics;
using Medius.Core.Controls;
using Medius.Core.Extensions;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(PageView), typeof(Medius.iOS.Controls.PageViewRenderer))]
namespace Medius.iOS.Controls
{
	public class PageViewRenderer : ViewRenderer<PageView, UIView>
	{
		private PageRenderer ParentPageRenderer { get; set; }

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
			ParentPageRenderer = Platform.GetRenderer(parentPage) as PageRenderer;

			if (Platform.GetRenderer(page) == null)
			{
				Platform.SetRenderer(page, Platform.CreateRenderer(page));
			}

			PageRenderer = Platform.GetRenderer(page) as PageRenderer;
		}

		private void RemovePageRenderer()
		{
			PageRenderer.WillMoveToParentViewController(null);

            PageRenderer.RemoveFromParentViewController();
            PageRenderer.View.RemoveFromSuperview();
		}

		private void AddPageRenderer()
        {
            UIViewController parentViewController = null;
		    UIView parentView = null;
		    if (!Element.IsFloating)
		    {
                PageRenderer.View.Frame = new CGRect(0, 0, Bounds.Width, Bounds.Height);

                parentViewController = ParentPageRenderer;
		        parentView = this;
		    }
		    else
		    {
                PageRenderer.View.Frame = GetViewDimensions(false);

                parentViewController = UIApplication.SharedApplication.KeyWindow.RootViewController;
		        parentView = UIApplication.SharedApplication.KeyWindow;
		    }

            PageRenderer.SetElementSize(new Size(Bounds.Width, Bounds.Height));

            parentView.AddSubview(PageRenderer.View);
            parentViewController.AddChildViewController(PageRenderer);
			PageRenderer.DidMoveToParentViewController(parentViewController);

            PageRenderer.View.Hidden = false;
            Element.Content.Layout(PageRenderer.View.Frame.ToRectangle());
        }

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			var page = Element?.Content;
			page?.Layout(new Rectangle(0, 0, Bounds.Width, Bounds.Height));
		}

	    private CGRect GetViewDimensions(bool show)
	    {
	        var mainDimensions = UIScreen.MainScreen.Bounds;
            var dimensions = UIScreen.MainScreen.Bounds;
            if (Element.WidthRequest > 0)
            {
                dimensions.Width = (nfloat)Element.WidthRequest;
            }
            if (Element.HeightRequest > 0)
            {
                dimensions.Height = (nfloat)Element.HeightRequest;
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
	        if (PageRenderer?.View == null) return;
            
            UIView.Animate(0.25, 0, UIViewAnimationOptions.CurveEaseOut,
                () =>
                {
                    PageRenderer.View.Frame = GetViewDimensions(shouldShow);
                }, null);
	    }

	    protected override void OnElementChanged(ElementChangedEventArgs<PageView> e)
	    {
	        base.OnElementChanged(e);

            ChangePage(Element.Content);
            SetShown(Element.Shown);
	    }

	    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == nameof(Element.Content))
			{
				ChangePage(Element?.Content);
			}
            else if (e.PropertyName == nameof(Element.Shown))
			{
			    SetShown(Element.Shown);
			}
		}
	}
}
