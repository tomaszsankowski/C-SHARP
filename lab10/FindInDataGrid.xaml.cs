using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using lab10;
using System.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;

namespace lab10
{
    public partial class FindInDataGrid : Window
    {
        public BindingList<Car> myItems { get; set; }

        public FindInDataGrid(List<Car> items)
        {
            InitializeComponent();

            myItems = new BindingList<Car>(items);

            LoadProperties(typeof(Car), "");
        }

        private void LoadProperties(Type type, string prefix)
        {
            foreach (var prop in type.GetProperties())
            {
                if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(int) || prop.PropertyType == typeof(double))
                {
                    comboBox.Items.Add(prefix + prop.Name);
                }
                else
                {
                    LoadProperties(prop.PropertyType, prefix + prop.Name + ".");
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string propertyName = comboBox.SelectedItem.ToString();
            string searchValue = textBox.Text;

            if(searchValue == "")
            {
                MessageBox.Show("Please enter a value to search for");
                return;
            }
            else
            {
                var itemList = new List<Car>(myItems);
                var foundItems = itemList.FindAll(item =>
                {
                    var propertyNames = propertyName.Split('.');
                    object propertyValue = item;
                    foreach (var name in propertyNames)
                    {
                        if (propertyValue == null)
                        {
                            return false;
                        }
                        propertyValue = propertyValue.GetType().GetProperty(name).GetValue(propertyValue, null);
                    }
                    return propertyValue?.ToString() == searchValue;
                });

                resultComboBox.Items.Clear();
                foreach (var item in foundItems)
                {
                    resultComboBox.Items.Add(item);
                }
            }
        }
    }
}