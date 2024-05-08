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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace lab11
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Tasks_OnClick(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(textBoxK.Text, out int K))
            {
                MessageBox.Show("Nieprawidłowa wartość dla K");
                return;
            }

            if (!int.TryParse(textBoxN.Text, out int N))
            {
                MessageBox.Show("Nieprawidłowa wartość dla N");
                return;
            }

            if (K>N)
            {
                MessageBox.Show("K musi być mniejsze od N");
                return;
            }

            Task<int> taskLicznik = Task.Factory.StartNew<int>(() =>
            {

                int res = 1;
                for (int i = (N - K + 1); i <= N; i++)
                {
                    res *= i;
                }
                return res;
            });

            Task<int> taskMianownik = Task.Factory.StartNew<int>(() =>
            {
                int res = 1;
                for (int i = 1; i <= K; i++)
                {
                    res *= i;
                }
                return res;
            });

            int wynik = taskLicznik.Result / taskMianownik.Result;
            TasksTextBox.Text = wynik.ToString();
        }

        private static int Licznik(int K, int N)
        {
            int res = 1;
            for (int i = (N - K + 1); i <= N; i++)
            {
                res *= i;
            }
            return res;
        }

        public static int Mianownik(int K)
        {
            int res = 1;
            for (int i = 1; i <= K; i++)
            {
                res *= i;
            }
            return res;
        }
        public void Delegates_OnClick(object sender, RoutedEventArgs e)
        {
            Func<int, int, int> opLicznik = Licznik;
            Func<int, int> opMianownik = Mianownik;

            IAsyncResult resLicznik = opLicznik.BeginInvoke(int.Parse(textBoxK.Text), int.Parse(textBoxN.Text), null, null);
            IAsyncResult resMianownik = opMianownik.BeginInvoke(int.Parse(textBoxK.Text), null, null);

            int wynik = opLicznik.EndInvoke(resLicznik) / opMianownik.EndInvoke(resMianownik);

            DelegatesTextBox.Text = wynik.ToString();
        }

        private Task<int> MianownikAsync(int K)
        {
            return Task.Run(() =>
            {
                int res = 1;
                for (int i = 1; i <= K; i++)
                {
                    res *= i;
                }
                return res;
            });
        }

        private Task<int> LicznikAsync(int K, int N)
        {
            return Task.Run(() =>
            {
                int res = 1;
                for (int i = (N - K + 1); i <= N; i++)
                {
                    res *= i;
                }
                return res;
            });
        }

        public async void Async_OnClick(object sender, RoutedEventArgs e)
        {
            int K = int.Parse(textBoxK.Text);
            int N = int.Parse(textBoxN.Text);

            Task<int> taskLicznik = LicznikAsync(K, N);
            Task<int> taskMianownik = MianownikAsync(K);

            int wynik = await taskLicznik / await taskMianownik;

            AsyncTextBox.Text = wynik.ToString();
        }
    }
}
