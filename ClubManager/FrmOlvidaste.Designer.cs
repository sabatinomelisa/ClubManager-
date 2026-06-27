namespace ClubManager
{
    partial class FrmOlvidaste
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOlvidaste));
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.txtViejaPass = new System.Windows.Forms.TextBox();
            this.txtNuevaPass = new System.Windows.Forms.TextBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblViejaPass = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCambiarPass = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.btnVolver = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtUsuario
            // 
            this.txtUsuario.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsuario.Location = new System.Drawing.Point(307, 181);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(368, 29);
            this.txtUsuario.TabIndex = 0;
            // 
            // txtViejaPass
            // 
            this.txtViejaPass.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtViejaPass.Location = new System.Drawing.Point(307, 226);
            this.txtViejaPass.Name = "txtViejaPass";
            this.txtViejaPass.Size = new System.Drawing.Size(368, 29);
            this.txtViejaPass.TabIndex = 1;
            // 
            // txtNuevaPass
            // 
            this.txtNuevaPass.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNuevaPass.Location = new System.Drawing.Point(307, 271);
            this.txtNuevaPass.Name = "txtNuevaPass";
            this.txtNuevaPass.Size = new System.Drawing.Size(368, 29);
            this.txtNuevaPass.TabIndex = 2;
            // 
            // lblUsuario
            // 
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.BackColor = System.Drawing.Color.Transparent;
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblUsuario.Location = new System.Drawing.Point(121, 184);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(166, 22);
            this.lblUsuario.TabIndex = 3;
            this.lblUsuario.Text = "Nombre de Usuario";
            // 
            // lblViejaPass
            // 
            this.lblViejaPass.AutoSize = true;
            this.lblViejaPass.BackColor = System.Drawing.Color.Transparent;
            this.lblViejaPass.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblViejaPass.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblViejaPass.Location = new System.Drawing.Point(132, 234);
            this.lblViejaPass.Name = "lblViejaPass";
            this.lblViejaPass.Size = new System.Drawing.Size(155, 22);
            this.lblViejaPass.TabIndex = 4;
            this.lblViejaPass.Text = "Contraseña Actual";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(133, 278);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 22);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nueva Contraseña";
            // 
            // btnCambiarPass
            // 
            this.btnCambiarPass.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCambiarPass.Location = new System.Drawing.Point(307, 312);
            this.btnCambiarPass.Name = "btnCambiarPass";
            this.btnCambiarPass.Size = new System.Drawing.Size(368, 31);
            this.btnCambiarPass.TabIndex = 6;
            this.btnCambiarPass.Text = "Cambiar Contraseña";
            this.btnCambiarPass.UseVisualStyleBackColor = true;
            this.btnCambiarPass.Click += new System.EventHandler(this.btnCambiarPass_Click);
            // 
            // lblResultado
            // 
            this.lblResultado.AutoSize = true;
            this.lblResultado.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultado.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblResultado.Location = new System.Drawing.Point(304, 417);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(0, 22);
            this.lblResultado.TabIndex = 7;
            // 
            // btnVolver
            // 
            this.btnVolver.Font = new System.Drawing.Font("Microsoft Tai Le", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVolver.Location = new System.Drawing.Point(308, 354);
            this.btnVolver.Name = "btnVolver";
            this.btnVolver.Size = new System.Drawing.Size(368, 31);
            this.btnVolver.TabIndex = 8;
            this.btnVolver.Text = "Volver";
            this.btnVolver.UseVisualStyleBackColor = true;
            this.btnVolver.Click += new System.EventHandler(this.btnVolver_Click);
            // 
            // FrmOlvidaste
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnVolver);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.btnCambiarPass);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblViejaPass);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.txtNuevaPass);
            this.Controls.Add(this.txtViejaPass);
            this.Controls.Add(this.txtUsuario);
            this.DoubleBuffered = true;
            this.Name = "FrmOlvidaste";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "¿Olvidaste tu Contraseña?";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.TextBox txtViejaPass;
        private System.Windows.Forms.TextBox txtNuevaPass;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblViejaPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCambiarPass;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Button btnVolver;
    }
}