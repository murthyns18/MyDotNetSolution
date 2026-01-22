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
                commandType: CommandType.StoredProcedure
            );
        }

        public IEnumerable<LoanDetail> GetLoanDetails(int loanId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@LoanId", loanId);

            return dbConnection.Query<LoanDetail>(
                "LoanDetail_GetByLoanId",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public string SaveLoan(LoanHeader loan)
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
                    commandType: CommandType.StoredProcedure
                );

                int loanId = headerParams.Get<int>("@LoanId");

                // 2️⃣ Insert Loan Details
                foreach (var bookId in loan.BookIds)
                {
                    var detailParams = new DynamicParameters();
                    detailParams.Add("@LoanId", loanId);
                    detailParams.Add("@BookId", bookId);

                    dbConnection.Execute(
                        "LoanDetail_Insert",
                        detailParams,
                        commandType: CommandType.StoredProcedure
                    );
                }

                return headerParams.Get<string>("@Result");
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

            return dbConnection.QuerySingle<string>(
                "Loan_Return",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
