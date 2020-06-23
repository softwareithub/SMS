using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SERP.Utilities.CommanHelper
{
    public static class CommanDeleteHelper
    {
        public static T CommanDeleteCode<T>(T entity, int userId)
        {
            Type modelType = entity.GetType();
            PropertyInfo[] pinfos = modelType.GetProperties();
            foreach (var prop in pinfos)
            {
                if (prop.Name == "IsDeleted")
                {
                    prop.SetValue(entity, 1, null);
                }
                else if (prop.Name == "IsActive")
                {
                    prop.SetValue(entity, 0, null);
                }
                else if (prop.Name == "UpdatedBy")
                {
                    prop.SetValue(entity, userId, null);
                }
                else if (prop.Name == "UpdatedDate")
                {
                    prop.SetValue(entity, DateTime.Now.Date, null);
                }
            }
            return entity;
        }

        public static T CommanUpdateCode<T>(T entity, int userId)
        {
            Type modelType = entity.GetType();
            PropertyInfo[] pinfos = modelType.GetProperties();
            foreach (var prop in pinfos)
            {
              
                 if (prop.Name == "UpdatedBy")
                {
                    prop.SetValue(entity, userId, null);
                }
                else if (prop.Name == "UpdatedDate")
                {
                    prop.SetValue(entity, DateTime.Now.Date, null);
                }
            }
            return entity;
        }
    }
}
