using System;
using System.Drawing;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class SuperAdminDashboardForm : Form
    {
        FlowLayoutPanel cards = new FlowLayoutPanel();

        public SuperAdminDashboardForm()
        {
            Theme.Apply(this, "Super Admin Dashboard");

            // MAIN LAYOUT
            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 3;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 80)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 140)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header(
                "Super Admin Dashboard - " + Session.FullName
            );
            header.Dock = DockStyle.Fill;

            // MENU
            var menu = new FlowLayoutPanel();
            menu.Dock = DockStyle.Fill;
            menu.Padding = new Padding(15, 10, 15, 5);
            menu.WrapContents = true;
            menu.AutoScroll = true;
            menu.BackColor = Color.White;

            menu.Controls.Add(
                Theme.Button(
                    "Manage Gym Owners",
                    (o, e) =>
                        new ManageGymOwnersForm().ShowDialog()
                )
            );

            menu.Controls.Add(
                Theme.Button(
                    "Categories",
                    (o, e) =>
                        new CategoryManagementForm().ShowDialog()
                )
            );

            menu.Controls.Add(
                Theme.Button(
                    "Platform Reports",
                    (o, e) =>
                        new PlatformReportsForm().ShowDialog()
                )
            );

            menu.Controls.Add(
                Theme.Button(
                    "Review Moderation",
                    (o, e) =>
                        new ReviewModerationForm().ShowDialog()
                )
            );

            menu.Controls.Add(
                Theme.Button(
                    "Profile",
                    (o, e) =>
                        new ProfileForm().ShowDialog()
                )
            );

            menu.Controls.Add(
                Theme.Button(
                    "Logout",
                    (o, e) => Close(),
                    true
                )
            );

            // SUMMARY CARDS
            cards.Dock = DockStyle.Fill;
            cards.Padding = new Padding(25, 15, 25, 25);
            cards.AutoScroll = true;
            cards.WrapContents = true;
            cards.BackColor = Theme.Background;

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(menu, 0, 1);
            root.Controls.Add(cards, 0, 2);

            Controls.Add(root);

            Load += RefreshData;
        }

        void RefreshData(object o, EventArgs e)
        {
            try
            {
                cards.Controls.Clear();

                var s = Repositories.PlatformSummary();

                if (s.Rows.Count == 0)
                    return;

                var r = s.Rows[0];

                cards.Controls.Add(
                    Theme.Card(
                        "Paid Orders",
                        r["Orders"].ToString()
                    )
                );

                cards.Controls.Add(
                    Theme.Card(
                        "Gross Sales",
                        "Tk " +
                        Convert.ToDecimal(
                            r["GrossSales"]
                        ).ToString("N0")
                    )
                );

                cards.Controls.Add(
                    Theme.Card(
                        "Platform Commission",
                        "Tk " +
                        Convert.ToDecimal(
                            r["Commission"]
                        ).ToString("N0")
                    )
                );

                cards.Controls.Add(
                    Theme.Card(
                        "Selling Gyms",
                        r["SellingGyms"].ToString()
                    )
                );

                var info = new Label();

                info.Text =
                    "Super Admin controls business approval, " +
                    "categories, platform-wide reporting and " +
                    "review moderation. Gym-owner data is kept " +
                    "separate by GymId.";

                info.Width = 1000;
                info.Height = 70;
                info.Font = new Font("Segoe UI", 11F);
                info.Padding = new Padding(10);

                cards.Controls.Add(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}