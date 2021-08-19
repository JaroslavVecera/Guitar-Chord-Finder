using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace GuitarChordFinder
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public List<Fingering> Fingerings { get; set; }
        Fret Fret { get; set; } = new Fret(new PatternOptions() { FretRange = 4, MaxFret = 20, RequiredFingers = 4 });
        public string Search { get; set; } = "";
        public RelayCommand Command { get; set; }
        public bool ErrorLabelVisibility { get; set; }
        public bool LabelVisibility { get; set; }
        public string Label { get; set; }
        public string ErrorLabel { get; set; }

        public MainViewModel()
        {
             Command = new RelayCommand(() => CountFingerings(Search));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void CountFingerings(string name)
        {
            Fingerings = new List<Fingering>();
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
                    foreach (var p in patterns.Item1)
                        Fingerings.Add(new Fingering(p));
                    Label = "notes: " + patterns.Item2;
                }
            }
            NotifyChange();
        }

        void NotifyChange()
        { 
            OnPropertyChanged("Fingerings");
            OnPropertyChanged("IsError");
            OnPropertyChanged("LabelVisibility");
            OnPropertyChanged("ErrorLabelVisibility");
            OnPropertyChanged("Label");
            OnPropertyChanged("ErrorLabel");
        }

        void GroupFingerings(List<Pattern> patterns)
        {
            var list = new LinkedList<Pattern>(patterns);
            while(patterns.Count > 0)
            {
                FingeringGroup group = new FingeringGroup(new Fingering(list.First.Value));
                //FingeringGroups.Add(group);
                list.RemoveFirst();
                if (list.Count == 0)
                    break;
                for (var n = list.First; n != null; n = n.Next)
                {
                    if (!group.IsSubset(n.Value))
                        continue;
                    group.Add(new Fingering(n.Value));
                    list.Remove(n.Value);
                }
            }
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
