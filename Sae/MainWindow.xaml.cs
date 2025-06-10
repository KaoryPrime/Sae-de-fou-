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

        public MainWindow()
        {
            InitializeComponent();
            LoginPage();
            LoadUserControlByRole();
        }
        private void DashBoardEpl() 
        {
            MainContentContainer.Content = new DashEmploye();
        }
        private void DashBoardResp() 
        {
            MainContentContainer.Content = new DashResponsable();
        }
        private void LoginPage() 
        {
            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                // Récupérer l'employé connecté depuis LoginWindow
                employeConnecte = loginWindow.EmployeConnecte;
                // Initialiser MainWindow avec cet employé
            }
            else
            {
                // Login annulé, fermer l'application
                Application.Current.Shutdown();
            }
        }

        private void LoadUserControlByRole()
        {
            // Vider le conteneur principal
            MainContentContainer.Content = null;

            switch (employeConnecte.Numrole)
            {
                case 1: // Employé
                    DashBoardEpl();
                    break;

                case 2: // Responsable atelier
                    DashBoardResp();
                    break;


                default:
                    MessageBox.Show("Rôle non reconnu", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

    }
}