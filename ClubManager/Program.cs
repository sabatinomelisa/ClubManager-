using Patrones.Singleton.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClubManager
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Usuario usuario = new Usuario();
            usuario.Username = "prueba";
            usuario.Password = "prueba";

            try
            {
                SessionManager.Login(usuario);
                SessionManager u = SessionManager.GetInstance;
                SessionManager.Logout(usuario);

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
