using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace NetCalc
{
    public partial class CidrPage : ContentPage
    {
        Dictionary<int, string> IpMasks = new Dictionary<int, string>
        {
            { 0, "0.0.0.0" }, { 1, "128.0.0.0" }, { 2, "192.0.0.0" },
            { 3, "224.0.0.0" }, { 4, "240.0.0.0" }, { 5, "248.0.0.0" },
            { 6, "252.0.0.0" }, { 7, "254.0.0.0" }, { 8, "255.0.0.0" },
            { 9, "255.128.0.0" }, { 10, "255.192.0.0" }, { 11, "255.224.0.0" },
            { 12, "255.240.0.0" }, { 13, "255.248.0.0" }, { 14, "255.252.0.0" },
            { 15, "255.254.0.0" }, { 16, "255.255.0.0" }, { 17, "255.255.128.0" },
            { 18, "255.255.192.0" }, { 19, "255.255.224.0" }, { 20, "255.255.240.0" },
            { 21, "255.255.248.0" }, { 22, "255.255.252.0" }, { 23, "255.255.254.0" },
            { 24, "255.255.255.0" }, { 25, "255.255.255.128" }, { 26, "255.255.255.192" },
            { 27, "255.255.255.224" }, { 28, "255.255.255.240" }, { 29, "255.255.255.248" },
            { 30, "255.255.255.252" }, { 31, "255.255.255.254" }, { 32, "255.255.255.255" }
        };

        string ipPattern = @"^(([1|2]{0,1}[0-9]{1,2}\.){3}[1|2]{0,1}[0-9]{1,2})$";

        public CidrPage()
        {
            InitializeComponent();

            Padding = new Thickness(0, Device.OnPlatform(20, 0, 20), 0, 0);

            //Начальные значения
            foreach (string IpMask in IpMasks.Values)
            {
                maskPicker.Items.Add(IpMask);
            }

            maskPicker.SelectedIndex = 8;
            maskSlider.Minimum = 0;
            maskSlider.Maximum = 32;
            maskSlider.Value = 8;
            IPv4 ip = new IPv4("1.0.0.0", "255.0.0.0");
            cidrNotationLabel.Text = ip.SubnetAdd + "/" + Convert.ToString(maskPicker.SelectedIndex);
            wcMaskLabel.Text = ip.WildcardMask;
            addRangeLabel.Text = ip.SubnetAdd + " - " + ip.BroadcastAdd;
            maxAddsLabel.Text = ip.MaxAdds.ToString();

            //Привязка
            maskSlider.BindingContext = maskPicker;
            maskSlider.SetBinding(Slider.ValueProperty, "SelectedIndex");

            //При изменении
            ipAddressEntry.TextChanged += (sender, args) =>
            {
                string ipToCheck = (sender as Entry).Text;

                if (Regex.IsMatch(ipToCheck, ipPattern))
                {
                    (sender as Entry).TextColor = Color.Default;
                    ip.SetIp(ipToCheck);
                    cidrNotationLabel.Text = ip.SubnetAdd + "/" + Convert.ToString(maskPicker.SelectedIndex);
                    addRangeLabel.Text = ip.SubnetAdd + " - " + ip.BroadcastAdd;
                }
                else
                {
                    (sender as Entry).TextColor = Color.Red;
                }
            };

            maskPicker.SelectedIndexChanged += (sender, args) =>
            {
                ip.SetMask(maskPicker.Items[(sender as Picker).SelectedIndex]);
                cidrNotationLabel.Text = ip.SubnetAdd + "/" + Convert.ToString(maskPicker.SelectedIndex);
                wcMaskLabel.Text = ip.WildcardMask;
                addRangeLabel.Text = ip.SubnetAdd + " - " + ip.BroadcastAdd;
                maxAddsLabel.Text = ip.MaxAdds.ToString();
            };
        }
    }
}
