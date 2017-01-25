using System.ComponentModel;
using CoreGraphics;
using Medius.Core.Controls;
using Medius.Core.Extensions;
using Medius.iOS.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(PageView), typeof(PageViewRenderer))]
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
				if (Element != null)
				{
					SetNativeControl(new UIView());
				}
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
			SetNativeControl(new UIView());
			PageRenderer.RemoveFromParentViewController();
		}

		private void AddPageRenderer()
		{
			SetNativeControl(PageRenderer.NativeView);

			ParentPageRenderer.AddChildViewController(PageRenderer.ViewController);

			PageRenderer.NativeView.Frame = new CGRect(0, 0, Bounds.Width, Bounds.Height);
			PageRenderer.SetElementSize(new Size(Bounds.Width, Bounds.Height));

			PageRenderer.DidMoveToParentViewController(ParentPageRenderer);
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			var page = Element?.Content;
			page?.Layout(new Rectangle(0, 0, Bounds.Width, Bounds.Height));
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == "Content" || e.PropertyName == "Renderer")
			{
				Device.BeginInvokeOnMainThread(() => ChangePage(Element?.Content));
			}
		}
	}
}
