using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace DataHandler
{
    public class SerialHandler  
    {
        
        public string getserila(MySqlConnection mySqlCon, string scode)
        {
            
            string mreturn = "";
            string sMySqlString = "";
            MySqlDataReader reader = null;
            MySqlCommand cmd = null;

            try
            {
                sMySqlString = "select strno,mlength,initcode from SERIAL where scode='" + scode + "'";
                cmd = new MySqlCommand(sMySqlString, mySqlCon);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string seriacode = reader.GetString(2) + reader.GetString(0).PadLeft(Convert.ToInt32(reader.GetString(1)), '0');
                    mreturn = seriacode;

                    reader.Close();
                    reader.Dispose();
                    sMySqlString = "update SERIAL set strno = strno + 1 where scode='" + scode + "'";
                    cmd.CommandText = sMySqlString;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    mreturn = "";
                }

                return mreturn;
            }
            catch (Exception)
            {
                mreturn = "";
            }

            finally
            {
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
            }
            return mreturn;
        }

        public string getserilalReference(ref MySqlConnection mySqlCon, string scode)
        {

            string mreturn = "";
            string sMySqlString = "";
            MySqlDataReader reader = null;
            MySqlCommand cmd = null;

            try
            {
                sMySqlString = "select strno,mlength,initcode from SERIAL where scode='" + scode + "'";
                cmd = new MySqlCommand(sMySqlString, mySqlCon);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string seriacode = reader.GetString(2) + reader.GetString(0).PadLeft(Convert.ToInt32(reader.GetString(1)), '0');
                    mreturn = seriacode;

                    reader.Close();
                    reader.Dispose();
                    sMySqlString = "update SERIAL set strno = strno + 1 where scode='" + scode + "'";
                    cmd.CommandText = sMySqlString;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    mreturn = "";
                }

                return mreturn;
            }
            catch (Exception)
            {
                mreturn = "";
            }

            finally
            {
                reader.Close();
                reader.Dispose();
                cmd.Dispose();
            }
            return mreturn;
        }

    }
}
