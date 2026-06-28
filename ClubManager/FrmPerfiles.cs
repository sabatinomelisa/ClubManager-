using SERVICIOS.Composite;
using BLL;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ClubManager
{
    public class FrmPerfiles : Form
    {
        private readonly RolBLL rolBLL;
        private ComboBox cmbRoles;
        private TreeView tvwPermisos;
        private Button btnCerrar;
        private Label lblRol;
        private Label lblTitulo;

        public FrmPerfiles()
        {
            rolBLL = new RolBLL();
            InicializarControles();
            CargarRoles();
            VisualStyleHelper.AplicarEstiloBase(this);
        }

        private void InicializarControles()
        {
            Text = "Gestión de perfiles";
            StartPosition = FormStartPosition.CenterScreen;
            Width = 650;
            Height = 500;

            lblTitulo = new Label();
            lblTitulo.Text = "Árbol de roles y permisos";
            lblTitulo.Left = 20;
            lblTitulo.Top = 20;
            lblTitulo.Width = 400;
            lblTitulo.Height = 24;

            lblRol = new Label();
            lblRol.Text = "Rol";
            lblRol.Left = 20;
            lblRol.Top = 60;
            lblRol.Width = 80;

            cmbRoles = new ComboBox();
            cmbRoles.Left = 110;
            cmbRoles.Top = 55;
            cmbRoles.Width = 300;
            cmbRoles.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRoles.SelectedIndexChanged += cmbRoles_SelectedIndexChanged;

            tvwPermisos = new TreeView();
            tvwPermisos.Left = 20;
            tvwPermisos.Top = 100;
            tvwPermisos.Width = 590;
            tvwPermisos.Height = 310;

            btnCerrar = new Button();
            btnCerrar.Text = "Cerrar";
            btnCerrar.Left = 500;
            btnCerrar.Top = 420;
            btnCerrar.Width = 110;
            btnCerrar.Click += btnCerrar_Click;

            Controls.Add(lblTitulo);
            Controls.Add(lblRol);
            Controls.Add(cmbRoles);
            Controls.Add(tvwPermisos);
            Controls.Add(btnCerrar);
        }

        private void CargarRoles()
        {
            try
            {
                List<Rol> roles = rolBLL.ListarRoles();
                cmbRoles.DataSource = roles;
                cmbRoles.DisplayMember = "Nombre";
                cmbRoles.ValueMember = "Id";
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void CargarArbolDePermisos()
        {
            if (cmbRoles.SelectedItem == null)
            {
                return;
            }

            try
            {
                Rol rolSeleccionado = (Rol)cmbRoles.SelectedItem;
                Rol rolCompleto = rolBLL.ObtenerRolConComponentes(rolSeleccionado.Id);

                tvwPermisos.Nodes.Clear();

                TreeNode nodoRol = new TreeNode(rolCompleto.Nombre);
                nodoRol.Tag = rolCompleto;
                tvwPermisos.Nodes.Add(nodoRol);

                AgregarComponentesAlNodo(nodoRol, rolCompleto);
                nodoRol.ExpandAll();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void AgregarComponentesAlNodo(TreeNode nodoPadre, Componente componentePadre)
        {
            foreach (Componente componenteHijo in componentePadre.ObtenerComponentes())
            {
                string prefijo = componenteHijo.EsRol ? "Rol: " : "Permiso: ";
                TreeNode nodoHijo = new TreeNode(prefijo + componenteHijo.Nombre);
                nodoHijo.Tag = componenteHijo;
                nodoPadre.Nodes.Add(nodoHijo);

                AgregarComponentesAlNodo(nodoHijo, componenteHijo);
            }
        }

        private void cmbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarArbolDePermisos();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
