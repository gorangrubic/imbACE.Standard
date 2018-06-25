//using MySql.Data;
//using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace imbACE.Core.data.mysql
{
    public enum dataBaseTargetType
    {
        none,
        mySql,
        mariaDb
    }

    /// <summary>
    /// Settings for database server connection
    /// </summary>
    public class dataBaseTarget
    {
        public DataTable GetTable(String tableName)
        {
            switch (type)
            {
                case dataBaseTargetType.mySql:
                    return aceMySqlTools.getDataTable(tableName, this);
                    break;
            }

            return new DataTable(tableName);
        }

        /// <summary>
        /// Default constructor for XML Serialization
        /// </summary>
        public dataBaseTarget()
        {
        }

        public dataBaseTargetType type { get; set; } = dataBaseTargetType.mySql;

        public string databasePassword { get; set; } = "imbveles1234";
        public string databaseUsername { get; set; } = "imbveles";
        public string databaseName { get; set; } = "imbveles";
        public Boolean connectionOldGuids { get; set; } = false;
        public String connectionCharset { get; set; } = "utf8";
        public String databaseHost { get; set; } = "127.0.0.1";

        public Boolean allowZeroDateTime { get; set; } = true;
        public Boolean ConvertZeroDateTime { get; set; } = true;
    }
}