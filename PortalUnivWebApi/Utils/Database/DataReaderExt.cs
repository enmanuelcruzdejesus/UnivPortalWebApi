using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalUnivWebApi.Utils.Database
{
    public static  class DataReaderExt
    {
        public static IEnumerable<string> GetColumnNames(this IDataReader reader)
        {
            var schemaTable = reader.GetSchemaTable();
            return schemaTable == null
                ? Enumerable.Empty<string>()
                : schemaTable.Rows.OfType<DataRow>().Select(row => row["ColumnName"].ToString());
        }
        public static List<object> ToObjectList(this IDataReader dataReader, bool ignoreUnmappedColumns = true)
        {
            var list = new List<object>();
            while (dataReader.Read())
            {
                IEnumerable<string> columnsName = DataReaderExt.GetColumnNames(dataReader);//A custom method implemented below 
                var obj = new ExpandoObject() as IDictionary<string, object>;

                foreach (var columnName in columnsName)
                {
                    obj.Add(columnName, dataReader[columnName]);
                }
                var expando = (ExpandoObject)obj;

                list.Add(expando);
            }

            return list;
        }

       
    }
}
