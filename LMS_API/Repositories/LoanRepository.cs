using Dapper;
using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LMS_API.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly IDbConnection dbConnection;

        public LoanRepository(string? connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }

        public IEnumerable<LoanHeader> GetLoanList(int loanId = 0)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LoanId", loanId);

            return dbConnection.Query<LoanHeader>(
                "Loan_GetList",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public IEnumerable<LoanDetail> GetLoanDetails(int loanId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LoanId", loanId);

            return dbConnection.Query<LoanDetail>(
                "LoanDetail_GetByLoanId",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public string TakeLoan(LoanHeader loan)
        {
            try
            {
                // 1️⃣ Insert Loan Header
                var headerParams = new DynamicParameters();
                headerParams.Add("@BorrowerId", loan.BorrowerId);
                headerParams.Add("@LoanDate", loan.LoanDate);

                headerParams.Add("@LoanId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                headerParams.Add("@Result", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                dbConnection.Execute(
                    "LoanHeader_Insert",
                    headerParams,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 600
                );

                int loanId = headerParams.Get<int>("@LoanId");
                string result = headerParams.Get<string>("@Result");

                // Stop if header failed
                if (loanId <= 0)
                    return result;

                // 2️⃣ Insert Loan Details
                foreach (var bookId in loan.BookIds)
                {
                    var detailParams = new DynamicParameters();
                    detailParams.Add("@LoanId", loanId);
                    detailParams.Add("@BorrowerId", loan.BorrowerId); // ✅ REQUIRED
                    detailParams.Add("@BookId", bookId);
                    detailParams.Add("@Result", dbType: DbType.String,
                                     direction: ParameterDirection.Output,
                                     size: 500);

                    dbConnection.Execute(
                        "LoanDetail_Insert",
                        detailParams,
                        commandType: CommandType.StoredProcedure,
                        commandTimeout: 600
                    );

                    var detailResult = detailParams.Get<string>("@Result");

                    if (detailResult != "Book issued successfully")
                        return detailResult;
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ReturnLoan(int loanId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LoanId", loanId);
            parameters.Add("@Result", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

            dbConnection.Execute(
                "Loan_Return",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            return parameters.Get<string>("@Result");
        }
    }
}
