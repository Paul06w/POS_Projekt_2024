using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _240308_Notiz_App_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Notiz> notizen = new List<Notiz>();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        public async Task LoadData()
        {
            string url = "http://10.10.3.7:8080/api/notizen";

            notizen = await GetNotizenAsync(url);
            foreach (Notiz notiz in notizen)
            {
                AddButton(notiz.gstitle);
            }
        }

        public async Task<List<Notiz>> GetNotizenAsync(string url)
        {
            List<Notiz> notizen = new List<Notiz>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    // Deserialisieren des JSON-Strings in eine Liste von Notiz-Objekten
                    notizen = JsonSerializer.Deserialize<List<Notiz>>(responseData);
                }
                else
                {
                    Console.WriteLine("Fehler beim Senden der GET-Anfrage: " + response.StatusCode);
                }
            }

            return notizen;
        }




        private void ButtonNeueNotiz_Click(object sender, RoutedEventArgs e)
        {
            string timestamp = DateTime.Now.ToString("dd.MM.yyyy, HH:mm");

            Notiz n = new Notiz(timestamp, "", false);
            n.Show();

            AddButton(DateTime.Now.ToString(timestamp));
            notizen.Add(n);

        }

        private void AddButton(string buttonText)
        {
            Button button = new Button
            {
                Content = buttonText,
                Width = 150,
                Height = 30,
                Margin = new Thickness(5)
            };

            // Füge einen Event Handler für den Button-Click hinzu
            button.Click += ButtonVorhandeneNotiz_Click;

            // Füge den Button zum StackPanel in der ScrollView hinzu
            WrapPanelNotizen.Children.Add(button);
        }

        private void ButtonVorhandeneNotiz_Click(object sender, RoutedEventArgs e)
        {
            //Notiz n = new Notiz();

            Button button = sender as Button;
            List<Notiz> notizenneu = new List<Notiz>();
            //notizenneu = notizen;

            if (button != null)
            {
                /*foreach (Notiz n in notizenneu)
                {
                    if (button.Content.ToString() == n.gstitle)
                    {
                        Notiz no = new Notiz(n.gstitle, n.gstext, n.gscheck);
                        no.Show();
                        notizen.Remove(n);
                        notizen.Add(no);
                    }
                }*/

                foreach (Notiz n in notizen)
                {
                    if (button.Content.ToString() == n.gstitle)
                    {
                        Notiz no = new Notiz(n.gstitle, n.gstext, n.gscheck);
                        no.Show();
                        notizenneu.Add(no); // Hinzufügen zur neuen Liste
                    }
                    else
                    {
                        notizenneu.Add(n); // Hinzufügen zur neuen Liste ohne Änderung
                    }
                }

                // Entfernen der Elemente aus der ursprünglichen Liste
                /*foreach (Notiz n in notizenneu)
                {
                    notizen.Remove(n);
                }*/

                // Hinzufügen der neuen Elemente zur ursprünglichen Liste
                //notizen.AddRange(notizenneu);
                
                notizen = notizenneu;
            }

        }
    }
}