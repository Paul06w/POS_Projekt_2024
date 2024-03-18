using System.Text;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        List<Notiz> notizen = new List<Notiz>();

        private void ButtonNeueNotiz_Click(object sender, RoutedEventArgs e)
        {
            Notiz n = new Notiz();
            n.Show();

            AddButton(DateTime.Now.ToString("dd. MMM yyyy, HH:mm"));

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
            Notiz n = new Notiz();
            n.Show();

            

        }
    }
}