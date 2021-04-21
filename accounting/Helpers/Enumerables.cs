using System.Collections.Generic;

namespace accounting.Helpers
{
    public class Enumerables
    {
        public Dictionary<int, string> GetComboTipo()
        {
            return new Dictionary<int, string>
            {
               {0,"Producto"},
                {1,"Servicio"}
            };
        }
    }
}