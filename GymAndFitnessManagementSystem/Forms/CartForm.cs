using System;
using System.Drawing;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class CartForm : Form
    {
        DataGridView g = Theme.Grid();
        Label total = new Label();

        public CartForm()
        {
            Theme.Apply(this, "Cart");

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
            var header = Theme.Header("Shopping Cart");
            header.Dock = DockStyle.Fill;

            // TOOLBAR
            var bar = new FlowLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.Padding = new Padding(15, 8, 15, 5);
            bar.WrapContents = false;
            bar.BackColor = Color.White;

            total.AutoSize = true;
            total.Font = new Font(
                "Segoe UI Semibold",
                12F,
                FontStyle.Bold
            );
            total.Padding = new Padding(5, 10, 25, 0);

            var remove = Theme.Button(
                "Remove Selected",
                Remove,
                true
            );

            var checkout = Theme.Button(
                "Checkout",
                (o, e) =>
                {
                    new CheckoutForm().ShowDialog();
                    LoadData();
                }
            );

            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );

            remove.Width = 180;
            checkout.Width = 160;
            close.Width = 140;

            bar.Controls.Add(total);
            bar.Controls.Add(remove);
            bar.Controls.Add(checkout);
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
            var t = Repositories.Cart(Session.UserId);

            g.DataSource = t;

            decimal sum = 0;

            foreach (System.Data.DataRow r in t.Rows)
            {
                sum += Convert.ToDecimal(
                    r["LineTotal"]
                );
            }

            total.Text =
                "Total: Tk " + sum.ToString("N2");
        }

        void Remove(object o, EventArgs e)
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show(
                    "Select an item first."
                );
                return;
            }

            Repositories.RemoveCartItem(
                Convert.ToInt32(
                    g.CurrentRow.Cells[
                        "CartItemId"
                    ].Value
                )
            );

            LoadData();
        }
    }
}