using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GuitarChordFinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubsetsPage : ContentPage
    {
        public SubsetsPage(List<Fingering> subsets)
        {
            InitializeComponent();
            carousel.ItemsSource = subsets;
        }
    }
}