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
using System.Collections;

namespace WPFStroi
{
    /// <summary>
    /// Логика взаимодействия для AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window
    {
        int i = 0;
        public static string username = MainWindow.username, userpassword = MainWindow.userpassword;
        public AddWorker()
        {
            InitializeComponent();
            using (App.ConnectContext connect = new App.ConnectContext(username, userpassword))
            {
                var profession = from prof in connect.professions
                                 select new
                                 {
                                     id = prof.id,
                                     profession = prof.name_prof
                                 };
                foreach (var p in profession)
                {
                    App.listprof.Add(new App.Profession { id = p.id, name_prof = p.profession });
                    profbox.Items.Add(App.listprof[i++].name_prof);
                }
                i = 0;
                var machine = from mech in connect.machines
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
                var country = from coun in connect.countrys
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
            }
        }
        private void back_Click(object sender, RoutedEventArgs e)
        {
            Close();
            DirectorWindow dw = new DirectorWindow();
            dw.Show();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Person person = null;
            using (App.ConnectContext loginconnect = new App.ConnectContext(username, userpassword))
            {
                try
                {
                    person = new App.Person
                    {
                        name = name.Text,
                        familia = fam.Text,
                        otchestvo = otch.Text,
                        bday = Convert.ToDateTime(bday.SelectedDate.Value.ToShortDateString()),
                        gender = (r1.IsChecked == true) ? r1.Content.ToString() : (r2.IsChecked == true) ? r2.Content.ToString() : "Нет данных",
                        inn = (inn.Text == "") ? "Нет данных" : inn.Text,
                        id_country = (countrycb.SelectedIndex != -1) ? App.listcountry[countrycb.SelectedIndex].id : 0,
                        telefon = (tel.Text == "") ? "Нет данных" : tel.Text,
                        id_prof = (profbox.SelectedIndex != -1) ? App.listprof[profbox.SelectedIndex].id : 0,
                        id_m = (techn.SelectedIndex != -1) ? App.listmachine[techn.SelectedIndex].id : 0,
                        date_in = (datain.SelectedDate.ToString() == "") ? DateTime.Now : Convert.ToDateTime(datain.SelectedDate.Value.ToShortDateString()),
                        salary = (oklad.Text == "") ? 0 : int.Parse(oklad.Text)
                    };
                }
                catch (Exception) { MessageBox.Show("Некорректный ввод", "Ошибка"); return; }
                loginconnect.persons.Add(person);
                loginconnect.SaveChanges();
            }
            Close();
            DirectorWindow dw = new DirectorWindow();
            dw.Show();
        }
    }
}

/*App.Postgreconnect pc = new App.Postgreconnect(MainWindow.username, MainWindow.userpassword);
            pc.stringconnect().Open();
            string nom = (nomer.Text == "") ? "null" : nomer.Text,
                n = (name.Text == "") ? "null" : name.Text,
                f = (fam.Text == "") ? "null" : fam.Text,
                o = (otch.Text == "") ? "null" : otch.Text,
                bd = (bday.SelectedDate.ToString() == "") ? "null" : bday.SelectedDate.Value.ToShortDateString(),
                g = (r1.IsChecked == true) ? r1.Content.ToString() : (r2.IsChecked == true) ? r2.Content.ToString() : "null",
                i = (inn.Text == "") ? "null" : inn.Text,
                c = (countrycb.SelectedIndex + 1 != -1) ? (countrycb.SelectedIndex + 1).ToString() : "null",
                t = (tel.Text == "") ? "null" : tel.Text,
                p = (profbox.SelectedIndex + 1 != -1) ? (profbox.SelectedIndex + 1).ToString() : "null",
                tec = (techn.Text == "") ? "null" : techn.Text,
                d = (datain.SelectedDate.ToString() == "") ? "null" : datain.SelectedDate.Value.ToShortDateString(),
                ok = (oklad.Text == "") ? "null" : oklad.Text;
            //    .postconnect.stringconnect().Open();
            //insert into persons(id_p, name, familia, otchestvo, bday,gender,inn,id_country,telefon, id_prof,id_m, date_in, salary) values(4, 'Валерий', 'Синиванов', 'Валерьевич','2021-01-01', 'Муж', 123456789014, 155, +79287891234,2,null, current_date, 12000);
            query = ("insert into persons(id_p, name, familia, otchestvo, bday,gender,inn,id_country,telefon, id_prof,id_m, date_in, salary) values(" +
                nom + ",'" + n + "','" + f + "','" + o + "','" + bd + "','" + g + "','" + i + "'," + c + ",'" + t + "'," + p + ",'" + tec + "','" + d + "'," + ok + ");");
            //nom,n,f,o,bd,g,i,c,t,p,tec,d,ok
            //postconnect.query(query);
            pc.query(query);
            pc.stringconnect().Close();
            Close();
            DirectorWindow dw = new DirectorWindow();
            dw.Show();*/