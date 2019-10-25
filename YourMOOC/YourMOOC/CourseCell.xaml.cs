using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YourMOOC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseCell : ViewCell
    {
        public CourseCell()
        {
            InitializeComponent();
        }

        public void OpenLink_Tapped(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri(CourseLink.Text));
        }
    }
}