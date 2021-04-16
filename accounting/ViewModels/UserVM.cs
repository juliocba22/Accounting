using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    
        #region --[LOGIN]--

        public class UserLoginVM
        {
            [Required]
            [Display(Name = "Usuario")]
            public string user_name { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string password { get; set; }

            [Display(Name = "¿Recordar cuenta?")]
            public bool rememberme { get; set; }
        }

        public class UserChangePasswordVM
        {
            public int id { get; set; }

            [Required(ErrorMessage = "* Campo Obligatorio")]
            [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña Actual")]
            public string password { get; set; }

            [Required(ErrorMessage = "* Campo Obligatorio")]
            [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Nueva Contraseña")]
            public string new_password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirmar contraseña")]
            [Compare("ContrasenaNueva", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
            public string confirm_password { get; set; }
        }

        #endregion --[LOGIN]--
}