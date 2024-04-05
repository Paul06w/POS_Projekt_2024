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
            string timestamp = DateTime.Now.ToString("dd.MM.yyyy, HH:mm:ss");

            Notiz n = new Notiz(timestamp, "", false);
            n.Show();

            AddButton(DateTime.Now.ToString(timestamp));
            notizen.Add(n);

        }

        private void AddButton(string buttonText)
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
                Background = new SolidColorBrush(Colors.LightBlue),
            };

            // Erstelle den Button
            Button button = new Button
            {
                Content = buttonText,
                Background = new SolidColorBrush(Colors.LightBlue),
                Width = 150,
                Height = 50,
                Margin = new Thickness(3, 3, 5, 3) // Füge einen Abstand zum nächsten Steuerelement hinzu
            };

            // Füge einen Event Handler für den Button-Click hinzu
            button.Click += ButtonVorhandeneNotiz_Click;


            Image image = new Image
            {
                Source = new BitmapImage(new Uri("/notiz.png", UriKind.RelativeOrAbsolute))
            };


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
                Background = myBrush
            };

            trashButton.Click += TrashButton_Click;

            System.Windows.Controls.Image trashImage = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri("./bin2.jpg", UriKind.RelativeOrAbsolute)),
            };

            // Füge die Steuerelemente zum StackPanel hinzu
            panel.Children.Add(image);
            panel.Children.Add(button);
            panel.Children.Add(checkBox);
            panel.Children.Add(trashButton);
            //panel.Children.Add(trashImage);


            // Füge das StackPanel zum übergeordneten Panel (WrapPanelNotizen) hinzu
            WrapPanelNotizen.Children.Add(panel);


        }

        private void TrashButton_Click(object sender, RoutedEventArgs e)
        {
            Button c = sender as Button;
            StackPanel parentPanel = VisualTreeHelper.GetParent(c) as StackPanel;

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