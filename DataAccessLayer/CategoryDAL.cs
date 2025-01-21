using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CategoryDAL
    {
        public List<Category> GetCategories()
        {
            using (EticaretDb2Context context = new EticaretDb2Context())
            {
                var list = context.Categories.ToList();
                return list;
            }

        }
    }
}
