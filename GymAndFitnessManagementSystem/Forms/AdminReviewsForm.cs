using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class AdminReviewsForm : Form
    {
        DataGridView g = Theme.Grid();

        public AdminReviewsForm()
        {
            Theme.Apply(this, "Customer Reviews");

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
                "Customer Reviews - View Only for Gym Owner"
            );
            header.Dock = DockStyle.Fill;

            // TOOLBAR
            var bar = Theme.Toolbar();
            bar.Dock = DockStyle.Fill;
            bar.Padding = new Padding(12, 8, 12, 5);

            var info = new Label();
            info.Text =
                "Gym owners can view customer feedback. " +
                "Only Super Admin can moderate reviews.";
            info.AutoSize = true;
            info.Padding = new Padding(5, 10, 20, 0);

            var refresh = Theme.Button(
                "Refresh",
                (o, e) => LoadData()
            );

            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );

            refresh.Width = 150;
            close.Width = 150;

            bar.Controls.Add(info);
            bar.Controls.Add(refresh);
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
            if (!Session.GymId.HasValue)
                return;

            g.DataSource =
                Repositories.GymReviews(
                    Session.GymId.Value
                );
        }
    }
}