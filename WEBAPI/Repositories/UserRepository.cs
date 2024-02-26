using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
using Npgsql;
using WEBAPI.Models;

namespace WEBAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
          private readonly NpgsqlConnection _conn;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(NpgsqlConnection connection, IHttpContextAccessor httpContextAccessor)
        {
            _conn = connection;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Register(tbluser user)
        {
            try
            {
                _conn.Open();
                using (var command = new NpgsqlCommand("INSERT INTO t_muser(c_username, c_emailid, c_password,c_userrole) VALUES (@username, @email, @password,'User')", _conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@username", user.c_username);
                    command.Parameters.AddWithValue("@email", user.c_emailid);
                    command.Parameters.AddWithValue("@password", user.c_password);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Register method: {ex.Message}");
            }
            finally
            {
                    _conn.Close();           
            }
        }

         public bool IsUser(string email)
          {
        try
        {
            _conn.Open();
            string query = "select * from t_muser where  c_emailid=@email";
            var command = new NpgsqlCommand(query, _conn);
            command.Parameters.AddWithValue("@email", email);
            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }
             else
             {
                return false;
             }            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            _conn.Close();
        }
        return false;
    }

    public bool Login(tbluser user)
    {
        try
        {
            _conn.Open();  
            string query="SELECT * FROM t_muser WHERE c_emailid = @e And c_password=@p ";
            var cmd = new NpgsqlCommand(query,_conn);
            cmd.Parameters.AddWithValue("@e",user.c_emailid);
            cmd.Parameters.AddWithValue("@p",user.c_password); 
            var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var role = reader["c_userrole"].ToString();
                // var emailid = reader["c_emailid"].ToString();
                // var username=reader["c_username"].ToString();
                var session = _httpContextAccessor.HttpContext.Session;
                session.SetString("role",role);
                // session.SetString("emailid",emailid);
                // session.SetString("username",username);
                return true;
            } 
            else
            {
                return false;
            }

        }
        catch (System.Exception)
        {

            throw;
        }
        finally
        {
            _conn.Close();
        }
    }
    }
}