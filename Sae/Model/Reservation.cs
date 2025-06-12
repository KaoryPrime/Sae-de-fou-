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

        // --- Champs privés pour les nouvelles propriétés ---
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

        // --- Getters et Setters dans votre style ---
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
            get { return dateRetourEffectiveLocation; }
            set { dateRetourEffectiveLocation = value; }
        }

        public DateTime? DateRetourReelleLocation
        {
            get { return dateRetourReelleLocation; }
            set { dateRetourReelleLocation = value; }
        }

        public decimal PrixTotal
        {
            get { return this.prixTotal; }
            set { this.prixTotal = value; }
        }

        // --- Getters et Setters pour les nouvelles propriétés ---
        public string ClientNom
        {
            get { return clientNom; }
            set { clientNom = value; }
        }
        public string ClientPrenom
        {
            get { return clientPrenom; }
            set { clientPrenom = value; }
        }
        public string MaterielNom
        {
            get { return materielNom; }
            set { materielNom = value; }
        }

        // ... (Gardez vos méthodes Equals, GetHashCode, et ToString existantes ici)

        public List<Reservation> FindAll()
        {
            List<Reservation> lesReservations = new List<Reservation>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from reservation ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesReservations.Add(new Reservation(
                        (int)dr["NUMRESERVATION"],
                        (int)dr["NUMMATERIEL"],
                        (int)dr["NUMEMPLOYE"],
                        (int)dr["NUMCLIENT"],
                        dr["DATERESERVATION"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["DATERESERVATION"],
                        dr["DATEDEBUTLOCATION"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["DATEDEBUTLOCATION"],
                        dr["DATERETOUREFFECTIVELOCATION"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["DATERETOUREFFECTIVELOCATION"],
                        dr["DATERETOURREELLELOCATION"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["DATERETOURREELLELOCATION"],
                        (decimal)dr["PRIXTOTAL"]
                    ));
                }
                return lesReservations;
            }
        }

        // --- NOUVELLE MÉTHODE ---
        public List<Reservation> FindAllWithDetails()
        {
            List<Reservation> lesReservations = new List<Reservation>();
            string query = @"
                SELECT r.*, c.NOMCLIENT, c.PRENOMCLIENT, m.NOMMATERIEL
                FROM reservation r
                LEFT JOIN client c ON r.NUMCLIENT = c.NUMCLIENT
                LEFT JOIN materiel m ON r.NUMMATERIEL = m.NUMMATERIEL
                ORDER BY r.DATERESERVATION DESC";

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(query))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    Reservation reservation = new Reservation(
                        (int)dr["NUMRESERVATION"],
                        (int)dr["NUMMATERIEL"],
                        (int)dr["NUMEMPLOYE"],
                        (int)dr["NUMCLIENT"],
                        dr["DATERESERVATION"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["DATERESERVATION"],
                        dr["DATEDEBUTLOCATION"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["DATEDEBUTLOCATION"],
                        dr["DATERETOUREFFECTIVELOCATION"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["DATERETOUREFFECTIVELOCATION"],
                        dr["DATERETOURREELLELOCATION"] == DBNull.Value ? (DateTime?)null : (DateTime)dr["DATERETOURREELLELOCATION"],
                        (decimal)dr["PRIXTOTAL"]
                    );
                    reservation.ClientNom = dr["NOMCLIENT"] as string;
                    reservation.ClientPrenom = dr["PRENOMCLIENT"] as string;
                    reservation.MaterielNom = dr["NOMMATERIEL"] as string;

                    lesReservations.Add(reservation);
                }
            }
            return lesReservations;
        }
    }
}
