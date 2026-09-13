using System;
using System.Data;
using System.Drawing;
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

            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1000, 650);

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 4;
            root.BackColor = Color.White;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 70)
            );

            // Filter area একটু বড় করা হয়েছে
            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 125)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 65)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header =
                Theme.Header("Browse, Search & Filter All Gyms");

            header.Dock = DockStyle.Fill;

            // =========================
            // FILTER AREA
            // =========================
            var filters = new TableLayoutPanel();
            filters.Dock = DockStyle.Fill;
            filters.ColumnCount = 5;
            filters.RowCount = 1;
            filters.Padding = new Padding(12, 8, 12, 8);
            filters.BackColor = Color.White;

            for (int i = 0; i < 5; i++)
            {
                filters.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 20F)
                );
            }

            // TYPE FILTER
            type.Items.Clear();
            type.Items.AddRange(new object[]
            {
                "All Types",
                "Product",
                "Service",
                "Membership"
            });

            type.SelectedIndex = 0;

            // PRICE FILTER
            price.Items.Clear();
            price.Items.AddRange(new object[]
            {
                "All Prices",
                "Under 1000",
                "1000 - 3000",
                "3000+"
            });

            price.SelectedIndex = 0;

            filters.Controls.Add(
                MakeBox("Search", q), 0, 0
            );

            filters.Controls.Add(
                MakeBox("Category", cat), 1, 0
            );

            filters.Controls.Add(
                MakeBox("Type", type), 2, 0
            );

            filters.Controls.Add(
                MakeBox("Price", price), 3, 0
            );

            filters.Controls.Add(
                MakeBox("Location", loc), 4, 0
            );

            // =========================
            // BUTTON BAR
            // =========================
            var bar = Theme.Toolbar();
            bar.Dock = DockStyle.Fill;

            bar.Controls.Add(
                Theme.Button(
                    "Search / Filter",
                    (o, e) => LoadData()
                )
            );

            bar.Controls.Add(
                Theme.Button(
                    "View Details",
                    Details
                )
            );

            bar.Controls.Add(
                Theme.Button(
                    "Add to Cart",
                    Add
                )
            );

            bar.Controls.Add(
                Theme.Button(
                    "Close",
                    (o, e) => Close()
                )
            );

            // =========================
            // DATA GRID
            // =========================
            g.Dock = DockStyle.Fill;
            g.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.DisplayedCells;

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(filters, 0, 1);
            root.Controls.Add(bar, 0, 2);
            root.Controls.Add(g, 0, 3);

            Controls.Add(root);

            // =========================
            // LOAD CATEGORY + DATA
            // =========================
            Load += (o, e) =>
            {
                DataTable t =
                    Repositories.Categories();

                DataRow r = t.NewRow();

                r["CategoryId"] = 0;
                r["CategoryName"] =
                    "All Categories";
                r["Description"] = "";

                t.Rows.InsertAt(r, 0);

                cat.DataSource = t;
                cat.DisplayMember =
                    "CategoryName";
                cat.ValueMember =
                    "CategoryId";

                cat.SelectedIndex = 0;

                LoadData();
            };
        }

        // ==================================
        // FILTER BOX DESIGN
        // ==================================
        Panel MakeBox(
            string title,
            Control control
        )
        {
            Panel p = new Panel();

            p.Dock = DockStyle.Fill;
            p.Padding = new Padding(8);
            p.Margin = new Padding(5);
            p.BackColor = Color.White;

            Label l = new Label();

            l.Text = title;
            l.Dock = DockStyle.Top;
            l.Height = 30;
            l.ForeColor = Color.Black;
            l.BackColor = Color.White;
            l.Font =
                new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Regular
                );

            control.Dock = DockStyle.Top;
            control.Height = 34;
            control.Margin =
                new Padding(0, 5, 0, 0);

            control.BackColor = Color.White;
            control.ForeColor = Color.Black;

            control.Font =
                new Font(
                    "Segoe UI",
                    10F,
                    FontStyle.Regular
                );

            TextBox tb =
                control as TextBox;

            if (tb != null)
            {
                tb.AutoSize = false;
                tb.Height = 34;
                tb.BorderStyle =
                    BorderStyle.FixedSingle;
            }

            ComboBox cb =
                control as ComboBox;

            if (cb != null)
            {
                cb.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                cb.FlatStyle =
                    FlatStyle.Standard;

                cb.Height = 34;
            }

            p.Controls.Add(control);
            p.Controls.Add(l);

            return p;
        }

        // ==================================
        // LOAD / FILTER DATA
        // ==================================
        void LoadData()
        {
            int categoryId = 0;

            if (cat.SelectedValue != null)
            {
                int.TryParse(
                    cat.SelectedValue.ToString(),
                    out categoryId
                );
            }

            g.DataSource =
                Repositories.Browse(
                    q.Text.Trim(),

                    categoryId == 0
                        ? (int?)null
                        : categoryId,

                    type.Text ==
                    "All Types"
                        ? ""
                        : type.Text,

                    price.Text ==
                    "All Prices"
                        ? ""
                        : price.Text,

                    loc.Text.Trim()
                );
        }

        // ==================================
        // VIEW DETAILS
        // ==================================
        void Details(
            object sender,
            EventArgs e
        )
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show(
                    "Select an item first."
                );

                return;
            }

            int itemId =
                Convert.ToInt32(
                    g.CurrentRow
                     .Cells["ItemId"]
                     .Value
                );

            new ItemDetailsForm(
                itemId
            ).ShowDialog();

            LoadData();
        }

        // ==================================
        // ADD TO CART
        // ==================================
        void Add(
            object sender,
            EventArgs e
        )
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show(
                    "Select an item first."
                );

                return;
            }

            int itemId =
                Convert.ToInt32(
                    g.CurrentRow
                     .Cells["ItemId"]
                     .Value
                );

            Repositories.AddToCart(
                Session.UserId,
                itemId,
                1
            );

            MessageBox.Show(
                "Added to cart."
            );
        }
    }
}