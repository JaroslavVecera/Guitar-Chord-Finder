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
    public partial class FingeringGroupView : ContentView
    {
        public static readonly BindableProperty AnySubsetsProperty = BindableProperty.Create(
            nameof(AnySubsets), typeof(bool), typeof(FingeringGroupView));

        public bool AnySubsets
        {
            get { return (bool)GetValue(AnySubsetsProperty); }
            set { SetValue(AnySubsetsProperty, value); }
        }

        public static readonly BindableProperty FullChordProperty = BindableProperty.Create(
            nameof(FullChord), typeof(Fingering), typeof(FingeringGroupView), propertyChanged: FullChordPropertyChanged);

        public Fingering FullChord { get; set; }

        public static readonly BindableProperty SubsetsProperty = BindableProperty.Create(
            nameof(Subsets), typeof(List<Fingering>), typeof(FingeringGroupView));

        public List<Fingering> Subsets
        { 
            get { return (List <Fingering>) GetValue(SubsetsProperty); }
            set { SetValue(SubsetsProperty, value); }
        }

        public FingeringGroupView()
        {
            InitializeComponent();
        }

        private static void FullChordPropertyChanged(BindableObject o, object oldO, object newO)
        {
            if (newO == null)
                return;
            var c = (FingeringGroupView)o;
            c.DrawRepresentant((Fingering)newO);
            c.SetSubsetsButtonVisibility();
        }

        void DrawRepresentant(Fingering f)
        {
            representant.Xs = f.Xs;
            representant.FullCircles = f.FullCircles;
            representant.EmptyCircles = f.EmptyCircles;
            representant.Barres = f.Barres;
            representant.Position = f.Position;
            representant.ForceRedraw();
        }

        void SetSubsetsButtonVisibility()
        {
            subsetsButton.IsVisible = Subsets.Count > 1;
        }

        private void OnShowSubsets(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SubsetsPage(Subsets));
        }
    }
}