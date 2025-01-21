using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
   public class SellerDAL
    {
        public void Add(Seller kullanıcı)
        {
            using (EticaretDb2Context context = new EticaretDb2Context())
            {
                context.Sellers.Add(kullanıcı);
                context.SaveChanges();
            }
        }

        public Seller Login(string taxNo, string password)
        {
            using (EticaretDb2Context context = new EticaretDb2Context())
            {
                var seller = context.Sellers.Where(seller => seller.TaxNo == taxNo && seller.Password == password).FirstOrDefault();
                return seller;
            }
        }
    }
}
