using System;
using System.Data;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class BrowseForm : Form
    {
        DataGridView g = Theme.Grid();

        TextBox q = Theme.Text();
        TextBox loc = Theme.Text();

        ComboBox cat = Theme.Combo();
        ComboBox type = Theme.Combo();
        ComboBox price = Theme.Combo();

        public BrowseForm()
        {
            Theme.Apply(this, "Browse");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 4;

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 95));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var header = Theme.Header("Browse, Search & Filter All Gyms");
            header.Dock = DockStyle.Fill;

            // FILTERS
            var filters = new TableLayoutPanel();
            filters.Dock = DockStyle.Fill;
            filters.ColumnCount = 5;
            filters.RowCount = 1;
            filters.Padding = new Padding(10);

            for (int i = 0; i < 5; i++)
                filters.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 20F)
                );

            type.Items.AddRange(new object[]
            {
                "All Types",
                "Product",
                "Service",
                "Membership"
            });

            type.SelectedIndex = 0;

            price.Items.AddRange(new object[]
            {
                "All Prices",
                "Under 1000",
                "1000 - 3000",
                "3000+"
            });

            price.SelectedIndex = 0;

            filters.Controls.Add(MakeBox("Search", q), 0, 0);
            filters.Controls.Add(MakeBox("Category", cat), 1, 0);
            filters.Controls.Add(MakeBox("Type", type), 2, 0);
            filters.Controls.Add(MakeBox("Price", price), 3, 0);
            filters.Controls.Add(MakeBox("Location", loc), 4, 0);

            // BUTTONS
            var bar = Theme.Toolbar();
            bar.Dock = DockStyle.Fill;

            bar.Controls.Add(
                Theme.Button("Search / Filter",
                    (o, e) => LoadData())
            );

            bar.Controls.Add(
                Theme.Button("View Details", Details)
            );

            bar.Controls.Add(
                Theme.Button("Add to Cart", Add)
            );

            bar.Controls.Add(
                Theme.Button("Close",
                    (o, e) => Close())
            );

            // GRID
            g.Dock = DockStyle.Fill;
            g.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.DisplayedCells;

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(filters, 0, 1);
            root.Controls.Add(bar, 0, 2);
            root.Controls.Add(g, 0, 3);

            Controls.Add(root);

            Load += (o, e) =>
            {
                DataTable t = Repositories.Categories();

                DataRow r = t.NewRow();
                r["CategoryId"] = 0;
                r["CategoryName"] = "All Categories";
                r["Description"] = "";

                t.Rows.InsertAt(r, 0);

                cat.DataSource = t;
                cat.DisplayMember = "CategoryName";
                cat.ValueMember = "CategoryId";

                LoadData();
            };
        }

        Panel MakeBox(string title, Control control)
        {
            Panel p = new Panel();
            p.Dock = DockStyle.Fill;
            p.Padding = new Padding(5);

            Label l = new Label();
            l.Text = title;
            l.Dock = DockStyle.Top;
            l.Height = 25;

            control.Dock = DockStyle.Bottom;

            p.Controls.Add(control);
            p.Controls.Add(l);

            return p;
        }

        void LoadData()
        {
            int categoryId = 0;

            if (cat.SelectedValue != null)
                categoryId =
                    Convert.ToInt32(cat.SelectedValue);

            g.DataSource = Repositories.Browse(
                q.Text.Trim(),
                categoryId == 0
                    ? (int?)null
                    : categoryId,
                type.Text == "All Types"
                    ? ""
                    : type.Text,
                price.Text == "All Prices"
                    ? ""
                    : price.Text,
                loc.Text.Trim()
            );
        }

        void Details(object sender, EventArgs e)
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show("Select an item first.");
                return;
            }

            int itemId = Convert.ToInt32(
                g.CurrentRow.Cells["ItemId"].Value
            );

            new ItemDetailsForm(itemId).ShowDialog();

            LoadData();
        }

        void Add(object sender, EventArgs e)
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show("Select an item first.");
                return;
            }

            int itemId = Convert.ToInt32(
                g.CurrentRow.Cells["ItemId"].Value
            );

            Repositories.AddToCart(
                Session.UserId,
                itemId,
                1
            );

            MessageBox.Show("Added to cart.");
        }
    }
}