using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    class Etat
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
    }
}
