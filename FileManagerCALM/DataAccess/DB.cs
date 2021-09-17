using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mk.DBConnector;

namespace DataAccess
{
  public class DB : IODB
    {
        bool _connection;
        DBAdapter _dbsql;
        public DB()
        {
            connection();
        }

        public bool connection()
        {

            _dbsql = new DBAdapter(DatabaseType.MySql, Instance.NewInstance, "localhost", 3306, "datastore", "root", "", "MySql.log");
            _dbsql.Adapter.LogFile = true;

            try
            {
                _connection = true;

            }
            catch (Exception)
            {

                _connection = false;
            }
            return _connection;


        }

       
    }
}
