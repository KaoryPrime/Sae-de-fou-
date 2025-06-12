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

        public ReserverMateriel()
        {
            InitializeComponent();
            ChargeData();
            MaterielListBox.Items.Filter = RechercheMotClefMateriel;
            MotCleTextBox.TextChanged += MotCleTextBox_TextChanged;
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
        private bool RechercheMotClefMateriel(object obj)
        {
            if (String.IsNullOrEmpty(MotCleTextBox.Text))
                return true;
            Materiel unMateriel = obj as Materiel;
            return (unMateriel.Nommateriel.StartsWith(MotCleTextBox.Text, StringComparison.OrdinalIgnoreCase));
        }

        private void MotCleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(MaterielListBox.ItemsSource).Refresh();
        }

        private void CategorieComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void TypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}