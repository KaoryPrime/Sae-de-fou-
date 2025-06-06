using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    class Employe
    {
        private int numemploye;
        private int numrole;
        private string nom;
        private string prenom;
        private string login;
        private string mdp;

        public Employe() { }

        public Employe(int numemploye, int numrole, string nom, string prenom, string login, string mdp)
        {
            this.numemploye = numemploye;
            this.numrole = numrole;
            this.nom = nom;
            this.prenom = prenom;
            this.login = login;
            this.mdp = mdp;
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

        public string Nom
        {
            get
            {
                return nom;
            }

            set
            {
                nom = value;
            }
        }

        public string Prenom
        {
            get
            {
                return prenom;
            }

            set
            {
                prenom = value;
            }
        }

        public string Login
        {
            get
            {
                return login;
            }

            set
            {
                login = value;
            }
        }

        public string Mdp
        {
            get
            {
                return this.mdp;
            }

            set
            {
                this.mdp = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Employe employe &&
                   this.numemploye == employe.numemploye &&
                   this.numrole == employe.numrole &&
                   this.nom == employe.nom &&
                   this.prenom == employe.prenom &&
                   this.login == employe.login &&
                   this.mdp == employe.mdp;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.numemploye, this.numrole, this.nom, this.prenom, this.login, this.mdp);
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
