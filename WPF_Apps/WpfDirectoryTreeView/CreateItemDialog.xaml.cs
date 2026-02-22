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

namespace WpfDirectoryTreeView
{
    /// <summary>
    /// Logika interakcji dla klasy CreateItemDialog.xaml
    /// </summary>
    public partial class CreateItemDialog : Window
    {
        public string ItemName => nameTextBox.Text;
        public bool IsFile => fileRadioButton.IsChecked ?? false;
        public bool IsFolder => folderRadioButton.IsChecked ?? false;
        public bool IsArchive => archiveCheckBox.IsChecked ?? false;
        public bool IsHidden => hiddenCheckBox.IsChecked ?? false;
        public bool IsReadOnly => readOnlyCheckBox.IsChecked ?? false;
        public bool IsSystem => systemCheckBox.IsChecked ?? false;
        public CreateItemDialog()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

    }
}
