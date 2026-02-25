using Dapper;
using LMS_API.Interfaces;
using LMS_API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace LMS_API.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IDbConnection dbConnection;

        public PaymentRepository(string? connectionString)
        {
            dbConnection = new SqlConnection(connectionString);
        }

        public IEnumerable<Payment> GetList(string? fiscalYear = null, string? quarter = null)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@FiscalYear", fiscalYear);
            parameters.Add("@Quarter", quarter);

            return dbConnection.Query<Payment>(
                "Payment_GetList",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public string SavePayment(Payment payment)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@PaymentId", payment.PaymentId);
            parameters.Add("@ClaimId", payment.ClaimId);
            parameters.Add("@CountryRegion", payment.CountryRegion);
            parameters.Add("@ClaimStatus", payment.ClaimStatus);
            parameters.Add("@Program", payment.Program);
            parameters.Add("@Activity", payment.Activity);
            parameters.Add("@Amount", payment.Amount);
            parameters.Add("@FiscalYear", payment.FiscalYear);
            parameters.Add("@Quarter", payment.Quarter);
            parameters.Add("@ClaimIDOverride", payment.ClaimIDOverride);
            parameters.Add("@Remarks", payment.Remarks);
            parameters.Add("@AmountOverride", payment.AmountOverride);

            parameters.Add(
                "@Result",
                dbType: DbType.String,
                direction: ParameterDirection.Output,
                size: 500
            );

            dbConnection.Execute(
                "Payment_InsertUpdate",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            return parameters.Get<string>("@Result");
        }

        public string DeletePayment(int paymentId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@PaymentId", paymentId);

            return dbConnection.QuerySingle<string>(
                "Payment_Delete",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );
        }

        public string BulkSavePayments(List<Payment> payments)
        {
            DataTable paymentTable = new DataTable();
            paymentTable.Columns.Add("ClaimId", typeof(string));
            paymentTable.Columns.Add("CountryRegion", typeof(string));
            paymentTable.Columns.Add("ClaimStatus", typeof(string));
            paymentTable.Columns.Add("Program", typeof(string));
            paymentTable.Columns.Add("Activity", typeof(string));
            paymentTable.Columns.Add("Amount", typeof(decimal));
            paymentTable.Columns.Add("FiscalYear", typeof(string));
            paymentTable.Columns.Add("Quarter", typeof(string));
            paymentTable.Columns.Add("ClaimIDOverride", typeof(string));
            paymentTable.Columns.Add("Remarks", typeof(string));
            paymentTable.Columns.Add("AmountOverride", typeof(decimal));

            foreach (var p in payments)
            {
                paymentTable.Rows.Add(
                    p.ClaimId, 
                    p.CountryRegion, 
                    p.ClaimStatus, 
                    p.Program, 
                    p.Activity, 
                    p.Amount, 
                    p.FiscalYear, 
                    p.Quarter,
                    (object)p.ClaimIDOverride ?? DBNull.Value,
                    (object)p.Remarks ?? DBNull.Value,
                    (object)p.AmountOverride ?? DBNull.Value
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Payments", paymentTable.AsTableValuedParameter("PaymentType"));

            var result = dbConnection.QueryFirstOrDefault<dynamic>(
                "Payment_BulkInsert",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            return result?.Result ?? "Error during bulk insert.";
        }

        public string BulkUpdateOverrides(List<Payment> payments)
        {
            DataTable paymentTable = new DataTable();
            paymentTable.Columns.Add("ClaimId", typeof(string));
            paymentTable.Columns.Add("CountryRegion", typeof(string));
            paymentTable.Columns.Add("ClaimStatus", typeof(string));
            paymentTable.Columns.Add("Program", typeof(string));
            paymentTable.Columns.Add("Activity", typeof(string));
            paymentTable.Columns.Add("Amount", typeof(decimal));
            paymentTable.Columns.Add("FiscalYear", typeof(string));
            paymentTable.Columns.Add("Quarter", typeof(string));
            paymentTable.Columns.Add("ClaimIDOverride", typeof(string));
            paymentTable.Columns.Add("Remarks", typeof(string));
            paymentTable.Columns.Add("AmountOverride", typeof(decimal));

            foreach (var p in payments)
            {
                paymentTable.Rows.Add(
                    p.ClaimId,
                    p.CountryRegion,
                    p.ClaimStatus,
                    p.Program,
                    p.Activity,
                    p.Amount,
                    p.FiscalYear,
                    p.Quarter,
                    (object)p.ClaimIDOverride ?? DBNull.Value,
                    (object)p.Remarks ?? DBNull.Value,
                    (object)p.AmountOverride ?? DBNull.Value
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Payments", paymentTable.AsTableValuedParameter("PaymentType"));

            var result = dbConnection.QueryFirstOrDefault<dynamic>(
                "Payment_BulkUpdateOverrides",
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 600
            );

            return result?.Result ?? "Error during bulk update.";
        }
    }
}
