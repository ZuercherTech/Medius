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
            PageView.Content = new Pages.Page1();
        }

        public void Page2Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = new Pages.Page2();
        }

        public void Page3Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = GetPage(3);
        }

        public void Page4Click(object sender, EventArgs eventArgs)
        {
            PageView.Content = GetPage(4);
        }

        private Page GetPage(int number)
        {
            return new ContentPage
            {
                Content = new Label
                {
                    Text = number.ToString()
                }
            };
        }
    }
}
