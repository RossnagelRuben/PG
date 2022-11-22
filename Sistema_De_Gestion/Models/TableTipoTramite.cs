﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_De_Gestion.Models
{
    [Table("Table_Tipo_Tramite")]
    public partial class TableTipoTramite
    {
        public TableTipoTramite()
        {
            TableSubTramites = new HashSet<TableSubTramite>();
        }

        [Key]
        [Column("IDTramite")]
        public int Idtramite { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string Descripcion { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Organismo { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string TituloTramite { get; set; }
        public bool Habilitado { get; set; }
        [Column("Fecha_Alta", TypeName = "datetime")]
        public DateTime FechaAlta { get; set; }
        [Column("Fecha_Ult_Modificacion", TypeName = "datetime")]
        public DateTime FechaUltModificacion { get; set; }

        [InverseProperty("IdtramiteNavigation")]
        public virtual ICollection<TableSubTramite> TableSubTramites { get; set; }
    }
}