using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GuitarChordFinder
{
    public class Fingering : INotifyPropertyChanged
    {
        public List<Point> FullCircles { get; set; } = new List<Point>();
        public List<Point> EmptyCircles { get; set; } = new List<Point>();
        public List<Point> Xs{ get; set; } = new List<Point>();

        public int Position { get; set; }
        public bool HasBarre { get; set; }
        public List<Barre> Barres { get; set; } = new List<Barre>();
        public Pattern Pattern { get; private set; }
        public int Width { get; private set; }
        public Tuple<int, int> DiagramSize { get { return new Tuple<int, int>(Pattern.Size, Width); } }

        public Fingering(Pattern p, int width)
        {
            Width = width;
            Pattern = p;
            Position = p.Min;
            if (Position <= 2)
                Position = 1;
            int shred = 0;
            if (Position > 0)
                shred = Position - 1;
            for (int i = 0; i < p.Size; i++)
            {
                if (p.Frets[i] == 0)
                    EmptyCircles.Add(new Point(i, -1));
                else if (p.Frets[i] > 0)
                    FullCircles.Add(new Point(i, p.Frets[i] - 1 - shred));
                else
                    Xs.Add(new Point(i, -1));
            }
            HasBarre = p.Barre;
            if (HasBarre)
                CountBarre(shred, p);
        }

        void CountBarre(int shred, Pattern p)
        { 
            int min = p.Min;
            int by = min - shred - 1;
            int bx1 = p.Frets.ToList().IndexOf(min);
            int bx2 = p.Frets.ToList().FindLastIndex(x => x == min);
            Barres.Add(new Barre() { Height = by, Start = bx1, End = bx2 });
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
