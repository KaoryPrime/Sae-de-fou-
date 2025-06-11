using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sae.View
{
    public class Materiel
    {
        public int Nummateriel { get; set; }
        public int Numetat { get; set; }
        public int Numtype { get; set; }
        public string Reference { get; set; }
        public string Nommateriel { get; set; }
        public string Descriptif { get; set; }
        public decimal Prixjournee { get; set; }

        // Constructeur adapté
        public Materiel(int nummateriel, int numetat, int numtype, string reference, string nommateriel, string descriptif, decimal prixjournee)
        {
            this.Nummateriel = nummateriel;
            this.Numetat = numetat;
            this.Numtype = numtype;
            this.Reference = reference;
            this.Nommateriel = nommateriel;
            this.Descriptif = descriptif;
            this.Prixjournee = prixjournee;
        }
    }

    /// <summary>
    /// Logique d'interaction pour ReserverMateriel.xaml
    /// </summary>
    public partial class ReserverMateriel : UserControl
    {
        public ReserverMateriel()
        {
            InitializeComponent();
            LoadMaterielData();
        }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new DashEmploye();
        }

        private void LoadMaterielData()
        {
            string connectionString = "Host=srv-peda-new;Port=5433;Username=cinark;Password=wCFRUt;Database=loxam_bd;Options='-c search_path=cinark'";
            string query = "SELECT NUMMATERIEL, NUMETAT, NUMTYPE, REFERENCE, NOMMATERIEL, DESCRIPTIF, PRIXJOURNEE FROM MATERIEL";

            List<Materiel> materielList = new List<Materiel>();

            using (var conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Materiel materiel = new Materiel(
                            reader.GetInt32(0), // Nummateriel
                            reader.GetInt32(1), // Numetat
                            reader.GetInt32(2), // Numtype
                            reader.GetString(3), // Reference
                            reader.GetString(4), // Nommateriel
                            reader.GetString(5), // Descriptif
                            reader.GetDecimal(6) // Prixjournee
                        );
                        materielList.Add(materiel);
                    }
                }
            }

            // Lier la liste des matériels à la ListBox
            MaterielListBox.ItemsSource = materielList;
        }
    }
}
