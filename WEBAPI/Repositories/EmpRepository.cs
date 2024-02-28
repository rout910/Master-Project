using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using WEBAPI.Models;


namespace WEBAPI.Repositories
{
    public class EmpRepository : IEmpRepository
    {
        private readonly NpgsqlConnection _conn;

        public EmpRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }

        public List<tblemp> GetAll()
        {
            List<tblemp> students = new List<tblemp>();

            try
            {
                _conn.Open();
                using var command = new NpgsqlCommand("SELECT * FROM t_mcrud", _conn);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tblemp student = new tblemp
                    {
                        c_empid = Convert.ToInt32(reader["c_empid"]),
                        c_empname = reader["c_empname"].ToString(),
                        c_gender = reader["c_gender"].ToString(),
                        c_shift = reader["c_shift"].ToString(),
                        c_deptid = Convert.ToInt32(reader["c_depid"]),
                        c_dob = reader.GetFieldValue<DateOnly>("c_dob"),
                        c_empimage = reader["c_empimage"].ToString()
                    };

                    students.Add(student);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _conn.Close();
            }

            return students;


        }
        public tblemp GetOne(int id)
        {
            try
            {
                _conn.Open();
                using var command = new NpgsqlCommand("SELECT * FROM t_mcrud WHERE c_empid = @Id", _conn);
                command.Parameters.AddWithValue("@Id", id);

                using var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new tblemp
                    {
                        c_empid = Convert.ToInt32(reader["c_empid"]),
                        c_empname = reader["c_empname"].ToString(),
                        c_gender = reader["c_gender"].ToString(),
                        c_shift = reader["c_shift"].ToString(),
                        c_deptid = Convert.ToInt32(reader["c_depid"]),
                        c_dob = reader.GetFieldValue<DateOnly>("c_dob"),
                        c_empimage = reader["c_empimage"].ToString()
                    };
                }
                else
                {
                    // Return null or handle case where student with the given ID is not found
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle exception (log, display error message, etc.)
                return null;
            }
            finally
            {
                _conn.Close();
            }
        }


        public void Insert(tblemp stud)
        {
            _conn.Open();
            using var command = new NpgsqlCommand("INSERT INTO t_mcrud(c_empname, c_gender, c_shift, c_depid, c_dob, c_empimage) VALUES (@empname, @gender, @shift, @depid, @dob, @image)", _conn);
            command.CommandType = CommandType.Text;

            // Set parameter values with explicit NpgsqlDbType
            command.Parameters.AddWithValue("@empname", stud.c_empname);
            command.Parameters.AddWithValue("@gender", stud.c_gender);
            command.Parameters.AddWithValue("@shift", stud.c_shift);
            command.Parameters.AddWithValue("@depid", stud.c_deptid);
            command.Parameters.AddWithValue("@dob", stud.c_dob);
            command.Parameters.AddWithValue("@image", stud.c_empimage);

            command.ExecuteNonQuery();
            _conn.Close();

        }

        public void Update(tblemp stud)
        {
            _conn.Open();
            using var command = new NpgsqlCommand("UPDATE t_mcrud SET c_empname = @Name, c_gender = @Gender,c_shift=@shift, c_depid = @DeptId, c_dob = @Dob, c_empimage = @EmpImage WHERE c_empid = @Id", _conn);
            command.CommandType = CommandType.Text;

            // Set parameter values with explicit NpgsqlDbType
            command.Parameters.AddWithValue("@Id", stud.c_empid);
            // command.Parameters.AddWithValue("@DocumentPath", stud.c_empname);
            command.Parameters.AddWithValue("@Name", stud.c_empname);
            command.Parameters.AddWithValue("@Gender", stud.c_gender);
            command.Parameters.AddWithValue("@shift", stud.c_shift);
            command.Parameters.AddWithValue("@DeptId", stud.c_deptid);
            command.Parameters.AddWithValue("@Dob", stud.c_dob);
            command.Parameters.AddWithValue("@EmpImage", stud.c_empimage);

            command.ExecuteNonQuery();
            _conn.Close();
        }
        public void Delete(int id)
        {
            using var command = new NpgsqlCommand("DELETE FROM t_mcrud WHERE c_empid =@Id", _conn);
            command.CommandType = CommandType.Text;
            command.Parameters.AddWithValue("@Id", id);

            _conn.Open();
            command.ExecuteNonQuery();

            _conn.Close();

        }
        public List<tbldept> GetDept()
        {
            List<tbldept> departments = new List<tbldept>();

            try
            {
                _conn.Open();
                using var command = new NpgsqlCommand("SELECT * FROM t_department", _conn);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tbldept department = new tbldept
                    {
                        c_deptid = Convert.ToInt32(reader["c_depid"]),
                        c_dename = reader["c_dename"].ToString()
                    };

                    departments.Add(department);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _conn.Close();
            }

            return departments;
        }
    }
}