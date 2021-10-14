using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GuitarChordFinder
{
    public class StepSlider : Slider
    {
        public static readonly BindableProperty CurrentStepValueProperty =
               BindableProperty.Create<StepSlider, double>(p => p.StepValue, 1.0f);

        public double StepValue
        {
            get { return (double)GetValue(CurrentStepValueProperty); }

            set { SetValue(CurrentStepValueProperty, value); }
        }

        public event Action<int> StepValueChanged;

        public StepSlider()
        {
            ValueChanged += OnSliderValueChanged;
        }

        private void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);
            Value = newStep * StepValue;
            StepValueChanged?.Invoke((int)Value);
        }
    }
}
