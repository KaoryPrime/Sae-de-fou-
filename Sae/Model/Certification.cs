using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    class Certification
    {
        private int numcertification;
        private string libellecertif;

        public Certification() { }

        public Certification(int numcertification, string libellecertif)
        {
            this.numcertification = numcertification;
            this.libellecertif = libellecertif;
        }

        public int Numcertification
        {
            get
            {
                return numcertification;
            }

            set
            {
                numcertification = value;
            }
        }

        public string Libellecertif
        {
            get
            {
                return this.libellecertif;
            }

            set
            {
                this.libellecertif = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Certification certification &&
                   this.numcertification == certification.numcertification &&
                   this.libellecertif == certification.libellecertif;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.numcertification, this.libellecertif);
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
