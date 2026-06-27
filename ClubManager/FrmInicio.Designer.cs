namespace ClubManager
{
    partial class FrmInicio
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInicio));
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.btnIngresar = new System.Windows.Forms.Button();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.btnOlvidaste = new System.Windows.Forms.Button();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.lblContraseña = new System.Windows.Forms.Label();
            this.btnSalir = new System.Windows.Forms.Button();
            this.cmbIdiomas = new System.Windows.Forms.ComboBox();
            this.lblIdioma = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.Location = new System.Drawing.Point(303, 176);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(269, 29);
            this.txtUsername.TabIndex = 1;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(303, 212);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(269, 29);
            this.txtPassword.TabIndex = 2;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Tai Le", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblUsuario.Location = new System.Drawing.Point(193, 174);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(104, 33);
            this.lblUsuario.TabIndex = 3;
            this.lblUsuario.Text = "Usuario";
            // 
            // btnIngresar
            // 
            this.btnIngresar.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIngresar.Location = new System.Drawing.Point(303, 256);
            this.btnIngresar.Name = "btnIngresar";
            this.btnIngresar.Size = new System.Drawing.Size(269, 31);
            this.btnIngresar.TabIndex = 5;
            this.btnIngresar.Text = "Ingresar";
            this.btnIngresar.UseVisualStyleBackColor = true;
            this.btnIngresar.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRegistrar.Location = new System.Drawing.Point(303, 292);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(269, 30);
            this.btnRegistrar.TabIndex = 6;
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnOlvidaste
            // 
            this.btnOlvidaste.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOlvidaste.Location = new System.Drawing.Point(303, 327);
            this.btnOlvidaste.Name = "btnOlvidaste";
            this.btnOlvidaste.Size = new System.Drawing.Size(269, 30);
            this.btnOlvidaste.TabIndex = 7;
            this.btnOlvidaste.Text = "¿Olvidaste tu contraseña?";
            this.btnOlvidaste.UseVisualStyleBackColor = true;
            this.btnOlvidaste.Click += new System.EventHandler(this.btnOlvidaste_Click);
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.BackColor = System.Drawing.Color.Transparent;
            this.lblMensaje.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblMensaje.Location = new System.Drawing.Point(299, 405);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(0, 22);
            this.lblMensaje.TabIndex = 8;
            // 
            // lblContraseña
            // 
            this.lblContraseña.AutoSize = true;
            this.lblContraseña.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblContraseña.Font = new System.Drawing.Font("Microsoft Tai Le", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblContraseña.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblContraseña.Location = new System.Drawing.Point(150, 215);
            this.lblContraseña.Name = "lblContraseña";
            this.lblContraseña.Size = new System.Drawing.Size(147, 33);
            this.lblContraseña.TabIndex = 9;
            this.lblContraseña.Text = "Contraseña";
            // 
            // btnSalir
            // 
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.Location = new System.Drawing.Point(303, 362);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(269, 30);
            this.btnSalir.TabIndex = 10;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // cmbIdiomas
            // 
            this.cmbIdiomas.FormattingEnabled = true;
            this.cmbIdiomas.Location = new System.Drawing.Point(667, 29);
            this.cmbIdiomas.Name = "cmbIdiomas";
            this.cmbIdiomas.Size = new System.Drawing.Size(121, 24);
            this.cmbIdiomas.TabIndex = 11;
            this.cmbIdiomas.SelectedIndexChanged += new System.EventHandler(this.cmbIdiomas_SelectedIndexChanged);
            // 
            // lblIdioma
            // 
            this.lblIdioma.AutoSize = true;
            this.lblIdioma.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblIdioma.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIdioma.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblIdioma.Location = new System.Drawing.Point(567, 31);
            this.lblIdioma.Name = "lblIdioma";
            this.lblIdioma.Size = new System.Drawing.Size(63, 22);
            this.lblIdioma.TabIndex = 12;
            this.lblIdioma.Text = "Idioma";
            // 
            // FrmInicio
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblIdioma);
            this.Controls.Add(this.cmbIdiomas);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.lblContraseña);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.btnOlvidaste);
            this.Controls.Add(this.btnRegistrar);
            this.Controls.Add(this.btnIngresar);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
            this.DoubleBuffered = true;
            this.Name = "FrmInicio";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Club Manager+";
            this.Load += new System.EventHandler(this.FrmInicio_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Button btnIngresar;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.Button btnOlvidaste;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Label lblContraseña;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.ComboBox cmbIdiomas;
        private System.Windows.Forms.Label lblIdioma;
    }
}

