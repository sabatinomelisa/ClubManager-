using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClubManager
{
    public class FrmInsigniasAdmin : Form
    {
        public FrmInsigniasAdmin()
        {
            Inicializar();
        }

        private void Inicializar()
        {
            Text = "Administración de insignias";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 900;
            Height = 540;
            KeyPreview = true;
            KeyDown += delegate(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Escape) Close(); };

            VisualStyleHelper.AplicarEstiloBase(this);

            Label titulo = CrearLabel("Administración de insignias", 40, 80, 760, 34, 16F, FontStyle.Bold);
            Label ayuda = CrearLabel("Seleccione qué operación quiere realizar. Separar estas acciones evita mezclar asignaciones a socios con edición del catálogo.", 40, 125, 780, 45, 10F, FontStyle.Regular);

            Button btnAsignar = CrearBoton("Buscar socio / asignar insignia", 60, 205, 330, 60);
            btnAsignar.Click += delegate { AbrirHijo(new FrmAsignarInsigniaSocio()); };

            Button btnCatalogo = CrearBoton("Crear o editar insignias", 430, 205, 330, 60);
            btnCatalogo.Click += delegate { AbrirHijo(new FrmCatalogoInsignias()); };

            Label descAsignar = CrearLabel("Use esta opción para buscar un socio, ver sus insignias actuales y asignarle una insignia con un nivel válido.", 60, 275, 330, 70, 9F, FontStyle.Regular);
            Label descCatalogo = CrearLabel("Use esta opción para crear nuevas insignias, cargar imagen y administrar sus niveles/requisitos.", 430, 275, 330, 70, 9F, FontStyle.Regular);

            Button btnVolver = CrearBoton("Volver al menú", 60, 390, 250, 40);
            btnVolver.Click += delegate { Close(); };
        }

        private void AbrirHijo(Form form)
        {
            Hide();
            form.FormClosed += delegate { Show(); BringToFront(); };
            form.Show();
        }

        private Label CrearLabel(string texto, int left, int top, int width, int height, float size, FontStyle style)
        {
            Label label = new Label();
            label.Text = texto;
            label.Left = left;
            label.Top = top;
            label.Width = width;
            label.Height = height;
            label.ForeColor = Color.White;
            label.BackColor = Color.FromArgb(120, 0, 0, 0);
            label.Font = new Font("Segoe UI", size, style);
            Controls.Add(label);
            return label;
        }

        private Button CrearBoton(string texto, int left, int top, int width, int height)
        {
            Button boton = new Button();
            boton.Text = texto;
            boton.Left = left;
            boton.Top = top;
            boton.Width = width;
            boton.Height = height;
            boton.BackColor = Color.FromArgb(75, 12, 24);
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderColor = Color.White;
            boton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            Controls.Add(boton);
            return boton;
        }
    }
}
