using System.Drawing;
using System.Windows.Forms;

namespace ClubManager
{
    public static class PasswordVisibilityHelper
    {
        public static Button AgregarBoton(Form formulario, TextBox campoPassword)
        {
            Button boton = new Button();
            boton.Text = "👁";
            boton.Width = 38;
            boton.Height = campoPassword.Height;
            boton.Left = campoPassword.Right + 4;
            boton.Top = campoPassword.Top;
            boton.TabStop = false;
            boton.Cursor = Cursors.Hand;
            boton.UseVisualStyleBackColor = true;

            boton.Click += delegate
            {
                bool mostrarPassword = campoPassword.UseSystemPasswordChar;
                campoPassword.UseSystemPasswordChar = !mostrarPassword;
                boton.Text = mostrarPassword ? "🙈" : "👁";
                campoPassword.Focus();
            };

            formulario.Controls.Add(boton);
            boton.BringToFront();
            return boton;
        }
    }
}
