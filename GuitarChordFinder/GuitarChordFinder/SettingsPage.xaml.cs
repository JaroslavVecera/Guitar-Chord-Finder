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
    public partial class SettingsPage : TabbedPage
    {
        int MinStrings { get { return 3; } }
        int MaxStrings { get { return 8; } }

        FingeringOptions Options { get; set; }
        List<StringTuningView> Strings { get; } = new List<StringTuningView>();

        public SettingsPage(FingeringOptions options)
        {
            InitializeComponent();
            BindingContext = options;
            Options = options;

            InitializeTuningTab();
            InitializeOtherTab();
        }

        void InitializeTuningTab()
        {
            foreach (Tone t in CurrentTuning(Options.Tuning))
                AddRow(t);
        }

        void InitializeOtherTab()
        {
            maxFretPicker.StepValueChanged += MaxFretChanged;
            requiredFingersPicker.StepValueChanged += RequiredFingersChanged;
            fretRangePicker.StepValueChanged += FretRangeChanged;
            InitializePickers();
        }

        void MaxFretChanged(int obj)
        {
            Options.MaxFret = obj;
            maxFret.Text = obj.ToString();
        }

        void RequiredFingersChanged(int obj)
        {
            Options.RequiredFingers = obj;
            requiredFingers.Text = obj.ToString();
        }

        void FretRangeChanged(int obj)
        {
            Options.FretRange = obj;
            fretRange.Text = obj.ToString();
        }

        void InitializePickers()
        {
            MaxFretChanged(Options.MaxFret);
            FretRangeChanged(Options.FretRange);
            RequiredFingersChanged(Options.RequiredFingers);

            maxFretPicker.Value = Options.MaxFret;
            fretRangePicker.Value = Options.FretRange;
            requiredFingersPicker.Value = Options.RequiredFingers;
        }

        List<Tone> CurrentTuning(int[] tuning)
        {
            return tuning.ToList().Select(i => new Tone(i)).ToList();
        }

        protected override void OnDisappearing()
        {
            Options.Tuning = GetTuning();
            Options.NotifyChange();
        }

        int[] GetTuning()
        {
            int[] tuning = new int[Strings.Count];
            for (int i = 0; i < Strings.Count; i++)
                tuning[i] = Strings[i].Tone.ToIndex();
            return tuning;
        }

        private void OnPlusClicked(object sender, EventArgs e)
        {
            if (Strings.Count < MaxStrings)
                AddRow(new Tone() { Name = 0, Octave = 0 });
            if (Strings.Count == MaxStrings)
                addButton.IsEnabled = false;
        }

        void AddRow(Tone tone)
        { 
            var str = new StringTuningView(tone);
            str.RemoveClicked += OnRemoveClicked;
            Strings.Add(str);
            rows.Children.Add(str);
            Strings.ForEach(s => s.CanRemove = true);
        }

        private void OnRemoveClicked(StringTuningView obj)
        {
            if (Strings.Count == MinStrings)
                return;
            obj.RemoveClicked -= OnRemoveClicked;
            Strings.Remove(obj);
            rows.Children.Remove(obj);
            addButton.IsEnabled = true;
            if (Strings.Count == MinStrings)
                Strings.ForEach(s => s.CanRemove = false);
        }
    }
}