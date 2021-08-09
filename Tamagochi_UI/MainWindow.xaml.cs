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

using System.Collections.Generic;

namespace Tamagochi_UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<int, string> typeImages;

        public MainWindow()
        {
            InitializeComponent();


            Button myButton = new Button();
            myButton.Width = 100;
            myButton.Height = 30;
            myButton.Content = "Кнопка2";
            myButton.HorizontalAlignment = HorizontalAlignment.Left;
            myButton.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            testGrid.Children.Add(myButton);

            typeImages = new Dictionary<int, string>();

            typeImages.Add(1, "img/choose_cat.png");
            typeImages.Add(2, "img/choose_dog.png");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string text = textBox1.Text;
            // ComboBoxItem selectedItem = (ComboBoxItem)typeComboBox.SelectedItem;

            TextBlock tb = (TextBlock)typeComboBox.SelectedItem;

            if ((text != ""))
            {
                MessageBox.Show(tb.Text);
            }
        }

        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            //ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            TextBlock tb = (TextBlock)comboBox.SelectedItem;

            if (tb != null)
            {
                string g;
                typeImages.TryGetValue(Int32.Parse(tb.Text), out g);
                var uriSource = new Uri(@"/Tamagochi_UI;img/"+g, UriKind.Relative);
                typeimage.Source = new BitmapImage( new Uri("pack://application:,,,/AssemblyName;" + g));

            }
            

        }
    }
}
