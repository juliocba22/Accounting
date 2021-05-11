using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    public class PermisoIndexVM
    {

        #region --[INDEX]--
        public IEnumerable<ListPermisos> list { get; set; }
        [Display(Name = "rol")]
        public byte rol_id { get; set; }
        public string rol { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }

        #endregion --[INDEX]--

    }

    public class PermisoCreateVM
    {
        #region --[CREATE]--

        [Display(Name = "* Rol")]
        [Required(ErrorMessage = "* required")]
        public byte rol_id { get; set; }

        public IEnumerable<ListPaginas> list { get; set; }

        public bool Accion { get; set; }

        [Display(Name = "* Pagina")]
        [Required(ErrorMessage = "* required")]
        public byte pagina_id { get; set; }

        [Display(Name = "Pagina")]
        public string page_name { get; set; }

        [Display(Name = "Rol")]
        public string rol_name { get; set; }

        #endregion --[CREATE]--
    }
}