namespace Sistema_De_Gestion.Models
{
    public class Usuario
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; }
    }
}