using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace lab10
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    /// 
    public class Engine : IComparable<Engine>
    {
        public double Displacement { get; set; }
        public double HorsePower { get; set; }
        [XmlAttribute("model")]
        public string Model { get; set; }

        public Engine(double displacement, double horsePower, string model)
        {
            Displacement = displacement;
            HorsePower = horsePower;
            Model = model;
        }

        public Engine()
        {
            Model = string.Empty;
        }

        public int CompareTo(Engine other)
        {
            return this.HorsePower.CompareTo(other.HorsePower);
        }
    }

    [XmlType("car")]
    public class Car
    {
        public int Year { get; set; }
        public string Model { get; set; }


        [XmlElement("engine")]
        public virtual Engine Motor { get; set; }

        public Car(string model, Engine engine, int year)
        {
            Year = year;
            Model = model;
            Motor = engine;
        }

        public Car()
        {
            Motor = new Engine();
            Model = string.Empty;
        }
    }
    public partial class MainWindow : Window
    {
        List<Car> myCars;
        BindingList<Car> myCarsBindingList;
        BindingList<Car> originalCarsList;
        private Dictionary<string, ListSortDirection> sortDirections = new Dictionary<string, ListSortDirection>();
        public MainWindow()
        {
            InitializeComponent();
            myCars = DeserializeCars("CarsCollection.xml");
            myCarsBindingList = new BindingList<Car>(myCars);
            originalCarsList = myCarsBindingList;
            dataGrid.ItemsSource = myCarsBindingList;

            dataGrid.Sorting += DataGrid_Sorting;
        }

        static void SerializeCars(string filePath, List<Car> cars)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            TextWriter writer = new StreamWriter(filePath);
            try
            {
                serializer.Serialize(writer, cars);
            }
            finally
            {
                writer.Dispose();
            }
        }


        static List<Car> DeserializeCars(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Car>), new XmlRootAttribute("cars"));
            TextReader reader = new StreamReader(filePath);
            try
            {
                List<Car> deserializedCars = serializer.Deserialize(reader) as List<Car>;
                return deserializedCars;
            }
            finally
            {
                reader.Dispose();
            }
        }

        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Car selectedCar = dataGrid.SelectedItem as Car;
                myCarsBindingList.Remove(selectedCar);
            }
        }


        private void Szukaj_Click(object sender, RoutedEventArgs e)
        {
            SearchWindow searchWindow = new SearchWindow();
            if (searchWindow.ShowDialog() == true)
            {
                ApplyFilter(searchWindow);
            }
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            myCarsBindingList = new BindingList<Car>(originalCarsList);
            dataGrid.ItemsSource = myCarsBindingList;
        }

        private void ApplyFilter(SearchWindow searchWindow)
        {
            var filteredCars = originalCarsList.Where(car =>
            {
                bool filterModel = string.IsNullOrEmpty(searchWindow.Model) ||
                                   car.Model.ToLower().Contains(searchWindow.Model.ToLower());

                bool filterEngineModel = string.IsNullOrEmpty(searchWindow.EngineModel) ||
                                         car.Motor.Model.ToLower().Contains(searchWindow.EngineModel.ToLower());

                bool filterYear = string.IsNullOrEmpty(searchWindow.Year) ||
                                  car.Year.ToString().Equals(searchWindow.Year);

                bool filterDisplacement = string.IsNullOrEmpty(searchWindow.Displacement) ||
                                          car.Motor.Displacement.ToString().Equals(searchWindow.Displacement);

                bool filterHorsePower = string.IsNullOrEmpty(searchWindow.HorsePower) ||
                                        car.Motor.HorsePower.ToString().Equals(searchWindow.HorsePower);

                return filterModel && filterEngineModel && filterYear && filterDisplacement && filterHorsePower;
            }).ToList();

            dataGrid.ItemsSource = new BindingList<Car>(filteredCars);
        }



        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            string sortBy = e.Column.SortMemberPath;
            ListSortDirection direction;

            if (sortDirections.ContainsKey(sortBy))
            {
                direction = sortDirections[sortBy];
                direction = (direction == ListSortDirection.Ascending) ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            sortDirections[sortBy] = direction;

            e.Column.SortDirection = direction;

            Sort(sortBy, direction);

            e.Handled = true;
        }


        private void Sort(string sortBy, ListSortDirection direction)
        {
            PropertyInfo property;
            if (sortBy.StartsWith("Motor."))
            {
                property = typeof(Engine).GetProperty(sortBy.Substring("Motor.".Length));
                sortBy = "Motor";
            }
            else
            {
                property = typeof(Car).GetProperty(sortBy);
            }

            if (property == null)
                return;

            if (direction == ListSortDirection.Ascending)
            {
                if (sortBy == "Motor")
                {
                    myCarsBindingList = new BindingList<Car>(myCarsBindingList.OrderBy(x => property.GetValue(x.Motor)).ToList());
                }
                else
                {
                    myCarsBindingList = new BindingList<Car>(myCarsBindingList.OrderBy(x => property.GetValue(x)).ToList());
                }
            }
            else
            {
                if (sortBy == "Motor")
                {
                    myCarsBindingList = new BindingList<Car>(myCarsBindingList.OrderByDescending(x => property.GetValue(x.Motor)).ToList());
                }
                else
                {
                    myCarsBindingList = new BindingList<Car>(myCarsBindingList.OrderByDescending(x => property.GetValue(x)).ToList());
                }
            }

            dataGrid.ItemsSource = myCarsBindingList;
        }




    }

}
