using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CLModel;
using CLDB;

namespace CLBL
{
    public class Repository
    {
        DBConnect dbcon = new DBConnect();
        public List<ProductResult> GetAll(string Hal)
        {
            return dbcon.GetAll(Hal);
        }

        public List<string> GetHalls()
        {
            return dbcon.GetHalls();
        }
    }
}
