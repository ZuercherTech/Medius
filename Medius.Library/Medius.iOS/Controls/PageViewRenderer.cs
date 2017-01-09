using System.ComponentModel;
using Medius.Core.Controls;
using Medius.Core.Extensions;
using Medius.iOS.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(PageView), typeof(PageViewRenderer))]
namespace Medius.iOS.Controls
{
	public class PageViewRenderer : ViewRenderer<PageView, ViewControllerView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<PageView> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.ViewController = null;
			}

			if (e.NewElement != null)
			{
				var viewControllerView = new ViewControllerView(Bounds);
				SetNativeControl(viewControllerView);
			}
		}

		private void ChangePage(Page page)
		{
			if (page == null)
			{
				if (Control != null)
				{
					Control.ViewController = null;
				}
				return;
			}

			page.Parent = Element.GetParentPage();

			var parentPage = Element.GetParentPage();
			var parentPageRenderer = Platform.GetRenderer(parentPage);
			Control.ParentViewController = parentPageRenderer.ViewController;

			var pageRenderer = Platform.GetRenderer(page);
			Control.ViewController = pageRenderer?.ViewController ?? page.CreateViewController();
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
