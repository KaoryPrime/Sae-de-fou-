using System;
using System.Collections.Generic;
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
    }
}
