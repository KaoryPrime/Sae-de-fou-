using Sae.Model;
using System;
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

        private void ChargeData()
        {
            if (materielAReserver == null) return;

            // Afficher les infos du matériel
            TxtNomMateriel.Text = materielAReserver.Nommateriel;
            TxtCategorie.Text = materielAReserver.Categorie.Libellecategorie;
            TxtPrix.Text = $"{materielAReserver.Prixjournee:C}/jour";
            if (!string.IsNullOrEmpty(materielAReserver.ImagePath))
            {
                ImageMateriel.Source = new BitmapImage(new Uri(materielAReserver.ImagePath));
            }

            // Charger les clients dans la ComboBox
            ClientComboBox.ItemsSource = new Client().FindAll();
        }

        private void Dates_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (DateDebutPicker.SelectedDate.HasValue && DateFinPicker.SelectedDate.HasValue)
            {
                DateTime debut = DateDebutPicker.SelectedDate.Value;
                DateTime fin = DateFinPicker.SelectedDate.Value;

                if (fin > debut)
                {
                    double nbJours = Math.Ceiling((fin - debut).TotalDays);
                    decimal total = (decimal)nbJours * materielAReserver.Prixjournee;
                    TxtTotal.Text = $"{total:C}";
                }
                else
                {
                    TxtTotal.Text = "0,00 €";
                }
            }
        }

        private void ButtonConfirmer_Click(object sender, RoutedEventArgs e)
        {
            // --- 1. Validation du formulaire (avec la nouvelle règle de date) ---
            if (ClientComboBox.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un client.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!DateDebutPicker.SelectedDate.HasValue || !DateFinPicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Veuillez sélectionner une date de début et de fin.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (DateFinPicker.SelectedDate.Value <= DateDebutPicker.SelectedDate.Value)
            {
                MessageBox.Show("La date de fin doit être après la date de début.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // --- NOUVEAUTÉ : Vérifier si la date n'est pas dans le passé ---
            if (DateDebutPicker.SelectedDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show("La date de début de la réservation ne peut pas être dans le passé.", "Date invalide", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // --- 2. Création de l'objet Réservation ---
            Reservation nouvelleReservation = new Reservation
            {
                NumMateriel = materielAReserver.Nummateriel,
                NumClient = ((Client)ClientComboBox.SelectedItem).Numclient,
                NumEmploye = employeConnecte?.Numemploye ?? 1, // Utilise l'ID de l'employé connecté
                DateReservation = DateTime.Now,
                DateDebutLocation = DateDebutPicker.SelectedDate.Value,
                DateRetourEffectiveLocation = DateFinPicker.SelectedDate.Value,
                PrixTotal = decimal.Parse(TxtTotal.Text, System.Globalization.NumberStyles.Currency)
            };

            // --- 3. Insertion en base de données et actions post-création ---
            if (nouvelleReservation.Create())
            {
                // --- NOUVEAUTÉ : Mise à jour de l'état si la location commence aujourd'hui ---
                if (DateDebutPicker.SelectedDate.Value.Date == DateTime.Today)
                {
                    // L'ID pour "En location" est 2, à vérifier dans votre base de données.
                    int idEtatEnLocation = 2;
                    materielAReserver.UpdateEtat(idEtatEnLocation, "Début de location.");
                }

                // --- NOUVEAUTÉ : Message de confirmation clair pour l'utilisateur ---
                MessageBox.Show("Réservation confirmée avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Naviguer vers une autre page, par exemple le dashboard
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