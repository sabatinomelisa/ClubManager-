using System;
using System.Threading;
using System.Windows.Forms;
using BLL;

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

            InicializarIntegridad();

            Application.Run(new FrmInicio());
        }

        private static void InicializarIntegridad()
        {
            try
            {
                IntegridadBLL integridadBLL = new IntegridadBLL();
                integridadBLL.InicializarSiCorresponde();
            }
            catch
            {
                // La validación obligatoria se realiza en login/logout.
            }
        }
    }
}
