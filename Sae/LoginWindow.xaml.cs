using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Sae.Model;

namespace Sae
{
    public partial class LoginWindow : Window
    {
        public List<Employe> LesEmployes { get; set; }
        public Employe EmployeConnecte { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            ChargeData();
            this.KeyDown += LoginWindow_KeyDown;
            this.Loaded += (s, e) => UsernameTextBox.Focus();
        }

        private void ChargeData()
        {
            try
            {
                LesEmployes = new List<Employe>(new Employe().FindAll());
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur SQL lors du chargement des employés");
                MessageBox.Show("Problème lors de récupération des données", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }

        private void LoginWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LoginButton_Click(sender, e);
            else if (e.Key == Key.Escape)
                this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(username))
            {
                ShowError("Veuillez saisir votre identifiant.");
                UsernameTextBox.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowError("Veuillez saisir votre mot de passe.");
                PasswordBox.Focus();
                return;
            }

            try
            {
                EmployeConnecte = AuthenticateUser(username, password);
                if (EmployeConnecte != null)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    ShowError("Identifiant ou mot de passe incorrect.");
                    PasswordBox.Clear();
                    UsernameTextBox.Focus();
                }
            }
            catch (Exception ex)
            {
                LogError.Log(ex, "Erreur lors de l'authentification");
                ShowError("Erreur de connexion. Veuillez réessayer.");
            }
        }

        private Employe AuthenticateUser(string username, string password)
        {
            foreach (Employe employe in LesEmployes)
            {
                if (employe.Login.Equals(username, StringComparison.OrdinalIgnoreCase) && employe.Mdp == password)
                {
                    return employe;
                }
            }
            return null;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(this, message, "Attention", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}