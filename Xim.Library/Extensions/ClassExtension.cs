using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Xim.Library.Extensions
{
    public static class ClassExtension
    {
        public static TDes Map<TDes>(object source, PropertyInfo[] sourcePrs = null, PropertyInfo[] desPrs = null)
        {
            if (source == null)
            {
                return default;
            }

            var des = Activator.CreateInstance<TDes>();
            Map(source, des, sourcePrs, desPrs);
            return des;
        }

        public static void Map(object source, object des, PropertyInfo[] sourcePrs = null, PropertyInfo[] desPrs = null)
        {
            if (source == null)
            {
                return;
            }

            var prs = sourcePrs ?? source.GetType().GetProperties();
            var prd = desPrs ?? des.GetType().GetProperties();

            foreach (var ps in prs)
            {
                var pd = prd.FirstOrDefault(n => n.Name == ps.Name);
                if (pd == null
                    || !pd.CanWrite || !ps.CanWrite)
                {
                    continue;
                }

                pd.SetValue(des, ps.GetValue(source));
            }
        }

        public static List<TDes> Map<TDes>(IList source)
        {
            var result = new List<TDes>();
            if (source?.Count == 0)
            {
                return result;
            }

            var prs = source[0].GetType().GetProperties();
            var prd = typeof(TDes).GetProperties();

            foreach (var item in source)
            {
                var des = Map<TDes>(item, prs, prd);
                result.Add(des);
            }
            return result;
        }
    }
}