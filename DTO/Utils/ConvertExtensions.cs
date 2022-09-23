using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DTO.Utils
{
    public static class ConvertExtensions
    {
        public static T CopyToEntity<T>(this object me)
        {
            var entity2 = (T)Activator.CreateInstance(typeof(T));
            var entity2Proprierties = typeof(T).GetProperties();

            foreach (var property in me.GetType().GetProperties())
            {
                var prop = entity2Proprierties.SingleOrDefault(x => x.Name == property.Name);
                if (prop == null) continue;

                try
                {
                    prop.SetValue(entity2, property.GetValue(me));
                }
                catch { }
            }

            return entity2;
        }
        public static void CopyToEntity<T>(this object me, ref T t, bool update = true)
        {
            var entity2Proprierties = typeof(T).GetProperties();

            var props = me.GetType().GetProperties();
            foreach (var property in props.Any(x => update && Attribute.IsDefined(x, typeof(UpdateAttribute))) ? props.Where(x => Attribute.IsDefined(x, typeof(UpdateAttribute))) : props)
            {
                var prop = entity2Proprierties.SingleOrDefault(x => x.Name == property.Name);
                if (prop == null) continue;

                try
                {
                    prop.SetValue(t, property.GetValue(me));
                }
                catch { }
            }
        }

        public static T1 CopyToEntity<T1>(this DbDataReader dbReader, string baseName = null)
        {
            var entity2 = (T1)Activator.CreateInstance(typeof(T1));
            var entity2Proprierties = typeof(T1).GetProperties();

            Regex regex = string.IsNullOrWhiteSpace(baseName) ? null : new Regex(Regex.Escape(baseName));

            for (int i = 0; i < dbReader.FieldCount; i++)
            {
                var prop = entity2Proprierties.SingleOrDefault(x => x.Name == (string.IsNullOrWhiteSpace(baseName) ? dbReader.GetName(i) : regex.Replace(dbReader.GetName(i), "", 1)));
                if (prop == null) continue;

                try
                {
                    prop.SetValue(entity2, dbReader.GetValue(i));
                }
                catch { }
            }

            return entity2;
        }

        public static bool In<T>(this T source, params T[] list)
        {
            return list.Contains(source);
        }

        public static DateTime? GetDateTimeFromDbDataReader(this object reader) => reader == DBNull.Value ? null : (DateTime?)reader;
        public static TimeSpan? GetTimeSpanFromDbDataReader(this object reader) => reader == DBNull.Value ? null : (TimeSpan?)reader;
        public static int? GetIntFromDbDataReader(this object reader) => reader == DBNull.Value ? null : (int?)reader;
        public static double? GetDoubleFromDbDataReader(this object reader) => reader == DBNull.Value ? null : (double?)reader;
        public static string GetStringFromDbDataReader(this object reader) => reader == DBNull.Value ? null : (string)reader;
        public static bool? GetBoolFromDbDataReader(this object reader) => reader == DBNull.Value ? null : (bool?)reader;
    }
}
