using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Text;
using Npgsql;
using Sae.Model;
using System.Windows.Controls;

namespace Sae
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                // Récupérer l'employé connecté depuis LoginWindow
                Employe employe = loginWindow.EmployeConnecte;
                // Initialiser MainWindow avec cet employé
            }
            else
            {
                // Login annulé, fermer l'application
                Application.Current.Shutdown();
            }
        }

    }
}