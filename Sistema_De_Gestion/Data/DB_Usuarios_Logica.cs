using Microsoft.Data.SqlClient;
using Sistema_De_Gestion.Models;
using System.Data;
using System.Text;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Sistema_De_Gestion.Data
{
    //En esta clase vamos a crear toda la logica para los usuarios
    public class DB_Usuarios_Logica
    {
        private readonly EstudioJuridicoContext _context;

        //creamos una lista de usuarios
        public List<TableUsuario> ListaUsuarios()
        {
            var estudioJuridicoContext = _context.TableUsuarios;
            return estudioJuridicoContext.ToList();
        }

        //Metodo para saber si el usuario es valido
        public TableUsuario UsuarioValido(string username, string password)
        {
            try
            {
                //obtenemos el usuario
                TableUsuario user = ObtenerUsuario(username);

                if (!string.IsNullOrEmpty(user.Usuario))
                {
                    byte[] hashedPassword = Encrypt.HashPassword(Encoding.UTF8.GetBytes(password), user.Token);
                    if (hashedPassword.SequenceEqual(user.Pass)) { return user; }
                    else { return null; }
                }
                else { return null; }
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        private TableUsuario ObtenerUsuario(string username)
        {
            TableUsuario user = new TableUsuario();
            using (SqlConnection connection = new SqlConnection("server=SQL5101.site4now.net; database=db_a8f929_estudiojuridico; User Id=db_a8f929_estudiojuridico_admin; Password=Estudio1998...;"))
            {
                string saltSaved = "select Usuario, Token, Pass, ID_Rol from Table_Usuarios where Usuario = @Usuario";

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = saltSaved;
                    command.Parameters.Add("@Usuario", SqlDbType.VarChar, 50).Value = username;

                    try
                    {
                        connection.Open();
                        using (SqlDataReader oReader = command.ExecuteReader())
                        {
                            if (oReader.Read())
                            {
                                user.Usuario = oReader["Usuario"].ToString();
                                user.Token = (byte[])oReader["Token"];
                                user.Pass = (byte[])oReader["Pass"];
                                user.IdRol = Convert.ToInt32(oReader["ID_Rol"]);
                            }
                        }
                    }
                    catch (SqlException ex)
                    {}
                    finally
                    {connection.Close();}
                }
            }
            return (user);
        }
    }
}