using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _240308_Notiz_App_Client
{
    /// <summary>
    /// Interaktionslogik für Notiz.xaml
    /// </summary>
    public partial class Notiz : Window
    {
        public string id { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public bool check { get; set; }

        public Notiz()
        {
            InitializeComponent();
        }

        private string crlf = "\n";

        public Notiz(string timestamp, string text, bool check)
        {
            InitializeComponent();
            title = timestamp;
            this.text = text.Replace(";", crlf);
            this.check = check;
            textfield.Text = text;

            Closing += Notiz_Closing;
        }

        public Notiz(string id, string timestamp, string text, bool check)
        {
            InitializeComponent();
            this.id = id;
            title = timestamp;
            this.text = text.Replace(";", crlf);
            this.check = check;
            textfield.Text = text;

            Closing += Notiz_Closing;
        }

        public string gsid
        {
            get { return id; }
            set { id = value; }
        }

        public string gstitle
        {
            get { return title; }
            set { title = value; }
        }

        public string gstext
        {
            get { return text.Replace(crlf, ";"); }
            set { text = value; }
        }

        public bool gscheck
        {
            get { return check; }
            set { check = value; }
        }

        private async void textfield_TextChanged(object sender, TextChangedEventArgs e)
        {
            text = textfield.Text;
            //MainWindow m = new MainWindow();
            //m.TextChanged(text, this);
            
        }

        private async Task SendPutRequest(string json)
        {
            try
            {
                // URL des Spring Boot-Endpunkts
                string url = "http://10.10.3.7:8080/api/notiz";

                // Daten, die Sie im Request-Body senden möchten
                //string jsonData = "{\"key1\":\"value1\",\"key2\":\"value2\"}";

                // Erstellen des HttpClient-Objekts
                using (HttpClient client = new HttpClient())
                {
                    // Konfigurieren des Request-Headers (falls erforderlich)
                    // client.DefaultRequestHeaders.Add("HeaderName", "HeaderValue");

                    // Erstellen des HttpRequestMessage-Objekts
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);

                    // Hinzufügen der Daten zum Request-Body
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Senden der Anfrage und Warten auf die Antwort
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Überprüfen des Antwortstatus
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("PUT-Anfrage erfolgreich gesendet.");
                        // Wenn Sie auf die Antwort zugreifen möchten:
                        // string responseBody = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Console.WriteLine($"Fehler bei der PUT-Anfrage. Statuscode: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der PUT-Anfrage: {ex.Message}");
            }
        }

        private async void Notiz_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //string json = $"{{\"id\":\"{id}\",\"title\":\"{title}\",\"text\":\"{text}\",\"check\":{check.ToString().ToLower()}}}";
            //string json = JsonSerializer.Serialize(this);
            string extratext = text.Replace(crlf, ";");
            string json = $"{{\"id\":\"{id}\",\"title\":\"{title}\",\"text\":\"{extratext}\",\"check\":{check.ToString().ToLower()}}}";
            //string json = JsonConvert.SerializeObject(this);
            await SendPutRequest(json);
        }

        public void Replace()
        {
            text = text.Replace(";", crlf);
        }
    }
}
