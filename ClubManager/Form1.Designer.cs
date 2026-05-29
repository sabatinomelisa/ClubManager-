namespace ClubManager
{
    partial class Form1
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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.btnRegistrarLogin = new System.Windows.Forms.Button();
            this.btnRegistrarLogout = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.dgvBitacora = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(12, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(172, 20);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Bitácora del sistema";
            // 
            // btnRegistrarLogin
            // 
            this.btnRegistrarLogin.Location = new System.Drawing.Point(16, 45);
            this.btnRegistrarLogin.Name = "btnRegistrarLogin";
            this.btnRegistrarLogin.Size = new System.Drawing.Size(130, 30);
            this.btnRegistrarLogin.TabIndex = 1;
            this.btnRegistrarLogin.Text = "Registrar login";
            this.btnRegistrarLogin.UseVisualStyleBackColor = true;
            this.btnRegistrarLogin.Click += new System.EventHandler(this.btnRegistrarLogin_Click);
            // 
            // btnRegistrarLogout
            // 
            this.btnRegistrarLogout.Location = new System.Drawing.Point(152, 45);
            this.btnRegistrarLogout.Name = "btnRegistrarLogout";
            this.btnRegistrarLogout.Size = new System.Drawing.Size(130, 30);
            this.btnRegistrarLogout.TabIndex = 2;
            this.btnRegistrarLogout.Text = "Registrar logout";
            this.btnRegistrarLogout.UseVisualStyleBackColor = true;
            this.btnRegistrarLogout.Click += new System.EventHandler(this.btnRegistrarLogout_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(288, 45);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(130, 30);
            this.btnActualizar.TabIndex = 3;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // dgvBitacora
            // 
            this.dgvBitacora.AllowUserToAddRows = false;
            this.dgvBitacora.AllowUserToDeleteRows = false;
            this.dgvBitacora.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBitacora.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBitacora.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBitacora.Location = new System.Drawing.Point(16, 90);
            this.dgvBitacora.Name = "dgvBitacora";
            this.dgvBitacora.ReadOnly = true;
            this.dgvBitacora.Size = new System.Drawing.Size(856, 359);
            this.dgvBitacora.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.dgvBitacora);
            this.Controls.Add(this.btnActualizar);
            this.Controls.Add(this.btnRegistrarLogout);
            this.Controls.Add(this.btnRegistrarLogin);
            this.Controls.Add(this.lblTitulo);
            this.Name = "Form1";
            this.Text = "Club Manager - Bitácora";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBitacora)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Button btnRegistrarLogin;
        private System.Windows.Forms.Button btnRegistrarLogout;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.DataGridView dgvBitacora;
    }
}
