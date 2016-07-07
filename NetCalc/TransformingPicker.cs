using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NetCalc
{
    public class TransformingPicker : Picker
    {
        #region TrMinProperty
        public static readonly BindableProperty TrMinProperty =
            BindableProperty.Create("TrMin", typeof(int), typeof(TransformingPicker), 0, BindingMode.TwoWay, propertyChanged:OnTrMinPropertyChanged, propertyChanging:OnTrMinPropertyChanging);

        public int TrMin
        {
            set
            {
                SetValue(TrMinProperty, value);
            }
            get
            {
                return (int)GetValue(TrMinProperty);
            }
        }

        private static void OnTrMinPropertyChanging(BindableObject bindable, Object oldValue, Object newValue)
        {
            List<string> originalItems = new List<string> { "255.0.0.0", "255.128.0.0", "255.192.0.0",
                                                                "255.224.0.0", "255.240.0.0", "255.248.0.0",
                                                                "255.252.0.0", "255.254.0.0", "255.255.0.0",
                                                                "255.255.128.0", "255.255.192.0", "255.255.224.0",
                                                                "255.255.240.0", "255.255.248.0", "255.255.252.0",
                                                                "255.255.254.0", "255.255.255.0", "255.255.255.128",
                                                                "255.255.255.192", "255.255.255.224", "255.255.255.240",
                                                                "255.255.255.248", "255.255.255.252"};

            TransformingPicker picker = (TransformingPicker)bindable;
            picker.Items.Clear();
            int index = 0;
            switch((int)newValue)
            {
                case 1:
                    index = 8;
                    break;
                case 2:
                    index = 16;
                    break;
                default:
                    index = 0;
                    break;
            }
            
            for(int i = index; i < originalItems.Count; i++)
            {
                picker.Items.Add(originalItems[i]);
            }
        }

        private static void OnTrMinPropertyChanged(BindableObject bindable, Object oldValue, Object newValue)
        {
            TransformingPicker picker = (TransformingPicker)bindable;
            picker.SelectedIndex = 0;
        }

        #endregion
    }
}
