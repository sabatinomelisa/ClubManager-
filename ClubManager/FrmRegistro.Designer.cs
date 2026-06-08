namespace ClubManager
{
    partial class FrmRegistro
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNroDoc = new System.Windows.Forms.TextBox();
            this.cmbTipDoc = new System.Windows.Forms.ComboBox();
            this.lblTipDoc = new System.Windows.Forms.Label();
            this.lblNroDoc = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblApellido = new System.Windows.Forms.Label();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblNacionalidad = new System.Windows.Forms.Label();
            this.txtNacionalidad = new System.Windows.Forms.TextBox();
            this.lblFecNac = new System.Windows.Forms.Label();
            this.txtFecNac = new System.Windows.Forms.TextBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtNroDoc
            // 
            this.txtNroDoc.Location = new System.Drawing.Point(183, 113);
            this.txtNroDoc.Name = "txtNroDoc";
            this.txtNroDoc.Size = new System.Drawing.Size(172, 22);
            this.txtNroDoc.TabIndex = 0;
            // 
            // cmbTipDoc
            // 
            this.cmbTipDoc.FormattingEnabled = true;
            this.cmbTipDoc.Items.AddRange(new object[] {
            "DNI",
            "Pasaporte"});
            this.cmbTipDoc.Location = new System.Drawing.Point(183, 79);
            this.cmbTipDoc.Name = "cmbTipDoc";
            this.cmbTipDoc.Size = new System.Drawing.Size(172, 24);
            this.cmbTipDoc.TabIndex = 1;
            // 
            // lblTipDoc
            // 
            this.lblTipDoc.AutoSize = true;
            this.lblTipDoc.Location = new System.Drawing.Point(39, 79);
            this.lblTipDoc.Name = "lblTipDoc";
            this.lblTipDoc.Size = new System.Drawing.Size(126, 16);
            this.lblTipDoc.TabIndex = 2;
            this.lblTipDoc.Text = "Tipo de Documento";
            // 
            // lblNroDoc
            // 
            this.lblNroDoc.AutoSize = true;
            this.lblNroDoc.Location = new System.Drawing.Point(19, 113);
            this.lblNroDoc.Name = "lblNroDoc";
            this.lblNroDoc.Size = new System.Drawing.Size(146, 16);
            this.lblNroDoc.TabIndex = 3;
            this.lblNroDoc.Text = "Número de Documento";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(183, 148);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(172, 22);
            this.txtNombre.TabIndex = 4;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Location = new System.Drawing.Point(109, 151);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(56, 16);
            this.lblNombre.TabIndex = 5;
            this.lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            this.lblApellido.AutoSize = true;
            this.lblApellido.Location = new System.Drawing.Point(108, 185);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(57, 16);
            this.lblApellido.TabIndex = 7;
            this.lblApellido.Text = "Apellido";
            // 
            // txtApellido
            // 
            this.txtApellido.Location = new System.Drawing.Point(183, 182);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(172, 22);
            this.txtApellido.TabIndex = 6;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Location = new System.Drawing.Point(40, 289);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(125, 16);
            this.lblUsuario.TabIndex = 9;
            this.lblUsuario.Text = "Nombre de Usuario";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(183, 283);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(172, 22);
            this.txtUsuario.TabIndex = 8;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(89, 325);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(76, 16);
            this.lblPassword.TabIndex = 11;
            this.lblPassword.Text = "Contraseña";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(183, 319);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(172, 22);
            this.txtPassword.TabIndex = 10;
            // 
            // lblNacionalidad
            // 
            this.lblNacionalidad.AutoSize = true;
            this.lblNacionalidad.Location = new System.Drawing.Point(77, 257);
            this.lblNacionalidad.Name = "lblNacionalidad";
            this.lblNacionalidad.Size = new System.Drawing.Size(88, 16);
            this.lblNacionalidad.TabIndex = 15;
            this.lblNacionalidad.Text = "Nacionalidad";
            // 
            // txtNacionalidad
            // 
            this.txtNacionalidad.Location = new System.Drawing.Point(183, 251);
            this.txtNacionalidad.Name = "txtNacionalidad";
            this.txtNacionalidad.Size = new System.Drawing.Size(172, 22);
            this.txtNacionalidad.TabIndex = 14;
            // 
            // lblFecNac
            // 
            this.lblFecNac.AutoSize = true;
            this.lblFecNac.Location = new System.Drawing.Point(30, 222);
            this.lblFecNac.Name = "lblFecNac";
            this.lblFecNac.Size = new System.Drawing.Size(135, 16);
            this.lblFecNac.TabIndex = 13;
            this.lblFecNac.Text = "Fecha de Nacimiento";
            this.lblFecNac.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFecNac
            // 
            this.txtFecNac.Location = new System.Drawing.Point(183, 216);
            this.txtFecNac.Name = "txtFecNac";
            this.txtFecNac.Size = new System.Drawing.Size(172, 22);
            this.txtFecNac.TabIndex = 12;
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Location = new System.Drawing.Point(183, 372);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(172, 32);
            this.btnRegistrar.TabIndex = 16;
            this.btnRegistrar.Text = "Registrar Usuario";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // FrmRegistro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.lblNacionalidad);
            this.Controls.Add(this.txtNacionalidad);
            this.Controls.Add(this.lblFecNac);
            this.Controls.Add(this.txtFecNac);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.lblApellido);
            this.Controls.Add(this.txtApellido);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblNroDoc);
            this.Controls.Add(this.lblTipDoc);
            this.Controls.Add(this.cmbTipDoc);
            this.Controls.Add(this.txtNroDoc);
            this.Name = "FrmRegistro";
            this.Text = "Registro de Usuario";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNroDoc;
        private System.Windows.Forms.ComboBox cmbTipDoc;
        private System.Windows.Forms.Label lblTipDoc;
        private System.Windows.Forms.Label lblNroDoc;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblApellido;
        private System.Windows.Forms.TextBox txtApellido;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblNacionalidad;
        private System.Windows.Forms.TextBox txtNacionalidad;
        private System.Windows.Forms.Label lblFecNac;
        private System.Windows.Forms.TextBox txtFecNac;
        private System.Windows.Forms.Button btnRegistrar;
    }
}