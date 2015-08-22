using stonefw.Utility.EntitySql.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stonefw.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var entity = new SysEnumNameEntity();            
            entity.Type = "123";
            entity.Value = "123";
            entity.Name = "555";
            //entity.ExecInsert();
            //entity.ExecUpdate();
            //entity.ExecDeletey();

            
            Console.ReadKey();
        }
    }
}
