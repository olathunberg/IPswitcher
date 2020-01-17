﻿using System.Windows;

namespace TTech.IP_Switcher.Features.MessageBox
{
    /// <summary>
    /// Interaction logic for MessageBoxView.xaml
    /// </summary>
    public partial class MessageBoxView : Window
    {
        public MessageBoxView(MessageBoxViewModel owner)
        {
            InitializeComponent();

            MainGrid.DataContext = owner;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
