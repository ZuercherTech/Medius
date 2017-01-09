using Xamarin.Forms;

namespace Medius.Core.Extensions
{
	public static class VisualElementExtensions
	{
		public static Page GetParentPage(this VisualElement element)
		{
			if (element == null) return null;

			var parent = element.Parent;
			while (parent != null)
			{
				if (parent is Page)
				{
					break;
				}
				parent = parent.Parent;
			}

			return parent as Page;
		}
	}
}
