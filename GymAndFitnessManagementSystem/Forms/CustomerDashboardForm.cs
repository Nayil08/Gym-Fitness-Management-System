using System;
using System.Drawing;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;

namespace GymAndFitnessManagementSystem.Forms
{
    public class CustomerDashboardForm : Form
    {
        public CustomerDashboardForm()
        {
            Theme.Apply(this, "Customer Dashboard");

            // MAIN LAYOUT
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 2;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 80)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header(
                "Customer Dashboard - " + Session.FullName
            );
            header.Dock = DockStyle.Fill;

            // BODY
            var body = new TableLayoutPanel();
            body.Dock = DockStyle.Fill;
            body.Padding = new Padding(35, 25, 35, 25);

            body.ColumnCount = 3;
            body.RowCount = 3;

            body.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.33F)
            );

            body.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.33F)
            );

            body.ColumnStyles.Add(
                new ColumnStyle(SizeType.Percent, 33.34F)
            );

            body.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 100)
            );

            body.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 100)
            );

            body.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // BUTTONS
            AddButton(
                body,
                "Browse Gyms & Items",
                () => new BrowseForm().ShowDialog(),
                0, 0
            );

            AddButton(
                body,
                "Shopping Cart",
                () => new CartForm().ShowDialog(),
                1, 0
            );

            AddButton(
                body,
                "Order History / Invoice",
                () => new OrderHistoryForm().ShowDialog(),
                2, 0
            );

            AddButton(
                body,
                "My Profile",
                () => new ProfileForm().ShowDialog(),
                0, 1
            );

            AddButton(
                body,
                "Logout",
                () => Close(),
                1, 1,
                true
            );

            // INFORMATION TEXT
            var info = new Label();
            info.Text =
                "Search and compare membership packages, training services " +
                "and products from all approved gyms. Use filters for " +
                "category, type, price and location.";

            info.Dock = DockStyle.Fill;
            info.Font = new Font("Segoe UI", 11F);
            info.Padding = new Padding(10, 15, 10, 10);

            body.Controls.Add(info, 0, 2);
            body.SetColumnSpan(info, 3);

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(body, 0, 1);

            Controls.Add(root);
        }

        void AddButton(
            TableLayoutPanel panel,
            string text,
            Action action,
            int column,
            int row,
            bool danger = false)
        {
            var button = Theme.Button(
                text,
                (o, e) => action(),
                danger
            );

            button.Dock = DockStyle.Fill;
            button.Margin = new Padding(10);

            panel.Controls.Add(
                button,
                column,
                row
            );
        }
    }
}