using System;
using Xamarin.Forms;

namespace Medius.Example
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void Page1Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = GetPage(1);
        }

        public void Page2Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = GetPage(2);
        }

        public void Page3Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = GetPage(3);
        }

        public void Page4Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = GetPage(4);
        }

        private ContentPage GetPage(int index)
        {
            return new ContentPage
            {
                Content = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        new Label
                        {
                            Text = $"Page {index}"
                        }
                    }
                }
            };
        }
    }
}
