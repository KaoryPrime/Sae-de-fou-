using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    public class Necessiter
    {
        private int numcertification;
        private int nummateriel;

        public Necessiter() { }

        public Necessiter(int numcertification, int nummateriel)
        {
            this.numcertification = numcertification;
            this.nummateriel = nummateriel;
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

        public int Nummateriel
        {
            get
            {
                return this.nummateriel;
            }

            set
            {
                this.nummateriel = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Necessiter necessiter &&
                   this.numcertification == necessiter.numcertification &&
                   this.nummateriel == necessiter.nummateriel;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.numcertification, this.nummateriel);
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
