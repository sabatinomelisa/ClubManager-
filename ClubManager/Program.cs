using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManager
{
    public static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Codigo para mostrar por tres segundos el formulario de bienvenida
            FrmBienvenida bienvenida = new FrmBienvenida();

            bienvenida.Show();

            Application.DoEvents();

            Thread.Sleep(3000); // 3 segundos

            bienvenida.Close();

            //Ir al inicio

            Application.Run(new FrmInicio());
        }
    }
}
