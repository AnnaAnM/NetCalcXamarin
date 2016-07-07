using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace NetCalc
{
    public partial class TabPage : TabbedPage
    {
        public TabPage()
        {
            InitializeComponent();

            Children.Add(new CidrPage());
            Children.Add(new ClassfulPage());
        }
    }
}
