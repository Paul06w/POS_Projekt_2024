using System.Net.Http;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        public static string serverAddress = "192.168.9.66";

        public MainWindow()
        {
            InitializeComponent();

            LoadData();
        }

        public async Task LoadData()
        {
            notizen = await GetNotizenAsync();
            foreach (Notiz n in notizen)
            {
                n.Replace();
            }

            foreach (Notiz notiz in notizen)
            {
                AddButton(notiz.gstitle);
                
                //Funktion zum finden der Checkboxen und checken, wenn der Wert in der Datenbank true ist
                if(notiz.gscheck == true)
                {
                    WrapPanel wp = WrapPanelNotizen as WrapPanel;
                    if (wp != null)
                    {
                        foreach (var child in wp.Children)      //Durchlaufen der untergeordneten Elemente des Panels um Button zu finden
                        {
                            if (child is StackPanel)
                            {
                                StackPanel stackp = child as StackPanel;

                                foreach(var childsp in stackp.Children)
                                {
                                    if (childsp is Button)
                                    {
                                        Button b = childsp as Button;

                                        if(b.Content != null)
                                        {
                                            if (b.Content.ToString() == notiz.gstitle)
                                            {
                                                foreach (var child2 in stackp.Children)
                                                {
                                                    if (child2 is CheckBox)
                                                    {
                                                        CheckBox c = child2 as CheckBox;

                                                        c.IsChecked = true;
                                                    }
                                                }
                                            }
                                        }                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void CheckNotiz()
        {

        }

        public async Task<List<Notiz>> GetNotizenAsync()
        {
            List<Notiz> notizen = new List<Notiz>();
            string url = $"http://{serverAddress}:8080/api/notizen";

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    //responseData = responseData.Replace(";", "\r\n");

                    notizen = JsonSerializer.Deserialize<List<Notiz>>(responseData);        // Deserialisieren des JSON-Strings in eine Liste von Notiz-Objekten
                    foreach (Notiz n in notizen)
                    {
                        n.Replace();
                    }
                }
                else
                {
                    Console.WriteLine("Fehler beim Senden der GET-Anfrage: " + response.StatusCode);
                }
            }

            return notizen;
        }




        private async void ButtonNeueNotiz_Click(object sender, RoutedEventArgs e)
        {
            string timestamp = DateTime.Now.ToString("dd.MM.yyyy, HH:mm:ss");

            Notiz n = new Notiz(timestamp, "", false);
            //n.Show();

            string title = DateTime.Now.ToString(timestamp);
            string text = "";
            bool check = false;

            string json = $"{{\"title\":\"{title}\",\"text\":\"{text}\",\"check\":{check.ToString().ToLower()}}}";
            await SendPostRequest(json);

            AddButton(DateTime.Now.ToString(timestamp));
            //notizen.Add(n);
            notizen.Clear();
            notizen = await GetNotizenAsync();

        }

        private async void AddButton(string buttonText)
        {
            StackPanel panel = new StackPanel           //StackPanel für horizontale Steuerelemente 
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5),
                //Background = new SolidColorBrush(Colors.LightBlue),
            };

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("./notiz.png", UriKind.RelativeOrAbsolute));

            Button button = new Button
            {
                Content = buttonText,
                Background = brush,
                Width = 150,
                Height = 150,
                Margin = new Thickness(3, 3, 5, 3),             //Abstand zu nächsten Steuerelementen
                BorderBrush = new SolidColorBrush(Colors.White)
            };

            ControlTemplate buttonTemplate = (ControlTemplate)FindResource("NoMouseOverButtonTemplate");
            button.Template = buttonTemplate;

            button.Click += ButtonVorhandeneNotiz_Click;        //Event Handler

            Image image = new Image
            {
                Source = new BitmapImage(new Uri("/notiz.png", UriKind.RelativeOrAbsolute)),
                Width = 50,
                Height = 50,
            };

            image.MouseLeftButtonUp += ButtonVorhandeneNotiz_Click;
            image.MouseEnter += ButtonVorhandeneNotiz_Click;

            CheckBox checkBox = new CheckBox
            {
                //Content = "Check",
                Margin = new Thickness(0, 0, 5, 0)          //Abstand zu nächsten Steuerelementen
            };

            //checkBox.Checked += CheckBox_Checked;
            checkBox.Click += CheckBox_Checked;

            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri("./bin.png", UriKind.RelativeOrAbsolute));
            
            Button trashButton = new Button
            {
                Width = 30,
                Height = 30,
                Background = myBrush,
                BorderBrush = new SolidColorBrush(Colors.White)
            };

            trashButton.Template = buttonTemplate;
            trashButton.Click += TrashButton_Click;

            System.Windows.Controls.Image trashImage = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri("./bin2.jpg", UriKind.RelativeOrAbsolute)),
            };

            //Steuerelemente zum StackPanel
            panel.Children.Add(button);
            panel.Children.Add(checkBox);
            panel.Children.Add(trashButton);

            //StackPanel zu WrapPanel
            WrapPanelNotizen.Children.Add(panel);
        }

        private async Task SendPostRequest(string json)
        {
            try
            {
                string url = $"http://{serverAddress}:8080/api/notiz";              //URL des Spring Boot-Endpunkts

                using (HttpClient client = new HttpClient())                        //Erstellen des HttpClient-Objekts
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);      //Erstellen des HttpRequestMessage-Objekts

                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");   //Hinzufügen der Daten zum Request-Body

                    HttpResponseMessage response = await client.SendAsync(request);                 //Senden der Anfrage und Warten auf die Antwort

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("POST-Anfrage erfolgreich gesendet.");
                    }
                    else
                    {
                        Console.WriteLine($"Fehler bei der POST-Anfrage. Statuscode: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der POST-Anfrage: {ex.Message}");
            }
        }



        private async Task SendPutRequest(string json)
        {
            try
            {
                string url = $"http://{serverAddress}:8080/api/notiz";                  //URL des Spring Boot-Endpunkts

                using (HttpClient client = new HttpClient())                            //Erstellen des HttpClient-Objekts
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, url);       //Erstellen des HttpRequestMessage-Objekts

                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");   //Hinzufügen der Daten zum Request-Body

                    HttpResponseMessage response = await client.SendAsync(request);                 //Senden der Anfrage und Warten auf die Antwort

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("PUT-Anfrage erfolgreich gesendet.");
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



        private async void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            Button c = sender as Button;
            StackPanel parentPanel = VisualTreeHelper.GetParent(c) as StackPanel;

            if (parentPanel != null)
            {
                foreach (var child in parentPanel.Children)
                {
                    if (child is Button)
                    {
                        Button button = child as Button;

                        foreach (Notiz n in notizen)
                        {
                            if (button.Content.ToString() == n.gstitle)
                            {
                                await SendDeleteRequest(n.gsid);
                            }
                        }
                        break;
                    }
                }
            }

            parentPanel.Visibility = Visibility.Collapsed;
        }


        private async Task SendDeleteRequest(string id)
        {
            try
            {
                string url = $"http://{serverAddress}:8080/api/notiz/" + id;            //URL des Spring Boot-Endpunkts

                using (HttpClient client = new HttpClient())                            //Erstellen des HttpClient-Objekts
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);    //Erstellen des HttpRequestMessage-Objekts

                    HttpResponseMessage response = await client.SendAsync(request);                 //Senden der Anfrage und Warten auf die Antwort

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("DELETE-Anfrage erfolgreich gesendet.");
                    }
                    else
                    {
                        Console.WriteLine($"Fehler bei der DELETE-Anfrage. Statuscode: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Senden der DELETE-Anfrage: {ex.Message}");
            }
        }

        private async void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox c = sender as CheckBox;
            StackPanel parentPanel = VisualTreeHelper.GetParent(c) as StackPanel;

            if (parentPanel != null)
            {
                foreach (var child in parentPanel.Children)
                {
                    if (child is Button)
                    {
                        Button button = child as Button;
                        
                        if (c.IsChecked == true)
                        {
                            //button.IsEnabled = false;
                            foreach(Notiz n in notizen)
                            {
                                if(n.gstitle == button.Content.ToString())
                                {
                                    n.gscheck = true;
                                    string json = $"{{\"id\":\"{n.gsid}\",\"title\":\"{n.gstitle}\",\"text\":\"{n.gstext}\",\"check\":{n.gscheck.ToString().ToLower()}}}";
                                    await SendPutRequest(json);
                                }
                            }
                        }
                        else
                        {
                            //button.IsEnabled = true;
                            foreach (Notiz n in notizen)
                            {
                                if (n.gstitle == button.Content.ToString())
                                {
                                    n.gscheck = false;
                                    string json = $"{{\"id\":\"{n.gsid}\",\"title\":\"{n.gstitle}\",\"text\":\"{n.gstext}\",\"check\":{n.gscheck.ToString().ToLower()}}}";
                                    await SendPutRequest(json);
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }


        private void ButtonVorhandeneNotiz_Click(object sender, RoutedEventArgs e)
        {
            //Notiz n = new Notiz();

            Button button = sender as Button;
            List<Notiz> notizenneu = new List<Notiz>();
            //notizenneu = notizen;

            if (button != null)
            {
                foreach (Notiz n in notizen)
                {
                    if (button.Content.ToString() == n.gstitle)
                    {
                        Notiz no = new Notiz(n.gsid, n.gstitle, n.gstext.Replace(";", "\r\n"), n.gscheck);
                        no.Show();
                        notizenneu.Add(no); //Hinzufügen zur neuen Liste
                    }
                    else
                    {
                        notizenneu.Add(n); //Hinzufügen zur neuen Liste ohne Änderung
                    }
                }                
                notizen = notizenneu;
            }

        }


        public async void TextChanged(string text, Notiz notiz)
        {
            string json = $"{{\"id\":\"{notiz.gsid}\",\"title\":\"{notiz.gstitle}\",\"text\":\"{notiz.gstext}\",\"check\":{notiz.gscheck.ToString().ToLower()}}}";
            await SendPutRequest(json);
        }
    }
}