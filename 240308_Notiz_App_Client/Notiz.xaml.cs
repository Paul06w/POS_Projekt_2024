using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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

        public Notiz(string timestamp, string text, bool check)
        {
            InitializeComponent();
            title = timestamp;
            this.text = text;
            this.check = check;
            textfield.Text = text;
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
            get { return text; }
            set { text = value; }
        }

        public bool gscheck
        {
            get { return check; }
            set { check = value; }
        }

        private void textfield_TextChanged(object sender, TextChangedEventArgs e)
        {
            text = textfield.Text;
        }
    }
}
