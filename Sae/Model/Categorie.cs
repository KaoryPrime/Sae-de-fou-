using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    public class Categorie
    {
        private int numcategorie;
        private string libellecategorie;

        public Categorie() { }

        public Categorie(int numcategorie, string libellecategorie)
        {
            this.numcategorie = numcategorie;
            this.libellecategorie = libellecategorie;
        }

        public int Numcategorie
        {
            get 
            {
                return numcategorie;
            }
            set 
            {
                numcategorie = value;
            }
        }

        public string Libellecategorie
        {
            get
            { 
                return this.libellecategorie; 
            }
            set 
            {
                this.libellecategorie = value; 
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Categorie categorie &&
                   this.numcategorie == categorie.numcategorie &&
                   this.libellecategorie == categorie.libellecategorie;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.numcategorie, this.libellecategorie);
        }

        public override string? ToString()
        {
            return this.libellecategorie;
        }


        //Algorithme de récupération et de mapping des données depuis la base.
        // <returns>Une liste d'objets Categorie.</returns>
        public List<Categorie> FindAll()
        {
            List<Categorie> lesCategories = new List<Categorie>();

            // 1. Préparer la commande SQL. Utiliser "using" garantit que la connexion est bien fermée.
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select numcategorie, libellecategorie from categorie order by libellecategorie;"))
            {
                // 2. Exécuter la commande et stocker les résultats bruts dans un DataTable.
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                // 3. Parcourir chaque ligne de données (DataRow) retournée.
                foreach (DataRow dr in dt.Rows)
                {
                    // 4. convertir les données brutes en un objet Categorie typé.
                    lesCategories.Add(new Categorie(
                        (int)dr["numcategorie"],
                        (string)dr["libellecategorie"]
                    ));
                }
                return lesCategories;
            }
        }
    }
}