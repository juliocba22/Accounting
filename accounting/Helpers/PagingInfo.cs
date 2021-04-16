using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace accounting.Helpers
{
    public class PagingInfo
    {

        #region Tags

        // Defecto
        public PagingInfo()
        {
            this.ItemVer = 3;
            this.Intervalo = 4;
            this.ItemIni = " Primera ";
            this.ItemSig = " >> ";
            this.ItemInt = " ... ";
            this.ItemAnt = " << ";
            this.ItemFin = " Ultima ";
        }

        // Páginas visibles, +/- la activa.
        public int Intervalo { get; set; }

        // A partir de que Nro de páginas, se comienzan a ver las Tag:
        public int ItemVer { get; set; }

        // Tag Inicio. Ej: Primera, Inicio, << , ...
        public string ItemIni { get; set; }

        // Tag Anterior. Ej: << , <, Anterior, Ant, ...
        public string ItemAnt { get; set; }

        // Tag Siguiente. Ej: >> , >, Siguiente, Sig, ...
        public string ItemSig { get; set; }

        // Tag Fin. Ej: Ultima, Fin, >> , ...
        public string ItemFin { get; set; }

        // Tag Intervalo. Ej: ..., ***
        public string ItemInt { get; set; }

        #endregion

        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }
    }
}