using System;
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

        public Dictionary<int, string> GetUM()
        {
            return new Dictionary<int, string>
            {
               {0,"Valor unitario por sesión"},
                {1,"Valor unitario por hora"},
                {2,"Valor unitario por visita"},
                {3,"Valor unitario por Km"}
            };
        }

        public Dictionary<int, string> GetTF()
        {
            return new Dictionary<int, string>
            {
               {0,"Factura"},
               {1,"Recibo"},
               {2,"Voluntariado"}
            };
        }

        public class enumCC
        {
            public string id {get;set;}

            public string desc { get; set; }

            public IEnumerable<enumCC> GetCCD()
            {
                enumCC e = new enumCC();
                enumCC e1 = new enumCC();
                enumCC e2 = new enumCC();
                List<enumCC> l = new List<enumCC>();
                e.id = "CUIT";
                e.desc = "CUIT";
                l.Add(e);

                e1.id = "CUIL";
                e1.desc = "CUIL";
                l.Add(e1);

                e2.id = "DNI";
                e2.desc = "DNI";
                l.Add(e2);

                return l;
            }
        }
    }
}