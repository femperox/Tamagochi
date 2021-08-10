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
using System.Threading;

using TamagochiLib;

namespace Tamagochi_UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<int, string> typeImages;
        Dictionary<int, string> genderImages;

        Player<Tamagochi> player = new Player<Tamagochi>();

        public MainWindow()
        {
            InitializeComponent();

            textBox1.IsEnabled = true;

            typeImages = new Dictionary<int, string>();
            genderImages = new Dictionary<int, string>();

            typeImages.Add(1, "Resourses/choose_cat.png");
            typeImages.Add(2, "Resourses/choose_dog.png");

            genderImages.Add(2, "Resourses/choose_female.png");
            genderImages.Add(1, "Resourses/choose_male.png");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            string name = "";

            try
            {              
                name = textBox1.Text;
                if (!name.Equals(""))
                {
                    int type = Int32.Parse(((TextBlock)typeComboBox.SelectedItem).Text);
                    int gender = Int32.Parse(((TextBlock)genderComboBox.SelectedItem).Text);

                    player.Get(type, name, gender,
                        (o, ex) => MessageBox.Show(ex.Mes),
                        (o, ex) => MessageBox.Show(ex.Mes),
                        (o, ex) => MessageBox.Show(ex.Mes),
                        (o, ex) => MessageBox.Show(ex.Mes),
                        (o, ex) => MessageBox.Show(ex.Mes),
                        (o, ex) => MessageBox.Show(ex.Mes),
                        (o, ex) => MessageBox.Show(ex.Mes),
                        (o, ex) => MessageBox.Show(ex.Mes));
                        TimerCallback AliveTimerCallback = new TimerCallback(player.TimerCheckAlive);
                        player.CheckAliveTimer = new Timer(AliveTimerCallback, null, 0, player.CheckAliveTime);

                    button1.IsEnabled = false;
                    textBox1.IsEnabled = false;
                    typeComboBox.IsEnabled = false;
                    genderComboBox.IsEnabled = false;

                }
                else MessageBox.Show("Укажите имя!");
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Укажите вид/пол!");
            }
        }


        private void changeImage(Image im, string choise, Dictionary<int,string> dict)
        {
            string g;
            dict.TryGetValue(Int32.Parse(choise), out g);

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(g, UriKind.Relative);
            bi3.EndInit();
            im.Stretch = Stretch.Fill;
            im.Source = bi3;

        }

        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBlock tb = (TextBlock)comboBox.SelectedItem;

            if (tb != null)
            {
                changeImage(typeimage, tb.Text, typeImages);

            }
            

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            TextBlock tb = (TextBlock)comboBox.SelectedItem;

            if (tb != null)
            {

                changeImage(genderimage, tb.Text, genderImages);

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (player.CheckAlive()) player.GetStats();
            player.Dead();
        }
    }
}
