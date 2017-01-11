using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Medius.Example.Pages
{
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();

            ListView.ItemsSource = new List<object>
            {
                new
                {
                    FirstText = "Hello",
                    SecondText = "World"
                },
                new
                {
                    FirstText = "Foo",
                    SecondText = "Bar"
                }
            };
        }
    }
}
