using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace GuitarChordFinder
{
    public class MainViewModel : INotifyPropertyChanged
    {

        public List<FingeringGroup> FingeringGroups { get; set; }
        public FingeringOptions Options { get; private set; } = new FingeringOptions() { FretRange = 4, MaxFret = 20, RequiredFingers = 4 };
        Fret Fret { get; set; }
        public string Search { get; set; } = "";
        public string LastSearch { get; set; } = "";
        public RelayCommand Command { get; set; }
        public bool ErrorLabelVisibility { get; set; }
        public bool LabelVisibility { get; set; }
        public string Label { get; set; }
        public string ErrorLabel { get; set; }

        public MainViewModel()
        {
            Command = new RelayCommand(() => { CountFingerings(Search); LastSearch = Search; });
            Options.OnOptionsChanged += () => CountFingerings(LastSearch);
            Fret = new Fret(Options);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void CountFingerings(string name)
        {
            FingeringGroups = new List<FingeringGroup>();
            if (name == "")
            {
                LabelVisibility = false;
                ErrorLabelVisibility = false;
            }
            else
            {
                LabelVisibility = true;
                var patterns = Fret.ListPatterns(name);
                if (patterns == null || patterns.Item1.Count == 0)
                {
                    ErrorLabelVisibility = true;
                    ErrorLabel = patterns == null ? "Invalid chord symbol" : "Try less strict options";
                    LabelVisibility = patterns != null;
                }
                else
                    ErrorLabelVisibility = false;
                Label = "";

                if (patterns != null)
                {
                    GroupFingerings(patterns.Item1);
                    Label = "notes: " + patterns.Item2;
                }
            }
            NotifyChange();
        }

        void NotifyChange()
        { 
            OnPropertyChanged("FingeringGroups");
            OnPropertyChanged("IsError");
            OnPropertyChanged("LabelVisibility");
            OnPropertyChanged("ErrorLabelVisibility");
            OnPropertyChanged("Label");
            OnPropertyChanged("ErrorLabel");
        }

        void GroupFingerings(List<Pattern> patterns)
        {
            var list = new LinkedList<Pattern>(patterns);
            while(list.Count > 0)
            {
                FingeringGroup group = new FingeringGroup(new Fingering(list.First.Value, Options.FretRange));
                FingeringGroups.Add(group);
                list.RemoveFirst();
                if (list.Count == 0)
                    break;
                for (var n = list.First; n != null;)
                {
                    var currentN = n;
                    n = n.Next;
                    if (!group.IsSubset(currentN.Value))
                        continue;
                    group.Add(new Fingering(currentN.Value, Options.FretRange));
                    list.Remove(currentN.Value);
                }
            }
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
