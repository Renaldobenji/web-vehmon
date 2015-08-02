using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using WebDAL;

namespace WebDAL
{
    public static class MySQLStoredProcedure
    {
        public static List<T> ExecuteStoredProcedure<T>(this vehmonEntities dbContext, string storedProcedureName, params object[] parameters)
        {
            string storedProcedureCommand = "CALL " + storedProcedureName + "(";

            List<object> augmentedParameters = parameters.ToList();

            storedProcedureCommand = AddParametersToCommand(storedProcedureCommand, augmentedParameters);

            storedProcedureCommand += ");";

            return dbContext.Database.SqlQuery<T>(storedProcedureCommand).ToList<T>();
        }        

        private static string AddParametersToCommand(string storedProcedureCommand, List<object> augmentedParameters)
        {
            for (int i = 0; i < augmentedParameters.Count(); i++)
            {
                storedProcedureCommand = AddParameterToCommand(storedProcedureCommand, augmentedParameters, i);
            }
            return storedProcedureCommand;
        }

        private static string AddParameterToCommand(string storedProcedureCommand, List<object> augmentedParameters, int i)
        {
            if (augmentedParameters[i].GetType() == typeof(string))
            {
                storedProcedureCommand += "'";
            }

            storedProcedureCommand += (augmentedParameters[i].ToString());

            if (augmentedParameters[i].GetType() == typeof(string))
            {
                storedProcedureCommand += "'";
            }

            if (i < augmentedParameters.Count - 1)
            {
                storedProcedureCommand += ",";
            }

            return storedProcedureCommand;
        }
    }
}