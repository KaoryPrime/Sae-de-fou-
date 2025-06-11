using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    public class Materiel
    {
        private int nummateriel;
        private int numetat;
        private int numtype;
        private string reference;
        private string nommateriel;
        private string descriptif;
        private decimal prixjournee;

        private Etat etat;
        private Categorie categorie;
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

        public Materiel(int nummateriel, int numetat, int numtype, string reference, string nommateriel, string descriptif, decimal prixjournee, Etat etat, Categorie categorie)
        {
            this.nummateriel = nummateriel;
            this.numetat = numetat;
            this.numtype = numtype;
            this.reference = reference;
            this.nommateriel = nommateriel;
            this.descriptif = descriptif;
            this.prixjournee = prixjournee;
            this.etat = etat;
            this.categorie = categorie;
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

        public Etat Etat
        {
            get 
            {
                return etat; 
            }
            set 
            { 
                etat = value;
            }
        }

        public Categorie Categorie
        {
            get 
            {
                return categorie;
            }
            set 
            {
                categorie = value;
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
        public List<Materiel> FindMaterielResp()
        {
            List<Materiel> lesMateriels = new List<Materiel>();

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(@"
            SELECT m.*, e.LIBELLEETAT, c.LIBELLECATEGORIE, c.NUMCATEGORIE 
            FROM materiel m 
            LEFT JOIN etat e ON m.NUMETAT = e.NUMETAT 
            LEFT JOIN type t ON m.NUMTYPE = t.NUMTYPE
            LEFT JOIN categorie c ON t.NUMCATEGORIE = c.NUMCATEGORIE
            Where e.NUMETAT=8 or e.NUMETAT=7;"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    // Créer les objets Etat et Categorie
                    Etat etat = new Etat((int)dr["NUMETAT"], (string)dr["LIBELLEETAT"]);
                    Categorie categorie = new Categorie((int)dr["NUMCATEGORIE"], (string)dr["LIBELLECATEGORIE"]);

                    // Créer l'objet Materiel avec les propriétés de navigation
                    lesMateriels.Add(new Materiel(
                        (int)dr["NUMMATERIEL"],
                        (int)dr["NUMETAT"],
                        (int)dr["NUMTYPE"],
                        (string)dr["REFERENCE"],
                        (string)dr["NOMMATERIEL"],
                        (string)dr["DESCRIPTIF"],
                        (decimal)dr["PRIXJOURNEE"],
                        etat,
                        categorie
                    ));
                }
                return lesMateriels;
            }
        }
    }
}
