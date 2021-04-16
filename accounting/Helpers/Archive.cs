using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace accounting.Helpers
{
    public class Archive
    {

        public static DataTable CsvToDataTable(HttpPostedFileBase file, DataTable dt, out int cant, out bool error, out Exception ex, int carrier_id)
        {
            error = false; cant = 0;
            ex = null;
            try
            {
                using (var csvReader = new System.IO.StreamReader(file.InputStream))
                {
                    string inputLine = "";
                    while ((inputLine = csvReader.ReadLine()) != null)
                    {
                        string[] values = inputLine.Split(new char[] { ',' });
                        var row = dt.NewRow();
                        for (int x = 0; x < values.Length && !string.IsNullOrEmpty(values[x]); x++)
                        {
                            row["msisdn"] = long.Parse(values[0]);
                            if (row["carrier_id"].ToString() == carrier_id.ToString())
                            {
                                // Por el momento solo tiene validaciones para SV Digicel.
                                if (!(values[0].StartsWith("503") && values[0].Length == 11))
                                    continue;
                            }
                            cant++;
                            dt.Rows.Add(row);
                        }
                    }

                    csvReader.Close();
                }
            }
            catch (Exception fex)
            {
                ex = fex;
                error = true;
            }

            if (error)
                return null;
            else
                return dt;
        }

        public static DataTable CsvToDataTable_Contenido(HttpPostedFileBase file, DataTable dt, out bool error, out Exception ex)
        {
            error = false;
            ex = null;
            try
            {
                using (var csvReader = new System.IO.StreamReader(file.InputStream))
                {
                    string inputLine = "";
                    while ((inputLine = csvReader.ReadLine()) != null)
                    {
                        var row = dt.NewRow();
                        if (!string.IsNullOrEmpty(inputLine))
                            row["mensaje"] = inputLine.ToString().Trim();
                    }

                    csvReader.Close();
                }
            }
            catch (Exception fex)
            {
                ex = fex;
                error = true;
            }

            if (error)
                return null;
            else
                return dt;
        }

        public static DataTable CsvToDataTable_BlackList(HttpPostedFileBase file, DataTable dt,int client_id, out int cant, out bool error, out Exception ex)
        {
            error = false; cant = 0;
            ex = null;
            try
            {
                using (var csvReader = new System.IO.StreamReader(file.InputStream))
                {
                    string inputLine = "";
                    while ((inputLine = csvReader.ReadLine()) != null)
                    {
                        var row = dt.NewRow();
                        if (!string.IsNullOrEmpty(inputLine))
                        {
                            cant++;
                            row["msisdn"] = long.Parse(inputLine.ToString().Trim());
                            row["blacklist_id"] = long.Parse(cant.ToString());
                            row["client_id"] = client_id;
                            dt.Rows.Add(row);

                        }
                    }

                    csvReader.Close();
                }
            }
            catch (Exception fex)
            {
                ex = fex;
                error = true;
            }

            if (error)
                return null;
            else
                return dt;
        }


        public static DataTable CsvToDataTableBroadcast(string path , string name , string message, DataTable dt, out int cant, out bool error, out Exception ex, int carrier_id , int broadcast_id )
        {
            error = false; cant = 0;
            ex = null;
            try
            {
                var CSVFile = System.IO.File.ReadAllLines(path + name);

                for (int x = 0; x < CSVFile.Length && !string.IsNullOrEmpty(CSVFile[x]); x++)
                {
                    string[] values = CSVFile[x].Split(new char[] { ';' });
                    var row = dt.NewRow();
                        
                       
                       row["msisdn"] = long.Parse(values[0]);
                    if (values.Length > 0 ) {
                        if (!String.IsNullOrEmpty(values[1].ToString())) { 
                            row["message"] = values[1];
                        }
                        else
                        {
                            row["message"] = message;
                        }
                    }
                    else
                    {
                        row["message"] = message;
                    }

                    row["broadcast_id"] = broadcast_id;
                            if (row["carrier_id"].ToString() == carrier_id.ToString())
                            {
                                // Por el momento solo tiene validaciones para SV Digicel.
                                if (!(values[0].StartsWith("503") && values[0].Length == 11))
                                    continue;
                            }
                            cant++;
                            dt.Rows.Add(row);
                 }

            }
            catch (Exception fex)
            {
                ex = fex;
                error = true;
            }

            if (error)
                return null;
            else
                return dt;
        }

        public static int CsvToQuantity(HttpPostedFileBase file, out int cant, out bool error, out Exception ex, int carrier_id, out int cantError  , out int cantErrorMessage)
        {
            // int cant = 0;
            cant = 0;
            cantError = 0;
            cantErrorMessage = 0;
            error = false; 
            ex = null;
            try
            {
                using (var csvReader = new System.IO.StreamReader(file.InputStream))
                {
                    string inputLine = "";
                    while ((inputLine = csvReader.ReadLine()) != null)
                    {
                        string[] values = inputLine.Split(new char[] { ';' });
                        
                        if ( carrier_id==36)
                        {
                            // Por el momento solo tiene validaciones para SV Digicel.
                            if (!(values[0].StartsWith("503") && values[0].Length == 11)) { 
                                cantError++;
                                continue;
                            }
                        }

                        if (values.Count() > 1)
                        {
                            if (values[1].ToString().Length > 160)
                            {
                                cantErrorMessage++;
                                continue;
                            }
                        }
                        cant++;
                    }

                    csvReader.Close();
                }

               
            }
            catch (Exception fex)
            {
                ex = fex;
                error = true;
            }

            return cant;
        }
    }
}