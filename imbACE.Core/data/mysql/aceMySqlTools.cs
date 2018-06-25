using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace imbACE.Core.data.mysql
{
    /// <summary>
    /// Simple to use tools to import and export MySql server data data TODO: Export to MySQL server
    /// </summary>
    public static class aceMySqlTools
    {
        private static dataBaseTarget _commonTarget = new dataBaseTarget();

        /// <summary>
        /// Default target used by the system
        /// </summary>
        /// <value>
        /// The common target.
        /// </value>
        public static dataBaseTarget commonTarget
        {
            get
            {
                return _commonTarget;
            }
        }

        /// <summary>
        /// Gets complete table from specified target, if target is null it will use <see cref="commonTarget"/>
        /// </summary>
        /// <param name="dataTableName">Name of the data table.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static DataTable getDataTable(String dataTableName, dataBaseTarget target = null)
        {
            DataTable dt = new DataTable(dataTableName);

            String query = "SELECT * FROM " + dataTableName + "";

            using (MySqlConnection con = getConnection(target))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (MySqlDataAdapter sda = new MySqlDataAdapter(cmd))
                    {
                        sda.Fill(dt);
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Create and open a <see cref="MySqlConnection"/>
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="open">if set to <c>true</c> [open].</param>
        /// <returns></returns>
        public static MySqlConnection getConnection(dataBaseTarget target, Boolean open = true)
        {
            String conString = getConnectionString(target);

            MySqlConnection output = new MySqlConnection(conString);
            if (open) output.Open();

            return output;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public static String getConnectionString(dataBaseTarget target)
        {
            MySqlConnectionStringBuilder conBuilder = new MySqlConnectionStringBuilder();
            if (target == null) target = new dataBaseTarget();
            conBuilder.Password = target.databasePassword;
            conBuilder.UserID = target.databaseUsername;
            conBuilder.OldGuids = target.connectionOldGuids;
            conBuilder.Database = target.databaseName;
            conBuilder.CharacterSet = target.connectionCharset;
            conBuilder.Server = target.databaseHost;
            conBuilder.AllowZeroDateTime = true;
            conBuilder.ConvertZeroDateTime = true;

            return conBuilder.ToString();
        }
    }
}