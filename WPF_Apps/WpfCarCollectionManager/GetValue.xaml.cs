using System;
using System.Windows;

namespace WpfCarCollectionManager
{
    /// <summary>
    /// Logika interakcji dla klasy GetValue.xaml
    /// </summary>
    public partial class GetValue : Window
    {
        public string EnteredValue { get; private set; }

        public GetValue()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            EnteredValue = textBoxValue.Text;
            DialogResult = true;
            Close();
        }
    }
}
