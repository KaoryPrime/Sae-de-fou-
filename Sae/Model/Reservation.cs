using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model
{
    class Reservation
    {
        private int numReservation;
        private int numMateriel;
        private int numEmploye;
        private int numClient;
        private DateTime? dateReservation;
        private DateTime? dateDebutLocation;
        private DateTime? dateRetourEffectiveLocation;
        private DateTime? dateRetourReelleLocation;
        private decimal prixTotal;

        public Reservation() { }

        public Reservation(int numReservation, int numMateriel, int numEmploye, int numClient, DateTime? dateReservation, DateTime? dateDebutLocation, DateTime? dateRetourEffectiveLocation, DateTime? dateRetourReelleLocation, decimal prixTotal)
        {
            this.numReservation = numReservation;
            this.numMateriel = numMateriel;
            this.numEmploye = numEmploye;
            this.numClient = numClient;
            this.dateReservation = dateReservation;
            this.dateDebutLocation = dateDebutLocation;
            this.dateRetourEffectiveLocation = dateRetourEffectiveLocation;
            this.dateRetourReelleLocation = dateRetourReelleLocation;
            this.prixTotal = prixTotal;
        }

        public int NumReservation
        {
            get
            {
                return numReservation;
            }

            set
            {
                numReservation = value;
            }
        }

        public int NumMateriel
        {
            get
            {
                return numMateriel;
            }

            set
            {
                numMateriel = value;
            }
        }

        public int NumEmploye
        {
            get
            {
                return numEmploye;
            }

            set
            {
                numEmploye = value;
            }
        }

        public int NumClient
        {
            get
            {
                return numClient;
            }

            set
            {
                numClient = value;
            }
        }

        public DateTime? DateReservation
        {
            get
            {
                return dateReservation;
            }

            set
            {
                dateReservation = value;
            }
        }

        public DateTime? DateDebutLocation
        {
            get
            {
                return dateDebutLocation;
            }

            set
            {
                dateDebutLocation = value;
            }
        }

        public DateTime? DateRetourEffectiveLocation
        {
            get
            {
                return dateRetourEffectiveLocation;
            }

            set
            {
                dateRetourEffectiveLocation = value;
            }
        }

        public DateTime? DateRetourReelleLocation
        {
            get
            {
                return dateRetourReelleLocation;
            }

            set
            {
                dateRetourReelleLocation = value;
            }
        }

        public decimal PrixTotal
        {
            get
            {
                return this.prixTotal;
            }

            set
            {
                this.prixTotal = value;
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is Reservation reservation &&
                   this.numReservation == reservation.numReservation &&
                   this.numMateriel == reservation.numMateriel &&
                   this.numEmploye == reservation.numEmploye &&
                   this.numClient == reservation.numClient &&
                   this.dateReservation == reservation.dateReservation &&
                   this.dateDebutLocation == reservation.dateDebutLocation &&
                   this.dateRetourEffectiveLocation == reservation.dateRetourEffectiveLocation &&
                   this.dateRetourReelleLocation == reservation.dateRetourReelleLocation &&
                   this.prixTotal == reservation.prixTotal;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(this.numReservation);
            hash.Add(this.numMateriel);
            hash.Add(this.numEmploye);
            hash.Add(this.numClient);
            hash.Add(this.dateReservation);
            hash.Add(this.dateDebutLocation);
            hash.Add(this.dateRetourEffectiveLocation);
            hash.Add(this.dateRetourReelleLocation);
            hash.Add(this.prixTotal);
            return hash.ToHashCode();
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}
