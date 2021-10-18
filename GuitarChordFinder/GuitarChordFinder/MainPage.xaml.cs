using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GuitarChordFinder
{
    public partial class MainPage : ContentPage
    {
        bool IsLandscape { get; set; } = false;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnSettingsClicked(object sender, EventArgs e)
        {
            var task = Navigation.PushAsync(new SettingsPage(((MainViewModel)BindingContext).Options));
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width > height && !IsLandscape)
            {
                IsLandscape = true;
                ((MainViewModel)BindingContext).Refresh();
            }
            else if (width <= height && IsLandscape)
            {
                IsLandscape = false;
                ((MainViewModel)BindingContext).Refresh();
            }
        }
    }
}
