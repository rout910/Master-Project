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

        public UserRepository(NpgsqlConnection connection)
        {
            _conn = connection;
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

        // Prepare the SQL query with parameter placeholders
        string query = "SELECT * FROM t_muser WHERE c_emailid = @email AND c_password = @password";
        
        // Create a NpgsqlCommand with the SQL query and connection
        var cmd = new NpgsqlCommand(query, _conn);
        
        // Add parameter values to the command
        cmd.Parameters.AddWithValue("@email", user.c_emailid);
        cmd.Parameters.AddWithValue("@password", user.c_password); 
        
        // Execute the query and read the result
        var reader = cmd.ExecuteReader();
        
        // Check if a user with the provided email and password exists
        if (reader.Read())
        {
            // Close connection
            _conn.Close();
            
            return true;
        } 
        else
        {
            // Close connection
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
        
        _conn.Close();
    }
}
    }
}
