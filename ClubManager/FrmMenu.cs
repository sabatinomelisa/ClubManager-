using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BE;
using BLL;
using SERVICIOS;
using SERVICIOS.Observer;

namespace ClubManager
{
    public partial class FrmMenu : Form, IOberverIdioma
    {
        public FrmMenu()
        {
            InitializeComponent();
            ConfigurarMenuPrincipal();
            TratamientoIdioma.Instancia.Suscribir(this);
            if (TratamientoIdioma.Instancia.IdiomaActual != null)
            {
                ActualizarIdioma();
            }
        }

        private void ConfigurarMenuPrincipal()
        {
            menuStrip1.Items.Clear();
            menuStrip1.Dock = DockStyle.Top;
            menuStrip2.Visible = false;

            ToolStripMenuItem seguridadMenu = CrearMenuItem("seguridadMenu", "Seguridad", null);
            ToolStripMenuItem bitacoraItem = CrearMenuItem("bitacoraItem", "Bitácora", bitacoraItem_Click);
            ToolStripMenuItem perfilesItem = CrearMenuItem("perfilesItem", "Perfiles", perfilesItem_Click);
            ToolStripMenuItem integridadItem = CrearMenuItem("integridadItem", "Dígitos verificadores", integridadItem_Click);
            ToolStripMenuItem controlCambiosItem = CrearMenuItem("controlCambiosItem", "Control de cambios", controlCambiosItem_Click);
            ToolStripMenuItem idiomasItem = CrearMenuItem("idiomasItem", "Idiomas", idiomasItem_Click);
            ToolStripMenuItem logoutItem = CrearMenuItem("logoutItem", "Cerrar sesión", logoutItem_Click);

            seguridadMenu.DropDownItems.Add(bitacoraItem);
            seguridadMenu.DropDownItems.Add(perfilesItem);
            seguridadMenu.DropDownItems.Add(integridadItem);
            seguridadMenu.DropDownItems.Add(controlCambiosItem);
            seguridadMenu.DropDownItems.Add(idiomasItem);
            seguridadMenu.DropDownItems.Add(logoutItem);

            ToolStripMenuItem clubMenu = CrearMenuItem("clubMenu", "Club", null);
            ToolStripMenuItem sociosItem = CrearMenuItem("sociosItem", "Socios", sociosItem_Click);
            ToolStripMenuItem pagosItem = CrearMenuItem("pagosItem", "Pagos y cuotas", pagosItem_Click);
            ToolStripMenuItem jugadoresItem = CrearMenuItem("jugadoresItem", "Jugadores", jugadoresItem_Click);
            ToolStripMenuItem eventosItem = CrearMenuItem("eventosItem", "Eventos deportivos", eventosItem_Click);
            ToolStripMenuItem finanzasItem = CrearMenuItem("finanzasItem", "Ingresos y egresos", finanzasItem_Click);
            ToolStripMenuItem publicacionesItem = CrearMenuItem("publicacionesItem", "Comunicación interna", publicacionesItem_Click);
            ToolStripMenuItem insigniasItem = CrearMenuItem("insigniasItem", "Insignias", insigniasItem_Click);
            ToolStripMenuItem reportesItem = CrearMenuItem("reportesItem", "Reportes", reportesItem_Click);

            clubMenu.DropDownItems.Add(sociosItem);
            clubMenu.DropDownItems.Add(pagosItem);
            clubMenu.DropDownItems.Add(jugadoresItem);
            clubMenu.DropDownItems.Add(eventosItem);
            clubMenu.DropDownItems.Add(finanzasItem);
            clubMenu.DropDownItems.Add(publicacionesItem);
            clubMenu.DropDownItems.Add(insigniasItem);
            clubMenu.DropDownItems.Add(reportesItem);

            menuStrip1.Items.Add(seguridadMenu);
            menuStrip1.Items.Add(clubMenu);
        }

        private ToolStripMenuItem CrearMenuItem(string nombre, string texto, EventHandler accionClick)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(texto);
            item.Name = nombre;

            if (accionClick != null)
            {
                item.Click += accionClick;
            }

            return item;
        }

        public void ActualizarIdioma()
        {
            try
            {
                if (TratamientoIdioma.Instancia.IdiomaActual == null)
                {
                    return;
                }

                TraduccionBLL traduccionBLL = new TraduccionBLL();
                List<TraduccionBE> traducciones = traduccionBLL.Listar(TratamientoIdioma.Instancia.IdiomaActual.Id);

                foreach (TraduccionBE traduccion in traducciones)
                {
                    AplicarTraduccionMenu(menuStrip1.Items, traduccion.NombreControl, traduccion.Traduccion);

                    if (traduccion.NombreControl == "frmMenu")
                    {
                        Text = traduccion.Traduccion;
                    }
                }
            }
            catch
            {
            }
        }

        private void AplicarTraduccionMenu(ToolStripItemCollection items, string nombreControl, string texto)
        {
            foreach (ToolStripItem item in items)
            {
                if (item.Name == nombreControl)
                {
                    item.Text = texto;
                }

                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null && menuItem.DropDownItems.Count > 0)
                {
                    AplicarTraduccionMenu(menuItem.DropDownItems, nombreControl, texto);
                }
            }
        }

        private void bitacoraItem_Click(object sender, EventArgs e)
        {
            new FrmBitacora().Show();
        }

        private void perfilesItem_Click(object sender, EventArgs e)
        {
            new FrmPerfiles().Show();
        }

        private void integridadItem_Click(object sender, EventArgs e)
        {
            new FrmIntegridad().Show();
        }

        private void controlCambiosItem_Click(object sender, EventArgs e)
        {
            new FrmControlCambios().Show();
        }

        private void idiomasItem_Click(object sender, EventArgs e)
        {
            new FrmIdiomas().Show();
        }

        private void sociosItem_Click(object sender, EventArgs e)
        {
            new FrmSocios().Show();
        }

        private void pagosItem_Click(object sender, EventArgs e)
        {
            new FrmModuloClub("Pagos").Show();
        }

        private void jugadoresItem_Click(object sender, EventArgs e)
        {
            new FrmModuloClub("Jugadores").Show();
        }

        private void eventosItem_Click(object sender, EventArgs e)
        {
            new FrmModuloClub("Eventos").Show();
        }

        private void finanzasItem_Click(object sender, EventArgs e)
        {
            new FrmModuloClub("Finanzas").Show();
        }

        private void publicacionesItem_Click(object sender, EventArgs e)
        {
            new FrmModuloClub("Comunicación").Show();
        }

        private void insigniasItem_Click(object sender, EventArgs e)
        {
            new FrmModuloClub("Insignias").Show();
        }

        private void reportesItem_Click(object sender, EventArgs e)
        {
            new FrmModuloClub("Reportes").Show();
        }

        private void logoutItem_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioBLL usuarioBLL = new UsuarioBLL();
                usuarioBLL.Logout();

                FrmInicio inicio = new FrmInicio();
                inicio.Show();
                Close();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            TratamientoIdioma.Instancia.Desuscribir(this);

            if (SessionManager.SesionIniciada)
            {
                try
                {
                    UsuarioBLL usuarioBLL = new UsuarioBLL();
                    usuarioBLL.Logout();
                }
                catch
                {
                }
            }

            base.OnFormClosing(e);
        }
    }
}
