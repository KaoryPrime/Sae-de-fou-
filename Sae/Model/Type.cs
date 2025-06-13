using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    public class Type
    {
        private int numtype;
        private int numcategorie;
        private string libelletype;

        public Type() { }

        public Type(int numtype, int numcategorie, string libelletype)
        {
            this.numtype = numtype;
            this.numcategorie = numcategorie;
            this.libelletype = libelletype;
        }

        public int Numtype
        {
            get
            {
                return numtype;
            }

            set
            {
                numtype = value;
            }
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

        public string Libelletype
        {
            get
            {
                return this.libelletype;
            }

            set
            {
                this.libelletype = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Type type &&
                   this.numtype == type.numtype &&
                   this.numcategorie == type.numcategorie &&
                   this.libelletype == type.libelletype;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.numtype, this.numcategorie, this.libelletype);
        }

        public override string? ToString()
        {
            return base.ToString();
        }

        // Algorithme pour récupérer tous les types de matériel depuis la BDD.
        public List<Type> FindAll()
        {
            List<Type> lesTypes = new List<Type>();

            // 1. Préparer la commande SQL (lister les colonnes est plus robuste que "select *").
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT numtype, numcategorie, libelletype FROM type ORDER BY libelletype;"))
            {
                // 2. Exécuter la requête pour obtenir les données brutes.
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);

                // 3. Pour chaque ligne de résultat, convertir les données en objet.
                foreach (DataRow dr in dt.Rows)
                {
                    lesTypes.Add(new Type(
                        (int)dr["NUMTYPE"],
                        (int)dr["NUMCATEGORIE"],
                        (string)dr["LIBELLETYPE"]
                    ));
                }
            }
            // 4. Retourner la liste complète.
            return lesTypes;
        }
    }
}
