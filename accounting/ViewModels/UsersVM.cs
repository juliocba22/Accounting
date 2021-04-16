using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    public class UsersIndexVM
    {

        #region --[INDEX]--
        //campos para listado
        public IEnumerable<ListUsers> list { get; set; }

        //campos busqueda y paginacion si los tuviese
        [Display(Name = "nombre")]
        public string name { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }

        #endregion --[INDEX]--

    }

    public class UsersCreateVM
    {
        #region --[CREATE]--
        public long id { get; set; }

        [Display(Name = "nombre")]
        [Required(ErrorMessage = "* required")]
        public string name { get; set; }

        [Display(Name = "rol")]
        [Required(ErrorMessage = "* required")]
        public byte rol_id { get; set; }

        [Display(Name = "rol")]
        [Required(ErrorMessage = "* required")]
        public byte rol_name { get; set; }

        [Display(Name = "nombre usuario")]
        [Required(ErrorMessage = "* required")]
        public string user_name { get; set; }

        [Display(Name = "contraseña")]
        [Required(ErrorMessage = "* required")]
        public string password { get; set; }

        [Display(Name = "activo")]
        [Required(ErrorMessage = "* required")]
        public bool active { get; set; }

        public DateTime register_date { get; set; }
        public byte state_id { get; set; }
        public int client_id { get; set; }
        //guardamos fecha de actualizacion o delete, no de alta
        public DateTime update_date { get; set; }
        public long update_user_id { get; set; }

        #endregion --[CREATE]--
    }

}