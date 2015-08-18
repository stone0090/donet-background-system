using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntityExpressions2.Utility.InternalEntityUtilitys;
using stonefw.Utility.EntityExpressions2.Entity;

namespace stonefw.Utility.EntityExpressions2.Utilitys
{
    /// <summary>
    /// 创建数据库字段的查询
    /// </summary>
    public static class BuildDbColumnSelect
    {
        /// <summary>
        /// 填充实体类和数据库的映射关系
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="dbColumnNames"></param>
        /// <param name="entityPropertys"></param>
        public static void FillEntityMappings(Type entityType, List<string> dbColumnNames, List<PropertyInfo> entityPropertys)
        {
            DbTableMappingEntity definitionItme = entityType.GetDefinitionItem();
            string[] memberNames = definitionItme.EntityEffectiveMemberNames;
            for (int i = 0; i < memberNames.Length; i++)
            {
                dbColumnNames.Add(definitionItme.DbColumnNameMappings[memberNames[i]]);
                entityPropertys.Add(definitionItme.EntityEffectivePropertyInfos[memberNames[i]]);
            }
        }
    }
}
