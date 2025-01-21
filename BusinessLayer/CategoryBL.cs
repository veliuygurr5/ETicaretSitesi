using DataAccessLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CategoryBL
    {
        CategoryDAL data_access = new CategoryDAL();

        public List<Category> GetCategories()
        {
            return data_access.GetCategories();
        }
    }
}
