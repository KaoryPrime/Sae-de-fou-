using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    public class Dispose
    {
        private int numclient;
        private int numcertification;

        public Dispose() { }

        public Dispose(int numclient, int numcertification)
        {
            this.numclient = numclient;
            this.numcertification = numcertification;
        }

        public int Numclient
        {
            get
            {
                return numclient;
            }

            set
            {
                numclient = value;
            }
        }

        public int Numcertification
        {
            get
            {
                return this.numcertification;
            }

            set
            {
                this.numcertification = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Dispose dispose &&
                   this.numclient == dispose.numclient &&
                   this.numcertification == dispose.numcertification;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.numclient, this.numcertification);
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
