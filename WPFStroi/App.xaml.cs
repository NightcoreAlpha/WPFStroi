using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace WPFStroi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public class Postgreconnect
        {
            static public string Login { get; set; }
            static public string Password { get; set; }
            static public string Query { get; set; }
            string connectionString;
            static NpgsqlConnection c;
            static NpgsqlCommand querystring;
            NpgsqlDataReader querydata;

            public Postgreconnect(string login, string password)
            {
                Login = login; Password = password;
            }

            public NpgsqlConnection stringconnect()
            {
                connectionString = "Server=localhost;Port=5432;User id=" + Login + ";Password=" + Password + ";Database=bw;";
                c = new NpgsqlConnection(connectionString);
                return c;
            }

            public NpgsqlDataReader query(string query)
            {
                Query = query;
                querystring = new NpgsqlCommand(Query, c);
                querydata = querystring.ExecuteReader();
                return querydata;
            }
        }

        public class ConnectContext : DbContext
        {
            public static string Username, Userpassword;
            public DbSet<Person> persons { get; set; }
            public DbSet<Country> countrys { get; set; }
            public DbSet<Profession> professions { get; set; }
            public DbSet<Machine> machines { get; set; }
            public DbSet<Product> products { get; set; }
            public DbSet<Vid> vids { get; set; }
            public DbSet<Manufacturer> manufacturers { get; set; }
            public DbSet<Viewstprageinfo> viewstorageinfo { get; set; }
            public ConnectContext(string username, string userpassword)
            {
                Username = username; Userpassword = userpassword;
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Viewstprageinfo>((pc =>
                {
                    pc.HasNoKey();
                    pc.ToView("storageinfo");
                }));
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                try
                { optionsBuilder.UseNpgsql("Server=localhost;Port=5432;Username=" + Username + ";Password=" + Userpassword + ";Database=buildworld;"); }
                catch (Exception exp) { MessageBox.Show("Ошибка подключения: " + exp.Message, "Ошибка"); }
            }
        }

        public static List<Profession> listprof = new List<Profession>();
        public static List<Country> listcountry = new List<Country>();
        public static List<Machine> listmachine = new List<Machine>();

        public class Person
        {
            public int id { get; set; }
            public string name { get; set; }
            public string familia { get; set; }
            public string otchestvo { get; set; }
            public DateTime bday { get; set; }
            public string gender { get; set; }
            public string inn { get; set; }
            public int id_country { get; set; }
            public string telefon { get; set; }
            public int id_prof { get; set; }
            public int? id_m { get; set; }
            public DateTime date_in { get; set; }
            public double salary { get; set; }
        }

        public class Country
        {
            public int id { get; set; }
            public string country { get; set; }
        }

        public class Profession
        {
            public int id { get; set; }
            public string name_prof { get; set; }
        }

        public class Machine
        {
            public int id { get; set; }
            public string machine { get; set; }
            public DateTime year_release { get; set; }
            public int capacity { get; set; }
        }

        public class Product
        {
            public int id { get; set; }
            public string title { get; set; }
            public int id_vid { get; set; }
            public string description { get; set; }
            public double? weight { get; set; }
            public int id_manufacturer { get; set; }
            public int storage_life { get; set; }
            public int id_country { get; set; }
            public double price { get; set; }
        }

        public class Vid
        {
            public int id { get; set; }
            public string vid { get; set; }
        }

        public class Manufacturer
        {
            public int id { get; set; }
            public string title_company { get; set; }
            public string address { get; set; }
            public string inn{ get; set; }
            public int id_country{ get; set; }
            public DateTime date_create { get; set; }
        }

        public class Viewstprageinfo
        {
            public string title { get; set; }
            public double quantity { get; set; }
        }
    }
}