using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer
{
    public class UserDAL
    {
        public void Add(User kullanıcı)
        {
            using (EticaretDb2Context context = new EticaretDb2Context())
            {
                context.Users.Add(kullanıcı);
                context.SaveChanges();
            }
        }

        public bool Login(string username, string password)
        {
            using (EticaretDb2Context context = new EticaretDb2Context())
            {
                var selected_user = context.Users.Where(m => m.UserName == username && m.Password == password).FirstOrDefault();
                if (selected_user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public int FindUserIDByMail(string mail)
        {
            using (EticaretDb2Context context = new EticaretDb2Context())
            {
                return context.Users.Where(user => user.Mail == mail).FirstOrDefault().UserId;
            }
        }

        public int FindUserIDByUserName(string username)
        {
            using (EticaretDb2Context context = new EticaretDb2Context())
            {
                return context.Users.Where(user => user.UserName == username).FirstOrDefault().UserId;
            }
        }

        public User MyProfile(int id)
        {
            using (EticaretDb2Context context = new EticaretDb2Context())
            {
                return context.Users.Find(id);
            }
        }

        public bool PasswordChange(int userid, string oldpassword, string newpassword)
        {
            using (EticaretDb2Context context = new EticaretDb2Context())
            {
                var user = context.Users.Find(userid);


                if (user == null)
                {
                    return false;
                }
                else if (user.Password != oldpassword)
                {
                    return false;
                }
                else
                {
                    user.Password = newpassword;
                    context.SaveChanges();
                    return true;
                }



            }
        }

        //public List<Product> FilterProducts(int? category,string? text,int? upperprice,int? lowerprice)
        //{
        //    using (EticaretDb2Context context = new EticaretDb2Context())
        //    {
        //        //if (category == null && text != null && price != null)
        //        //{
        //        //    return context.Products.Where(p => p.Title.Contains(text) && p.Price <= price).ToList();

        //        //}
        //        //else if (category == null && text == null && price != null)
        //        //{
        //        //    return context.Products.Where(p=>p.Price <= price).ToList();
        //        //}
        //        //else if (category == null && text != null && price == null)
        //        //{
        //        //    return context.Products.Where(p => p.Title.Contains(text)).ToList();
        //        //}
        //        //else if (category != null && text == null && price == null)
        //        //{
        //        //    return context.Products.Where(p => p.Categoryid==category).ToList();
        //        //}
        //        //else
        //        //{

        //        //}

        //        var query = context.Products.AsQueryable();

        //        if (category != null)
        //        {
        //            query = query.Where(p => p.Categoryid == category);
        //        }

        //        if (text != null)
        //        {
        //            query = query.Where(p => p.Title.Contains(text));
        //        }

        //        if (price != null)
        //        {
        //            query = query.Where(p => p.Price <= price);
        //        }

        //        return query.ToList();
        //    }
        //}

    }
}




