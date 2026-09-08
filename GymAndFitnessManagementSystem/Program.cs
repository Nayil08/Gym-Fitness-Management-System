using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Data;
using GymAndFitnessManagementSystem.Forms;
namespace GymAndFitnessManagementSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try { DatabaseInitializer.EnsureDatabase(); }
            catch (Exception ex) { MessageBox.Show("Database setup could not finish automatically. The app can still be reviewed.\n\n" + ex.Message + "\n\nRun database\\schema.sql in SQL Server if needed.", "Database Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            Application.Run(new LoginForm());
        }
    }
}
