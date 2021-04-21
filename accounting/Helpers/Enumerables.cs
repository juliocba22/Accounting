using System.Collections.Generic;

namespace accounting.Helpers
{
    public class Enumerables
    {
        public Dictionary<int, string> GetComboTipo { get; } = new Dictionary<int, string>()
        {

             {0,"Producto"},
             {1,"Servicio"},
        };



    }
}