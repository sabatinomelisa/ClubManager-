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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRegistro));
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
            this.lblContraseña = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblNacionalidad = new System.Windows.Forms.Label();
            this.txtNacionalidad = new System.Windows.Forms.TextBox();
            this.lblFecNac = new System.Windows.Forms.Label();
            this.txtFecNac = new System.Windows.Forms.TextBox();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.btnVolver = new System.Windows.Forms.Button();
            this.lblMail = new System.Windows.Forms.Label();
            this.txtMail = new System.Windows.Forms.TextBox();
            this.lblTelefono = new System.Windows.Forms.Label();
            this.txtTelefono = new System.Windows.Forms.TextBox();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblIdioma = new System.Windows.Forms.Label();
            this.cmbIdiomas = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txtNroDoc
            // 
            this.txtNroDoc.Location = new System.Drawing.Point(221, 226);
            this.txtNroDoc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.cmbTipDoc.Location = new System.Drawing.Point(221, 191);
            this.cmbTipDoc.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbTipDoc.Name = "cmbTipDoc";
            this.cmbTipDoc.Size = new System.Drawing.Size(172, 24);
            this.cmbTipDoc.TabIndex = 1;
            // 
            // lblTipDoc
            // 
            this.lblTipDoc.AutoSize = true;
            this.lblTipDoc.BackColor = System.Drawing.Color.Transparent;
            this.lblTipDoc.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTipDoc.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTipDoc.Location = new System.Drawing.Point(46, 192);
            this.lblTipDoc.Name = "lblTipDoc";
            this.lblTipDoc.Size = new System.Drawing.Size(162, 22);
            this.lblTipDoc.TabIndex = 2;
            this.lblTipDoc.Text = "Tipo de Documento";
            // 
            // lblNroDoc
            // 
            this.lblNroDoc.AutoSize = true;
            this.lblNroDoc.BackColor = System.Drawing.Color.Transparent;
            this.lblNroDoc.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNroDoc.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblNroDoc.Location = new System.Drawing.Point(16, 226);
            this.lblNroDoc.Name = "lblNroDoc";
            this.lblNroDoc.Size = new System.Drawing.Size(192, 22);
            this.lblNroDoc.TabIndex = 3;
            this.lblNroDoc.Text = "Número de Documento";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(221, 262);
            this.txtNombre.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(172, 22);
            this.txtNombre.TabIndex = 4;
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.BackColor = System.Drawing.Color.Transparent;
            this.lblNombre.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblNombre.Location = new System.Drawing.Point(135, 262);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(73, 22);
            this.lblNombre.TabIndex = 5;
            this.lblNombre.Text = "Nombre";
            // 
            // lblApellido
            // 
            this.lblApellido.AutoSize = true;
            this.lblApellido.BackColor = System.Drawing.Color.Transparent;
            this.lblApellido.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApellido.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblApellido.Location = new System.Drawing.Point(136, 297);
            this.lblApellido.Name = "lblApellido";
            this.lblApellido.Size = new System.Drawing.Size(72, 22);
            this.lblApellido.TabIndex = 7;
            this.lblApellido.Text = "Apellido";
            // 
            // txtApellido
            // 
            this.txtApellido.Location = new System.Drawing.Point(221, 297);
            this.txtApellido.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtApellido.Name = "txtApellido";
            this.txtApellido.Size = new System.Drawing.Size(172, 22);
            this.txtApellido.TabIndex = 6;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.Color.Transparent;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblUsuario.Location = new System.Drawing.Point(422, 262);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(160, 22);
            this.lblUsuario.TabIndex = 9;
            this.lblUsuario.Text = "Nombre de Usuario";
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(604, 262);
            this.txtUsuario.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(172, 22);
            this.txtUsuario.TabIndex = 8;
            // 
            // lblContraseña
            // 
            this.lblContraseña.AutoSize = true;
            this.lblContraseña.BackColor = System.Drawing.Color.Transparent;
            this.lblContraseña.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContraseña.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblContraseña.Location = new System.Drawing.Point(485, 297);
            this.lblContraseña.Name = "lblContraseña";
            this.lblContraseña.Size = new System.Drawing.Size(97, 22);
            this.lblContraseña.TabIndex = 11;
            this.lblContraseña.Text = "Contraseña";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(604, 297);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(172, 22);
            this.txtPassword.TabIndex = 10;
            // 
            // lblNacionalidad
            // 
            this.lblNacionalidad.AutoSize = true;
            this.lblNacionalidad.BackColor = System.Drawing.Color.Transparent;
            this.lblNacionalidad.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNacionalidad.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblNacionalidad.Location = new System.Drawing.Point(472, 226);
            this.lblNacionalidad.Name = "lblNacionalidad";
            this.lblNacionalidad.Size = new System.Drawing.Size(110, 22);
            this.lblNacionalidad.TabIndex = 15;
            this.lblNacionalidad.Text = "Nacionalidad";
            // 
            // txtNacionalidad
            // 
            this.txtNacionalidad.Location = new System.Drawing.Point(604, 226);
            this.txtNacionalidad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtNacionalidad.Name = "txtNacionalidad";
            this.txtNacionalidad.Size = new System.Drawing.Size(172, 22);
            this.txtNacionalidad.TabIndex = 14;
            // 
            // lblFecNac
            // 
            this.lblFecNac.AutoSize = true;
            this.lblFecNac.BackColor = System.Drawing.Color.Transparent;
            this.lblFecNac.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFecNac.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblFecNac.Location = new System.Drawing.Point(411, 192);
            this.lblFecNac.Name = "lblFecNac";
            this.lblFecNac.Size = new System.Drawing.Size(171, 22);
            this.lblFecNac.TabIndex = 13;
            this.lblFecNac.Text = "Fecha de Nacimiento";
            this.lblFecNac.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtFecNac
            // 
            this.txtFecNac.Location = new System.Drawing.Point(604, 192);
            this.txtFecNac.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFecNac.Name = "txtFecNac";
            this.txtFecNac.Size = new System.Drawing.Size(172, 22);
            this.txtFecNac.TabIndex = 12;
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrar.Location = new System.Drawing.Point(604, 358);
            this.btnRegistrar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(172, 32);
            this.btnRegistrar.TabIndex = 16;
            this.btnRegistrar.Text = "Registrar Usuario";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.BackColor = System.Drawing.Color.Transparent;
            this.lblResultado.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultado.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblResultado.Location = new System.Drawing.Point(225, 410);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(0, 22);
            this.lblResultado.TabIndex = 17;
            // 
            // btnVolver
            // 
            this.btnVolver.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVolver.Location = new System.Drawing.Point(604, 402);
            this.btnVolver.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(172, 30);
            this.btnVolver.TabIndex = 18;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // lblMail
            // 
            this.lblMail.AutoSize = true;
            this.lblMail.BackColor = System.Drawing.Color.Transparent;
            this.lblMail.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMail.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblMail.Location = new System.Drawing.Point(157, 332);
            this.lblMail.Name = "lblMail";
            this.lblMail.Size = new System.Drawing.Size(51, 22);
            this.lblMail.TabIndex = 20;
            this.lblMail.Text = "eMail";
            // 
            // txtMail
            // 
            this.txtMail.Location = new System.Drawing.Point(221, 332);
            this.txtMail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMail.Name = "txtMail";
            this.txtMail.Size = new System.Drawing.Size(172, 22);
            this.txtMail.TabIndex = 19;
            // 
            // lblTelefono
            // 
            this.lblTelefono.AutoSize = true;
            this.lblTelefono.BackColor = System.Drawing.Color.Transparent;
            this.lblTelefono.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTelefono.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTelefono.Location = new System.Drawing.Point(132, 368);
            this.lblTelefono.Name = "lblTelefono";
            this.lblTelefono.Size = new System.Drawing.Size(76, 22);
            this.lblTelefono.TabIndex = 22;
            this.lblTelefono.Text = "Teléfono";
            // 
            // txtTelefono
            // 
            this.txtTelefono.Location = new System.Drawing.Point(221, 368);
            this.txtTelefono.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTelefono.Name = "txtTelefono";
            this.txtTelefono.Size = new System.Drawing.Size(172, 22);
            this.txtTelefono.TabIndex = 21;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.BackColor = System.Drawing.Color.Transparent;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Tai Le", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTitulo.Location = new System.Drawing.Point(213, 118);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(363, 39);
            this.lblTitulo.TabIndex = 23;
            this.lblTitulo.Text = "Registro de Nuevo Socio";
            // 
            // lblIdioma
            // 
            this.lblIdioma.AutoSize = true;
            this.lblIdioma.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblIdioma.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdioma.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblIdioma.Location = new System.Drawing.Point(555, 24);
            this.lblIdioma.Name = "lblIdioma";
            this.lblIdioma.Size = new System.Drawing.Size(63, 22);
            this.lblIdioma.TabIndex = 25;
            this.lblIdioma.Text = "Idioma";
            // 
            // cmbIdiomas
            // 
            this.cmbIdiomas.FormattingEnabled = true;
            this.cmbIdiomas.Location = new System.Drawing.Point(655, 22);
            this.cmbIdiomas.Name = "cmbIdiomas";
            this.cmbIdiomas.Size = new System.Drawing.Size(121, 24);
            this.cmbIdiomas.TabIndex = 24;
            this.cmbIdiomas.SelectedIndexChanged += new System.EventHandler(this.cmbIdiomas_SelectedIndexChanged_1);
            // 
            // FrmRegistro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblIdioma);
            this.Controls.Add(this.cmbIdiomas);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblTelefono);
            this.Controls.Add(this.txtTelefono);
            this.Controls.Add(this.lblMail);
            this.Controls.Add(this.txtMail);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.lblNacionalidad);
            this.Controls.Add(this.txtNacionalidad);
            this.Controls.Add(this.lblFecNac);
            this.Controls.Add(this.txtFecNac);
            this.Controls.Add(this.lblContraseña);
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
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmRegistro";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registro de Usuario";
            this.Load += new System.EventHandler(this.FrmRegistro_Load);
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
        private System.Windows.Forms.Label lblContraseña;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblNacionalidad;
        private System.Windows.Forms.TextBox txtNacionalidad;
        private System.Windows.Forms.Label lblFecNac;
        private System.Windows.Forms.TextBox txtFecNac;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Button btnVolver;
        private System.Windows.Forms.Label lblMail;
        private System.Windows.Forms.TextBox txtMail;
        private System.Windows.Forms.Label lblTelefono;
        private System.Windows.Forms.TextBox txtTelefono;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblIdioma;
        private System.Windows.Forms.ComboBox cmbIdiomas;
    }
}