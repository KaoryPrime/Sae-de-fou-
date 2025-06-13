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
        private string _currentConnectionString;

        public static DataAccess Instance
        {
            get 
            { 
                return instance; 
            }
        }

        // Le constructeur est privé pour garantir une seule instance (Singleton).
        private DataAccess() { }


        //Algorithme clé pour configurer dynamiquement la connexion après l'authentification.
        public void SetConnectionDetails(string username, string password)
        {
            _currentConnectionString = $"Host=srv-peda-new;Port=5433;Username={username};Password={password};Database=loxam_bd;Options='-c search_path=cinark'";

            try
            {
                // S'assure que toute connexion précédente est fermée avant d'en créer une nouvelle.
                if (connection != null && connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
                connection = new NpgsqlConnection(_currentConnectionString);
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb lors de la définition des détails de connexion : \n" + _currentConnectionString);
                throw;
            }
        }


        // Méthode utilitaire pour fournir une connexion valide et ouverte à la demande.
        public NpgsqlConnection GetConnection()
        {
            if (connection == null)
            {
                throw new InvalidOperationException("Les détails de connexion n'ont pas été définis via SetConnectionDetails.");
            }

            // Ouvre la connexion uniquement si elle est fermée ou cassée.
            if (connection.State == ConnectionState.Closed || connection.State == ConnectionState.Broken)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {
                    LogError.Log(ex, "Pb de connexion dans GetConnection \n" + _currentConnectionString);
                    throw;
                }
            }
            return connection;
        }

        // Méthodes d'exécution des requêtes SQL 
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
            try
            {
                cmd.Connection = GetConnection();
                // ExecuteScalar est utilisé ici pour récupérer une valeur (ex: l'ID de la ligne insérée).
                return (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete insert " + cmd.CommandText);
                throw;
            }
        }

        public int ExecuteSet(NpgsqlCommand cmd)
        {
            try
            {
                cmd.Connection = GetConnection();
                // ExecuteNonQuery est pour les commandes qui ne retournent pas de données (UPDATE, DELETE).
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete set " + cmd.CommandText);
                throw;
            }
        }

        public object ExecuteSelectUneValeur(NpgsqlCommand cmd)
        {
            try
            {
                cmd.Connection = GetConnection();
                // ExecuteScalar est optimisé pour récupérer une seule valeur (ex: COUNT(*)).
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Pb avec une requete select " + cmd.CommandText);
                throw;
            }
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