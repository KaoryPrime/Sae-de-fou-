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
        private static readonly Dictionary<string, string> MaterialImageMap = new Dictionary<string, string>
        {
            { "Nacelle articulée", "Nacelle" },
            { "Nacelle 17 mètre", "Nacelle" },
            { "Dresse bordure gazon", "dresse" },
            { "Remorque basculante", "remorque" },
            { "Broyeur de ronces", "remorque" },
            { "Bétonnière 160L", "betoniere" },
            { "Perceuse sans fil", "perceuse" },
            { "Perceuse professionnelle", "perceuse" },
            { "Marteau piqueur 30kg", "piqueur" },
            { "Meuleuse 125mm", "meuleuse" },
            { "Meuleuse d'angle 230mm", "meuleuse" }
        };

        // Nouvelle propriété pour obtenir le chemin de l'image
        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(Nommateriel))
                    return null;

                if (MaterialImageMap.TryGetValue(Nommateriel, out string imageName))
                {
                    // Construit le chemin d'accès que WPF peut comprendre
                    return $"pack://application:,,,/img/{imageName}.jpg";
                }
                // Retourne une image par défaut si aucune n'est trouvée
                return "pack://application:,,,/img/default.jpg"; // Assurez-vous d'avoir une image default.jpg dans votre dossier /img/
            }
        }

        private int nummateriel;
        private int numetat;
        private int numtype;
        private string reference;
        private string nommateriel;
        private string descriptif;
        private decimal prixjournee;
        private string commentaires;

        private Etat etat;
        private Categorie categorie;
        public Model.Type Type { get; set; }

        public Materiel() { }

        public Materiel(int nummateriel, int numetat, int numtype, string reference, string nommateriel, string descriptif, decimal prixjournee,string commentaires)
        {
            this.nummateriel = nummateriel;
            this.numetat = numetat;
            this.numtype = numtype;
            this.reference = reference;
            this.nommateriel = nommateriel;
            this.descriptif = descriptif;
            this.prixjournee = prixjournee;
            this.commentaires = commentaires;
        }

        public Materiel(int nummateriel, int numetat, int numtype, string reference, string nommateriel, string descriptif, decimal prixjournee, Etat etat, Categorie categorie , string commentaires)
        {
            this.nummateriel = nummateriel;
            this.numetat = numetat;
            this.numtype = numtype;
            this.reference = reference;
            this.nommateriel = nommateriel;
            this.descriptif = descriptif;
            this.prixjournee = prixjournee;
            this.commentaires = commentaires;
            this.etat = etat;
            this.categorie = categorie;
            this.commentaires = commentaires;
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
        public string Commentaires
        {
            get
            {
                return commentaires;
            }
            set
            {
                commentaires = value;
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
                   this.prixjournee == materiel.prixjournee &&
                   this.commentaires == materiel.commentaires;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.nummateriel, this.numetat, this.numtype, this.reference, this.nommateriel, this.descriptif, this.prixjournee, this.commentaires);
        }

        public override string? ToString()
        {
            return base.ToString();
        }


        public bool UpdateEtat(int nouvelIdEtat, string commentaires)
        {
            try
            {
                string query = "UPDATE materiel SET numetat = @numetat , commentaires = @commentaires WHERE nummateriel = @nummateriel";
                using (var cmd = new NpgsqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@numetat", nouvelIdEtat);
                    cmd.Parameters.AddWithValue("@commentaires", commentaires);
                    cmd.Parameters.AddWithValue("@nummateriel", this.Nummateriel);

                    int rowsAffected = DataAccess.Instance.ExecuteSet(cmd);

                    if (rowsAffected > 0)
                    {
                        this.Numetat = nouvelIdEtat;
                        this.Commentaires = commentaires;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur mise à jour état matériel");
                return false;
            }
        }


        public List<Materiel> FindMaterielResp()
        {
            List<Materiel> lesMateriels = new List<Materiel>();

            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(@"
               SELECT m.NUMMATERIEL, m.NUMETAT, m.NUMTYPE, m.REFERENCE, m.NOMMATERIEL, 
               m.DESCRIPTIF, m.PRIXJOURNEE, m.COMMENTAIRES,
               e.LIBELLEETAT, c.LIBELLECATEGORIE, c.NUMCATEGORIE
               FROM materiel m 
               LEFT JOIN etat e ON m.NUMETAT = e.NUMETAT 
               LEFT JOIN type t ON m.NUMTYPE = t.NUMTYPE
               LEFT JOIN categorie c ON t.NUMCATEGORIE = c.NUMCATEGORIE"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    // Créer les objets Etat et Categorie
                    Etat etat = new Etat((int)dr["NUMETAT"], (string)dr["LIBELLEETAT"]);
                    Categorie categorie = new Categorie((int)dr["NUMCATEGORIE"], (string)dr["LIBELLECATEGORIE"]);

                    string commentaires = dr["COMMENTAIRES"] == DBNull.Value ? string.Empty : (string)dr["COMMENTAIRES"];
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
                        categorie,
                        commentaires
                    ));
                }
                return lesMateriels;
            }
        }

        public List<Materiel> LoadMaterielData()
        {
            List<Materiel> materielList = new List<Materiel>();

            // La requête SQL est modifiée pour joindre la table TYPE et récupérer ses informations
            using (NpgsqlCommand cmdSelect = new NpgsqlCommand(@"
                SELECT m.*, 
                       e.LIBELLEETAT, 
                       c.NUMCATEGORIE, c.LIBELLECATEGORIE,
                       t.LIBELLETYPE 
                FROM materiel m 
                LEFT JOIN etat e ON m.NUMETAT = e.NUMETAT 
                LEFT JOIN type t ON m.NUMTYPE = t.NUMTYPE
                LEFT JOIN categorie c ON t.NUMCATEGORIE = c.NUMCATEGORIE"))
            {
                DataTable dt = DataAccess.Instance.ExecuteSelect(cmdSelect);
                foreach (DataRow dr in dt.Rows)
                {
                    Etat etat = new Etat((int)dr["NUMETAT"], (string)dr["LIBELLEETAT"]);
                    Categorie categorie = new Categorie((int)dr["NUMCATEGORIE"], (string)dr["LIBELLECATEGORIE"]);

                    // Créer l'objet Type et le lier au matériel
                    Model.Type type = new Model.Type(
                        (int)dr["NUMTYPE"],
                        (int)dr["NUMCATEGORIE"],
                        dr["LIBELLETYPE"] == DBNull.Value ? "" : (string)dr["LIBELLETYPE"]
                    );

                    string commentaires = dr["COMMENTAIRES"] == DBNull.Value ? string.Empty : (string)dr["COMMENTAIRES"];

                    Materiel materiel = new Materiel(
                        (int)dr["NUMMATERIEL"],
                        (int)dr["NUMETAT"],
                        (int)dr["NUMTYPE"],
                        (string)dr["REFERENCE"],
                        (string)dr["NOMMATERIEL"],
                        (string)dr["DESCRIPTIF"],
                        (decimal)dr["PRIXJOURNEE"],
                        etat,
                        categorie,
                        commentaires
                    );

                    // Assigner l'objet Type au matériel
                    materiel.Type = type;
                    materielList.Add(materiel);
                }
                return materielList;
            }
        }
    }
}