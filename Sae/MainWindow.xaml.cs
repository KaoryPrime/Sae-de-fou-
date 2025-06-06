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
        private DataAccess dataAccess;

        public MainWindow(string username) 
        {
            InitializeComponent();
            dataAccess = DataAccess.Instance;
        }

        public MainWindow()
        {
            InitializeComponent();
            dataAccess = DataAccess.Instance; 
        }

    }
}