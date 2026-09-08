using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class ManageGymOwnersForm : Form
    {
        DataGridView g = Theme.Grid();

        public ManageGymOwnersForm()
        {
            Theme.Apply(this, "Manage Gym Owners");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 3;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 64)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 70)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header(
                "Manage Gym Owners"
            );
            header.Dock = DockStyle.Fill;

            // BUTTON BAR
            var bar = Theme.Toolbar();
            bar.Dock = DockStyle.Fill;

            var refresh = Theme.Button(
                "Refresh",
                (o, e) => LoadData()
            );

            var approve = Theme.Button(
                "Approve",
                (o, e) => SetStatus("Active")
            );

            var suspend = Theme.Button(
                "Suspend",
                (o, e) => SetStatus("Suspended"),
                true
            );

            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );

            bar.Controls.Add(refresh);
            bar.Controls.Add(approve);
            bar.Controls.Add(suspend);
            bar.Controls.Add(close);

            // GRID
            g.Dock = DockStyle.Fill;
            g.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(bar, 0, 1);
            root.Controls.Add(g, 0, 2);

            Controls.Add(root);

            Load += (o, e) => LoadData();
        }

        void LoadData()
        {
            try
            {
                g.DataSource =
                    Repositories.PendingAndActiveGyms();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void SetStatus(string status)
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show(
                    "Select a gym owner first."
                );
                return;
            }

            int gymId = Convert.ToInt32(
                g.CurrentRow.Cells["GymId"].Value
            );

            Repositories.SetGymStatus(
                gymId,
                status
            );

            LoadData();

            MessageBox.Show(
                status == "Active"
                    ? "Gym owner approved."
                    : "Gym owner suspended."
            );
        }
    }
}