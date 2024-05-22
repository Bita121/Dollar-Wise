using Microsoft.Maui;
using Microsoft.Maui.Controls;
using SQLitePCL;
using Microsoft.Data.Sqlite;
using System;
using Dollar_Wise.Services;
using System.IO;

namespace Dollar_Wise
{
    public partial class App : Application
    {
        public static DatabaseService Database { get; private set; }

        public App()
        {
            InitializeComponent();
            Database = new DatabaseService();
            MainPage = new AppShell();
        }
    }

}
