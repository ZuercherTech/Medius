using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            PageView.Content = GetPage(1, Color.Blue);
        }

        public void Page2Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = GetPage(2, Color.Red);
        }

        public void Page3Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = GetPage(3, Color.Green);
        }

        public void Page4Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = GetPage(4, Color.Lime);
        }

        private ContentPage GetPage(int index, Color color)
        {
            return new ContentPage
            {
                Content = new StackLayout
                {
                    BackgroundColor = color,
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
