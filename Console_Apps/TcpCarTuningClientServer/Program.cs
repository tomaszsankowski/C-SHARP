using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;


namespace TcpCarTuningClientServer
{
    class Program
    {
        public class Car(double horsePower, string model, string manufacturer, int year)
        {
            public double HorsePower { get; set; } = horsePower;
            public string Model { get; set; } = model;
            public string Manufacturer { get; set; } = manufacturer;
            public int Year { get; set; } = year;

            public override string ToString()
            {
                return $"HorsePower: {HorsePower}, Model: {Model}, Manufacturer: {Manufacturer}, Year: {Year}";
            }
        }

        public class Client
        {
            private TcpClient? client;
            private NetworkStream? stream;

            public void Connect(string server, int port)
            {
                client = new TcpClient(server, port);
                stream = client.GetStream();
            }

            public void SendObject(Car car)
            {
                if (stream != null && stream.CanWrite)
                {
                    Console.WriteLine("Sending car to tune: " + car);

                    JsonSerializerSettings settings = new()
                    {
                        TypeNameHandling = TypeNameHandling.All
                    };

                    string jsonedCar = JsonConvert.SerializeObject(car, settings);

                    byte[] byteMessage = Encoding.ASCII.GetBytes(jsonedCar);
                    stream.Write(byteMessage, 0, byteMessage.Length);

                    Thread.Sleep(1000);
                }
            }

            public Car? ReadResponse()
            {
                string car = string.Empty;

                if (stream != null && client != null && stream.CanRead)
                {
                    byte[] bytes = new byte[client.ReceiveBufferSize];
                    stream.Read(bytes, 0, (int)client.ReceiveBufferSize);
                    car = Encoding.ASCII.GetString(bytes);
                }

                JsonSerializerSettings settings = new()
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                Car? outputCar = JsonConvert.DeserializeObject<Car>(car, settings);

                Console.WriteLine("OMG my car was tuned: " + outputCar);

                Thread.Sleep(1000);
                
                return outputCar;

            }


            public void Disconnect()
            {
                stream?.Close();
                client?.Close();
            }
        }


        public class Server
        {
            private TcpListener? tcpListener;
            private volatile bool isRunning;

            private readonly List<TcpClient> clients = [];

            public void Start(int port)
            {
                tcpListener = new TcpListener(IPAddress.Loopback, port);
                tcpListener.Start();

                isRunning = true;

                Thread listenThread = new(new ThreadStart(ListenForClients));
                listenThread.Start();

                Console.WriteLine("Press any key to disconnect all clients...");
                Console.ReadKey();


                isRunning = false;
                listenThread.Interrupt();
                tcpListener.Stop();

                lock (clients)
                {
                    foreach (var client in clients)
                    {
                        client.Close();
                    }

                    clients.Clear();
                }
            }

            private void ListenForClients()
            {
                while (isRunning)
                {
                    try
                    {
                        if (tcpListener != null)
                        {
                            TcpClient client = tcpListener.AcceptTcpClient();
                            lock (clients)
                            {
                                clients.Add(client);
                            }

                            Thread clientThread = new(new ParameterizedThreadStart(HandleClientComm));
                            clientThread.Start(client);
                        }
                    }
                    catch (SocketException)
                    {
                        if (!isRunning)
                        {
                            break;
                        }
                        throw;
                    }
                    catch (ThreadInterruptedException)
                    {
                        break;
                    }
                }
            }


            private void HandleClientComm(object? client)
            {
                TcpClient? tcpClient = (TcpClient?)client;
                if (tcpClient != null)
                {
                    NetworkStream clientStream = tcpClient.GetStream();

                    byte[] message = new byte[4096];
                    int bytesRead;

                    while (true)
                    {
                        try
                        {
                            bytesRead = clientStream.Read(message, 0, 4096);
                        }
                        catch
                        {
                            break;
                        }

                        if (bytesRead == 0)
                        {
                            break;
                        }

                        string receivedMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
                        Car? receivedCar = JsonConvert.DeserializeObject<Car>(receivedMessage);

                        if (receivedCar != null)
                        {
                            Console.WriteLine("New car to tune: " + receivedCar);
                            receivedCar.HorsePower *= 1.5;
                            receivedCar.Manufacturer = "Manhard";

                            JsonSerializerSettings settings = new()
                            {
                                TypeNameHandling = TypeNameHandling.All
                            };

                            string jsonedCar = JsonConvert.SerializeObject(receivedCar, settings);

                            byte[] responseBytes = Encoding.ASCII.GetBytes(jsonedCar);

                            Thread.Sleep(1000);

                            Console.WriteLine("Sending tuned car: " + receivedCar);

                            clientStream.Write(responseBytes, 0, responseBytes.Length);

                            Thread.Sleep(1000);
                        }
                    }
                    tcpClient.Close();
                }
            }
        }


        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "server":
                        Server server = new();
                        server.Start(5000);
                        break;
                    case "client":
                        Car car = new(560, "M5", "BMW", 2015);
                        Car car1 = new(60, "Fabia", "Skoda", 2008);
                        Car car2 = new(130, "Focus", "Ford", 2017);
                        Car car3 = new(250, "A6", "Audi", 2005);
                        Car car4 = new(400, "A8", "Audi", 2023);
                        Car[] cars = [car, car1, car2, car3, car4];
                        Client client = new();
                        client.Connect("localhost", 5000);
                        foreach (var c in cars)
                        {
                            client.SendObject(c);
                            Car? response = client.ReadResponse();
                        }
                        client.Disconnect();
                        break;
                    default:
                        Console.WriteLine("Nieznany argument: " + args[0]);
                        break;
                }
            }
            else
            {
                Console.WriteLine("Podaj argument 'server' lub 'client'");
            }
        }
    }

}
