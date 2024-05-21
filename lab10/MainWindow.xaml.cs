using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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

        public override string ToString()
        {
            return $"{Displacement}{HorsePower}{Model}";
        }
    }

    [XmlType("car")]
    public class Car
    {
        public int Year { get; set; }
        public string Model { get; set; }


        [XmlElement("engine")]
        public Engine Motor { get; set; }

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

        public override string ToString()
        {
            return $"Year: {Year}, Model: {Model}, Engine: {Motor.Model}, Displacement: {Motor.Displacement}, HorsePower: {Motor.HorsePower}";
        }
    }

    public class SortableBindingList<T> : BindingList<T>
    {
        protected override bool SupportsSortingCore => true;
        protected override bool SupportsSearchingCore => true;

        private readonly ListSortDirection sortDirection;
        private PropertyDescriptor sortProperty;

        public SortableBindingList(List<T> list) : base(list) { }

        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            if (Items is List<T> itemsList)
            {
                var property = typeof(T).GetProperty(prop.Name);
                if (property != null)
                {
                    if (property.PropertyType.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IComparable<>)))
                    {
                        itemsList.Sort(new ItemComparer<T>(property, direction));
                    }
                    else
                    {
                        MessageBox.Show("NIE MA COMPARABLE");
                    }
                }
            }
        }

        protected override void RemoveSortCore()
        {
            sortProperty = null;
        }

        protected override bool IsSortedCore => sortProperty != null;

        protected override ListSortDirection SortDirectionCore => sortDirection;

        protected override PropertyDescriptor SortPropertyCore => sortProperty;

        protected override int FindCore(PropertyDescriptor prop, object key)
        {
            if (Items is List<T> itemsList)
            {
                foreach (var item in itemsList)
                {
                    var property = TypeDescriptor.GetProperties(typeof(T)).Find(prop.Name, true);
                    if (property != null)
                    {
                        var value = property.GetValue(item);
                        if (value != null && value.ToString().Contains(key.ToString()))
                            return itemsList.IndexOf(item);
                    }
                }
            }
            return -1;
        }


        private class ItemComparer<TItem> : IComparer<TItem>
        {
            private readonly PropertyInfo property;
            private readonly ListSortDirection direction;

            public ItemComparer(PropertyInfo property, ListSortDirection direction)
            {
                this.property = property;
                this.direction = direction;
            }

            public int Compare(TItem x, TItem y)
            {
                var xValue = property.GetValue(x);
                var yValue = property.GetValue(y);

                if (xValue == null && yValue == null)
                    return 0;
                if (xValue == null)
                    return direction == ListSortDirection.Ascending ? -1 : 1;
                if (yValue == null)
                    return direction == ListSortDirection.Ascending ? 1 : -1;

                if (xValue is IComparable<Engine> comparableX && yValue is IComparable<Engine> comparableY)
                {
                    return direction == ListSortDirection.Ascending ? comparableX.CompareTo((Engine)yValue) : comparableY.CompareTo((Engine)xValue);
                }

                else if (xValue is IComparable comparableX2 && yValue is IComparable comparableY2)
                {
                    return direction == ListSortDirection.Ascending ? comparableX2.CompareTo(yValue) : comparableY2.CompareTo(xValue);
                }


                return 0;
            }

        }


        public void SortBy(string property, ListSortDirection direction)
        {
            ApplySortCore(TypeDescriptor.GetProperties(typeof(T)).Find(property, true), direction);
        }


        public int Find(string propertyName, object key)
        {
            PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(T)).Find(propertyName, true);
            if (prop != null)
            {
                return FindCore(prop, key);
            }
            return -1;
        }
    }

    public partial class MainWindow : Window
    {
        readonly List<Car> myCars;
        BindingList<Car> myCarsBindingList;
        readonly BindingList<Car> originalCarsList;
        FindInDataGrid<Car> zad4Grid;
        readonly private Dictionary<string, ListSortDirection> sortDirections = new Dictionary<string, ListSortDirection>();
        public MainWindow()
        {
            InitializeComponent();
            myCars = DeserializeCars("CarsCollection.xml");
            myCarsBindingList = new BindingList<Car>(myCars);
            originalCarsList = myCarsBindingList;
            dataGrid.ItemsSource = myCarsBindingList;

            dataGrid.Sorting += DataGrid_Sorting;

            zad4Grid = new FindInDataGrid<Car>(myCarsBindingList);

            foreach (string it in zad4Grid.properties)
            {
                inputComboBox.Items.Add(it);
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


        private void Zad1_Click(object sender, RoutedEventArgs e)
        {
            var queryExpressionSyntaxCars = from car in myCars
                                            where car.Model.StartsWith("A6")
                                            group car by car.Motor.Model == "TDI" ? "diesel" : "petrol" into g
                                            let avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement)
                                            orderby avgHPPL descending
                                            select new
                                            {
                                                engineType = g.Key,
                                                avgHPPL
                                            };

            var methodBasedQuerySyntaxCars = myCars.Where(car => car.Model.StartsWith("A6"))
                                          .GroupBy(car => car.Motor.Model == "TDI" ? "diesel" : "petrol")
                                          .Select(g => new
                                          {
                                              engineType = g.Key,
                                              avgHPPL = g.Average(car => car.Motor.HorsePower / car.Motor.Displacement)
                                          })
                                          .OrderByDescending(x => x.avgHPPL);

            zadaniaTextBox.AppendText("Expression Syntax Query result:\n");
            foreach (var group in queryExpressionSyntaxCars)
            {
                zadaniaTextBox.AppendText($"{group.engineType} : {group.avgHPPL}\n");
            }
            zadaniaTextBox.AppendText("\n");
            zadaniaTextBox.AppendText("Method Based Query result:\n");
            foreach (var group in methodBasedQuerySyntaxCars)
            {
                zadaniaTextBox.AppendText($"{group.engineType} : {group.avgHPPL}\n");
            }
        }

        private void Zad2_Click(object sender, RoutedEventArgs e)
        {
            Comparison<Car> arg1 = delegate (Car car1, Car car2)
            {
                return car2.Motor.HorsePower.CompareTo(car1.Motor.HorsePower);
            };

            Predicate<Car> arg2 = delegate (Car car)
            {
                return car.Motor.Model == "TDI";
            };

            Action<Car> arg3 = delegate (Car car)
            {
                MessageBox.Show(car.ToString());
            };

            myCars.Sort(new Comparison<Car>(arg1));
            myCars.FindAll(arg2).ForEach(arg3);
        }

        private void Zad3_Sort_Click(object sender, RoutedEventArgs e)
        {
            SortWindow sortWindow = new SortWindow();
            bool? result = sortWindow.ShowDialog();

            if (result == true)
            {
                string sortingProperty = sortWindow.SortingProperty;
                ListSortDirection sortingOrder = sortWindow.IsAscending ? ListSortDirection.Ascending : ListSortDirection.Descending;

                SortableBindingList<Car> sortedCarsList = new SortableBindingList<Car>(myCars);

                switch (sortingProperty)
                {
                    case "Year":
                        sortedCarsList.SortBy("Year", sortingOrder);
                        break;
                    case "Model":
                        sortedCarsList.SortBy("Model", sortingOrder);
                        break;
                    case "Motor":
                        sortedCarsList.SortBy("Motor", sortingOrder);
                        break;
                    default:
                        break;
                }

                zadaniaTextBox.Clear();
                zadaniaTextBox.AppendText($"Sorting by {sortingProperty}, {sortingOrder}\n");
                foreach (var car in sortedCarsList)
                {
                    zadaniaTextBox.AppendText($"{car.Year}\t{car.Model}\t{car.Motor.Displacement}\t{car.Motor.HorsePower}\t{car.Motor.Model}\n");
                }
            }
        }

        private void Zad3_Find_Click(object sender, RoutedEventArgs e)
        {
            GetValue searchWindow = new GetValue();
            if (searchWindow.ShowDialog() == true)
            {
                string searchString = searchWindow.EnteredValue;

                if (!string.IsNullOrEmpty(searchString))
                {
                    SortableBindingList<Car> sortedCarsList = new SortableBindingList<Car>(myCars);

                    int indexYear = sortedCarsList.Find(nameof(Car.Year), searchString);
                    int indexModel = sortedCarsList.Find(nameof(Car.Model), searchString);
                    int indexEngine = sortedCarsList.Find(nameof(Car.Motor), searchString);

                    if (indexYear != -1)
                    {
                        Car foundCar = myCarsBindingList[indexYear];
                        MessageBox.Show($"Znaleziono samochód: {foundCar}");
                    }
                    else if (indexModel != -1)
                    {
                        Car foundCar = myCarsBindingList[indexModel];
                        MessageBox.Show($"Znaleziono samochód: {foundCar}");
                    }
                    else if (indexEngine != -1)
                    {
                        Car foundCar = myCarsBindingList[indexEngine];
                        MessageBox.Show($"Znaleziono samochód: {foundCar}");
                    }
                    else
                    {
                        MessageBox.Show("Nie znaleziono żadnego samochodu pasującego do kryteriów wyszukiwania.");
                    }
                }
                else
                {
                    MessageBox.Show("Proszę wprowadzić kryteria wyszukiwania.");
                }
            }
        }

        private void Zad4_Click(object sender, EventArgs e)
        {
            if (inputComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select an item");
                return;
            }

            string propertyName = inputComboBox.SelectedItem.ToString();
            string searchValue = filtersTextBox.Text;

            if (searchValue == "")
            {
                MessageBox.Show("Please enter a value to search for");
                return;
            }
            else
            {
                outputComboBox.Items.Clear();
                zad4Grid.filter(propertyName, searchValue, myCarsBindingList);
                foreach (string str in zad4Grid.output)
                {
                    outputComboBox.Items.Add(str);
                }
            }
        }


        public class FindInDataGrid<T>
        {
            public BindingList<T> myItems { get; set; }
            public List<string> properties { get; set; }
            public List<string> output { get; set; }
            public FindInDataGrid(BindingList<T> items)
            {
                myItems = new BindingList<T>(items);
                properties = new List<string>();
                output = new List<string>();

                LoadProperties(typeof(T), "");
            }

            public void LoadProperties(Type type, string prefix)
            {
                foreach (var prop in type.GetProperties())
                {
                    if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(int) || prop.PropertyType == typeof(double))
                    {
                        properties.Add(prefix + prop.Name);
                    }
                    else
                    {
                        LoadProperties(prop.PropertyType, prefix + prop.Name + ".");
                    }
                }
            }

            public void filter(string propertyName, string searchValue, BindingList<T> myItemList)
            {
                myItems = myItemList;

                output.Clear();

                var foundItems = myItems.ToList().FindAll(item =>
                {
                    var propertyNames = propertyName.Split('.');
                    object propertyValue = item;
                    foreach (var name in propertyNames)
                    {
                        if (propertyValue == null)
                        {
                            return false;
                        }
                        propertyValue = propertyValue.GetType().GetProperty(name)?.GetValue(propertyValue, null);
                    }
                    return propertyValue?.ToString() == searchValue;
                });
                foreach (var item in foundItems)
                {
                    output.Add(item.ToString());
                }
            }
        }
    }

}
