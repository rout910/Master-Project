using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEBAPI.Models;

namespace WEBAPI.Repositories
{
    public interface IEmpRepository
    {
        List<tblemp> GetAll();

        void Insert (tblemp stud);


        void Update(tblemp stud);


        void  Delete (int id);

        tblemp GetOne(int id);
    }
}