using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using BLL;
using BE;

namespace ClubManager
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmBienvenida bienvenida = new FrmBienvenida();
            bienvenida.Show();
            Application.DoEvents();
            Thread.Sleep(3000);
            bienvenida.Close();

            VerificarIntegridadAntesDelLogin();

            Application.Run(new FrmInicio());
        }

        private static void VerificarIntegridadAntesDelLogin()
        {
            try
            {
                IntegridadBLL integridadBLL = new IntegridadBLL();
                integridadBLL.InicializarSiCorresponde();
                List<ResultadoIntegridadBE> resultados = integridadBLL.VerificarIntegridad();

                foreach (ResultadoIntegridadBE resultado in resultados)
                {
                    if (!resultado.Correcto)
                    {
                        MessageBox.Show(
                            "Se detectó un problema de integridad antes del login. Revisar Seguridad > Dígitos Verificadores.\n\n" + resultado.Mensaje,
                            "Control de integridad",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "No se pudo ejecutar la verificación inicial de integridad. Revisar la base de datos.\n\n" + exception.Message,
                    "Control de integridad",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
    }
}
