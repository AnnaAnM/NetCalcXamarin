using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCalc
{
    class TransformingSlider : Slider
    {
        #region TrMinimumProperty

        public static readonly BindableProperty TrMinimumProperty =
           BindableProperty.Create("TrMinimum", typeof(double), typeof(TransformingSlider), 8.0, BindingMode.OneWay, propertyChanged:OnTrMinimumPropertyChanged, coerceValue:OnTrMinimumPropertyCoerceValue);

        public double TrMinimum
        {
            set
            {
                SetValue(TrMinimumProperty, value);
            }
            get
            {
                return (double)GetValue(TrMinimumProperty);
            }
        }

        private static void OnTrMinimumPropertyChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            TransformingSlider slider = (TransformingSlider)bindable;
            slider.Minimum = slider.TrMinimum;
            slider.Value = slider.TrMinimum;
        }

        private static object OnTrMinimumPropertyCoerceValue(BindableObject bindable, Object value)
        {
            TransformingSlider slider = (TransformingSlider)bindable;
            double mask = 0;

            if ((double)value == 0)
            {
                mask = 8;
            }
            else if ((double)value == 1)
            {
                mask = 16;
            }
            else
            {
                mask = 24;
            }

            return mask;
        }

        #endregion

    }
}
