using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    public class NomRole
    {
        private int numrole;
        private int numemploye;
        private string nomrole;

        public NomRole() { }

        public NomRole(int numrole, int numemploye, string nomrole)
        {
            this.numrole = numrole;
            this.numemploye = numemploye;
            this.nomrole = nomrole;
        }

        public int Numrole
        {
            get
            {
                return numrole;
            }

            set
            {
                numrole = value;
            }
        }

        public int Numemploye
        {
            get
            {
                return numemploye;
            }

            set
            {
                numemploye = value;
            }
        }

        public string Nomrole
        {
            get
            {
                return this.nomrole;
            }

            set
            {
                this.nomrole = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is NomRole nomrole &&
                   this.numrole == nomrole.numrole &&
                   this.numemploye == nomrole.numemploye &&
                   this.nomrole == nomrole.nomrole;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.numrole, this.numemploye, this.nomrole);
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
