using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    class Materiel
    {
        private int nummateriel;
        private int numetat;
        private int numtype;
        private string reference;
        private string nommateriel;
        private string descriptif;
        private decimal prixjournee;

        public Materiel() { }

        public Materiel(int nummateriel, int numetat, int numtype, string reference, string nommateriel, string descriptif, decimal prixjournee)
        {
            this.nummateriel = nummateriel;
            this.numetat = numetat;
            this.numtype = numtype;
            this.reference = reference;
            this.nommateriel = nommateriel;
            this.descriptif = descriptif;
            this.prixjournee = prixjournee;
        }

        public int Nummateriel
        {
            get
            {
                return nummateriel;
            }

            set
            {
                nummateriel = value;
            }
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

        public string Reference
        {
            get
            {
                return reference;
            }

            set
            {
                reference = value;
            }
        }

        public string Nommateriel
        {
            get
            {
                return nommateriel;
            }

            set
            {
                nommateriel = value;
            }
        }

        public string Descriptif
        {
            get
            {
                return descriptif;
            }

            set
            {
                descriptif = value;
            }
        }

        public decimal Prixjournee
        {
            get
            {
                return this.prixjournee;
            }

            set
            {
                this.prixjournee = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Materiel materiel &&
                   this.nummateriel == materiel.nummateriel &&
                   this.numetat == materiel.numetat &&
                   this.numtype == materiel.numtype &&
                   this.reference == materiel.reference &&
                   this.nommateriel == materiel.nommateriel &&
                   this.descriptif == materiel.descriptif &&
                   this.prixjournee == materiel.prixjournee;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.nummateriel, this.numetat, this.numtype, this.reference, this.nommateriel, this.descriptif, this.prixjournee);
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
