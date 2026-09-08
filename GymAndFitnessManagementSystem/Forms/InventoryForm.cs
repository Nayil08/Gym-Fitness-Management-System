using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class InventoryForm : Form
    {
        DataGridView g = Theme.Grid();

        public InventoryForm()
        {
            Theme.Apply(this, "Inventory");

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 1;
            layout.RowCount = 3;

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 65));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var header = Theme.Header("Inventory Dashboard - Low Stock Alert");
            header.Dock = DockStyle.Fill;

            var bar = Theme.Toolbar();
            bar.Dock = DockStyle.Fill;

            bar.Controls.Add(new Label
            {
                Text = "Products at or below minimum stock are listed below.",
                AutoSize = true,
                Padding = new Padding(5, 10, 10, 0)
            });

            bar.Controls.Add(
                Theme.Button("Refresh", (o, e) => LoadData())
            );

            bar.Controls.Add(
                Theme.Button("Close", (o, e) => Close())
            );

            g.Dock = DockStyle.Fill;

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(bar, 0, 1);
            layout.Controls.Add(g, 0, 2);

            Controls.Add(layout);

            Load += (o, e) => LoadData();
        }

        void LoadData()
        {
            if (Session.GymId.HasValue)
            {
                g.DataSource = Repositories.LowStock(Session.GymId.Value);
            }
        }
    }
}