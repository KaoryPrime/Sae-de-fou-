using System;
using System.Collections.Generic;
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
    }
}
