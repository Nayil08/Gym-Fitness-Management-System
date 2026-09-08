using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class CategoryManagementForm : Form
    {
        DataGridView g = Theme.Grid();
        TextBox n = Theme.Text();
        TextBox d = Theme.Text();

        public CategoryManagementForm()
        {
            Theme.Apply(this, "Category Management");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 3;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 64)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 85)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header(
                "Category Management"
            );
            header.Dock = DockStyle.Fill;

            // INPUT + BUTTON BAR
            var bar = new FlowLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.Padding = new Padding(15, 8, 15, 5);
            bar.WrapContents = false;

            n.Width = 180;
            d.Width = 260;

            var nameLabel = new Label();
            nameLabel.Text = "Name";
            nameLabel.AutoSize = true;
            nameLabel.Padding =
                new Padding(0, 10, 5, 0);

            var descLabel = new Label();
            descLabel.Text = "Description";
            descLabel.AutoSize = true;
            descLabel.Padding =
                new Padding(15, 10, 5, 0);

            var add = Theme.Button(
                "Add",
                AddCategory
            );

            var del = Theme.Button(
                "Delete",
                DeleteCategory,
                true
            );

            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );

            add.Width = 140;
            del.Width = 140;
            close.Width = 140;

            bar.Controls.Add(nameLabel);
            bar.Controls.Add(n);

            bar.Controls.Add(descLabel);
            bar.Controls.Add(d);

            bar.Controls.Add(add);
            bar.Controls.Add(del);
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
            g.DataSource =
                Repositories.Categories();
        }

        void AddCategory(object o, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(n.Text))
            {
                MessageBox.Show(
                    "Enter category name."
                );
                return;
            }

            try
            {
                Repositories.AddCategory(
                    n.Text.Trim(),
                    d.Text.Trim()
                );

                n.Clear();
                d.Clear();

                LoadData();

                MessageBox.Show(
                    "Category added successfully."
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void DeleteCategory(object o, EventArgs e)
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show(
                    "Select a category first."
                );
                return;
            }

            try
            {
                int id = Convert.ToInt32(
                    g.CurrentRow.Cells[
                        "CategoryId"
                    ].Value
                );

                Repositories.DeleteCategory(id);

                LoadData();

                MessageBox.Show(
                    "Category deleted successfully."
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Category is in use. " +
                    ex.Message
                );
            }
        }
    }
}