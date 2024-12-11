using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PSS_HVCement.Utils
{
    public static class DataTableExtension
    {
        /// <summary>
        /// DataTable to List<typeparamref name="T">
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dt)
        {
            List<T> result = new List<T>();

            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();

            var cols = dt.Columns.Cast<DataColumn>().ToList();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(row.ToModel<T>(cols, type, propertyInfos));
            }

            return result;
        }
        /// <summary>
        /// DataTable to ObservableCollection<typeparamref name="T">
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(this DataTable dt)
        {
            ObservableCollection<T> result = new ObservableCollection<T>();

            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();

            var cols = dt.Columns.Cast<DataColumn>().ToList();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(row.ToModel<T>(cols, type, propertyInfos));
            }

            return result;
        }

        /// <summary>
        /// DataRow to Model<typeparamref name="T">
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="row"></param>
        /// <param name="cols"></param>
        /// <param name="type"></param>
        /// <param name="propertyInfos"></param>
        /// <returns></returns>
        private static T ToModel<T>(this DataRow row, List<DataColumn> cols, Type type, PropertyInfo[] propertyInfos)
        {
            T instance = (T)Activator.CreateInstance(type);
            foreach (PropertyInfo pi in propertyInfos)
            {
                if (cols.Any(c => c.ColumnName.ToUpper() == pi.Name.ToUpper()))
                {
                    object value = row[pi.Name];
                    if (pi.PropertyType.ToString() != value.GetType().ToString())
                    {
                        if (value is DBNull) { continue; }
                        try
                        {
                            pi.SetValue(instance, Convert.ChangeType(value, Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType), null);
                        }
                        catch (Exception)
                        {

                        }
                    }
                    else
                    {
                        pi.SetValue(instance, value, null);
                    }
                }
            }
            return instance;
        }
    }
}
