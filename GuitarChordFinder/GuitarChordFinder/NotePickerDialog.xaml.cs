using Rg.Plugins.Popup.Extensions;
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
    public partial class NotePickerDialog : Rg.Plugins.Popup.Pages.PopupPage
    {
        private readonly Action<Tone> setResultAction;
        Tone Tone { get; } = new Tone();

        public void OnCancelClicked(object sender, EventArgs e)
        {
            setResultAction?.Invoke(null);
            this.Navigation.PopPopupAsync().ConfigureAwait(false);
        }

        public void OnConfirmClicked(object sender, EventArgs e)
        {
            setResultAction?.Invoke(Tone);
            this.Navigation.PopPopupAsync().ConfigureAwait(false);
        }

        public NotePickerDialog(Action<Tone> setResultAction, Tone oldValue)
        {
            InitializeComponent();
            this.setResultAction = setResultAction;
            namePicker.StepValueChanged += NameChanged;
            octavePicker.StepValueChanged += OctaveChanged;
            InitializePickers(oldValue.Name, oldValue.Octave);
        }

        void InitializePickers(int name, int octave)
        { 
            NameChanged(name);
            OctaveChanged(octave);
            namePicker.Value = name;
            octavePicker.Value = octave;
        }

        private void OctaveChanged(int obj)
        {
            Tone.Octave = obj;
            octave.Text = Tone.OctaveToString();
        }

        private void NameChanged(int obj)
        {
            Tone.Name = obj;
            name.Text = Tone.NameToString();
        }

        public static async Task<Tone> Create(INavigation navigation, Tone oldValue)
        {
            TaskCompletionSource<Tone> completionSource = new TaskCompletionSource<Tone>();

            void callback(Tone tone)
            {
                completionSource.TrySetResult(tone);
            }

            var popup = new NotePickerDialog(callback, oldValue);

            await navigation.PushPopupAsync(popup);

            return await completionSource.Task;
        }
    }
}