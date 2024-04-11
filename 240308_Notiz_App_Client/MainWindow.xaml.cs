using System.Net.Http;
using System.Reflection;
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

            /*ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri("./bin2.jpg", UriKind.RelativeOrAbsolute));
            this.Background = myBrush;*/

            LoadData();
        }

        public async Task LoadData()
        {
            string url = "http://10.10.3.7:8080/api/notizen";

            notizen = await GetNotizenAsync(url);
            foreach (Notiz notiz in notizen)
            {
                AddButton(notiz.gstitle);
                
                
                if(notiz.gscheck == true)
                {
                    WrapPanel wp = WrapPanelNotizen as WrapPanel;
                    if (wp != null)
                    {
                        // Durchlaufen Sie die untergeordneten Elemente des Panels, um den Button zu finden
                        foreach (var child in wp.Children)
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
                                                    if (child is CheckBox)
                                                    {
                                                        CheckBox c = child as CheckBox;

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




        private async void ButtonNeueNotiz_Click(object sender, RoutedEventArgs e)
        {
            string timestamp = DateTime.Now.ToString("dd.MM.yyyy, HH:mm:ss");

            Notiz n = new Notiz(timestamp, "", false);
            n.Show();

            string title = DateTime.Now.ToString(timestamp);
            string text = "";
            bool check = false;

            string json = $"{{\"title\":\"{title}\",\"text\":\"{text}\",\"check\":{check.ToString().ToLower()}}}";
            await SendPostRequest(json);

            AddButton(DateTime.Now.ToString(timestamp));
            //notizen.Add(n);
            notizen.Clear();
            notizen = await GetNotizenAsync("http://10.10.3.7:8080/api/notizen");

        }

        private async void AddButton(string buttonText)
        {
            /*
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
            */



            // Erstelle ein StackPanel, um die Steuerelemente horizontal anzuordnen
            StackPanel panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(5),
                //Background = new SolidColorBrush(Colors.LightBlue),
            };

            ImageBrush brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("./notiz.png", UriKind.RelativeOrAbsolute));

            // Erstelle den Button
            Button button = new Button
            {
                Content = buttonText,
                Background = brush,
                Width = 150,
                Height = 150,
                Margin = new Thickness(3, 3, 5, 3), // Füge einen Abstand zum nächsten Steuerelement hinzu
                BorderBrush = new SolidColorBrush(Colors.White)
            };

            ControlTemplate buttonTemplate = (ControlTemplate)FindResource("NoMouseOverButtonTemplate");
            button.Template = buttonTemplate;

            // Füge einen Event Handler für den Button-Click hinzu
            button.Click += ButtonVorhandeneNotiz_Click;


            Image image = new Image
            {
                Source = new BitmapImage(new Uri("/notiz.png", UriKind.RelativeOrAbsolute)),
                Width = 50,
                Height = 50,
            };

            image.MouseLeftButtonUp += ButtonVorhandeneNotiz_Click;
            image.MouseEnter += ButtonVorhandeneNotiz_Click;


            // Erstelle die CheckBox
            CheckBox checkBox = new CheckBox
            {
                //Content = "Check",
                Margin = new Thickness(0, 0, 5, 0) // Füge einen Abstand zum nächsten Steuerelement hinzu
            };

            //checkBox.Checked += CheckBox_Checked;
            checkBox.Click += CheckBox_Checked;

            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri("./bin.png", UriKind.RelativeOrAbsolute));
            // Erstelle den Button für den Mülleimer
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

            // Füge die Steuerelemente zum StackPanel hinzu
            //panel.Children.Add(image);
            panel.Children.Add(button);
            panel.Children.Add(checkBox);
            panel.Children.Add(trashButton);
            //panel.Children.Add(trashImage);


            // Füge das StackPanel zum übergeordneten Panel (WrapPanelNotizen) hinzu
            WrapPanelNotizen.Children.Add(panel);


        }

        private async Task SendPostRequest(string json)
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
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

                    // Hinzufügen der Daten zum Request-Body
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Senden der Anfrage und Warten auf die Antwort
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Überprüfen des Antwortstatus
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("POST-Anfrage erfolgreich gesendet.");
                        // Wenn Sie auf die Antwort zugreifen möchten:
                        // string responseBody = await response.Content.ReadAsStringAsync();
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
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);

                    // Hinzufügen der Daten zum Request-Body
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    // Senden der Anfrage und Warten auf die Antwort
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Überprüfen des Antwortstatus
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("POST-Anfrage erfolgreich gesendet.");
                        // Wenn Sie auf die Antwort zugreifen möchten:
                        // string responseBody = await response.Content.ReadAsStringAsync();
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



        private async void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            Button c = sender as Button;
            StackPanel parentPanel = VisualTreeHelper.GetParent(c) as StackPanel;

            
            /*foreach(Notiz n in notizen)
            {
                if (c.Content.ToString() == n.gstitle)
                {
                    await SendDeleteRequest(n.gsid);
                }
            }*/

            if (parentPanel != null)
            {
                // Durchlaufen Sie die untergeordneten Elemente des Panels, um den Button zu finden
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

                        // Hier haben Sie Zugriff auf den Button
                        // Führen Sie hier die gewünschten Aktionen mit dem Button aus
                        break; // Beenden Sie die Schleife, sobald der Button gefunden wurde
                    }
                }
            }


            parentPanel.Visibility = Visibility.Collapsed;

            /*if (parentPanel != null)
            {
                // Durchlaufen Sie die untergeordneten Elemente des Panels, um den Button zu finden
                foreach (var child in parentPanel.Children)
                {
                    if (child is Button)
                    {
                        Button button = child as Button;

                        

                        button.Visibility = Visibility.Collapsed;

                        // Hier haben Sie Zugriff auf den Button
                        // Führen Sie hier die gewünschten Aktionen mit dem Button aus
                        break; // Beenden Sie die Schleife, sobald der Button gefunden wurde
                    }
                }
            }*/
        }


        private async Task SendDeleteRequest(string id)
        {
            try
            {
                // URL des Spring Boot-Endpunkts
                string url = "http://10.10.3.7:8080/api/notiz/" + id; // Beispiel-URL

                // Erstellen des HttpClient-Objekts
                using (HttpClient client = new HttpClient())
                {
                    // Erstellen des HttpRequestMessage-Objekts
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, url);

                    // Senden der Anfrage und Warten auf die Antwort
                    HttpResponseMessage response = await client.SendAsync(request);

                    // Überprüfen des Antwortstatus
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

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox c = sender as CheckBox;
            StackPanel parentPanel = VisualTreeHelper.GetParent(c) as StackPanel;

            if (parentPanel != null)
            {
                // Durchlaufen Sie die untergeordneten Elemente des Panels, um den Button zu finden
                foreach (var child in parentPanel.Children)
                {
                    if (child is Button)
                    {
                        Button button = child as Button;
                        
                        if (c.IsChecked == true)
                        {
                            button.IsEnabled = false;
                        }
                        else
                        {
                            button.IsEnabled = true;
                        }
                        
                        // Hier haben Sie Zugriff auf den Button
                        // Führen Sie hier die gewünschten Aktionen mit dem Button aus
                        break; // Beenden Sie die Schleife, sobald der Button gefunden wurde
                    }
                }
            }
        }



        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            // Wenn das übergeordnete Element null ist oder nicht vom angegebenen Typ ist, durchsuchen Sie weiter
            if (parentObject == null || parentObject is T)
                return (T)parentObject;
            else
                return FindParent<T>(parentObject);
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