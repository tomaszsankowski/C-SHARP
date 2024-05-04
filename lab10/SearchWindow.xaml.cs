using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace lab10
{
    /// <summary>
    /// Logika interakcji dla klasy filter.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        public string Model { get; set; }
        public string EngineModel { get; set; }
        public string Year { get; set; }
        public string Displacement { get; set; }
        public string HorsePower { get; set; }

        public SearchWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
