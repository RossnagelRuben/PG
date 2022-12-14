// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_De_Gestion.Models
{
    [Table("Table_Telefonos")]
    public partial class TableTelefono
    {
        public TableTelefono()
        {
            TableUsuarios = new HashSet<TableUsuario>();
        }

        [Key]
        [Column("ID_Telefono")]
        public int IdTelefono { get; set; }
        [Column("ID_Cliente")]
        public int IdCliente { get; set; }
        public int CodigoDeArea { get; set; }
        public int Telefono { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Obs { get; set; }
        public bool Habilitado { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaAlta { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaUltModificacion { get; set; }

        [ForeignKey("IdCliente")]
        [InverseProperty("TableTelefonos")]
        public virtual TableCliente IdClienteNavigation { get; set; }
        [InverseProperty("IdTelefonoNavigation")]
        public virtual ICollection<TableUsuario> TableUsuarios { get; set; }
    }
}