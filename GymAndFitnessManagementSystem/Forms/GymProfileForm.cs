using System;
using System.Drawing;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class GymProfileForm : Form
    {
        TextBox n = Theme.Text();
        TextBox a = Theme.Text();
        TextBox c = Theme.Text();
        Label st = new Label();

        public GymProfileForm()
        {
            Theme.Apply(this, "Gym Profile");

            // Main layout - header and content are completely separate
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 2;
            root.BackColor = Theme.Background;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 64)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header("Gym Profile");
            header.Dock = DockStyle.Fill;

            // BODY
            var body = new Panel();
            body.Dock = DockStyle.Fill;
            body.BackColor = Theme.Background;
            body.AutoScroll = true;

            int left = 180;
            int width = 600;
            int y = 40;

            // Gym Name
            var lblName = new Label();
            lblName.Text = "Gym Name";
            lblName.SetBounds(left, y, width, 25);
            body.Controls.Add(lblName);

            y += 30;

            n.SetBounds(left, y, width, 32);
            body.Controls.Add(n);

            y += 70;

            // Address
            var lblAddress = new Label();
            lblAddress.Text = "Address";
            lblAddress.SetBounds(left, y, width, 25);
            body.Controls.Add(lblAddress);

            y += 30;

            a.SetBounds(left, y, width, 32);
            body.Controls.Add(a);

            y += 70;

            // Contact
            var lblContact = new Label();
            lblContact.Text = "Contact";
            lblContact.SetBounds(left, y, width, 25);
            body.Controls.Add(lblContact);

            y += 30;

            c.SetBounds(left, y, width, 32);
            body.Controls.Add(c);

            y += 65;

            // Status and Commission
            st.SetBounds(left, y, width, 35);
            st.Font = new Font("Segoe UI Semibold", 11F);
            body.Controls.Add(st);

            y += 55;

            // Save button
            var save = Theme.Button(
                "Save Changes",
                (o, e) => SaveProfile()
            );

            save.SetBounds(left, y, 220, 45);
            body.Controls.Add(save);

            // Close button
            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );

            close.SetBounds(left + 240, y, 220, 45);
            body.Controls.Add(close);

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(body, 0, 1);

            Controls.Add(root);

            Load += (o, e) => LoadData();
        }

        void LoadData()
        {
            if (!Session.GymId.HasValue)
                return;

            var t = Repositories.GymProfile(
                Session.GymId.Value
            );

            if (t.Rows.Count == 0)
                return;

            var r = t.Rows[0];

            n.Text = r["GymName"].ToString();
            a.Text = r["Address"].ToString();
            c.Text = r["Contact"].ToString();

            decimal commission =
                Convert.ToDecimal(r["CommissionRate"]) * 100;

            st.Text =
                "Status: " + r["Status"].ToString() +
                "   |   Commission: " +
                commission.ToString("0") + "%";
        }

        void SaveProfile()
        {
            if (string.IsNullOrWhiteSpace(n.Text))
            {
                MessageBox.Show("Gym name is required.");
                return;
            }

            if (string.IsNullOrWhiteSpace(a.Text))
            {
                MessageBox.Show("Address is required.");
                return;
            }

            Repositories.UpdateGym(
                Session.GymId.Value,
                n.Text.Trim(),
                a.Text.Trim(),
                c.Text.Trim()
            );

            MessageBox.Show(
                "Gym profile updated successfully."
            );

            LoadData();
        }
    }
}