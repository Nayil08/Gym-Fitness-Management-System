using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class PlatformReportsForm : Form
    {
        public PlatformReportsForm()
        {
            Theme.Apply(this, "Platform Reports");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 2;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 75)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header(
                "Platform Sales, Commission and Quality Report"
            );
            header.Dock = DockStyle.Fill;

            // TABS
            var tabs = new TabControl();
            tabs.Dock = DockStyle.Fill;
            tabs.Padding = new System.Drawing.Point(15, 8);

            var summaryTab = new TabPage("Summary");
            var lowRatedTab = new TabPage("Low Rated Gyms");

            tabs.TabPages.Add(summaryTab);
            tabs.TabPages.Add(lowRatedTab);

            // SUMMARY
            var flow = new FlowLayoutPanel();
            flow.Dock = DockStyle.Fill;
            flow.Padding = new Padding(25, 25, 20, 20);
            flow.AutoScroll = true;
            flow.WrapContents = true;

            var s = Repositories.PlatformSummary().Rows[0];

            flow.Controls.Add(
                Theme.Card(
                    "Orders",
                    s["Orders"].ToString()
                )
            );

            flow.Controls.Add(
                Theme.Card(
                    "Gross Sales",
                    "Tk " +
                    Convert.ToDecimal(
                        s["GrossSales"]
                    ).ToString("N2")
                )
            );

            flow.Controls.Add(
                Theme.Card(
                    "Commission",
                    "Tk " +
                    Convert.ToDecimal(
                        s["Commission"]
                    ).ToString("N2")
                )
            );

            flow.Controls.Add(
                Theme.Card(
                    "Selling Gyms",
                    s["SellingGyms"].ToString()
                )
            );

            summaryTab.Controls.Add(flow);

            // LOW RATED GYMS GRID
            var g = Theme.Grid();
            g.Dock = DockStyle.Fill;
            g.DataSource = Repositories.LowRatedGyms();

            lowRatedTab.Controls.Add(g);

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(tabs, 0, 1);

            Controls.Add(root);
        }
    }
}