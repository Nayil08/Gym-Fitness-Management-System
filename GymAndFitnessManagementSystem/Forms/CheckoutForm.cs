using System;
using System.Drawing;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class CheckoutForm : Form
    {
        ComboBox method = Theme.Combo();
        Label total = new Label();
        Label msg = new Label();

        public CheckoutForm()
        {
            Theme.Apply(this, "Checkout");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 2;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 64)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header("Checkout & Payment");
            header.Dock = DockStyle.Fill;

            // BODY
            var body = new Panel();
            body.Dock = DockStyle.Fill;
            body.BackColor = Theme.Background;

            int left = 180;

            // TOTAL
            total.SetBounds(left, 45, 600, 50);
            total.Font = new Font(
                "Segoe UI Semibold",
                18F,
                FontStyle.Bold
            );

            body.Controls.Add(total);

            // PAYMENT LABEL
            var lbl = new Label();
            lbl.Text = "Payment Method";
            lbl.SetBounds(left, 125, 300, 30);
            body.Controls.Add(lbl);

            // PAYMENT COMBO
            method.SetBounds(left, 160, 400, 35);

            method.Items.AddRange(
                new object[]
                {
                    "Cash",
                    "Card",
                    "Mobile Banking"
                }
            );

            method.SelectedIndex = 0;

            body.Controls.Add(method);

            // ERROR MESSAGE
            msg.SetBounds(left, 215, 600, 45);
            msg.ForeColor = Theme.Danger;
            body.Controls.Add(msg);

            // CONFIRM BUTTON
            var confirm = Theme.Button(
                "Confirm Payment",
                Pay
            );

            confirm.SetBounds(left, 285, 220, 48);
            body.Controls.Add(confirm);

            // CANCEL BUTTON
            var cancel = Theme.Button(
                "Cancel",
                (o, e) => Close()
            );

            cancel.SetBounds(left + 240, 285, 180, 48);
            body.Controls.Add(cancel);

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(body, 0, 1);

            Controls.Add(root);

            Load += (o, e) => LoadTotal();
        }

        void LoadTotal()
        {
            decimal sum = 0;

            foreach (System.Data.DataRow r
                in Repositories.Cart(Session.UserId).Rows)
            {
                sum += Convert.ToDecimal(
                    r["LineTotal"]
                );
            }

            total.Text =
                "Amount to Pay: Tk " +
                sum.ToString("N2");
        }

        void Pay(object o, EventArgs e)
        {
            try
            {
                int id = Repositories.Checkout(
                    Session.UserId,
                    method.Text
                );

                MessageBox.Show(
                    "Payment successful. Order #" +
                    id +
                    " created.\nInvoice is available in Order History."
                );

                Close();
            }
            catch (Exception ex)
            {
                msg.Text = ex.Message;
            }
        }
    }
}