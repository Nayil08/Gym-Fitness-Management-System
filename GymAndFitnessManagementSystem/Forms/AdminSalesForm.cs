using System;
using System.Drawing;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class AdminSalesForm : Form
    {
        DataGridView g = Theme.Grid();
        Label total = new Label();

        public AdminSalesForm()
        {
            Theme.Apply(this, "Sales & Earnings");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 3;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 64)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 75)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header(
                "Sales & Earnings Report"
            );
            header.Dock = DockStyle.Fill;

            // TOOLBAR
            var bar = new FlowLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.Padding = new Padding(15, 8, 15, 5);
            bar.BackColor = Color.White;
            bar.WrapContents = false;

            total.Font = new Font(
                "Segoe UI Semibold",
                12F,
                FontStyle.Bold
            );

            total.AutoSize = true;
            total.Padding = new Padding(8, 10, 25, 0);

            var refresh = Theme.Button(
                "Refresh",
                (o, e) => LoadData()
            );

            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );

            refresh.Width = 170;
            close.Width = 170;

            bar.Controls.Add(total);
            bar.Controls.Add(refresh);
            bar.Controls.Add(close);

            // GRID
            g.Dock = DockStyle.Fill;

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(bar, 0, 1);
            root.Controls.Add(g, 0, 2);

            Controls.Add(root);

            Load += (o, e) => LoadData();
        }

        void LoadData()
        {
            if (!Session.GymId.HasValue)
                return;

            g.DataSource =
                Repositories.GymSales(
                    Session.GymId.Value
                );

            total.Text =
                "Net earnings after platform commission: Tk " +
                Repositories.GymEarnings(
                    Session.GymId.Value
                ).ToString("N2");
        }
    }
}