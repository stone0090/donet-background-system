/*
 * First creat by wukea[2013/09/09]
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using stonefw.Utility.EntityExpressions.Utilitys.InternalEntityUtilitys;

namespace stonefw.Utility.EntityExpressions.Utilitys
{
    /// <summary>
    /// 创建数据库字段的查询
    /// </summary>
    public static class BuildDBColumnSelect
    {
        /// <summary>
        /// 填充实体类和数据库的映射关系
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="dbColumnNames"></param>
        /// <param name="entityPropertys"></param>
        public static void FillEntityMappings(Type entityType, List<string> dbColumnNames, List<PropertyInfo> entityPropertys)
        {
            EntityDefinitionBufferItem definitionItme = entityType.GetDefinitionItem();
            string[] memberNames = definitionItme.EntityEffectiveMemberNames;
            for (int i = 0; i < memberNames.Length; i++)
            {
                dbColumnNames.Add(definitionItme.DBColumnNameMappings[memberNames[i]]);
                entityPropertys.Add(definitionItme.EntityEffectivePropertyInfos[memberNames[i]]);
            }
        }
    }
}
