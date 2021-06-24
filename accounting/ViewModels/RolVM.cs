using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    public class RolVM
    {
        public byte id { get; set; }

        [Display(Name = "Descripcion")]
        [Required(ErrorMessage = "* requerido")]
        public string descripcion { get; set; }
    }

    public class RolVMIndex
    {
        public IEnumerable<ListRol> list { get; set; }

        public string descripcion { get; set; }

        public int page { get; set; }

        public PagingInfo pagingInfo { get; set; }
    }
}