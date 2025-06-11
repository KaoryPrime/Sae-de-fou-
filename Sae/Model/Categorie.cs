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

        public Categorie() {}

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
            return base.ToString();
        }
        public List<Categorie> FindAll()
        {
            List<Categorie> lesCategories = new List<Categorie>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("select * from categorie ;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    // Créer un nouvel objet Categorie pour chaque ligne du DataTable et l'ajouter à la liste
                    lesCategories.Add(new Categorie(
                        (int)dr["NUMCATEGORIE"],
                        (string)dr["LIBELLECATEGORIE"]
                    ));
                }
                return lesCategories;
            }
        }
    }
}
