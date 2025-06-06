using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    class Client
    {
        private int numclient;
        private string nomclient;
        private string prenomclient;

        public Client() { }

        public Client(int numclient, string nomclient, string prenomclient)
        {
            this.numclient = numclient;
            this.nomclient = nomclient;
            this.prenomclient = prenomclient;
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

        public string Nomclient
        {
            get
            {
                return nomclient;
            }

            set
            {
                nomclient = value;
            }
        }

        public string Prenomclient
        {
            get
            {
                return this.prenomclient;
            }

            set
            {
                this.prenomclient = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Client client &&
                   this.numclient == client.numclient &&
                   this.nomclient == client.nomclient &&
                   this.prenomclient == client.prenomclient;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.numclient, this.nomclient, this.prenomclient);
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
