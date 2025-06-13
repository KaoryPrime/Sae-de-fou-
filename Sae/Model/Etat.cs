using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    public class Etat
    {
        private int numetat;
        private string libelleetat;

        public Etat() { }

        public Etat(int numetat, string libelleetat)
        {
            this.numetat = numetat;
            this.libelleetat = libelleetat;
        }

        public int Numetat
        {
            get
            {
                return numetat;
            }

            set
            {
                numetat = value;
            }
        }

        public string Libelleetat
        {
            get
            {
                return this.libelleetat;
            }

            set
            {
                this.libelleetat = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Etat etat &&
                   this.numetat == etat.numetat &&
                   this.libelleetat == etat.libelleetat;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.numetat, this.libelleetat);
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        // Algorithme pour récupérer tous les états depuis la BDD.
        public List<Etat> FindAll()
        {
            List<Etat> lesEtats = new List<Etat>();

            // 1. Préparer la commande SQL.
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from etat ;"))
            {
                // 2. Exécuter la requête pour obtenir les données brutes.
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                // 3. Pour chaque ligne de résultat, convertir les données en objet "Etat".
                foreach (DataRow dr in dt.Rows)
                {
                    lesEtats.Add(new Etat(
                        (int)dr["NUMETAT"],
                        (string)dr["LIBELLEETAT"]
                    ));
                }
                // 4. Retourner la liste remplie.
                return lesEtats;
            }
        }
    }
}
