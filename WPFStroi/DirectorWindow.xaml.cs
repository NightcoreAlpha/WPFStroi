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
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace WPFStroi
{
    /// <summary>
    /// Логика взаимодействия для DirectorWindow.xaml
    /// </summary>
    public partial class DirectorWindow : Window
    {
        public static string username = MainWindow.username, userpassword = MainWindow.userpassword;
        public static int n = 0, id = 0;
        List<Personout> pers = new List<Personout>();
        //List<App.Country> country = new List<App.Country>();
        List<Productout> listproducts = new List<Productout>();
        public DirectorWindow()
        {
            InitializeComponent();
            pers.Clear();
            personvisio();
            productvisio();
        }
        public void personvisio()
        {
            using (App.ConnectContext loginconnect = new App.ConnectContext(username, userpassword))
            { 
                try
                {
                    var persons = from per in loginconnect.persons
                                  join co in loginconnect.countrys on per.id_country equals co.id
                                  orderby per.id
                                  join pr in loginconnect.professions on per.id_prof equals pr.id
                                  join ma in loginconnect.machines on per.id_m equals ma.id into mash
                                  from submash in mash.DefaultIfEmpty()
                                  select new
                                  {
                                      id = per.id,
                                      name = per.name,
                                      familia = per.familia,
                                      otchestvo = per.otchestvo,
                                      bday = per.bday,
                                      gender = per.gender,
                                      inn = per.inn,
                                      id_country = co.country,
                                      telefon = per.telefon,
                                      id_prof = pr.name_prof,
                                      id_m = submash.machine,
                                      date_in = per.date_in,
                                      salary = per.salary
                                  };
                    pers.Clear();
                    foreach (var per in persons)
                    {
                        pers.Add(new Personout()
                        {
                            id = per.id,
                            name = per.name,
                            familia = per.familia,
                            otchestvo = per.otchestvo,
                            bday = per.bday,
                            gender = per.gender,
                            inn = per.inn,
                            id_country = per.id_country,
                            telefon = per.telefon,
                            id_prof = per.id_prof,
                            id_m = (per.id_m == null) ? "Отсутствует" : per.id_m.ToString(),
                            date_in = per.date_in,
                            salary = per.salary
                        });
                    }
                    persongri.ItemsSource = persons.Select(x => new { personname = x.familia + " " + x.name }).ToList();
                }
                catch (Exception exp) { MessageBox.Show("Не робит: " + exp.Message, "Ошибка"); }
            }
        }

        public void productvisio()
        {

            using (App.ConnectContext loginconnect = new App.ConnectContext(username, userpassword))
            {
                /*var productinfo = from productinf in loginconnect.viewstorageinfo
                                  select new
                                  {
                                      title = productinf.title,
                                      quantity = productinf.quantity
                                  };*/
                var product = from prod in loginconnect.products
                              join con in loginconnect.countrys on prod.id_country equals con.id
                              orderby prod.id
                              join vid in loginconnect.vids on prod.id_vid equals vid.id
                              join man in loginconnect.manufacturers on prod.id_manufacturer equals man.id
                              join storinfo in loginconnect.viewstorageinfo on prod.title equals storinfo.title
                              select new
                              {
                                  id = prod.id,
                                  title = storinfo.title,
                                  vid = vid.vid,
                                  description = prod.description,
                                  weight = prod.weight,
                                  manuf = man.title_company,
                                  storagelife = prod.storage_life,
                                  country = con.country,
                                  price = prod.price,
                                  quantity = storinfo.quantity
                              };
                /*
                 * from prod in loginconnect.products
                              join con in loginconnect.countrys on prod.id_country equals con.id
                              orderby prod.id
                              join vid in loginconnect.vids on prod.id_vid equals vid.id
                              join man in loginconnect.manufacturers on prod.id_manufacturer equals man.id
                */


                listproducts.Clear();
                foreach (var prod in product)
                {
                    listproducts.Add(new Productout()
                    {
                        id = prod.id,
                        title = prod.title,
                        vid = prod.vid,
                        description = prod.description,
                        weight = (prod.weight == null)? 0 : prod.weight ,
                        manufacturer = prod.manuf,
                        storage_life = prod.storagelife,
                        country = prod.country,
                        price = prod.price

                    });
                }
                gridproduct.ItemsSource = product.ToList();

            }
        }

        private void addpersonbutton_Click(object sender, RoutedEventArgs e)
        {
            AddWorker addworker = new AddWorker();
            addworker.ShowInTaskbar = false;
            addworker.Owner = this;
            addworker.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            Hide();
            addworker.ShowDialog();
        }
        private void persongri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            n = persongri.SelectedIndex;
            var namegrid = pers[n];
            name.Text = namegrid.name.ToString();
            fam.Text = namegrid.familia.ToString();
            otch.Text = namegrid.otchestvo.ToString();
            tel.Text = namegrid.telefon.ToString();
            prof.Text = namegrid.id_prof.ToString();
            bday.Text = namegrid.bday.ToShortDateString();
            gender.Text = namegrid.gender.ToString();
            inn.Text = namegrid.inn.ToString();
            countrybox.Text = namegrid.id_country.ToString();
            tehnika.Text = namegrid.id_m.ToString();
            dateinbox.Text = namegrid.date_in.ToShortDateString();
            oklad.Text = namegrid.salary.ToString();
            id = namegrid.id;
        }
        private void Window_Activated(object sender, EventArgs e)
        {}

        private void gridproduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            n = gridproduct.SelectedIndex;
            var productgrid = listproducts[n];
            productname.Text = productgrid.title.ToString();
            productvid.Text = productgrid.vid.ToString();
            productweight.Text = (productgrid.weight != 0)? productgrid.weight.ToString()+" кг" : "Нет данных";
            productlife.Text = productgrid.storage_life.ToString();
            productcountry.Text = productgrid.country.ToString();
            productmanuf.Text = productgrid.manufacturer.ToString();
            productprice.Text = productgrid.price.ToString();
            productdescription.Text = productgrid.description.ToString();
            }

            private void Window_Closing(object sender, CancelEventArgs e)
        {
            MainWindow mw = new MainWindow();
            Hide();
            mw.Show();
        }
        private void deletebutton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult ds = MessageBox.Show("Удалить выбранного сотрудника?", "Подтверждение удаления", MessageBoxButton.YesNo);
            switch (ds)
            {
                case MessageBoxResult.Yes:
                    using (App.ConnectContext loginconnect = new App.ConnectContext(username, userpassword))
                    {
                        try
                        {
                            App.Person p = loginconnect.persons.Where(x => x.id == id).FirstOrDefault();
                            loginconnect.persons.Remove(p);
                            loginconnect.SaveChanges();
                        }
                        catch (Exception exp) { MessageBox.Show("Не робит: " + exp.Message, "Ошибка"); }
                    }
                    personvisio();
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }

        private void editbutton_Click(object sender, RoutedEventArgs e)
        {
            EditPerson ep = new EditPerson();
            ep.ShowDialog();
        }

        public class Personout
        {
            public int id { get; set; }
            public string name { get; set; }
            public string familia { get; set; }
            public string otchestvo { get; set; }
            public DateTime bday { get; set; }
            public string gender { get; set; }
            public string inn { get; set; }
            public string id_country { get; set; }
            public string telefon { get; set; }
            public string id_prof { get; set; }
            public string? id_m { get; set; }
            public DateTime date_in { get; set; }
            public double salary { get; set; }
        }
        public class Productout
        {
            public int id { get; set; }
            public string title { get; set; }
            public string vid { get; set; }
            public string description { get; set; }
            public double? weight { get; set; }
            public string manufacturer { get; set; }
            public int storage_life { get; set; }
            public string country { get; set; }
            public double price { get; set; }
        }
    }
}
