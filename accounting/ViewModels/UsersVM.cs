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

        [Display(Name = "* Nombre y Apellido")]
        [Required(ErrorMessage = "* required")]
        public string name { get; set; }

        [Display(Name = "* Rol")]
        [Required(ErrorMessage = "* required")]
        public byte rol_id { get; set; }

        [Display(Name = "* Rol")]
        [Required(ErrorMessage = "* required")]
        public byte rol_name { get; set; }

        [Display(Name = "* Nombre Usuario")]
        [Required(ErrorMessage = "* required")]
        public string user_name { get; set; }

        [Display(Name = "* Contraseña")]
        [Required(ErrorMessage = "* required")]
        public string password { get; set; }

        [Display(Name = "* Activo")]
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