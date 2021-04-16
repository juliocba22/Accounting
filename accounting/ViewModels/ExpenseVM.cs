using accounting.Helpers;
using accounting.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace accounting.ViewModels
{
    public class ExpenseIndexVM
    {

        #region --[INDEX]--
        public IEnumerable<ListExpense> list { get; set; }

        [Display(Name = "nombre")]
        public string name { get; set; }

        [Display(Name = "tipo gasto")]
        public string expense_type { get; set; }
        public int page { get; set; }
        public PagingInfo pagingInfo { get; set; }

        #endregion --[INDEX]--

    }

    public class ExpenseCreateVM
    {
        #region --[CREATE]--
        public long id { get; set; }

        [Display(Name = "nombre")]
        [Required(ErrorMessage = "* required")]
        public string name { get; set; }

        [Display(Name = "descripcion")]
        [Required(ErrorMessage = "* required")]
        public string description { get; set; }

        [Display(Name = "fecha gasto")]
        [Required(ErrorMessage = "* required")]
        [DataType(DataType.DateTime, ErrorMessage = "Formato inválido.")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime date_expense { get; set; }


        [Display(Name = "tipo gasto")]
        [Required(ErrorMessage = "* required")]
        public byte expense_id { get; set; }

        [Display(Name = "tipo gasto")]
        //[Required(ErrorMessage = "* required")]
        public string expense_name { get; set; }//para details
        public DateTime register_date { get; set; }

        #endregion --[CREATE]--
    }
}