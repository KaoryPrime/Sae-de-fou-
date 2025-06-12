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
        public List<Type> FindAll()
        {
            List<Type> lesTypes = new List<Type>();
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand("SELECT * FROM type ORDER BY libelletype;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    lesTypes.Add(new Type(
                        (int)dr["NUMTYPE"],
                        (int)dr["NUMCATEGORIE"],
                        (string)dr["LIBELLETYPE"]
                    ));
                }
            }
            return lesTypes;
        }
    }
}
