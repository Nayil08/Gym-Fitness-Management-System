using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class AdminDashboardForm : Form
    {
        public AdminDashboardForm()
        {
            Theme.Apply(this, "Gym Owner Dashboard");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 2;
            root.Padding = new Padding(0);

            // Header-er jonno separate row
            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 85)
            );

            // Menu-er jonno baki space
            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            var header = Theme.Header(
                "Gym Owner Dashboard - " + Session.FullName
            );
            header.Dock = DockStyle.Fill;

            var menu = new FlowLayoutPanel();
            menu.Dock = DockStyle.Fill;
            menu.Padding = new Padding(35, 25, 35, 35);
            menu.AutoScroll = true;
            menu.WrapContents = true;

            Add(menu, "Gym Profile",
                () => new GymProfileForm().ShowDialog());

            Add(menu, "Products & Services",
                () => new ItemManagementForm().ShowDialog());

            Add(menu, "Trainers",
                () => new TrainerManagementForm().ShowDialog());

            Add(menu, "Inventory / Low Stock",
                () => new InventoryForm().ShowDialog());

            Add(menu, "Sales & Earnings",
                () => new AdminSalesForm().ShowDialog());

            Add(menu, "Offers & Discounts",
                () => new OfferManagementForm().ShowDialog());

            Add(menu, "Customer Reviews",
                () => new AdminReviewsForm().ShowDialog());

            Add(menu, "My Profile",
                () => new ProfileForm().ShowDialog());

            Add(menu, "Logout",
                () => Close(), true);

            try
            {
                decimal earn = Session.GymId.HasValue
                    ? Repositories.GymEarnings(Session.GymId.Value)
                    : 0;

                menu.Controls.Add(
                    Theme.Card(
                        "Current Net Earnings",
                        "Tk " + earn.ToString("N2")
                    )
                );
            }
            catch
            {
            }

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(menu, 0, 1);

            Controls.Add(root);
        }

        void Add(
            FlowLayoutPanel p,
            string text,
            Action action,
            bool danger = false)
        {
            var b = Theme.Button(
                text,
                (o, e) => action(),
                danger
            );

            b.Width = 245;
            b.Height = 58;

            p.Controls.Add(b);
        }
    }
}