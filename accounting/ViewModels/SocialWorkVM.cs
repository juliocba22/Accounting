using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    public class SocialWorkIndexVM
    {

        #region --[INDEX]--
        public IEnumerable<ListSocialWork> list { get; set; }

        [Display(Name = "nombre")]
        public string name { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }

        #endregion --[INDEX]--
    }

    public class SocialWorkCreateVM
    {
        #region --[CREATE]--
        public byte id { get; set; }

        [Display(Name = "nombre")]
        [Required(ErrorMessage = "* required")]
        public string name { get; set; }

        [Display(Name = "descripcion")]
        [Required(ErrorMessage = "* required")]
        public string description { get; set; }

        [Display(Name = "telefono")]
        public string phone { get; set; }

        [Display(Name = "mail")]
        public string mail { get; set; }

        public DateTime register_date { get; set; }

        #endregion --[CREATE]--
    }
}