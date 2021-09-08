using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace GuitarChordFinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FingeringView : ContentView
    {
        double _screenWidth;
        double _firstStringX;
        double _lastStringX;
        double _lastFretY;
        double _firstFretY;
        double _cellSize;
        double _circleSize;
        double _barreBottom;
        double _barreHeight;

        public static readonly BindableProperty FullCirclesProperty = BindableProperty.Create(
            nameof(FullCircles), typeof(List<System.Drawing.Point>), typeof(FingeringView), propertyChanged: FullCirclesPropertyChanged);

        public List<System.Drawing.Point> FullCircles { get; set; }

        public static readonly BindableProperty EmptyCirclesProperty = BindableProperty.Create(
            nameof(EmptyCircles), typeof(List<System.Drawing.Point>), typeof(FingeringView), propertyChanged: EmptyCirclesPropertyChanged);

        public List<System.Drawing.Point> EmptyCircles { get; set; }

        public static readonly BindableProperty XsProperty = BindableProperty.Create(
            nameof(Xs), typeof(List<System.Drawing.Point>), typeof(FingeringView), propertyChanged: XsPropertyChanged);

        public List<System.Drawing.Point> Xs { get; set; }

        public static readonly BindableProperty PositionProperty = BindableProperty.Create(
            nameof(Position), typeof(int), typeof(FingeringView), propertyChanged: PositionPropertyChanged);

        public void ForceRedraw()
        {
            DrawPosition(Position);
            DrawXs(Xs);
            DrawEmptyCircles(EmptyCircles);
            DrawFullCircles(FullCircles);
            DrawBarres(Barres);
        }

        public int Position { get; set; }

        public static readonly BindableProperty BarresProperty = BindableProperty.Create(
            nameof(Barres), typeof(List<Barre>), typeof(FingeringView), propertyChanged: BarresPropertyChanged);

        public List<Barre> Barres { get; set; }

        public FingeringView()
        {
            InitializeComponent();
            CountConstants();
            DrawGrid();
        }

        void CountConstants()
        {
            _screenWidth = Application.Current.MainPage.Width;
            _firstStringX = _screenWidth * 0.2;
            _lastStringX = _screenWidth * 0.8;
            _lastFretY = _screenWidth * 0.8;
            _firstFretY = _screenWidth * 0.2;
            _cellSize = _screenWidth * 0.12;
            _circleSize = _screenWidth * 0.08;
            _barreBottom = _cellSize / 4 * 3;
            _barreHeight = _cellSize / 2;
        }

        void DrawGrid()
        {
            for (int i = 0; i < 6; i++)
                MakeLine(_firstStringX + i * _cellSize, _firstFretY, _firstStringX + i * _cellSize, _lastFretY);
            for (int i = 0; i < 6; i++)
                MakeLine(_firstStringX, _firstFretY + i * _cellSize, _lastStringX, _firstFretY + i * _cellSize);
        }

        void MakeLine(double x1, double y1, double x2, double y2)
        {
            canvas.Children.Add(new Line() { X1 = x1, X2 = x2, Y1 = y1, Y2 = y2 }, Constraint.Constant(0));
        }

        public void DrawFullCircles(List<System.Drawing.Point> points)
        {
            foreach (System.Drawing.Point p in points)
                MakeCircle(p.X, p.Y, true);
        }

        public void DrawEmptyCircles(List<System.Drawing.Point> points)
        {
            foreach (System.Drawing.Point p in points)
                MakeCircle(p.X, p.Y, false);
        }

        void MakeCircle(double x, double y, bool filled)
        {
            Ellipse e = new Ellipse() { HeightRequest = _circleSize, WidthRequest = _circleSize };
            if (filled)
                e.Style = Application.Current.Resources["FullCircleStyle"] as Style;
            else
                e.Style = Application.Current.Resources["EmptyCircleStyle"] as Style;
            clearableCanvas.Children.Add(e, Constraint.Constant(_firstStringX + x * _cellSize - _circleSize / 2), Constraint.Constant(_firstFretY + (0.5 + y) * _cellSize - _circleSize / 2));
        }

        void DrawBarres(List<Barre> barres)
        {
            foreach (Barre b in barres)
                MakeBarre(b.Height, b.Start, b.End);
        }

        void MakeBarre(double height, double start, double end)
        {
            double width = (end - start) * _cellSize;

            string pathDescription = "M " + DToS(start * _cellSize) + "," + DToS(_barreBottom) + " A ";
            double upperRadius = width * width / 8 / _barreHeight + _barreHeight / 2;
            pathDescription += DToS(upperRadius) + "," + DToS(upperRadius) + " 0 0,1 " + DToS(end * _cellSize) + "," + DToS(_barreBottom) + " A ";
            double lowerRadius = width * width / 4 / _barreHeight + _barreHeight / 4;
            pathDescription += DToS(lowerRadius) + "," + DToS(lowerRadius) + " 0 0,0 " + DToS(start * _cellSize) + "," + DToS(_barreBottom);
            Geometry pathData = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(pathDescription);
            Path p = new Path() { Data = pathData };
            clearableCanvas.Children.Add(p, Constraint.Constant(_firstStringX), Constraint.Constant(-_cellSize + height * _cellSize + _firstFretY));
        }

        string DToS(double d)
        {
            return d.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
        }

        public void DrawXs(List<System.Drawing.Point> points)
        {
            foreach (System.Drawing.Point p in points)
                MakeX(p.X, p.Y);
        }

        void MakeX(int x, int y)
        {
            double h = _cellSize * 0.2;
            double centerX = _firstStringX + x * _cellSize - h;
            double centerY = _firstFretY + (0.5 + y) * _cellSize - h;
            clearableCanvas.Children.Add(new Polyline() { Points = new PointCollection() { new Point(h, h), new Point(3 * h, 3 * h), new Point(2 * h, 2 * h), new Point(h, 3 * h), new Point(3 * h, h) } }, Constraint.Constant(centerX - h), Constraint.Constant(centerY - h));
        }

        public void DrawPosition(int p)
        {
            if (p > 1)
                clearableCanvas.Children.Add(new Label() { Text = p.ToString(), FontSize = 25 }, Constraint.Constant(_lastStringX + _cellSize * 0.5), Constraint.Constant(_firstFretY));
            else
                clearableCanvas.Children.Add(new Polyline() { Points = new PointCollection() { new Point(_firstStringX, _firstFretY), new Point(_lastStringX, _firstFretY) }, StrokeThickness = 7 }, Constraint.Constant(0));

        }

        private static void FullCirclesPropertyChanged(BindableObject o, object oldO, object newO)
        {
            if (newO == null)
                return;
            var c = (FingeringView)o;
            c.DrawFullCircles((List<System.Drawing.Point>)newO);
        }

        private static void EmptyCirclesPropertyChanged(BindableObject o, object oldO, object newO)
        {
            if (newO == null)
                return;
            var c = (FingeringView)o;
            c.DrawEmptyCircles((List<System.Drawing.Point>)newO);
        }

        private static void XsPropertyChanged(BindableObject o, object oldO, object newO)
        {
            if (newO == null)
                return;
            var c = (FingeringView)o;
            c.DrawXs((List<System.Drawing.Point>)newO);
        }

        private static void PositionPropertyChanged(BindableObject o, object oldO, object newO)
        {
            if (newO == null)
                return;
            var c = (FingeringView)o;
            c.DrawPosition((int)newO);
        }

        private static void BarresPropertyChanged(BindableObject o, object oldO, object newO)
        {
            if (newO == null)
                return;
            var c = (FingeringView)o;
            c.DrawBarres((List<Barre>)newO);
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext == null)
                clearableCanvas.Children.Clear();
        }
    }
}