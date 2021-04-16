using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace accounting.Infra
{
    #region --[Users]--

    /// <summary>
    /// Listado utilizado en SubscriberDirectController.Index
    /// </summary>
    public class ListUsers
    {
        public long id { get; set; }
        public string name { get; set; }
        public byte rol_id { get; set; }
        public string rol_description { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public bool active { get; set; }
        public long? update_user_id { get; set; }
        public DateTime? update_date { get; set; }


    }

    #endregion --[Users]--

    #region rol
    public class ListRol
    {
        public int id { get; set; }
        public string description { get; set; }
    }
    #endregion

    #region --[Expense]--

    public class ListExpense
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public byte expense_id { get; set; }//para el combo
        public string expense_description { get; set; }
        public DateTime date_expense { get; set; }
    }

    #endregion --[Expense]--

    #region expense_type
    public class ListExpenseType
    {
        public int id { get; set; }
        public string description { get; set; }
    }
    #endregion

    #region --[SocialWork]--

    public class ListSocialWork
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string phone { get; set; }
        public string mail { get; set; }
    }

    #endregion

}