using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Numerics;
using System.IO.Compression;
using System.Windows.Forms;
using System.Net;

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

        private (int, int) GetKandN()
        {
            if (!int.TryParse(textBoxK.Text, out int K))
            {
                System.Windows.MessageBox.Show("Nieprawidłowa wartość dla K");
                return (-1, -1);
            }

            if (!int.TryParse(textBoxN.Text, out int N))
            {
                System.Windows.MessageBox.Show("Nieprawidłowa wartość dla N");
                return (-1, -1);
            }

            if (K > N)
            {
                System.Windows.MessageBox.Show("K musi być mniejsze od N");
                return (-1, -1);
            }

            return (K, N);
        }

        public void Tasks_OnClick(object sender, RoutedEventArgs e)
        {
            (int K, int N) = GetKandN();

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

        private static int Mianownik(int K)
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
            (int K, int N) = GetKandN();

            Func<int, int, int> opLicznik = Licznik;
            Func<int, int> opMianownik = Mianownik;

            IAsyncResult resLicznik = opLicznik.BeginInvoke(int.Parse(textBoxK.Text), int.Parse(textBoxN.Text), null, null);
            IAsyncResult resMianownik = opMianownik.BeginInvoke(int.Parse(textBoxK.Text), null, null);

            int wynik = opLicznik.EndInvoke(resLicznik) / opMianownik.EndInvoke(resMianownik);

            DelegatesTextBox.Text = wynik.ToString();
        }

        private static Task<int> MianownikAsync(int K)
        {
            int res = 1;
            for (int i = 1; i <= K; i++)
            {
                res *= i;
            }
            return Task.FromResult(res);
        }

        private static Task<int> LicznikAsync(int K, int N)
        {
            int res = 1;
            for (int i = (N - K + 1); i <= N; i++)
            {
                res *= i;
            }
            return Task.FromResult(res);
        }

        public async Task Async_OnClick(object sender, RoutedEventArgs e)
        {
            (int K, int N) = GetKandN();

            Task<int> licznikTask = LicznikAsync(K, N);
            Task<int> mianownikTask = MianownikAsync(K);

            await Task.WhenAll(licznikTask, mianownikTask);

            int wynik = licznikTask.Result / mianownikTask.Result;

            AsyncTextBox.Text = wynik.ToString();
        }


        public void Fibonacci_OnClick(object sender, RoutedEventArgs e)
        {
            int n = int.Parse(FibonacciTextBox.Text);
            if (n < 0)
            {
                System.Windows.MessageBox.Show("Nieprawidłowa wartość dla N");
                return;
            }

            progressBar.Minimum = 0;
            progressBar.Maximum = n;

            BackgroundWorker bw = new BackgroundWorker();

            bw.DoWork += (s, args) =>
            {
                BackgroundWorker worker = s as BackgroundWorker;
                BigInteger fibNMinus2 = 0;
                BigInteger fibNMinus1 = 1;
                BigInteger fibN = 0;
                for (int i = 0; i <= n; i++)
                {
                    if (bw.CancellationPending)
                    {
                        args.Cancel = true;
                        break;
                    }

                    if (i >= 2)
                    {
                        fibN = fibNMinus2 + fibNMinus1;
                        fibNMinus2 = fibNMinus1;
                        fibNMinus1 = fibN;
                    }
                    else
                    {
                        fibN = i;
                    }

                    Thread.Sleep(5); // Spowolnienie pętli

                    worker.ReportProgress(i);
                }
                args.Result = fibN; // Assign the final result to args.Result
            };
            bw.ProgressChanged += (s, args) =>
            {
                progressBar.Value = args.ProgressPercentage;
            };
            bw.RunWorkerCompleted += (s, args) =>
            {
                if (args.Cancelled)
                {
                    System.Windows.MessageBox.Show("Operacja anulowana");
                }
                else
                {
                    FibonacciResult.Text = args.Result.ToString();
                }
            };

            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync(n);
        }

        public void Decompress_OnClick(object sender, RoutedEventArgs e)
        {
            var dir = new FolderBrowserDialog();

            var result = dir.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string directoryPath = dir.SelectedPath;
                var files = System.IO.Directory.GetFiles(directoryPath, "*.gz");

                Parallel.ForEach(files, (file) =>
                {
                    using (var compressedFileStream = System.IO.File.OpenRead(file))
                    {
                        string targetFileName = System.IO.Path.GetFileNameWithoutExtension(file);

                        using (var decompressedFileStream = System.IO.File.Create(System.IO.Path.Combine(directoryPath, targetFileName)))
                        {
                            using (var decompressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
                            {
                                decompressionStream.CopyTo(decompressedFileStream);
                            }
                        }
                    }
                   System.IO.File.Delete(file);
                });
            }
        }


        public void Compress_OnClick(object sender, RoutedEventArgs e)
        {
            var dir = new FolderBrowserDialog();

            var result = dir.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string directoryPath = dir.SelectedPath;
                var files = System.IO.Directory.GetFiles(directoryPath);

                Parallel.ForEach(files, file =>
                {
                    using (var originalFileStream = System.IO.File.OpenRead(file))
                    {
                        using (var compressedFileStream = System.IO.File.Create(file + ".gz"))
                        {
                            using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                            {
                                originalFileStream.CopyTo(compressionStream);
                            }
                        }
                    }
                    System.IO.File.Delete(file);
                });
            }
        }

        public void Resolve_OnClick(object sender, RoutedEventArgs e)
        {
            string[] hostNames = { "www.microsoft.com", "www.apple.com",
            "www.google.com", "www.ibm.com", "cisco.netacad.net",
            "www.oracle.com", "www.nokia.com", "www.hp.com", "www.dell.com",
            "www.samsung.com", "www.toshiba.com", "www.siemens.com",
            "www.amazon.com", "www.sony.com", "www.canon.com", "www.alcatel-lucent.com",
            "www.acer.com", "www.motorola.com" };

            Parallel.ForEach(hostNames, hostName =>
            {
                var host = Dns.GetHostEntry(hostName);
                System.Windows.Application.Current.Dispatcher.BeginInvoke((Action)(() =>
                {
                    IPtextBox.AppendText(hostName + " => " + host.AddressList.First() + "\n");
                }));
            });
        }
    }
}
