using Npgsql;
using Sae.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Sae.View
{
    /// <summary>
    /// Logique d'interaction pour ReserverMateriel.xaml
    /// </summary>
    public partial class ReserverMateriel : UserControl
    {
        public ObservableCollection<Materiel> LesMaterieles { get; set; }
        public ICollectionView MaterielView { get; set; }
        public ReserverMateriel()
        {
            InitializeComponent();
            LesMaterieles = new ObservableCollection<Materiel>();
            MaterielView = CollectionViewSource.GetDefaultView(LesMaterieles);
            MaterielView.Filter = RechercheMotClefVin;
            ChargeData();
        }

        private void ButtonRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainContentContainer.Content = new DashEmploye();
        }
        private void ChargeData()
        {
            try
            {
                LesMaterieles = new ObservableCollection<Materiel>(new Materiel().LoadMaterielData());
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des materiels");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }


        }
        private bool RechercheMotClefVin(object obj)
        {
            Materiel unMateriel = obj as Materiel;
            if (unMateriel == null)
                return false;
            // Filtre par nom de vin
            if (MotCleTextBox != null && !String.IsNullOrEmpty(MotCleTextBox.Text))
            {
                if (String.IsNullOrEmpty(unMateriel.Nommateriel) ||
                    !unMateriel.Nommateriel.ToLower().Contains(MotCleTextBox.Text.ToLower()))
                {
                    return false;
                }
            }
            // Filtre par type de vin - CORRIGÉ
            if (CategorieComboBox != null && CategorieComboBox.SelectedItem is ComboBoxItem categorieItem &&
                categorieItem.Content.ToString() != "Toutes les catégories")
            {
                string categorieSelectionne = categorieItem.Content.ToString();
                // Mapping des types selon votre logique métier
                // Adaptez cette partie selon la structure de votre classe Vin
                string categorieMateriel = "";
                switch (unMateriel.Numtype)
                {
                    case 1:
                        categorieMateriel = "Élévation";
                        break;
                    case 2:
                        categorieMateriel = "Espace vert";
                        break;
                    case 3:
                        categorieMateriel = "Outillage électroportatif";
                        break;
                    case 4:
                        categorieMateriel = "Matériel de chantier";
                        break;
                    case 5:
                        categorieMateriel = "Transport";
                        break;
                    default:
                        categorieMateriel = "";
                        break;
                }
                if (!categorieMateriel.Equals(categorieSelectionne, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            // Filtre par appellation - CORRIGÉ
            if (TypeComboBox != null && TypeComboBox.SelectedItem is ComboBoxItem typeItem &&
                typeItem.Content.ToString() != "Tous les types")
            {
                string typeSelectionnee = typeItem.Content.ToString();
                string typeMateriel = "";
                if (unMateriel.Numtype != null)
                {
                    switch (unMateriel.Numtype)
                    {
                        case 1:
                            typeMateriel = "Bétonnière";
                            break;
                        case 2:
                            typeMateriel = "Marteau piqueur";
                            break;
                        case 3:
                            typeMateriel = "Meuleuse";
                            break;
                        case 4:
                            typeMateriel = "Nacelle";
                            break;
                        case 5:
                            typeMateriel = "Perceuse";
                            break;
                        case 6:
                            typeMateriel = "Taille entretien";
                            break;
                        case 7:
                            typeMateriel = "Transports végétaux";
                            break;
                        default:
                            typeMateriel = "";
                            break;
                    }
                }
                if (!typeMateriel.Equals(typeSelectionnee, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        private void RefreshRecherche(object sender, TextChangedEventArgs e)
        {
            MaterielView?.Refresh();
        }

        private void CategorieComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MaterielView?.Refresh();
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MaterielView?.Refresh();
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            // Vérifiez si le bouton et le vin sont valides
            if (btn != null && btn.Tag is Materiel materielSelectionne)
            {
                // Vérifiez si NumFournisseur et NumType2 sont non nuls avant d'y accéder
                if (materielSelectionne.NumFournisseur == null || materielSelectionne.NumType2 == null)
                {
                    MessageBox.Show("Le vin sélectionné est incomplet.");
                    return;
                }
                // Ajoutez à la collection VinsDemande
                VinsDemande.Add(new VinDemande(materielSelectionne.NomVin, DateTime.Now, 1));
                MessageBox.Show($"Vin '{materielSelectionne.NomVin}' ajouté ! Total: {VinsDemande.Count} vins");
            }
            else
            {
                MessageBox.Show("Erreur : Aucun vin sélectionné.");
            }
        }*/
    }
}
