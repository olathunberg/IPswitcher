using System.Windows.Controls;

namespace TTech.IP_Switcher.Features.WiFiManager
{
    /// <summary>
    /// Interaction logic for WiFiProfilesViewModel.xaml
    /// </summary>
    public partial class WiFiProfilesView : UserControl
    {
        public WiFiProfilesView()
        {
            InitializeComponent();

            if (MainGrid.DataContext is WiFiProfilesViewModel mainViewModel)
                mainViewModel.Owner = this;
        }
    }
}
