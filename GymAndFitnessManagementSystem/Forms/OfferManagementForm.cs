using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class OfferManagementForm : Form
    {
        DataGridView g = Theme.Grid();
        ComboBox item = Theme.Combo();
        TextBox pct = Theme.Text();
        DateTimePicker st = new DateTimePicker();
        DateTimePicker en = new DateTimePicker();

        public OfferManagementForm()
        {
            Theme.Apply(this, "Offers");

            var layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 1;
            layout.RowCount = 3;

            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 64));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            var header = Theme.Header("Offers & Discounts");
            header.Dock = DockStyle.Fill;

            var bar = Theme.Toolbar();
            bar.Dock = DockStyle.Fill;

            item.Width = 210;
            pct.Width = 70;
            st.Width = 125;
            en.Width = 125;

            bar.Controls.Add(item);
            bar.Controls.Add(pct);
            bar.Controls.Add(st);
            bar.Controls.Add(en);
            bar.Controls.Add(Theme.Button("Create Offer", Add));
            bar.Controls.Add(Theme.Button("Delete", Del, true));
            bar.Controls.Add(Theme.Button("Close", (o, e) => Close()));

            g.Dock = DockStyle.Fill;

            layout.Controls.Add(header, 0, 0);
            layout.Controls.Add(bar, 0, 1);
            layout.Controls.Add(g, 0, 2);

            Controls.Add(layout);

            Load += (o, e) => LoadAll();
        }

        void LoadAll()
        {
            var t = Repositories.ItemsForGym(Session.GymId.Value);

            item.DataSource = t;
            item.DisplayMember = "ItemName";
            item.ValueMember = "ItemId";

            g.DataSource = Repositories.Offers(Session.GymId.Value);
        }

        void Add(object o, EventArgs e)
        {
            decimal p;

            if (item.SelectedValue == null ||
                !decimal.TryParse(pct.Text, out p) ||
                p <= 0 ||
                p >= 100 ||
                en.Value.Date < st.Value.Date)
            {
                MessageBox.Show(
                    "Discount must be 1-99 and end date cannot be before start date."
                );
                return;
            }

            Repositories.AddOffer(
                Convert.ToInt32(item.SelectedValue),
                p,
                st.Value.Date,
                en.Value.Date.AddDays(1).AddSeconds(-1)
            );

            LoadAll();
        }

        void Del(object o, EventArgs e)
        {
            if (g.CurrentRow == null)
                return;

            Repositories.DeleteOffer(
                Convert.ToInt32(g.CurrentRow.Cells["OfferId"].Value)
            );

            LoadAll();
        }
    }
}