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
    public partial class StringTuningView : ContentView
    {
        Tone _tone;
        public Tone Tone { get { return _tone; } set { _tone = value; label.Text = value.ToString(); } }

        public bool CanRemove { get { return removeButton.IsEnabled; } set { removeButton.IsEnabled = value; } }

        public event Action<StringTuningView> RemoveClicked;

        public StringTuningView(Tone initial)
        {
            InitializeComponent();
            Tone t = new Tone() { Name = initial.Name, Octave = initial.Octave };
            Tone = t;
        }

        private async void OnChangeClicked(object sender, EventArgs e)
        {
            var newTone = await NotePickerDialog.Create(Navigation, Tone);
            if (newTone != null)
                Tone = newTone;
        }

        private void OnRemoveClicked(object sender, EventArgs e)
        {
            RemoveClicked?.Invoke(this);
        }
    }
}