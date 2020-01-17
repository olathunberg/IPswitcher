using System.Windows.Controls;

namespace TTech.IP_Switcher.Features.IpSwitcher
{
    /// <summary>
    /// Interaction logic for IpSwitcherView.xaml
    /// </summary>
    public partial class IpSwitcherView : UserControl
    {
        public IpSwitcherView()
        {
            InitializeComponent();

            if (MainGrid.DataContext is IpSwitcherViewModel mainViewModel)
                mainViewModel.Owner = this;
        }
    }
}
