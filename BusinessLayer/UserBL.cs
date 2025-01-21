using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class UserBL
    {
        UserDAL dataaccess = new UserDAL();
    
        public void AddBl(User kullanıcı)
        {
            dataaccess.Add(kullanıcı);
        }

        public bool Login(string username, string password)
        {
            if (username ==null || password == null)
            {
                return false;
            }
            else
            {
                return dataaccess.Login(username, password);
            }
          
        }

        public int FindUserIDByMail(string mail)
        {
            return dataaccess.FindUserIDByMail(mail);
        }

        public int FindUserIDByUserName(string username)
        {
            return dataaccess.FindUserIDByUserName(username);
        }
        public User MyProfile(int id)
        {
           return dataaccess.MyProfile(id);
        }

        public bool UpdatePassword(int userid, string oldpassword, string newpassword, string newpassword2)
        {
            if (newpassword == newpassword2)
            {
                return dataaccess.PasswordChange(userid, oldpassword, newpassword);
            }
            else
            {
                return false;
            }
        }


    }
}
