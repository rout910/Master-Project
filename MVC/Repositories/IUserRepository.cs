using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.Repositories
{
    public interface IUserRepository
    {
        public void Register(tbluser user);
        public bool IsUser(string email);
        public  bool Login(tbluser user); 
    }
}