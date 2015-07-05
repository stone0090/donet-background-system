using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using stonefw.Entity.SystemModule;
using stonefw.Utility;

namespace stonefw.Dao.SystemModule
{
    public class SysMenuDao
    {
        private Database _db;
        private Database Db
        {
            get { return _db ?? (_db = DatabaseFactory.CreateDatabase()); }
        }

        public List<SysMenuEntity> GetSysMenuList(int? menuId = null)
        {
            string sql = @"select * from Sys_Menu a
                            left join Sys_PageFuncPoint b on a.PageUrl = b.PageUrl
                            left join Sys_FuncPoint c ON b.FuncPointId = c.FuncPointId
                            left join Sys_MfpRelation d ON c.FuncPointId = d.FuncPointId
                            left join Sys_Module e ON d.ModuleId = e.ModuleId
                            where a.DeleteFlag = 0 ";
            if (menuId != null) sql += " and a.MenuId = @MenuId ";
            sql += " Order by MenuLevel,Seq ";
            DbCommand dm = Db.GetSqlStringCommand(sql);
            if (menuId != null) Db.AddInParameter(dm, "@MenuId", DbType.Int32, menuId);
            return DataTableHepler.DataTableToList<SysMenuEntity>(Db.ExecuteDataTable(dm));
        }

        public DataTable GetMenuData()
        {
            var sql = @"select * from Sys_Menu a
                        left join Sys_PageFuncPoint b on a.PageUrl = b.PageUrl
                        left join Sys_FuncPoint c ON b.FuncPointId = c.FuncPointId
                        left join Sys_MfpRelation d ON c.FuncPoint_Id = d.FuncPointId
                        left join Sys_Module e ON d.ModuleId = e.ModuleId
                        where a.DeleteFlag = 0 Order by Seq";
            return Db.ExecuteDataTable("select * from Sys_Menu where DeleteFlag = 0 Order by Seq");
        }

        public void UpdateSeq(int menuId, int seq)
        {
            const string strScript = "UPDATE Sys_Menu SET Seq = @Seq WHERE MenuId = @MenuId AND Seq <> @Seq";
            DbCommand dm = Db.GetSqlStringCommand(strScript);
            Db.AddInParameter(dm, "@MenuId", DbType.Int32, menuId);
            Db.AddInParameter(dm, "@Seq", DbType.Int32, seq);
            Db.ExecuteNonQuery(dm);
        }

        public void SeqRecal()
        {
            const string strScript =
                "UPDATE a SET Seq = b.rownumber FROM Sys_Menu a INNER JOIN (SELECT MenuId,row_number() OVER ( partition BY FatherNode ORDER BY Seq) as rownumber FROM SysMenu ) b ON a.MenuId = b.MenuId";
            Db.ExecuteNonQuery(strScript);
        }

        public void Recal(int menuId, int fatherNode, int sourceSeq, int targetSeq)
        {
            StringBuilder strScript = new StringBuilder();
            strScript.Append("update Sys_Menu set Seq = @sourceSeq ");
            strScript.Append("where FatherNode = @FatherNode and Seq = @targetSeq ");
            strScript.Append("update Menu set Seq = @targetSeq ");
            strScript.Append("where MenuId = @MenuId ");
            DbCommand dm = Db.GetSqlStringCommand(strScript.ToString());
            Db.AddInParameter(dm, "@MenuId", DbType.Int32, menuId);
            Db.AddInParameter(dm, "@FatherNode", DbType.Int32, fatherNode);
            Db.AddInParameter(dm, "@sourceSeq", DbType.Int32, sourceSeq);
            Db.AddInParameter(dm, "@targetSeq", DbType.Int32, targetSeq);
            Db.ExecuteNonQuery(dm);
        }

    }
}
