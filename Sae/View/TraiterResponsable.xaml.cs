using Sae.Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sae.View
{
    public partial class TraiterResponsable : UserControl
    {
        private Materiel materielSelectionne;

        public TraiterResponsable(Materiel materiel)
        {
            InitializeComponent();
            materielSelectionne = materiel;
            InitialiserInterface();
        }

        private void InitialiserInterface()
        {
            if (materielSelectionne != null)
            {
                TxtNomMateriel.Text = materielSelectionne.Nommateriel;
                TxtCategorie.Text = $"Catégorie: {materielSelectionne.Categorie?.Libellecategorie ?? "Non définie"}";
                TxtReference.Text = $"Référence: {materielSelectionne.Reference ?? "Non définie"}";
                TxtEtatActuel.Text = materielSelectionne.Etat?.Libelleetat ?? "État inconnu";
                TxtCommentaires.Text = materielSelectionne.Commentaires;

                DefinieCouleurEtat();
                ChargeImage(); // Utilise la méthode optimisée
                PreselectEtatRadioButton();
            }
            else
            {
                // Gérer le cas où aucun matériel n'est passé
                TxtNomMateriel.Text = "Aucun matériel sélectionné";
                BtnValider.IsEnabled = false;
            }
        }

        /// <summary>
        /// Charge l'image en utilisant la propriété ImagePath du modèle Matériel.
        /// </summary>
        private void ChargeImage()
        {
            try
            {
                if (!string.IsNullOrEmpty(materielSelectionne?.ImagePath))
                {
                    PictureTraiter.Content = new Image
                    {
                        Source = new BitmapImage(new Uri(materielSelectionne.ImagePath)),
                        Stretch = Stretch.Uniform
                    };
                }
                else
                {
                    PictureTraiter.Content = new TextBlock { Text = "Image non disponible", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors du chargement de l'image pour TraiterResponsable");
                PictureTraiter.Content = new TextBlock { Text = "Erreur image", HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
            }
        }

        private void DefinieCouleurEtat()
        {
            if (materielSelectionne?.Etat?.Libelleetat != null)
            {
                switch (materielSelectionne.Etat.Libelleetat.ToLower())
                {
                    case "à réviser":
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.Orange);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case "à réparer":
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.Red);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    case "disponible":
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.Green);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.White);
                        break;
                    default:
                        TxtEtatActuel.Background = new SolidColorBrush(Colors.Gray);
                        TxtEtatActuel.Foreground = new SolidColorBrush(Colors.White);
                        break;
                }
            }
        }

        private void PreselectEtatRadioButton()
        {
            if (materielSelectionne?.Etat?.Libelleetat != null)
            {
                switch (materielSelectionne.Etat.Libelleetat.ToLower())
                {
                    case "à réviser": RbAReviser.IsChecked = true; break;
                    case "à réparer": RbAReparer.IsChecked = true; break;
                    case "disponible": RbDisponible.IsChecked = true; break;
                }
            }
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nouvelIdEtat = 0;
                string nouvelEtat = "";

                if (RbAReviser.IsChecked == true) { nouvelIdEtat = 7; nouvelEtat = "À réviser"; }
                else if (RbAReparer.IsChecked == true) { nouvelIdEtat = 8; nouvelEtat = "À réparer"; }
                else if (RbDisponible.IsChecked == true) { nouvelIdEtat = 1; nouvelEtat = "Disponible"; }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un nouvel état.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                bool succes = materielSelectionne.UpdateEtat(nouvelIdEtat, TxtCommentaires.Text.Trim());

                if (succes)
                {
                    MessageBox.Show($"Le matériel a été mis à jour avec l'état: {nouvelEtat}", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    RetournerAuDashboard();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du matériel.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de la validation du traitement du matériel");
                MessageBox.Show("Une erreur est survenue lors de la validation.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            RetournerAuDashboard();
        }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            RetournerAuDashboard();
        }

        private void RetournerAuDashboard()
        {
            if (Application.Current.MainWindow is MainWindow mainWindow)
            {
                mainWindow.MainContentContainer.Content = new DashResponsable();
            }
        }
    }
}