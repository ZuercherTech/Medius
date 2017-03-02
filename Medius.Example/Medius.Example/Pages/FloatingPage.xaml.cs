using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medius.Core.Controls;
using Xamarin.Forms;

namespace Medius.Example.Pages
{
    public partial class FloatingPage : ContentPage
    {
        public FloatingPage()
        {
            InitializeComponent();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            PageView.Dismiss(this);
        }
    }
}
