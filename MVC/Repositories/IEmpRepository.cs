using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;

namespace MVC.Repositories
{
    public interface IEmpRepository
    {
        List<tblemp> GetAll();
        List<tbldept> GetDept();

        void Insert (tblemp stud);


        void Update(tblemp stud);


        void  Delete (int id);

        tblemp GetOne(int id);
    }
}