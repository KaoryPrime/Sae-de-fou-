using Npgsql;
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

namespace Sae.View
{
    public partial class CreerClient : UserControl
    {
        public CreerClient()
        {
            InitializeComponent();
        }

        private readonly string connectionString = "Host=srv-peda-new;Port=5433;Username=cinark;Password=wCFRUt;Database=loxam_bd;Options='-c search_path=cinark'";

        private void InsererClient()
        {
            // Récupérer les valeurs des champs
            string nom = TextBoxNom.Text;
            string prenom = TextBoxPrenom.Text;
            string email = TextBoxEmail.Text;
            string telephone = TextBoxTel.Text;

            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telephone))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "INSERT INTO CLIENT (NOMCLIENT, PRENOMCLIENT, MAILCLIENT, TELCLIENT) VALUES (@nom, @prenom, @email, @telephone)";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("nom", nom);
                        cmd.Parameters.AddWithValue("prenom", prenom);
                        cmd.Parameters.AddWithValue("email", email);
                        cmd.Parameters.AddWithValue("telephone", telephone);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Client ajouté avec succès.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void RetourDashEmploye()
        { 
        
        }

        private void ButtonCreerClient_Click(object sender, RoutedEventArgs e)
        {
            InsererClient();
        }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            RetourDashEmploye();
        }
    }
}
