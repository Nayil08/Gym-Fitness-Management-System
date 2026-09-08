using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class OrderHistoryForm : Form
    {
        DataGridView g = Theme.Grid();

        public OrderHistoryForm()
        {
            Theme.Apply(this, "Order History");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 3;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 64)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 65)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header(
                "Order History & Invoice"
            );
            header.Dock = DockStyle.Fill;

            // BUTTON BAR
            var bar = new FlowLayoutPanel();
            bar.Dock = DockStyle.Fill;
            bar.Padding = new Padding(15, 7, 15, 5);
            bar.WrapContents = false;
            bar.BackColor = Color.White;

            var refresh = Theme.Button(
                "Refresh",
                (o, e) => LoadData()
            );

            var invoice = Theme.Button(
                "View Invoice",
                Invoice
            );

            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );

            refresh.Width = 160;
            invoice.Width = 170;
            close.Width = 150;

            bar.Controls.Add(refresh);
            bar.Controls.Add(invoice);
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
                Repositories.Orders(
                    Session.UserId
                );
        }

        void Invoice(object o, EventArgs e)
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show(
                    "Select an order first."
                );
                return;
            }

            int order = Convert.ToInt32(
                g.CurrentRow.Cells["OrderId"].Value
            );

            var items =
                Repositories.OrderItems(order);

            var sb = new StringBuilder();

            sb.AppendLine(
                "GYM AND FITNESS MANAGEMENT SYSTEM"
            );

            sb.AppendLine(
                "Invoice - Order #" + order
            );

            sb.AppendLine(
                "Customer: " + Session.FullName
            );

            sb.AppendLine(
                new string('-', 55)
            );

            decimal total = 0;

            foreach (System.Data.DataRow r in items.Rows)
            {
                decimal subtotal =
                    Convert.ToDecimal(r["Subtotal"]);

                sb.AppendLine(
                    r["ItemName"] +
                    " | " +
                    r["GymName"] +
                    " | Qty " +
                    r["Quantity"] +
                    " | Tk " +
                    subtotal.ToString("N2")
                );

                total += subtotal;
            }

            sb.AppendLine(
                new string('-', 55)
            );

            sb.AppendLine(
                "TOTAL: Tk " +
                total.ToString("N2")
            );

            var f = new Form();
            f.Text = "Invoice";
            f.Size = new Size(650, 600);
            f.StartPosition =
                FormStartPosition.CenterParent;

            var t = new TextBox();
            t.Multiline = true;
            t.ReadOnly = true;
            t.Dock = DockStyle.Fill;
            t.Font = new Font(
                "Consolas",
                11F
            );
            t.ScrollBars = ScrollBars.Both;
            t.Text = sb.ToString();

            f.Controls.Add(t);
            f.ShowDialog();
        }
    }
}