using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xperimen.Stylekit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomCalendar : Grid
    {
        public DateTime CurrentDt;

        public CustomCalendar()
        {
            InitializeComponent();
            CurrentDt = DateTime.Now;
            SetupView();
        }

        public void SetupView()
        {

        }
    }
}