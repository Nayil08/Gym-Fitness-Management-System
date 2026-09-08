using System;
using System.Windows.Forms;
using GymAndFitnessManagementSystem.Common;
using GymAndFitnessManagementSystem.Data;

namespace GymAndFitnessManagementSystem.Forms
{
    public class TrainerManagementForm : Form
    {
        DataGridView g = Theme.Grid();

        TextBox name = Theme.Text();
        TextBox specialization = Theme.Text();
        TextBox phone = Theme.Text();
        TextBox experience = Theme.Text();
        TextBox availability = Theme.Text();

        public TrainerManagementForm()
        {
            Theme.Apply(this, "Trainer Management");

            var root = new TableLayoutPanel();
            root.Dock = DockStyle.Fill;
            root.ColumnCount = 1;
            root.RowCount = 3;

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 64)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 165)
            );

            root.RowStyles.Add(
                new RowStyle(SizeType.Percent, 100)
            );

            // HEADER
            var header = Theme.Header("Trainer Management");
            header.Dock = DockStyle.Fill;

            // EDITOR AREA
            var editor = new TableLayoutPanel();
            editor.Dock = DockStyle.Fill;
            editor.ColumnCount = 1;
            editor.RowCount = 2;
            editor.Padding = new Padding(12, 8, 12, 5);

            editor.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 95)
            );

            editor.RowStyles.Add(
                new RowStyle(SizeType.Absolute, 55)
            );

            // INPUTS
            var inputs = new FlowLayoutPanel();
            inputs.Dock = DockStyle.Fill;
            inputs.WrapContents = false;
            inputs.AutoScroll = true;

            name.Width = 170;
            specialization.Width = 190;
            phone.Width = 150;
            experience.Width = 130;
            availability.Width = 180;

            inputs.Controls.Add(
                Field("Name", name, 185)
            );

            inputs.Controls.Add(
                Field("Specialization", specialization, 205)
            );

            inputs.Controls.Add(
                Field("Phone", phone, 165)
            );

            inputs.Controls.Add(
                Field("Experience (Years)", experience, 150)
            );

            inputs.Controls.Add(
                Field("Availability", availability, 195)
            );

            // BUTTON AREA
            var buttons = new FlowLayoutPanel();
            buttons.Dock = DockStyle.Fill;
            buttons.WrapContents = false;

            var add = Theme.Button(
                "Add Trainer",
                Add
            );
            add.Width = 170;
            add.Height = 42;

            var del = Theme.Button(
                "Delete",
                Del,
                true
            );
            del.Width = 150;
            del.Height = 42;

            var close = Theme.Button(
                "Close",
                (o, e) => Close()
            );
            close.Width = 150;
            close.Height = 42;

            buttons.Controls.Add(add);
            buttons.Controls.Add(del);
            buttons.Controls.Add(close);

            editor.Controls.Add(inputs, 0, 0);
            editor.Controls.Add(buttons, 0, 1);

            // GRID
            g.Dock = DockStyle.Fill;

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(editor, 0, 1);
            root.Controls.Add(g, 0, 2);

            Controls.Add(root);

            Load += (o, e) => LoadData();
        }

        Panel Field(string title, Control control, int width)
        {
            var panel = new Panel();
            panel.Width = width;
            panel.Height = 85;
            panel.Margin = new Padding(5);

            var label = new Label();
            label.Text = title;
            label.Dock = DockStyle.Top;
            label.Height = 28;

            control.Dock = DockStyle.Bottom;

            panel.Controls.Add(control);
            panel.Controls.Add(label);

            return panel;
        }

        void LoadData()
        {
            if (Session.GymId.HasValue)
            {
                g.DataSource =
                    Repositories.Trainers(
                        Session.GymId.Value
                    );
            }
        }

        void Add(object o, EventArgs e)
        {
            int exp;

            if (string.IsNullOrWhiteSpace(name.Text) ||
                !int.TryParse(experience.Text, out exp) ||
                exp < 0)
            {
                MessageBox.Show(
                    "Enter trainer name and valid numeric experience."
                );
                return;
            }

            Repositories.AddTrainer(
                Session.GymId.Value,
                name.Text.Trim(),
                specialization.Text.Trim(),
                phone.Text.Trim(),
                exp,
                availability.Text.Trim()
            );

            LoadData();
            ClearFields();

            MessageBox.Show(
                "Trainer added successfully."
            );
        }

        void Del(object o, EventArgs e)
        {
            if (g.CurrentRow == null)
            {
                MessageBox.Show(
                    "Select a trainer first."
                );
                return;
            }

            Repositories.DeleteTrainer(
                Convert.ToInt32(
                    g.CurrentRow.Cells["TrainerId"].Value
                )
            );

            LoadData();

            MessageBox.Show(
                "Trainer deleted successfully."
            );
        }

        void ClearFields()
        {
            name.Clear();
            specialization.Clear();
            phone.Clear();
            experience.Clear();
            availability.Clear();
        }
    }
}