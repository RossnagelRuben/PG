﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Sistema_De_Gestion.Models
{
    [Table("Table_Logs")]
    public partial class TableLog
    {
        [Key]
        [Column("ID_Log")]
        public int IdLog { get; set; }
        [Required]
        [StringLength(250)]
        [Unicode(false)]
        public string Descripcion { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Usuario { get; set; }
        [Required]
        [Column("OBJ")]
        [Unicode(false)]
        public string Obj { get; set; }
        [Unicode(false)]
        public string Detaller { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime FechaLog { get; set; }
    }
}