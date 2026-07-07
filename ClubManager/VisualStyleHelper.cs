using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ClubManager
{
    public static class VisualStyleHelper
    {
        private static readonly Color ColorPrincipal = Color.FromArgb(112, 20, 36);
        private static readonly Color ColorPrincipalOscuro = Color.FromArgb(75, 12, 24);
        private static readonly Color ColorFondoPanel = Color.FromArgb(235, 255, 255, 255);

        public static void AplicarEstiloBase(Form formulario)
        {
            HabilitarDobleBuffer(formulario);
            formulario.SuspendLayout();
            formulario.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            formulario.BackColor = Color.White;
            formulario.FormBorderStyle = FormBorderStyle.FixedSingle;
            formulario.MaximizeBox = false;

            try
            {
                ComponentResourceManager resources = new ComponentResourceManager(typeof(FrmRegistro));
                Image imagenFondo = resources.GetObject("$this.BackgroundImage") as Image;
                if (imagenFondo != null)
                {
                    formulario.BackgroundImage = imagenFondo;
                    formulario.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
            catch
            {
                formulario.BackColor = ColorPrincipal;
            }

            AplicarEstiloControles(formulario.Controls);
            formulario.ResumeLayout(false);
            formulario.PerformLayout();
        }


        public static void HabilitarDobleBuffer(Control control)
        {
            if (control == null)
            {
                return;
            }

            try
            {
                PropertyInfo propiedad = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
                if (propiedad != null)
                {
                    propiedad.SetValue(control, true, null);
                }
            }
            catch
            {
                // La optimización visual no debe impedir el uso del formulario.
            }
        }

        private static void AplicarEstiloControles(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                HabilitarDobleBuffer(control);
                Label label = control as Label;
                if (label != null)
                {
                    label.BackColor = Color.Transparent;
                    label.ForeColor = Color.White;
                    label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }

                Button boton = control as Button;
                if (boton != null)
                {
                    boton.BackColor = ColorPrincipalOscuro;
                    boton.ForeColor = Color.White;
                    boton.FlatStyle = FlatStyle.Flat;
                    boton.FlatAppearance.BorderColor = Color.White;
                    boton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                }

                TextBox textBox = control as TextBox;
                if (textBox != null)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.BackColor = Color.White;
                    textBox.ForeColor = Color.Black;
                }

                ComboBox comboBox = control as ComboBox;
                if (comboBox != null)
                {
                    comboBox.BackColor = Color.White;
                    comboBox.ForeColor = Color.Black;
                }

                DataGridView grilla = control as DataGridView;
                if (grilla != null)
                {
                    grilla.BackgroundColor = Color.White;
                    grilla.BorderStyle = BorderStyle.FixedSingle;
                    grilla.EnableHeadersVisualStyles = false;
                    grilla.ColumnHeadersDefaultCellStyle.BackColor = ColorPrincipalOscuro;
                    grilla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    grilla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                    grilla.DefaultCellStyle.BackColor = Color.White;
                    grilla.DefaultCellStyle.ForeColor = Color.Black;
                    grilla.DefaultCellStyle.SelectionBackColor = ColorPrincipal;
                    grilla.DefaultCellStyle.SelectionForeColor = Color.White;
                    grilla.RowHeadersVisible = false;
                }

                TreeView treeView = control as TreeView;
                if (treeView != null)
                {
                    treeView.BackColor = Color.White;
                    treeView.ForeColor = Color.Black;
                    treeView.BorderStyle = BorderStyle.FixedSingle;
                }

                if (control.HasChildren)
                {
                    AplicarEstiloControles(control.Controls);
                }
            }
        }
    }
}
