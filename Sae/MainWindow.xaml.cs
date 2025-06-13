using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Text;
using Npgsql;
using Sae.Model;
using System.Windows.Controls;
using Sae.View;

namespace Sae
{
    public partial class MainWindow : Window
    {
        private Employe employeConnecte;

        public Employe EmployeConnecte
        {
            get 
            {
                return employeConnecte;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized; // Met la fenêtre en mode "agrandi"
            this.WindowStyle = WindowStyle.None;      // Supprime la barre de titre et les bordures

            LoginPage();
            LoadUserControlByRole(this.EmployeConnecte);
        }
        public void SeDeconnecter()
        {
            this.Visibility = Visibility.Hidden;
            LoginPage();
            LoadUserControlByRole(this.EmployeConnecte);
            this.Visibility = Visibility.Visible;
        }

        private void LoginPage()
        {
            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                // On assigne la valeur au champ privé
                this.employeConnecte = loginWindow.EmployeConnecte;
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void LoadUserControlByRole(Employe employe)
        {
            if (employe == null) return;
            MainContentContainer.Content = null;
            switch (employe.Numrole)
            {
                case 1:
                    MainContentContainer.Content = new DashEmploye();
                    break;
                case 2:
                    MainContentContainer.Content = new DashResponsable();
                    break;
                default:
                    MessageBox.Show("Rôle non reconnu", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

    }
}