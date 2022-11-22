﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_De_Gestion.Models
{
    [Table("Table_Clientes")]
    public partial class TableCliente
    {
        public TableCliente()
        {
            TableCorreos = new HashSet<TableCorreo>();
            TableRelacionClienteTramites = new HashSet<TableRelacionClienteTramite>();
            TableTelefonos = new HashSet<TableTelefono>();
        }

        [Key]
        [Column("ID_Cliente")]
        public int IdCliente { get; set; }
        [Column("DNI")]
        public int Dni { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string Nombres { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string Apellidos { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Direccion { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Obs { get; set; }
        public bool Habilitado { get; set; }
        [Required]
        [Column("LINK")]
        [StringLength(50)]
        [Unicode(false)]
        public string Link { get; set; }
        public int Cuil1 { get; set; }
        public int? Cuil2 { get; set; }
        [Column("Pass_Afip")]
        [StringLength(25)]
        [Unicode(false)]
        public string PassAfip { get; set; }
        [Column("Pass_Anses")]
        [StringLength(25)]
        [Unicode(false)]
        public string PassAnses { get; set; }
        [Column("Fecha_Alta", TypeName = "datetime")]
        public DateTime? FechaAlta { get; set; }
        [Column("Fecha_Ult_Modificacion", TypeName = "datetime")]
        public DateTime? FechaUltModificacion { get; set; }

        [InverseProperty("IdClienteNavigation")]
        public virtual ICollection<TableCorreo> TableCorreos { get; set; }
        [InverseProperty("IdClienteNavigation")]
        public virtual ICollection<TableRelacionClienteTramite> TableRelacionClienteTramites { get; set; }
        [InverseProperty("IdClienteNavigation")]
        public virtual ICollection<TableTelefono> TableTelefonos { get; set; }
    }
}