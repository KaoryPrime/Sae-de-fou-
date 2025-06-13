using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    public class Reservation
    {
        private int numReservation;
        private int numMateriel;
        private int numEmploye;
        private int numClient;
        private DateTime? dateReservation;
        private DateTime? dateDebutLocation;
        private DateTime? dateRetourEffectiveLocation;
        private DateTime? dateRetourReelleLocation;
        private decimal prixTotal;
        private string clientNom;
        private string clientPrenom;
        private string materielNom;


        public Reservation() { }

        public Reservation(int numReservation, int numMateriel, int numEmploye, int numClient, DateTime? dateReservation, DateTime? dateDebutLocation, DateTime? dateRetourEffectiveLocation, DateTime? dateRetourReelleLocation, decimal prixTotal)
        {
            this.numReservation = numReservation;
            this.numMateriel = numMateriel;
            this.numEmploye = numEmploye;
            this.numClient = numClient;
            this.dateReservation = dateReservation;
            this.dateDebutLocation = dateDebutLocation;
            this.dateRetourEffectiveLocation = dateRetourEffectiveLocation;
            this.dateRetourReelleLocation = dateRetourReelleLocation;
            this.prixTotal = prixTotal;
        }

        public int NumReservation
        {
            get
            { 
                return numReservation;
            }
            set 
            {
                numReservation = value; 
            }
        }

        public int NumMateriel
        {
            get 
            {
                return numMateriel; 
            }
            set 
            {
                numMateriel = value; 
            }
        }

        public int NumEmploye
        {
            get 
            {
                return numEmploye; 
            }
            set 
            {
                numEmploye = value; 
            }
        }

        public int NumClient
        {
            get 
            {
                return numClient; 
            }
            set 
            {
                numClient = value;
            }
        }

        public DateTime? DateReservation
        {
            get 
            {
                return dateReservation;
            }
            set
            { 
                dateReservation = value;
            }
        }

        public DateTime? DateDebutLocation
        {
            get 
            { 
                return dateDebutLocation; 
            }
            set 
            { 
                dateDebutLocation = value;
            }
        }

        public DateTime? DateRetourEffectiveLocation
        {
            get 
            {
                return dateRetourEffectiveLocation; 
            }

            set 
            { 
                dateRetourEffectiveLocation = value;
            }
        }

        public DateTime? DateRetourReelleLocation
        {
            get 
            { 
                return dateRetourReelleLocation; 
            }

            set 
            { 
                dateRetourReelleLocation = value;
            }
        }

        public decimal PrixTotal
        {
            get 
            {
                return this.prixTotal;
            }

            set
            {
                this.prixTotal = value;
            }
        }

        public string ClientNom
        {
            get 
            {
                return clientNom; 
            }
            set 
            { 
                clientNom = value; 
            }
        }
        public string ClientPrenom
        {
            get 
            { 
                return clientPrenom;
            }
            set 
            { 
                clientPrenom = value;
            }
        }
        public string MaterielNom
        {
            get 
            { 
                return materielNom; 
            }
            set 
            {
                materielNom = value;
            }
        }


        //Algorithme simple pour récupérer toutes les réservations brutes.
        public List<Reservation> FindAll()
        {
            List<Reservation> lesReservations = new List<Reservation>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from reservation;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                //La partie complexe ici est de gérer les dates qui peuvent être nulles dans la BDD.
                foreach (DataRow dr in dt.Rows)
                {
                    lesReservations.Add(new Reservation(
                        (int)dr["NUMRESERVATION"],
                        (int)dr["NUMMATERIEL"],
                        (int)dr["NUMEMPLOYE"],
                        (int)dr["NUMCLIENT"],
                        dr["DATERESERVATION"] == DBNull.Value ? null : (DateTime?)dr["DATERESERVATION"],
                        dr["DATEDEBUTLOCATION"] == DBNull.Value ? null : (DateTime?)dr["DATEDEBUTLOCATION"],
                        dr["DATERETOUREFFECTIVELOCATION"] == DBNull.Value ? null : (DateTime?)dr["DATERETOUREFFECTIVELOCATION"],
                        dr["DATERETOURREELLELOCATION"] == DBNull.Value ? null : (DateTime?)dr["DATERETOURREELLELOCATION"],
                        (decimal)dr["PRIXTOTAL"]
                    ));
                }
            }
            return lesReservations;
        }

       /// NOTE: Algorithme pour récupérer les réservations avec les détails du client et du matériel.
        /// </summary>
        public List<Reservation> FindAllWithDetails()
        {
            List<Reservation> lesReservations = new List<Reservation>();
            
            // 1. Définir la requête avec des jointures pour obtenir les noms du client et du matériel.
            string query = @"
                SELECT r.*, c.NOMCLIENT, c.PRENOMCLIENT, m.NOMMATERIEL
                FROM reservation r
                LEFT JOIN client c ON r.NUMCLIENT = c.NUMCLIENT
                LEFT JOIN materiel m ON r.NUMMATERIEL = m.NUMMATERIEL
                ORDER BY r.DATERESERVATION DESC";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(query))
            {
                // 2. Exécuter la requête.
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                
                // 3. Mapper chaque ligne en un objet Reservation complet.
                foreach (DataRow dr in dt.Rows)
                {
                    Reservation reservation = new Reservation(
                        (int)dr["NUMRESERVATION"],
                        (int)dr["NUMMATERIEL"],
                        (int)dr["NUMEMPLOYE"],
                        (int)dr["NUMCLIENT"],
                        dr["DATERESERVATION"] == DBNull.Value ? null : (DateTime?)dr["DATERESERVATION"],
                        dr["DATEDEBUTLOCATION"] == DBNull.Value ? null : (DateTime?)dr["DATEDEBUTLOCATION"],
                        dr["DATERETOUREFFECTIVELOCATION"] == DBNull.Value ? null : (DateTime?)dr["DATERETOUREFFECTIVELOCATION"],
                        dr["DATERETOURREELLELOCATION"] == DBNull.Value ? null : (DateTime?)dr["DATERETOURREELLELOCATION"],
                        (decimal)dr["PRIXTOTAL"]
                    );
                    
                    // 4. Remplir les propriétés supplémentaires avec les données des jointures.
                    reservation.ClientNom = dr["NOMCLIENT"] as string;
                    reservation.ClientPrenom = dr["PRENOMCLIENT"] as string;
                    reservation.MaterielNom = dr["NOMMATERIEL"] as string;

                    lesReservations.Add(reservation);
                }
            }
            return lesReservations;
        }
        //Algorithme pour insérer une nouvelle réservation dans la BDD.
        public bool Create()
        {
            try
            {
                // 1. Définir la requête d'insertion avec des paramètres pour la sécurité.
                string query = @"INSERT INTO reservation (NUMMATERIEL, NUMEMPLOYE, NUMCLIENT, DATERESERVATION, DATEDEBUTLOCATION, DATERETOUREFFECTIVELOCATION, PRIXTOTAL)
                                 VALUES (@numMateriel, @numEmploye, @numClient, @dateReservation, @dateDebut, @dateRetour, @prixTotal)";

                using (var cmd = new NpgsqlCommand(query))
                {
                    // 2. Associer les valeurs aux paramètres de la requête.
                    cmd.Parameters.AddWithValue("@numMateriel", this.NumMateriel);
                    cmd.Parameters.AddWithValue("@numEmploye", this.NumEmploye);
                    cmd.Parameters.AddWithValue("@numClient", this.NumClient);
                    // 3. Gérer correctement les dates qui peuvent être nulles.
                    cmd.Parameters.AddWithValue("@dateReservation", this.DateReservation ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateDebut", this.DateDebutLocation ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dateRetour", this.DateRetourEffectiveLocation ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@prixTotal", this.PrixTotal);

                    // 4. Exécuter la commande et vérifier si l'insertion a réussi.
                    int rowsAffected = DataAccess.Instance.ExecuteSet(cmd);
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de la création de la réservation");
                return false;
            }
        }
    }
}
