using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MVC.Models;
using Npgsql;

namespace MVC.Repositories
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
                using (var command = new NpgsqlCommand("INSERT INTO t_muser(c_username, c_emailid, c_password,c_userrole) VALUES (@username, @email, @password,'Admin')", _conn))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@username", user.c_username);
                    command.Parameters.AddWithValue("@email", user.c_emailid);
                    command.Parameters.AddWithValue("@password", user.c_password);
                    // command.Parameters.AddWithValue("@Role", user.c_userrole)
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

                // string query = "SELECT * FROM t_muser WHERE c_emailid = @email AND c_password = @password";
string query = "SELECT * FROM t_muser WHERE c_emailid = @email AND c_password = @password";

                var cmd = new NpgsqlCommand(query, _conn);

                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@email",
                    NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, // Set NpgsqlDbType
                    Value = user.c_emailid // Set Value
                });
                cmd.Parameters.Add(new NpgsqlParameter
                {
                    ParameterName = "@password",
                    NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Text, // Set NpgsqlDbType
                    Value = user.c_password // Set Value
                });

                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    var role = reader["c_userrole"].ToString();

                    _conn.Close();

                    return true;
                }
                else
                {
                    
                    _conn.Close();

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Login method: {ex.Message}");
                return false;
            }
            finally
            {
                // Close connection if it's open
                if (_conn.State == ConnectionState.Open)
                    _conn.Close();
            }
        }

    }
}