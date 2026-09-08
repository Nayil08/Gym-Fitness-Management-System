using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class ReviewModerationForm : Form
    {
        DataGridView g = Theme.Grid();

        public ReviewModerationForm()
        {
            Theme.Apply(this, "Review Moderation");

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
            var header = Theme.Header("Review Moderation");
            header.Dock = DockStyle.Fill;

            // BUTTON BAR
            var bar = Theme.Toolbar();
            bar.Dock = DockStyle.Fill;
            bar.Padding = new Padding(15, 8, 15, 5);

            var refresh = Theme.Button(
                "Refresh",
                (o, e) => LoadData()
            );

            var hide = Theme.Button(
                "Hide Selected",
                HideSelected,
                true
            );

            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );

            refresh.Width = 150;
            hide.Width = 180;
            close.Width = 150;

            bar.Controls.Add(refresh);
            bar.Controls.Add(hide);
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
            g.DataSource = Repositories.AllReviews();
        }

        void HideSelected(object o, EventArgs e)
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show("Select a review first.");
                return;
            }

            int reviewId = Convert.ToInt32(
                g.CurrentRow.Cells["ReviewId"].Value
            );

            Repositories.HideReview(reviewId);

            LoadData();

            MessageBox.Show("Review hidden successfully.");
        }
    }
}