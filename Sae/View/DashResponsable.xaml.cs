using Sae.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sae.View
{
    /// <summary>
    /// Logique d'interaction pour DashResponsable.xaml
    /// </summary>
    public partial class DashResponsable : UserControl
    {
        public Materiel MaterielSelectionne { get; private set; }
        public ObservableCollection<Materiel> LesMaterieles { get; set; }
        public DashResponsable()
        {
            InitializeComponent();
            ChargeData();
            dgmateriel.Items.Filter = RechercheMotTextBox;
            RechercheTextBox.TextChanged += RechercheTextBox_TextChanged;
        }

        private void RechercheTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(dgmateriel.ItemsSource).Refresh();
        }
        private bool RechercheMotTextBox(object obj)
        {
            if (String.IsNullOrEmpty(RechercheTextBox.Text))
                return true;
            Materiel unMateriel = obj as Materiel;
            return (unMateriel.Nommateriel.StartsWith(RechercheTextBox.Text, StringComparison.OrdinalIgnoreCase));
        }
        private void ChargeData()
        {
            try
            {
                LesMaterieles = new ObservableCollection<Materiel>(new Materiel().FindMaterielResp());
                this.DataContext = this;

            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des materiels");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
        private void TraiterMateriel_Click(object obj, RoutedEventArgs e)
        {
            try
            {
                Button btn = obj as Button;
                Materiel materielSelectionne = btn.Tag as Materiel;
                

                if (materielSelectionne != null)
                {
                        MaterielSelectionne = materielSelectionne;
                        MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
                    mainWindow.MainContentContainer.Content = new TraiterResponsable(materielSelectionne);
                        ChargeData();
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors du traitement du matériel");
                MessageBox.Show("Une erreur est survenue lors du traitement du matériel.","Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}