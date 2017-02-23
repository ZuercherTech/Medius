using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Medius.Example.Pages
{
    public partial class Page2 : ContentPage
    {
        public Page2()
        {
            InitializeComponent();

            BindingContext = new
            {
                Text = "Page Two"
            };
        }
    }
}
