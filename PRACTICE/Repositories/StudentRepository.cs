using PRACTICE.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace PRACTICE.Repositories
{
    public class StudentRepository
    {
        private readonly string _cs =
        "Server=.;Database=student;Trusted_Connection=True;TrustServerCertificate=True;";

        public void AddStudent(Student student)
        {
            using var con = new SqlConnection(_cs);

            string insertQuery = 
                @"INSERT INTO Student
                ( ID, Name, Course )
                 VALUES 
                (@StudentID, @StudentName, @StudentCourse)";

            con.Execute(insertQuery, student);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            using var con = new SqlConnection(_cs);

            string selectAllQuery = @"
                                    SELECT 
                                        ID     AS StudentID,
                                        Name   AS StudentName,
                                        Course AS StudentCourse
                                    FROM Student";

            return con.Query<Student>(selectAllQuery);
        }

        public Student GetStudentByID(int id)
        {
            using var con = new SqlConnection(_cs);

            string selectQuery = @"
                                    SELECT 
                                        ID     AS StudentID,
                                        Name   AS StudentName,
                                        Course AS StudentCourse
                                    FROM Student
                                    WHERE ID = @id;";

            return con.QueryFirstOrDefault<Student>(selectQuery, new { Id = @id });
        }


        public void UpdateStudent(Student std)
        {
            using var con = new SqlConnection(_cs);

            string query = @"
                            UPDATE Student
                            SET 
                                Name = @StudentName,
                                Course = @StudentCourse
                            WHERE ID = @StudentID";

            con.Execute(query, std);
        }

        public void DeleteStudentByID(int id)
        {
            using var con = new SqlConnection(_cs);

            string query = @"DELETE 
                            FROM Student
                            WHERE ID = @id";
            con.Execute(query, new { Id = id });
        }
    }
}
