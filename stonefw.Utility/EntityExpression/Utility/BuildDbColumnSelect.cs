using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using stonefw.Utility.EntityExpression.Entity;
using stonefw.Utility.EntityExpression.GenSQL;

namespace stonefw.Utility.EntityExpression.Utilitys
{
    /// <summary>
    /// 创建数据库字段的查询
    /// </summary>
    public static class BuildDbColumnSelect
    {
        /// <summary>
        /// 填充实体类和数据库的映射关系
        /// </summary>
        public static void FillEntityMappings(Type entityType, List<string> dbColumnNames, List<PropertyInfo> entityPropertyInfos)
        {
            DbTableMappingEntity dbTableMappingEntity = DbTableMapping.GetDbTableMappingEntity(entityType);
            List<string> memberNames = dbTableMappingEntity.EntityFieldNames;
            for (int i = 0; i < memberNames.Count; i++)
            {
                dbColumnNames.Add(dbTableMappingEntity.DbColumnNameMapping[memberNames[i]]);
                entityPropertyInfos.Add(dbTableMappingEntity.EntityPropertyInfoMapping[memberNames[i]]);
            }
        }
    }
}
