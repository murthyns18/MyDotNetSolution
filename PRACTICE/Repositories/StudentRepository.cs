using PRACTICE.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace PRACTICE.Repositories
{
    public class StudentRepository
    {
        private readonly string _cs =
        "Server=.;Database=STUDENT;Trusted_Connection=True;TrustServerCertificate=True;";


        public void AddStudent(Student student)
        {
            using var con = new SqlConnection(_cs);

            string insertQuery = 
                @"INSERT INTO student_details
                (
                    student_id, 
                    student_name, 
                    student_course
                )
                 VALUES (@StudentID, @StudentName, @StudentCourse)";

            con.Execute(insertQuery, student);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            using var con = new SqlConnection(_cs);

            string selectAllQuery = @"
                                    SELECT 
                                        student_id     AS StudentID,
                                        student_name   AS StudentName,
                                        student_course AS StudentCourse
                                    FROM student_details";

            return con.Query<Student>(selectAllQuery);
        }

        public Student GetStudentByID(int id)
        {
            using var con = new SqlConnection(_cs);

            string selectQuery = @"
                                    SELECT 
                                        student_id     AS StudentID,
                                        student_name   AS StudentName,
                                        student_course AS StudentCourse
                                    FROM student_details
                                    WHERE student_id = @id;";

            return con.QueryFirstOrDefault<Student>(selectQuery, new { Id = @id });
        }


        public void UpdateStudent(Student std)
        {
            using var con = new SqlConnection(_cs);

            string query = @"
                            UPDATE student_details
                            SET 
                                student_name = @StudentName,
                                student_course = @StudentCourse
                            WHERE student_id = @StudentID";

            con.Execute(query, std);
        }

        internal void DeleteStudentByID(int id)
        {
            using var con = new SqlConnection(_cs);

            string query = @"DELETE 
                            FROM student_details
                            WHERE student_id = @id";
            con.Execute(query, new { Id = id });
        }
    }
}
