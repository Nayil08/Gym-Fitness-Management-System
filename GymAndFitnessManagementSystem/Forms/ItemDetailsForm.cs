using System;
using System.Drawing;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class ItemDetailsForm : Form
    {
        int item;
        Label info = new Label();
        DataGridView g = Theme.Grid();

        NumericUpDown qty = new NumericUpDown
        {
            Minimum = 1,
            Maximum = 20,
            Value = 1,
            Width = 80
        };

        public ItemDetailsForm(int itemId)
        {
            item = itemId;

            Theme.Apply(this, "Item Details");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 4;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 64)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 70)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 190)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header(
                "Item Details & Reviews"
            );
            header.Dock = DockStyle.Fill;

            // BUTTON BAR
            var bar = new FlowLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.Padding = new Padding(15, 8, 15, 5);
            bar.WrapContents = false;

            var lblQty = new Label();
            lblQty.Text = "Quantity";
            lblQty.AutoSize = true;
            lblQty.Padding = new Padding(5, 10, 5, 0);

            qty.Height = 35;

            var add = Theme.Button(
                "Add to Cart",
                AddToCart
            );

            var review = Theme.Button(
                "Write / Update Review",
                WriteReview
            );

            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );

            add.Width = 170;
            review.Width = 210;
            close.Width = 150;

            bar.Controls.Add(lblQty);
            bar.Controls.Add(qty);
            bar.Controls.Add(add);
            bar.Controls.Add(review);
            bar.Controls.Add(close);

            // ITEM INFORMATION
            info.Dock = DockStyle.Fill;
            info.Padding = new Padding(25, 20, 25, 10);
            info.Font = new Font("Segoe UI", 11F);
            info.AutoSize = false;

            // REVIEW GRID
            g.Dock = DockStyle.Fill;
            g.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(bar, 0, 1);
            root.Controls.Add(info, 0, 2);
            root.Controls.Add(g, 0, 3);

            Controls.Add(root);

            Load += (o, e) => LoadData();
        }

        void LoadData()
        {
            var t = Repositories.ItemDetails(item);

            if (t.Rows.Count > 0)
            {
                var r = t.Rows[0];

                decimal price =
                    Convert.ToDecimal(r["Price"]);

                decimal discount =
                    Convert.ToDecimal(r["Discount"]);

                decimal finalPrice =
                    price * (1 - discount / 100m);

                info.Text =
                    r["ItemName"].ToString() +
                    " (" + r["ItemType"].ToString() + ")" +

                    "\nGym: " +
                    r["GymName"].ToString() +

                    "   |   Category: " +
                    r["CategoryName"].ToString() +

                    "\nPrice: Tk " +
                    price.ToString("N2") +

                    (discount > 0
                        ? "   |   Offer: " +
                          discount.ToString("0") +
                          "%   |   Final Price: Tk " +
                          finalPrice.ToString("N2")
                        : "") +

                    "\nLocation: " +
                    r["Address"].ToString() +

                    "\n\nDescription: " +
                    r["Description"].ToString();
            }

            g.DataSource =
                Repositories.ItemReviews(item);
        }

        void AddToCart(object o, EventArgs e)
        {
            Repositories.AddToCart(
                Session.UserId,
                item,
                Convert.ToInt32(qty.Value)
            );

            MessageBox.Show(
                "Item added to cart successfully."
            );
        }

        void WriteReview(object o, EventArgs e)
        {
            new ReviewForm(item).ShowDialog();

            // review dile sathe sathe grid refresh
            LoadData();
        }
    }
}