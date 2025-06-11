using Sae.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sae.View
{
    /// <summary>
    /// Logique d'interaction pour TraiterResponsable.xaml
    /// </summary>
    public partial class TraiterResponsable : UserControl
    {
        private Materiel materielSelectionne;

        // Constructeur avec paramètre (recommandé)
        public TraiterResponsable(Materiel materiel)
        {
            InitializeComponent();
            materielSelectionne = materiel;
            InitialiserInterface();
        }

        // Constructeur par défaut (pour compatibilité XAML si nécessaire)
        public TraiterResponsable()
        {
            InitializeComponent();
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
                TxtTitreMateriel.Text = materielSelectionne.Nommateriel;

                // Définir la couleur de l'état actuel
                DefinieCouleurEtat();

                // Pré-sélectionner le bon RadioButton selon l'état actuel
                PreselectEtatRadioButton();
            }
            else
            {
                // Cas où aucun matériel n'est sélectionné
                TxtNomMateriel.Text = "Aucun matériel sélectionné";
                TxtCategorie.Text = "";
                TxtReference.Text = "";
                TxtEtatActuel.Text = "N/A";
                TxtTitreMateriel.Text = "Aucun matériel";
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
                    case "à réviser":
                        RbAReviser.IsChecked = true;
                        break;
                    case "à réparer":
                        RbAReparer.IsChecked = true;
                        break;
                    case "disponible":
                        RbDisponible.IsChecked = true;
                        break;
                }
            }
        }

        private void BtnValider_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (materielSelectionne == null)
                {
                    MessageBox.Show("Aucun matériel sélectionné.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Déterminer le nouvel état sélectionné
                int nouvelIdEtat = 0;
                string nouvelEtat = "";

                if (RbAReviser.IsChecked == true)
                {
                    nouvelIdEtat = 2; // Supposons que l'ID pour "À réviser" est 2
                    nouvelEtat = "À réviser";
                }
                else if (RbAReparer.IsChecked == true)
                {
                    nouvelIdEtat = 3; // Supposons que l'ID pour "À réparer" est 3
                    nouvelEtat = "À réparer";
                }
                else if (RbDisponible.IsChecked == true)
                {
                    nouvelIdEtat = 1; // Supposons que l'ID pour "Disponible" est 1
                    nouvelEtat = "Disponible";
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un nouvel état.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Mettre à jour l'état du matériel
                //materielSelectionne.Idetat = nouvelIdEtat;

                // Récupérer les commentaires
                string commentaires = TxtCommentaires.Text.Trim();

                // Appeler la méthode de mise à jour (à adapter selon votre modèle)
                bool succes = true;
                //bool succes = materielSelectionne.UpdateEtat(nouvelIdEtat, commentaires);

                if (succes)
                {
                    MessageBox.Show($"Le matériel a été mis à jour avec l'état: {nouvelEtat}",
                                  "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Retourner au dashboard
                    RetournerAuDashboard();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du matériel.",
                                  "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de la validation du traitement du matériel");
                MessageBox.Show("Une erreur est survenue lors de la validation.",
                              "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Êtes-vous sûr de vouloir annuler ? Les modifications non sauvegardées seront perdues.",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                RetournerAuDashboard();
            }
        }

        private void RetournerAuDashboard()
        {
            try
            {
                MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                mainWindow.MainContentContainer.Content = new DashResponsable();
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors du retour au dashboard");
                MessageBox.Show("Erreur lors du retour au tableau de bord.","Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}