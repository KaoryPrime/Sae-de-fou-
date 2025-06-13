using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Sae.Model
{
    public class Client
    {

        private int numclient;
        private string nomclient;
        private string prenomclient;
        private string mailclient;
        private string telclient;

        public int Numclient
        {
            get 
            {
                return numclient; 
            }
            set 
            { 
                numclient = value; 
            }
        }
        public string Nomclient
        {
            get 
            {
                return nomclient; 
            }
            set 
            {
                nomclient = value; 
            
            }
        }
        public string Prenomclient
        {
            get 
            {
                return prenomclient; 
            }
            set 
            { 
                prenomclient = value;
            }
        }
        public string Mailclient
        {
            get 
            { 
                return mailclient; 
            }
            set 
            {
                mailclient = value;
            }
        }
        public string Telclient
        {
            get 
            {
                return telclient;
            }
            set 
            { 
                telclient = value;
            }
        }


        public Client() { }

        public Client(int numclient, string nomclient, string prenomclient, string mailclient, string telclient)
        {
            this.Numclient = numclient;
            this.Nomclient = nomclient;
            this.Prenomclient = prenomclient;
            this.Mailclient = mailclient;
            this.Telclient = telclient;
        }

        public override string ToString()
        {
            return $"{Nomclient?.ToUpper()} {Prenomclient}";
        }

        public override bool Equals(object? obj)
        {
            return obj is Client client && this.Numclient == client.Numclient;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Numclient);
        }


        //Algorithme pour récupérer tous les clients de la BDD et les mapper en objets.
        public List<Client> FindAll()
        {
            List<Client> lesClients = new List<Client>();

            // 1. Préparer la commande SQL. Lister les colonnes est plus sûr.
            string query = "SELECT numclient, nomclient, prenomclient, mailclient, telclient FROM client ORDER BY nomclient, prenomclient";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(query))
            {
                // 2. Exécuter la requête pour obtenir un DataTable.
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                // 3. Mapper chaque ligne du DataTable vers un objet Client.
                foreach (DataRow dr in dt.Rows)
                {
                    lesClients.Add(new Client(
                        (int)dr["numclient"],
                        (string)dr["nomclient"],
                        (string)dr["prenomclient"],
                        dr["mailclient"] as string, // "as string" gère les valeurs nulles
                        dr["telclient"] as string
                    ));
                }
            }
            return lesClients;
        }

        //Algorithme pour insérer ce client dans la base de données.
        public bool Create()
        {
            // 1. Définir la requête d'insertion avec des paramètres pour la sécurité.
            string query = "INSERT INTO client (NOMCLIENT, PRENOMCLIENT, MAILCLIENT, TELCLIENT) VALUES (@nom, @prenom, @email, @telephone)";

            using (var cmd = new NpgsqlCommand(query))
            {
                // 2. Associer les valeurs des propriétés de l'objet aux paramètres.
                cmd.Parameters.AddWithValue("nom", this.Nomclient);
                cmd.Parameters.AddWithValue("prenom", this.Prenomclient);
                cmd.Parameters.AddWithValue("email", this.Mailclient);
                cmd.Parameters.AddWithValue("telephone", this.Telclient);

                // 3. Exécuter la commande et vérifier si l'insertion a réussi.
                int rowsAffected = DataAccess.Instance.ExecuteSet(cmd);
                return rowsAffected > 0;
            }
        }
    }
}
