using Xamarin.Forms;

namespace Medius.Core.Controls
{
	public class PageView : View
	{
		public static readonly BindableProperty ContentProperty = BindableProperty.Create(
			nameof(Content), typeof(Page), typeof(PageView));

		public Page Content
		{
			get { return (Page) GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}
	}
}
