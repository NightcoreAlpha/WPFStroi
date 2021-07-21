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
using Npgsql;

namespace WPFStroi
{
    public partial class MainWindow : Window
    {
        App.ConnectContext loginconnect;
        public static string username, userpassword;

        public MainWindow()
        {
            InitializeComponent();
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            userpassword = userpasswordbox.Password;
            loginconnect = new App.ConnectContext(username, userpassword);
            try
            {
                if (username == "store_personal" && loginconnect.Database.CanConnect() == true)
                {
                    MessageBox.Show("Вы вошли как " + "'" + usernamebox.Text + "'", "Сообщение");
                    Hide();
                    PersonalWindow pw = new PersonalWindow();
                    pw.Show();
                }
                else if (username == "director" && loginconnect.Database.CanConnect() == true)
                {
                    MessageBox.Show("Вы вошли как " + "'" + usernamebox.Text + "'", "Сообщение");
                    Hide();
                    DirectorWindow pw = new DirectorWindow();
                    pw.Show();
                }
                else { MessageBox.Show("Вход не произведен [with]", "Сообщение"); }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Вы не вошли: "+exp.Message, "Сообщение");
            }
        }

        private void radiopersonal_Checked(object sender, RoutedEventArgs e)
        {
            if (radiopersonal.IsChecked == true) { usernamebox.Text = radiopersonal.Content.ToString(); username = "store_personal"; }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void radiodirector_Checked(object sender, RoutedEventArgs e)
        {
            if (radiodirector.IsChecked == true) { usernamebox.Text = radiodirector.Content.ToString(); username = "director"; }
        }
    }
}

