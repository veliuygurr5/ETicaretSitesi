using DataAccessLayer;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class SellerBL
    {
        SellerDAL dataaccess = new SellerDAL();
        public void AddBl(Seller kullanıcı)
        {
            dataaccess.Add(kullanıcı);
        }

        public Seller Login(string taxNo, string password)
        {
            return dataaccess.Login(taxNo, password);
        }


    }



}
