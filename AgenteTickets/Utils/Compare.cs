using System;
using System.Reflection;

namespace AgenteTickets.Utils
{
    public static class Compare
    {
        public static bool ObjectChanges<T>(T object1, T object2, params string[] propertyNames) where T : class
        {
            Type type1 = object1.GetType();
            Type type2 = object2.GetType();

            if (propertyNames?.Length > 0)
            {
                foreach (string propertyName in propertyNames)
                {
                    if (type1.GetProperty(propertyName).GetValue(object1) != type2.GetProperty(propertyName).GetValue(object2))
                    {
                        return true;
                    }
                }

                return false;
            }
            else
            {
                PropertyInfo[] properties1 = type1.GetProperties();
                PropertyInfo[] properties2 = type2.GetProperties();

                for (int i = 0; i < properties1.Length; i++)
                {
                    if (properties1[i].GetValue(object1) != properties2[i].GetValue(object2))
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
