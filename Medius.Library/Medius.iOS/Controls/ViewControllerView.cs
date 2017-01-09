using System;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Medius.iOS.Controls
{
	public class ViewControllerView : UIView
	{
		public ViewControllerView(CGRect frame) : base(frame)
		{
			BackgroundColor = Color.Transparent.ToUIColor();
		}

		public UIViewController ParentViewController { get; set; }

		private UIViewController _viewController;

		public UIViewController ViewController
		{
			get { return _viewController; }
			set
			{
				if (_viewController != null)
				{
					RemoveViewController();
				}

				_viewController = value;
				if (_viewController != null)
				{
					AddViewController();
				}
			}
		}

		private void AddViewController()
		{
			if (ParentViewController == null)
			{
				throw new Exception("No parent view controller was found.");
			}

			ParentViewController.AddChildViewController(_viewController);
			AddSubview(_viewController.View);

			_viewController.View.Frame = Bounds;
			_viewController.DidMoveToParentViewController(ParentViewController);
		}

		private void RemoveViewController()
		{
			if (ViewController == null) return;

			ViewController.WillMoveToParentViewController(null);
			ViewController.View.RemoveFromSuperview();
			ViewController.RemoveFromParentViewController();
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			// Fix sizing of children when orientation changes
			if (ViewController != null && ViewController.View.Subviews.Length > 0)
			{
				foreach (UIView view in ViewController.View.Subviews)
				{
					view.Frame = Bounds;
				}
			}
		}
	}
}
