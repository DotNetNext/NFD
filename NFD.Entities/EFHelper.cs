using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
   public  class EFHelper
    {
        public static void x<TEntity>()
        {

            Type entityType = typeof(TEntity);
            //表名
           string entitySetName = entityType.Name;
            //主键
            foreach (var prop in entityType.GetProperties())
            {
                var attr = prop.GetCustomAttributes(typeof(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), false).FirstOrDefault() as System.Data.Objects.DataClasses.EdmScalarPropertyAttribute;
                if (attr != null && attr.EntityKeyProperty)
                {
                   var keyProperty = prop.Name;
                   var   keyPropertyType = prop.PropertyType.Name;
                    break;
                }
            }
        }
    }
 