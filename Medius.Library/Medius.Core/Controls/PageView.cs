using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Medius.Core.Controls
{
    public enum PanelAttachPoint
    {
        Left,
        Top,
        Right,
        Bottom
    }

	public class PageView : View
	{

        public static readonly BindableProperty ContentProperty = BindableProperty.Create(
            nameof(Content), typeof(Page), typeof(PageView), null);
        public static readonly BindableProperty IsFloatingPanelProperty = BindableProperty.Create(
            nameof(IsFloating), typeof(bool), typeof(PageView), false);
        public static readonly BindableProperty AttachToProperty = BindableProperty.Create(
            nameof(AttachTo), typeof(PanelAttachPoint), typeof(PageView), PanelAttachPoint.Left);
        public static readonly BindableProperty ShownProperty = BindableProperty.Create(
            nameof(Shown), typeof(bool), typeof(PageView), false);

        public Page Content
        {
            get { return (Page)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public bool IsFloating
        {
            get { return (bool)GetValue(IsFloatingPanelProperty); }
            set { SetValue(IsFloatingPanelProperty, value); }
        }

        public PanelAttachPoint AttachTo
        {
            get { return (PanelAttachPoint)GetValue(AttachToProperty); }
            set { SetValue(AttachToProperty, value); }
        }

        public bool Shown
        {
            get { return (bool)GetValue(ShownProperty); }
            set { SetValue(ShownProperty, value); }
        }

	    public static IList<PageView> PageViews = new List<PageView>();

	    public PageView()
	    {
	        PageViews.Add(this);
	    }

	    public static void Dismiss(Page page)
	    {
	        PageViews.FirstOrDefault(p => p.Content == page).Shown = false;
	    }
    }
}
