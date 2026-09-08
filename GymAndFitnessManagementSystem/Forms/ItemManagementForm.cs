using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class ItemManagementForm : Form
    {
        DataGridView g = Theme.Grid();

        TextBox n = Theme.Text();
        TextBox price = Theme.Text();
        TextBox stock = Theme.Text();
        TextBox min = Theme.Text();
        TextBox desc = Theme.Text();

        ComboBox cat = Theme.Combo();
        ComboBox type = Theme.Combo();

        public ItemManagementForm()
        {
            Theme.Apply(this, "Products / Services / Memberships");

            // MAIN LAYOUT
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 3;

            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 210));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // HEADER
            var header = Theme.Header(
                "Product, Service & Membership CRUD"
            );
            header.Dock = DockStyle.Fill;

            // INPUT AREA
            var form = new TableLayoutPanel();
            form.Dock = DockStyle.Fill;
            form.Padding = new Padding(15, 10, 15, 5);
            form.ColumnCount = 4;
            form.RowCount = 3;
            form.BackColor = Theme.Background;

            for (int i = 0; i < 4; i++)
                form.ColumnStyles.Add(
                    new ColumnStyle(SizeType.Percent, 25)
                );

            form.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 70)
            );

            form.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 70)
            );

            form.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 60)
            );

            // TYPE OPTIONS
            type.Items.AddRange(
                new object[]
                {
                    "Product",
                    "Service",
                    "Membership"
                }
            );

            type.SelectedIndex = 0;

            // ROW 1
            form.Controls.Add(Field("Name", n), 0, 0);
            form.Controls.Add(Field("Category", cat), 1, 0);
            form.Controls.Add(Field("Type", type), 2, 0);
            form.Controls.Add(Field("Price", price), 3, 0);

            // ROW 2
            form.Controls.Add(Field("Stock Quantity", stock), 0, 1);
            form.Controls.Add(Field("Minimum Stock", min), 1, 1);

            desc.Dock = DockStyle.Fill;
            form.Controls.Add(Field("Description", desc), 2, 1);
            form.SetColumnSpan(form.GetControlFromPosition(2, 1), 2);

            // BUTTONS
            var buttons = new FlowLayoutPanel();
            buttons.Dock = DockStyle.Fill;
            buttons.Padding = new Padding(0, 3, 0, 0);

            buttons.Controls.Add(
                Theme.Button("Add", (o, e) => Save(false))
            );

            buttons.Controls.Add(
                Theme.Button("Update", (o, e) => Save(true))
            );

            buttons.Controls.Add(
                Theme.Button("Delete", (o, e) => Delete(), true)
            );

            buttons.Controls.Add(
                Theme.Button("Clear", (o, e) => ClearFields())
            );

            buttons.Controls.Add(
                Theme.Button("Close", (o, e) => Close())
            );

            form.Controls.Add(buttons, 0, 2);
            form.SetColumnSpan(buttons, 4);

            // GRID
            g.Dock = DockStyle.Fill;

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(form, 0, 1);
            root.Controls.Add(g, 0, 2);

            Controls.Add(root);

            g.SelectionChanged += (o, e) => Fill();

            Load += (o, e) =>
            {
                LoadCats();
                LoadData();
            };
        }

        Panel Field(string title, Control control)
        {
            var p = new Panel();
            p.Dock = DockStyle.Fill;
            p.Padding = new Padding(5);

            var label = new Label();
            label.Text = title;
            label.Dock = DockStyle.Top;
            label.Height = 24;

            control.Dock = DockStyle.Bottom;

            p.Controls.Add(control);
            p.Controls.Add(label);

            return p;
        }

        void LoadCats()
        {
            var t = Repositories.Categories();

            cat.DataSource = t;
            cat.DisplayMember = "CategoryName";
            cat.ValueMember = "CategoryId";
        }

        void LoadData()
        {
            if (!Session.GymId.HasValue)
                return;

            g.DataSource =
                Repositories.ItemsForGym(Session.GymId.Value);
        }

        void Fill()
        {
            if (g.CurrentRow == null)
                return;

            n.Text =
                g.CurrentRow.Cells["ItemName"].Value.ToString();

            type.Text =
                g.CurrentRow.Cells["ItemType"].Value.ToString();

            price.Text =
                g.CurrentRow.Cells["Price"].Value.ToString();

            stock.Text =
                g.CurrentRow.Cells["StockQty"].Value.ToString();

            min.Text =
                g.CurrentRow.Cells["MinStock"].Value.ToString();

            desc.Text =
                g.CurrentRow.Cells["Description"].Value.ToString();

            cat.Text =
                g.CurrentRow.Cells["CategoryName"].Value.ToString();
        }

        void Save(bool update)
        {
            decimal p;
            int s;
            int m;

            if (string.IsNullOrWhiteSpace(n.Text) ||
                !decimal.TryParse(price.Text, out p) ||
                p < 0 ||
                !int.TryParse(stock.Text, out s) ||
                s < 0 ||
                !int.TryParse(min.Text, out m) ||
                m < 0 ||
                cat.SelectedValue == null)
            {
                MessageBox.Show(
                    "Enter valid name, category, price and stock values."
                );
                return;
            }

            if (update && g.CurrentRow != null)
            {
                Repositories.UpdateItem(
                    Convert.ToInt32(
                        g.CurrentRow.Cells["ItemId"].Value
                    ),
                    Convert.ToInt32(cat.SelectedValue),
                    n.Text.Trim(),
                    type.Text,
                    p,
                    s,
                    m,
                    desc.Text.Trim()
                );

                MessageBox.Show(
                    "Item updated successfully."
                );
            }
            else
            {
                Repositories.AddItem(
                    Session.GymId.Value,
                    Convert.ToInt32(cat.SelectedValue),
                    n.Text.Trim(),
                    type.Text,
                    p,
                    s,
                    m,
                    desc.Text.Trim()
                );

                MessageBox.Show(
                    "Item added successfully."
                );
            }

            LoadData();
        }

        void Delete()
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show("Select an item first.");
                return;
            }

            try
            {
                Repositories.DeleteItem(
                    Convert.ToInt32(
                        g.CurrentRow.Cells["ItemId"].Value
                    )
                );

                LoadData();
                ClearFields();

                MessageBox.Show(
                    "Item deleted successfully."
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Cannot delete an item already used in orders. "
                    + ex.Message
                );
            }
        }

        void ClearFields()
        {
            n.Clear();
            price.Clear();
            stock.Clear();
            min.Clear();
            desc.Clear();

            if (type.Items.Count > 0)
                type.SelectedIndex = 0;

            if (cat.Items.Count > 0)
                cat.SelectedIndex = 0;

            g.ClearSelection();
        }
    }
}