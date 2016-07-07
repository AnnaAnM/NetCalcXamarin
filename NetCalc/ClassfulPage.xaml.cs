using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace NetCalc
{ 
    public partial class ClassfulPage : ContentPage
    {

        Dictionary<int, string> IpMasks = new Dictionary<int, string>
        {
            { 8, "255.0.0.0" }, { 9, "255.128.0.0" }, { 10, "255.192.0.0" },
            { 11, "255.224.0.0" }, { 12, "255.240.0.0" }, { 13, "255.248.0.0" },
            { 14, "255.252.0.0" }, { 15, "255.254.0.0" }, { 16, "255.255.0.0" },
            { 17, "255.255.128.0" }, { 18, "255.255.192.0" }, { 19, "255.255.224.0" },
            { 20, "255.255.240.0" }, { 21, "255.255.248.0" }, { 22, "255.255.252.0" },
            { 23, "255.255.254.0" }, { 24, "255.255.255.0" }, { 25, "255.255.255.128" },
            { 26, "255.255.255.192" }, { 27, "255.255.255.224" }, { 28, "255.255.255.240" },
            { 29, "255.255.255.248" }, { 30, "255.255.255.252" }
        };

        string ipPattern = @"^(([1|2]{0,1}[0-9]{1,2}\.){3}[1|2]{0,1}[0-9]{1,2})$";

        public ClassfulPage()
        {
            InitializeComponent();

            Padding = new Thickness(0, Device.OnPlatform(20, 0, 20), 0, 0);

            #region Content of stacklayout
            //classPicker - 2d element
            classPicker.Items.Add("A");
            classPicker.Items.Add("B");
            classPicker.Items.Add("C");
            classPicker.SelectedIndex = 0;

            //maskTrPicker - 3d element
            TransformingPicker maskTrPicker = new TransformingPicker();
            maskTrPicker.Title = "Mask";
            stackLayout.Children.Insert(3, maskTrPicker);
            foreach (string IpMask in IpMasks.Values)
            {
                maskTrPicker.Items.Add(IpMask);
            }
            maskTrPicker.SelectedIndex = 0;

            //maskTrSlider - 5th element
            TransformingSlider maskTrSlider = new TransformingSlider();
            stackLayout.Children.Insert(5, maskTrSlider);
            maskTrSlider.Maximum = 30;
            maskTrSlider.Minimum = 8;

            #endregion

            #region Initial parameters

            IPv4 ip = new IPv4("1.0.0.0", "255.0.0.0", 0);
            numSubnetLabel.Text = ip.MaxSubs.ToString();
            numHostLabel.Text = ip.MaxAdds.ToString();
            subIdLabel.Text = ip.SubnetAdd;
            brAddLabel.Text = ip.BroadcastAdd;
            //Fix AddRange
            addRangeLabel.Text = ip.AddRange;
            wcLabel.Text = ip.WildcardMask;

            #endregion

            #region Binding

            //Привязка для TrMinProperty of maskTrPicker
            Binding classBind = new Binding
            {
                Source = classPicker,
                Mode = BindingMode.Default,
                Path = "SelectedIndex",
            };
            //Привязка для TrMinimumProperty of maskTrSlider
            Binding classBind2 = new Binding
            {
                Source = classPicker,
                Mode = BindingMode.Default,
                Path = "SelectedIndex",
            };
            //Привязка для maskLabel
            Binding maskBind = new Binding
            {
                Source = maskTrSlider,
                Mode = BindingMode.OneWay,
                Path = "Value"
            };

            maskTrPicker.SetBinding(TransformingPicker.TrMinProperty, classBind);
            maskTrSlider.SetBinding(TransformingSlider.TrMinimumProperty, classBind2);
            maskLabel.SetBinding(Label.TextProperty, maskBind);
            
            #endregion

            #region Events

            maskTrSlider.ValueChanged += (sender, args) =>
            {
                TransformingSlider slider = (TransformingSlider)sender;
                
                string mask = "";
                IpMasks.TryGetValue((int)(maskTrSlider.Value), out mask);
                ip.SetMaskClassful(mask);
                numSubnetLabel.Text = ip.MaxSubs.ToString();
                numHostLabel.Text = ip.MaxAdds.ToString();
                subIdLabel.Text = ip.SubnetAdd;
                brAddLabel.Text = ip.BroadcastAdd;
                addRangeLabel.Text = ip.AddRange;
                wcLabel.Text = ip.WildcardMask;

                int index = (int)(slider.Value - slider.Minimum);
                maskTrPicker.SelectedIndex = index;
            };

            maskTrPicker.SelectedIndexChanged += (sender, args) =>
            {
                TransformingPicker picker = (TransformingPicker)sender;
                double val = 0;
                switch(picker.Items.Count)
                {
                    case 23:
                        val = picker.SelectedIndex + 8;
                        break;
                    case 15:
                        val = picker.SelectedIndex + 16;
                        break;
                    case 7:
                        val = picker.SelectedIndex + 24;
                        break;
                }
                maskTrSlider.Value = val;  
            };

            ipAddressEntry.TextChanged += (sender, args) =>
             {
                 string ipToCheck = (sender as Entry).Text;

                 if (Regex.IsMatch(ipToCheck, ipPattern))
                 {
                     (sender as Entry).TextColor = Color.Default;
                     ip.SetIpClassful(ipToCheck);
                     subIdLabel.Text = ip.SubnetAdd;
                     brAddLabel.Text = ip.BroadcastAdd;
                     addRangeLabel.Text = ip.AddRange;                     
                 }
                 else
                 {
                     (sender as Entry).TextColor = Color.Red;
                 }
             };

            classPicker.SelectedIndexChanged += (sender, args) =>
            {
                ip.SetClassOfNet((sender as Picker).SelectedIndex);
            };

            #endregion
        }
    }
}
