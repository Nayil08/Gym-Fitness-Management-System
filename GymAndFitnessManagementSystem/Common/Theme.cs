using System;
using System.Drawing;
using System.Windows.Forms;
namespace GymAndFitnessManagementSystem.Common
{
    public static class Theme
    {
        public static readonly Color Primary = Color.FromArgb(34, 87, 122);
        public static readonly Color Accent = Color.FromArgb(56, 176, 0);
        public static readonly Color Background = Color.FromArgb(245, 248, 250);
        public static readonly Color Danger = Color.FromArgb(190, 45, 45);
        public static void Apply(Form f, string title)
        {
            f.Text = title + " - Gym and Fitness Management System"; f.BackColor = Background; f.Font = new Font("Segoe UI", 10F); f.StartPosition = FormStartPosition.CenterScreen; f.Size = new Size(1120, 720); f.MinimumSize = new Size(980, 620);
        }
        public static Label Header(string text) => new Label { Text = text, Dock = DockStyle.Top, Height = 64, Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold), ForeColor = Color.White, BackColor = Primary, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(24,0,0,0) };
        public static Button Button(string text, EventHandler click, bool danger=false)
        {
            var b = new Button { Text = text, Height = 42, Width = 190, FlatStyle = FlatStyle.Flat, BackColor = danger ? Danger : Primary, ForeColor = Color.White, Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold), Cursor = Cursors.Hand, Margin = new Padding(8) };
            b.FlatAppearance.BorderSize = 0; if(click != null) b.Click += click; return b;
        }
        public static TextBox Text(string placeholder="") => new TextBox { Width = 250, Font = new Font("Segoe UI", 10F), Tag = placeholder, Margin = new Padding(8) };
        public static ComboBox Combo() => new ComboBox { Width = 220, DropDownStyle = ComboBoxStyle.DropDownList, Margin = new Padding(8) };
        public static DataGridView Grid()
        {
            return new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false, BackgroundColor = Color.White, BorderStyle = BorderStyle.Fixed3D };
        }
        public static FlowLayoutPanel Toolbar() => new FlowLayoutPanel { Dock = DockStyle.Top, Height = 65, Padding = new Padding(12,10,12,5), AutoScroll = true, BackColor = Color.White };
        public static Panel Card(string title, string value)
        {
            var p = new Panel { Width=235, Height=105, BackColor=Color.White, Margin=new Padding(10), Padding=new Padding(14), BorderStyle=BorderStyle.FixedSingle };
            p.Controls.Add(new Label{Text=value,Dock=DockStyle.Bottom,Height=48,Font=new Font("Segoe UI Semibold",22F,FontStyle.Bold),ForeColor=Primary});
            p.Controls.Add(new Label{Text=title,Dock=DockStyle.Top,Height=28,Font=new Font("Segoe UI",10F),ForeColor=Color.DimGray}); return p;
        }
    }
}
