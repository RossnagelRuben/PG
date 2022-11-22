using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sistema_De_Gestion.Models;
namespace Sistema_De_Gestion.Data
{
    public class Logs
    {
        //Contexto Base de Datos
        private readonly EstudioJuridicoContext _context;
        public Logs(EstudioJuridicoContext context)
        {
            _context = context;
        }

        //ignorar avisos de ciclos json
        JsonSerializerOptions options = new()
        {
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            WriteIndented = true
        };

        #region Conexion con la DB
        //Conexion con la DB
        SqlConnection CONNDB = new SqlConnection("server=SQL5101.site4now.net; database=db_a8f929_estudiojuridico; User Id=db_a8f929_estudiojuridico_admin; Password=Estudio1998...;");
        #endregion

        //  --- --- --- Clientes --- --- ---  //

        #region Logs Clientes

        //serializamos Alta Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public async Task<Boolean> Serialize_Alt_Cliente(int id, string usuario)
        {
            try
            {
                //obtenemos el OBJ por medio del ID
                var cliente = await _context.TableClientes.FirstOrDefaultAsync(j => j.IdCliente == id);
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(cliente, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Se dio de Alta Cliente ID: " + id.ToString();
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //serializamos la Mod del Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public async Task<Boolean> Serialize_Mod_Cliente(int id, string usuario)
        {
            try
            {
                //obtenemos el OBJ por medio del ID
                var cliente = await _context.TableClientes.FirstOrDefaultAsync(j => j.IdCliente == id);
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(cliente, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Se Modifico Cliente ID: " + id.ToString();
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //serializamos el Delet del Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public async Task<Boolean> Serialize_Delet_Cliente(int id, string usuario)
        {
            try
            {
                //obtenemos el OBJ por medio del ID
                var cliente = await _context.TableClientes.FirstOrDefaultAsync(j => j.IdCliente == id);
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(cliente, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Se Elimino Cliente ID: " + id.ToString();
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //serializamos el Error al dar de Alta el Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public void Serialize_Error_Alt_Cliente(TableCliente cliente, string usuario, string detalles)
        {
            try
            {
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(cliente, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Error Al momento de querer dar de alta Cliente.";
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, detaller, FechaLog) values ('{0}','{1}','{2}','{3}','{4}')", Mensj, usuario, SerializeOBJ, detalles ,datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //serializamos el Error al Modificar el Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public void Serialize_Error_Mod_Cliente(int id, TableCliente cliente, string usuario, string detalles)
        {
            try
            {
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(cliente, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Error Al momento de querer Modificar Cliente ID: " + id;
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, detaller, FechaLog) values ('{0}','{1}','{2}','{3}','{4}')", Mensj, usuario, SerializeOBJ, detalles, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        //  --- --- --- Correos --- --- ---  //

        #region Correos

        public void Serialize_Alt_Correo(TableCorreo correoOBJ, string usuario)
        {
            try
            {
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(correoOBJ, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Se dio de Alta Correo ID: " + correoOBJ.IdCorreo;
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //serializamos la Mod del Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public async Task<Boolean> Serialize_Mod_Correo(int id, string usuario)
        {
            try
            {
                //obtenemos el OBJ por medio del ID
                var correo = await _context.TableCorreos.FirstOrDefaultAsync(j => j.IdCorreo == id);
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(correo, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Se Modifico Correo ID: " + id.ToString();
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                    return true;

                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //serializamos el Delet del Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public async Task<Boolean> Serialize_Delet_Correo(int id, string usuario)
        {
            try
            {
                //obtenemos el OBJ por medio del ID
                var correo = await _context.TableCorreos.FirstOrDefaultAsync(j => j.IdCorreo == id);
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(correo, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Se Elimino Correo ID: " + id.ToString();
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        //  --- --- --- Telefonos --- --- ---  //

        #region Telefonos

        public void Serialize_Alt_Telefono(TableTelefono telefonoOBJ, string usuario)
        {
            try
            {
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(telefonoOBJ, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Se dio de Alta Telefono ID: " + telefonoOBJ.IdTelefono;
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //serializamos la Mod del Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public async Task<Boolean> Serialize_Mod_Telefono(int id, string usuario)
        {
            try
            {
                //obtenemos el OBJ por medio del ID
                var telefono = await _context.TableTelefonos.FirstOrDefaultAsync(j => j.IdTelefono == id);
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(telefono, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Se Modifico Telefono ID: " + id.ToString();
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //serializamos el Delet del Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public async Task<Boolean> Serialize_Delet_Telefono(int id, string usuario)
        {
            try
            {
                //obtenemos el OBJ por medio del ID
                var telefono = await _context.TableTelefonos.FirstOrDefaultAsync(j => j.IdTelefono == id);
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(telefono, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    string Mensj = "Se Elimino Telefono ID: " + id.ToString();
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        //  --- --- --- Relacion Cliente <-> Tramite --- --- ---  //

        #region Relacion Cliente <-> Tramite

        public void Serialize_Alt_Relacion(TableRelacionClienteTramite relacionOBJ, string usuario)
        {
            try
            {
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(relacionOBJ, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    //Cuscamos el tramite en la DB
                    TableSubTramite tramite = new TableSubTramite();
                    var listatramites = _context.TableSubTramites.ToList();
                    tramite = listatramites.Where(a => a.IdsubTramite == relacionOBJ.IdsubTramite).Single();
                    //Buscamos el cliente en la DB
                    TableCliente cliente = new TableCliente();
                    var listaclientes = _context.TableClientes.ToList();
                    cliente = listaclientes.Where(a => a.IdCliente == relacionOBJ.IdCliente).Single();
                    string Mensj = "Se dio de Alta Relacion ID: " + relacionOBJ.Idrelacion + " Tramite: " + tramite.TituloSubTramite + " Cliente: " + cliente.Apellidos + " " + cliente.Nombres;
                    //Agregamos la Fecha
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //serializamos la Mod del Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public void Serialize_Mod_Relacion(int id, string usuario)
        {
            try
            {
                //obtenemos el OBJ por medio del ID
                TableRelacionClienteTramite relacionOBJ = new TableRelacionClienteTramite();
                var listaRelacion = _context.TableRelacionClienteTramites.ToList();
                relacionOBJ = listaRelacion.Where(a => a.Idrelacion == id).Single();
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(relacionOBJ, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    //Cuscamos el tramite en la DB
                    TableSubTramite tramite = new TableSubTramite();
                    var listatramites = _context.TableSubTramites.ToList();
                    tramite = listatramites.Where(a => a.IdsubTramite == relacionOBJ.IdsubTramite).Single();
                    //Buscamos el cliente en la DB
                    TableCliente cliente = new TableCliente();
                    var listaclientes = _context.TableClientes.ToList();
                    cliente = listaclientes.Where(a => a.IdCliente == relacionOBJ.IdCliente).Single();
                    string Mensj = "Se Modifico Relacion ID: " + relacionOBJ.Idrelacion + " Tramite: " + tramite.TituloSubTramite + " Cliente: " + cliente.Apellidos + " " + cliente.Nombres;
                    //Agregamos la Fecha
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //serializamos el Delet del Cliente - pasamos el objeto a tipo json y guardamos el Log en la DB
        public async Task<Boolean> Serialize_Delet_Relacion(int id, string usuario)
        {
            try
            {
                //obtenemos el OBJ por medio del ID
                var relacionOBJ = await _context.TableRelacionClienteTramites.FirstOrDefaultAsync(j => j.Idrelacion == id);
                //Serializamos el OBJ
                string SerializeOBJ = JsonSerializer.Serialize(relacionOBJ, options);
                //hacemos el insert a la DB...
                using (SqlConnection Conn = CONNDB)
                {
                    //Abrimos la conexion con la DB
                    CONNDB.Open();
                    int Retorno = 0;
                    //Cuscamos el tramite en la DB
                    TableSubTramite tramite = new TableSubTramite();
                    var listatramites = _context.TableSubTramites.ToList();
                    tramite = listatramites.Where(a => a.IdsubTramite == relacionOBJ.IdsubTramite).Single();
                    //Buscamos el cliente en la DB
                    TableCliente cliente = new TableCliente();
                    var listaclientes = _context.TableClientes.ToList();
                    cliente = listaclientes.Where(a => a.IdCliente == relacionOBJ.IdCliente).Single();
                    string Mensj = "Se Elimino Relacion ID: " + relacionOBJ.Idrelacion + " Tramite: " + tramite.TituloSubTramite + " Cliente: " + cliente.Apellidos + " " + cliente.Nombres;
                    //Agregamos la Fecha
                    string datetimee = DateTime.Now.ToString("yyyyMMdd HH:mm");
                    SqlCommand Comando = new SqlCommand(string.Format("Insert Into Table_Logs (Descripcion, Usuario, OBJ, FechaLog) values ('{0}','{1}','{2}','{3}')", Mensj, usuario, SerializeOBJ, datetimee), Conn); //format 20221030 12:00
                    Retorno = Comando.ExecuteNonQuery();
                    //Cerramos la conexion con la DB
                    CONNDB.Close();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        //  --- --- --- Tramites --- --- ---  //

        #region Tramites

        #endregion

    }
}