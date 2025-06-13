using System.Collections.Generic;
using System.Data;
using System.Windows;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Sae.Model
{
    public class DataAccess
    {
        private static readonly DataAccess instance = new DataAccess();
        private NpgsqlConnection connection;
        private string _currentConnectionString; // Nouvelle variable pour la chaîne de connexion dynamique

        public static DataAccess Instance
        {
            get
            {
                return instance;
            }
        }

        //  Constructeur privé pour empêcher l'instanciation multiple
        //  Il ne doit plus initialiser la connexion car elle sera définie dynamiquement.
        private DataAccess()
        {
            // La connexion est initialisée via SetConnectionDetails
        }

        /// <summary>
        /// Définit les détails de connexion pour la base de données.
        /// Cette méthode doit être appelée après une authentification réussie.
        /// </summary>
        public void SetConnectionDetails(string username, string password)
        {
            // Remplacez 'srv-peda-new', '5433', 'loxam_bd', 'cinark' par vos informations de base de données
            // La recherche de chemin 'cinark' est maintenue si votre schéma est nommé 'cinark'
            _currentConnectionString = $"Host=srv-peda-new;Port=5433;Username={username};Password={password};Database=loxam_bd;Options='-c search_path=cinark'";

            try
            {
                // Si une connexion existait, la fermer et la recréer avec les nouvelles informations
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection = new NpgsqlConnection(_currentConnectionString);
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb lors de la définition des détails de connexion : \n" + _currentConnectionString);
                throw; // Relancer l'exception pour que l'appelant puisse la gérer
            }
        }


        // pour récupérer la connexion (et l'ouvrir si nécessaire)
        public NpgsqlConnection GetConnection()
        {
            if (connection == null)
            {
                // Ceci ne devrait pas arriver si SetConnectionDetails est appelé après login
                // C'est une mesure de sécurité pour éviter un NullReferenceException
                throw new InvalidOperationException("Les détails de connexion à la base de données n'ont pas été définis. Veuillez vous connecter d'abord.");
            }

            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb de connexion GetConnection \n" + _currentConnectionString);
                    throw;
                }
            }
            return connection;
        }

        // ... (Gardez le reste des méthodes ExecuteSelect, ExecuteInsert, ExecuteSet, ExecuteSelectUneValeur, CloseConnection inchangées)
        // ... (Le code complet de DataAccess.cs est long, assurez-vous de copier/coller ces modifications et de conserver le reste de votre code)
        public DataTable ExecuteSelect(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                cmd.Connection = GetConnection();
                using (var adapter = new NpgsqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL dans ExecuteSelect");
                throw;
            }
            return dataTable;
        }

        public int ExecuteInsert(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = (int)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete insert " + cmd.CommandText);
                throw;
            }
            return nb;
        }

        public int ExecuteSet(NpgsqlCommand cmd)
        {
            int nb = 0;
            try
            {
                cmd.Connection = GetConnection();
                nb = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete set " + cmd.CommandText);
                throw;
            }
            return nb;

        }

        public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
        {
            object res = null;
            try
            {
                cmd.Connection = GetConnection();
                res = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete select " + cmd.CommandText);
                throw;
            }
            return res;

        }

        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}




