using Sistema_De_Gestion.Models;
namespace Sistema_De_Gestion.Data
{
    public class Usuario2
    {
        public Usuario2()
        {
            TableRoles = new HashSet<TableRole>();
        }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Pas { get; set; }
        public string Token { get; set; }
        public int? IdTelefono { get; set; }
        public int? IdCorreo { get; set; }
        public int IdRol { get; set; }
        public virtual TableCorreo IdCorreoNavigation { get; set; }
        public virtual TableRole IdRolNavigation { get; set; }
        public virtual TableTelefono IdTelefonoNavigation { get; set; }
        public virtual ICollection<TableRole> TableRoles { get; set; }
    }
}