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

        public TraiterResponsable(Materiel materiel)
        {
            InitializeComponent();
            materielSelectionne = materiel;
            InitialiserInterface();
        }
        public TraiterResponsable()
        {
            InitializeComponent();
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
                TxtTitreMateriel.Text = materielSelectionne.Nommateriel;



                if (string.IsNullOrWhiteSpace(materielSelectionne.Commentaires))
                {
                    materielSelectionne.Commentaires = "Aucun commentaire"; // Valeur de test
                }
                TxtCommentaires.Text = materielSelectionne.Commentaires;
                System.Diagnostics.Debug.WriteLine($"TextBox Commentaires.Text après assignation: '{TxtCommentaires.Text}'");

                // Définir la couleur de l'état actuel
                DefinieCouleurEtat();
                ChargeImage();
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

        private void ChargeImage()
        {
            try
            {
                PictureTraiter.Content = null;

                if (materielSelectionne == null || string.IsNullOrEmpty(materielSelectionne.Nommateriel))
                    return;

                string imageName = "";
                if (MaterialImageMap.TryGetValue(materielSelectionne.Nommateriel, out imageName))
                {
                    string imagePath = $"pack://application:,,,/img/{imageName}.jpg";

                    Image machineImage = new Image
                    {
                        Width = 300,
                        Height = 300,
                        Stretch = Stretch.Uniform,
                        Source = new BitmapImage(new Uri(imagePath))
                    };

                    PictureTraiter.Content = machineImage;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Matériel '{materielSelectionne.Nommateriel}' non reconnu");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement de l'image: {ex.Message}");
                LogError.Log(ex, "Erreur lors du chargement de l'image du matériel");
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
                    nouvelIdEtat = 7; 
                    nouvelEtat = "À réviser";
                }
                else if (RbAReparer.IsChecked == true)
                {
                    nouvelIdEtat = 8; 
                    nouvelEtat = "À réparer";
                }
                else if (RbDisponible.IsChecked == true)
                {
                    nouvelIdEtat = 1; 
                    nouvelEtat = "Disponible";
                }
                else
                {
                    MessageBox.Show("Veuillez sélectionner un nouvel état.", "Attention", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                materielSelectionne.Numetat = nouvelIdEtat;


                string commentaires = TxtCommentaires.Text.Trim();

                bool succes = materielSelectionne.UpdateEtat(nouvelIdEtat, commentaires);

                if (succes)
                {
                    MessageBox.Show($"Le matériel a été mis à jour avec l'état: {nouvelEtat}","Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    RetournerAuDashboard();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour du matériel.","Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de la validation du traitement du matériel");
                MessageBox.Show("Une erreur est survenue lors de la validation.","Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Êtes-vous sûr de vouloir annuler ? Les modifications non sauvegardées seront perdues.",
                "Confirmation", MessageBoxButton.YesNo,MessageBoxImage.Question);
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

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new DashResponsable();
        }
    }
}