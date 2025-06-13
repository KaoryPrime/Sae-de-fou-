using Sae.Model;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Sae.View
{
    public partial class ConfirmerReservation : UserControl
    {
        private Materiel materielAReserver;
        private Employe employeConnecte;

        public ConfirmerReservation(Materiel materiel)
        {
            InitializeComponent();
            this.materielAReserver = materiel;

            if (Application.Current.MainWindow is MainWindow main)
            {
                this.employeConnecte = main.EmployeConnecte;
            }

            ChargeData();
        }

        // Algorithme pour charger les données initiales de la page.
        private void ChargeData()
        {
            if (materielAReserver == null) return;

            // 1. Afficher les informations du matériel sélectionné.
            TxtNomMateriel.Text = materielAReserver.Nommateriel;
            TxtCategorie.Text = materielAReserver.Categorie.Libellecategorie;
            TxtPrix.Text = $"{materielAReserver.Prixjournee:C}/jour"; // Le format 'C' utilise le symbole monétaire local.
            if (!string.IsNullOrEmpty(materielAReserver.ImagePath))
            {
                ImageMateriel.Source = new BitmapImage(new Uri(materielAReserver.ImagePath));
            }

            // 2. Charger la liste des clients dans la ComboBox.
            ClientComboBox.ItemsSource = new Client().FindAll();
        }

        // Algorithme de calcul du prix total, déclenché à chaque changement de date.
        private void Dates_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (DateDebutPicker.SelectedDate.HasValue && DateFinPicker.SelectedDate.HasValue)
            {
                DateTime debut = DateDebutPicker.SelectedDate.Value;
                DateTime fin = DateFinPicker.SelectedDate.Value;

                if (fin > debut)
                {
                    // Calcul du nombre de jours de location (on inclut le premier jour).
                    double nbJours = Math.Ceiling((fin - debut).TotalDays);
                    decimal total = (decimal)nbJours * materielAReserver.Prixjournee;
                    TxtTotal.Text = $"{total:C}"; // Affiche le total avec le format monétaire.
                }
                else
                {
                    TxtTotal.Text = "0,00 €"; // Ou string.Empty si vous préférez.
                }
            }
        }

        // Logique principale de validation et de création de la réservation.
        private void ButtonConfirmer_Click(object sender, RoutedEventArgs e)
        {
            // --- 1. Validation des entrées du formulaire ---
            DateTime debut = DateDebutPicker.SelectedDate.Value;
            DateTime fin = DateFinPicker.SelectedDate.Value;
            if ((fin - debut).TotalDays > 5)
            {
                MessageBox.Show("La durée de location ne peut pas excéder 5 jours.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (ClientComboBox.SelectedItem == null || !DateDebutPicker.SelectedDate.HasValue || !DateFinPicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner un client et des dates de début et de fin.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (fin <= debut)
            {
                MessageBox.Show("La date de fin doit être strictement après la date de début.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (debut < DateTime.Today)
            {
                MessageBox.Show("La date de début ne peut pas être dans le passé.", "Date invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- 2. Création de l'objet Réservation ---
            Reservation nouvelleReservation = new Reservation
            {
                NumMateriel = materielAReserver.Nummateriel,
                NumClient = ((Client)ClientComboBox.SelectedItem).Numclient,
                NumEmploye = employeConnecte?.Numemploye ?? 1, // NOTE: Utilise l'ID 1 comme solution de repli si l'employé n'est pas trouvé.
                DateReservation = DateTime.Now,
                DateDebutLocation = DateDebutPicker.SelectedDate.Value,
                DateRetourEffectiveLocation = DateFinPicker.SelectedDate.Value,
                PrixTotal = decimal.Parse(TxtTotal.Text, NumberStyles.Currency, CultureInfo.CurrentCulture) // NOTE: Utilisation explicite de la culture pour plus de robustesse.
            };

            // --- 3. Insertion en BDD et actions post-création ---
            if (nouvelleReservation.Create())
            {
                // NOTE: Logique métier pour changer l'état du matériel si la location commence aujourd'hui.
                if (DateDebutPicker.SelectedDate.Value.Date == DateTime.Today)
                {
                    int idEtatEnLocation = 2; // NOTE: L'ID pour "En location" est codé en dur (valeur magique).
                    materielAReserver.UpdateEtat(idEtatEnLocation, "Début de location.");
                }

                MessageBox.Show("Réservation confirmée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Naviguer vers une autre page après succès.
                if (Application.Current.MainWindow is MainWindow main)
                {
                    main.MainContentContainer.Content = new DashEmploye();
                }
            }
            else
            {
                MessageBox.Show("Une erreur est survenue lors de la création de la réservation.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ButtonAnnuler_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainWindow main)
            {
                main.MainContentContainer.Content = new ReserverMateriel();
            }
        }
    }
}