using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

namespace WPFStroi
{
    /// <summary>
    /// Логика взаимодействия для EditPerson.xaml
    /// </summary>
    public partial class EditPerson : Window
    {
        public static string username = MainWindow.username, userpassword = MainWindow.userpassword;

        int idedit = DirectorWindow.id, i = 0;

        private void editbutton_Click(object sender, RoutedEventArgs e)
        {
            Close();
            //DirectorWindow dw = new DirectorWindow();
            //dw.Show();
        }

        public EditPerson()
        {
            InitializeComponent();
            
            using (App.ConnectContext loginconnect = new App.ConnectContext(username, userpassword))
            {var profession = from prof in loginconnect.professions
                                 select new
                                 {
                                     id = prof.id,
                                     profession = prof.name_prof
                                 };
                foreach (var p in profession)
                {
                    App.listprof.Add(new App.Profession { id = p.id, name_prof = p.profession });
                    profbox.Items.Add(App.listprof[i++]);
                }
                i = 0;
                var machine = from mech in loginconnect.machines orderby mech.id
                              select new
                              {
                                  id = mech.id,
                                  mechname = mech.machine
                              };
                foreach (var m in machine)
                {
                    App.listmachine.Add(new App.Machine { id = m.id, machine = m.mechname });
                    techn.Items.Add(App.listmachine[i++].machine);
                }
                i = 0;
                var country = from coun in loginconnect.countrys
                              select new
                              {
                                  id = coun.id,
                                  countryname = coun.country
                              };
                foreach (var c in country)
                {
                    App.listcountry.Add(new App.Country { id = c.id, country = c.countryname });
                    countrycb.Items.Add(App.listcountry[i++].country);
                }
                i = 0;
                App.Person personedit = loginconnect.persons.Find(idedit);
                name.Text = personedit.name;
                fam.Text = personedit.familia;
                otch.Text = personedit.otchestvo;
                tel.Text = personedit.telefon;
                bday.SelectedDate = personedit.bday;
                //profbox.SelectedIndex = ;
                r1.IsChecked = (personedit.gender == "Муж") ? true : false;
                r2.IsChecked = (personedit.gender == "Жен") ? true : false;
                inn.Text = personedit.inn;
                countrycb.SelectedIndex = personedit.id_country;
                techn.SelectedIndex = (personedit.id_m == null)? 0 : (int)personedit.id_m;
                datain.SelectedDate = personedit.date_in;
                oklad.Text = personedit.salary.ToString();
                /*bday = namegrid.bday.ToShortDateString();
                gender = namegrid.gender.ToString();
                inn = namegrid.inn.ToString();
                countrybox = namegrid.id_country.ToString();
                tehnika = namegrid.id_m.ToString();
                dateinbox = namegrid.date_in.ToShortDateString();
                oklad = namegrid.salary.ToString();
            }*/
                
                
            }
        }
    }
}
