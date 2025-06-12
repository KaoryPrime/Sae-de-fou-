using Sae.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Sae.View
{
    /// <summary>
    /// Logique d'interaction pour TraiterEmploye.xaml
    /// </summary>
    public partial class TraiterEmploye : UserControl
    {
        private Materiel materielSelectionne;

        public TraiterEmploye(Materiel materiel)
        {
            InitializeComponent();
            materielSelectionne = materiel;
            InitialiserInterface();
        }

        private void InitialiserInterface()
        {
            if (materielSelectionne != null)
            {
                // Remplir les informations du matériel
                TxtNomMateriel.Text = materielSelectionne.Nommateriel;
                TxtCategorie.Text = $"Catégorie: {materielSelectionne.Categorie?.Libellecategorie ?? "Non définie"}";
                TxtReference.Text = $"Référence: {materielSelectionne.Reference ?? "Non définie"}";
                TxtEtatActuel.Text = materielSelectionne.Etat?.Libelleetat ?? "État inconnu";

                // Afficher les anciens commentaires s'ils existent
                TxtCommentaires.Text = materielSelectionne.Commentaires;

                // Appliquer la couleur sur le statut
                DefinieCouleurEtat();
            }
            else
            {
                // Cas où aucun matériel n'est sélectionné
                TxtNomMateriel.Text = "Aucun matériel sélectionné";
                // Désactiver les contrôles si aucun matériel n'est chargé
                ComboBoxEtat.IsEnabled = false;
                TxtCommentaires.IsEnabled = false;
                ButtonValider.IsEnabled = false;
            }
        }

        private void DefinieCouleurEtat()
        {
            if (materielSelectionne?.Etat?.Libelleetat != null)
            {
                switch (materielSelectionne.Etat.Libelleetat.ToLower())
                {
                    case "en location":
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.DodgerBlue);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    default:
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.LightGray);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.Black);
                        break;
                }
            }
        }

        private void ButtonValider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (materielSelectionne == null)
                {
                    MessageBox.Show("Aucun matériel sélectionné.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Valider la sélection du ComboBox
                if (ComboBoxEtat.SelectedIndex <= 0) // Le premier item est "-- Sélectionner --"
                {
                    MessageBox.Show("Veuillez sélectionner un nouvel état.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string selection = ((ComboBoxItem)ComboBoxEtat.SelectedItem).Content.ToString();
                int nouvelIdEtat = 0;
                string nouvelEtatLibelle = "";

                switch (selection)
                {
                    case "Disponible":
                        nouvelIdEtat = 1;
                        nouvelEtatLibelle = "Disponible";
                        break;
                    case "À réviser":
                        nouvelIdEtat = 7;
                        nouvelEtatLibelle = "À réviser";
                        break;
                    case "À réparer":
                        nouvelIdEtat = 8;
                        nouvelEtatLibelle = "À réparer";
                        break;
                }

                string commentaires = TxtCommentaires.Text.Trim();

                // Appeler la méthode de mise à jour du modèle
                bool succes = materielSelectionne.UpdateEtat(nouvelIdEtat, commentaires);

                if (succes)
                {
                    MessageBox.Show($"Le matériel '{materielSelectionne.Nommateriel}' a été mis à jour avec l'état : {nouvelEtatLibelle}", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Retourner à la liste des retours
                    MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.MainContentContainer.Content = new RetourMateriel();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du matériel.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de la validation du traitement du matériel par l'employé");
                MessageBox.Show("Une erreur est survenue lors de la validation.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            // Demander confirmation avant d'annuler
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir annuler ? Les modifications non sauvegardées seront perdues.",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.MainContentContainer.Content = new RetourMateriel();
            }
        }
    }
}