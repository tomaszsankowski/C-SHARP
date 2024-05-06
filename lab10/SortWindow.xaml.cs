using System.Windows;

namespace lab10
{
    public partial class SortWindow : Window
    {
        public string SortingProperty { get; private set; }
        public bool IsAscending { get; private set; }

        public SortWindow()
        {
            InitializeComponent();
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            if (YearRadioButton.IsChecked == true)
                SortingProperty = "Year";
            else if (ModelRadioButton.IsChecked == true)
                SortingProperty = "Model";
            else if (MotorRadioButton.IsChecked == true)
                SortingProperty = "Motor";

            IsAscending = AscendingRadioButton.IsChecked == true;

            DialogResult = true;
            Close();
        }
    }
}
