using System.Windows.Controls;

namespace TTech.IP_Switcher.Features.WiFiManager
{
    /// <summary>
    /// Interaction logic for WiFiNetworksView.xaml
    /// </summary>
    public partial class WiFiNetworksView : UserControl
    {
        public WiFiNetworksView()
        {
            InitializeComponent();

            if (MainGrid.DataContext is WiFiNetworksViewModel mainViewModel)
                mainViewModel.Owner = this;
        }
    }
}
