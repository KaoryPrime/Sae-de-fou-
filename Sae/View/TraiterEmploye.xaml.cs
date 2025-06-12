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
    /// Logique d'interaction pour TraiterEmploye.xaml
    /// </summary>
    public partial class TraiterEmploye : UserControl
    {
        private Materiel materielSelectionne;
        public TraiterEmploye()
        {
            InitializeComponent();
            InitialiserInterface();
        }

        public TraiterEmploye(Materiel materiel)
        {
            InitializeComponent();
            materielSelectionne = materiel;
            InitialiserInterface();
        }
        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new RetourMateriel();
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

            }
            else
            {
                // Cas où aucun matériel n'est sélectionné
                TxtNomMateriel.Text = "Aucun matériel sélectionné";
                TxtCategorie.Text = "";
                TxtReference.Text = "";
                TxtEtatActuel.Text = "N/A";
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

                // Déterminer le nouvel état sélectionné
                int nouvelIdEtat = 0;
                string nouvelEtat = "";

                if (ComboBoxEtat.SelectedItem.ToString() == "À réviser")
                {
                    nouvelIdEtat = 7;
                    nouvelEtat = "À réviser";
                }
                else if (ComboBoxEtat.SelectedItem.ToString() == "À réparer")
                {
                    nouvelIdEtat = 8;
                    nouvelEtat = "À réparer";
                }
                else if (ComboBoxEtat.SelectedItem.ToString() == "Disponible")
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
                    MessageBox.Show($"Le matériel a été mis à jour avec l'état: {nouvelEtat}", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
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
                LogError.Log(ex, "Erreur lors de la validation du traitement du matériel");
                MessageBox.Show("Une erreur est survenue lors de la validation.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
