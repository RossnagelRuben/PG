using Sistema_De_Gestion.Models;
namespace Sistema_De_Gestion.Data
{
    //En esta clase vamos a crear toda la logica para los usuarios
    public class DA_Logica
    {
        //creamos una lista de usuarios
        public List<Usuario> ListaUsuarios()
        {
             return new List<Usuario>
            {
                new Usuario{ Nombre = "Ruben", Correo = "ruben@gmail.com", Password = "1234", Roles = new string[]{ "Administrador" } },
                new Usuario{ Nombre = "Caja", Correo = "caja@gmail.com", Password = "1234", Roles = new string[]{ "Cajero" } },
            };
        }
        //metodo para validacion de si el usuario existe
        public Usuario ValidarUsuario(string _correo, string _clave)
        {
            return ListaUsuarios().Where(item => item.Correo == _correo && item.Password == _clave).FirstOrDefault();
        }
    }
}