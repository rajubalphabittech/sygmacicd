using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;
using atm.services.models;
using atm.services.models.payroll;

namespace atm.services
{
	public class DriverService : IDriverService
	{
		public DriverService() 
		{
		}

		public async Task<List<Driver>> GetDriversAsync()
		{
            using (var Db = new AtmContext())
            {
                return await Db.Database.SqlQuery<Driver>("exec [up_p_getDrivers]").ToListAsync();
            }
		}

		public async Task<List<DriverHelper>> GetDriverHelpersAsync()
		{
            using (var Db = new AtmContext())
            {
                return await Db.Database.SqlQuery<DriverHelper>("exec [up_p_getDriverHelpers]").ToListAsync();
            }
		}

		public async Task<List<PayrollForm>> GetFormsAsync(FormsQueryRequest request)
		{
			var param = new object[]
			{
								new SqlParameter("@formId", SqlDbType.Int) {Value = SqlInt32.Null, IsNullable = true},
								new SqlParameter("@sygmaCenterNo", SqlDbType.Int) {Value = SqlInt32.Null, IsNullable = true},
								new SqlParameter("@routeNo", SqlDbType.VarChar, 4) {Value = DBNull.Value, IsNullable = true},
								new SqlParameter("@weekending", SqlDbType.DateTime) {Value = request.WeekEnding},
								new SqlParameter("@fromDate", SqlDbType.DateTime) {Value = DBNull.Value, IsNullable = true},
								new SqlParameter("@toDate", SqlDbType.DateTime) {Value = DBNull.Value, IsNullable = true},
								new SqlParameter("@statusId", SqlDbType.Int) {Value = SqlInt32.Null, IsNullable = true},
								new SqlParameter("@formTypeId", SqlDbType.Int) {Value = SqlInt32.Null, IsNullable = true},
								new SqlParameter("@actualsUpdated", SqlDbType.Int) {Value = SqlInt32.Null, IsNullable = true},
								new SqlParameter("@employeeString", SqlDbType.VarChar, 100) {Value = DBNull.Value, IsNullable = true},
								new SqlParameter("@tractorString", SqlDbType.VarChar, 100) {Value = DBNull.Value, IsNullable = true},
								new SqlParameter("@trailerString", SqlDbType.VarChar, 100) {Value = DBNull.Value, IsNullable = true},
								new SqlParameter("@userName", SqlDbType.VarChar, 20) {Value = request.UserName}
			};
            using (var Db = new AtmContext())
            {
                var result = await Db.Database
                    .SqlQuery<PayrollForm>("exec [up_p_getForms] @userName = {0}, @weekending = {1}",
                            request.UserName,
                            request.WeekEnding
                    ).ToListAsync();

                return result;
            }
		}

		public async Task<FormCriteria> GetFormCriteriaAsync(string userName)
		{
            using (var Db = new AtmContext())
            {
                // Create command from the context in order to execute
                // the `GetReferrer` proc
                var command = Db.Database.Connection.CreateCommand();
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandText = $"[dbo].[up_p_getFormCriteria]";
                command.Parameters.Add(new SqlParameter("@userName", userName));

                var result = new FormCriteria();

                try
                {
                    Db.Database.Connection.Open();
                    var reader = await command.ExecuteReaderAsync();

                    // Drop down to the wrapped `ObjectContext` to get access to
                    // the `Translate` method
                    var objectContext = ((IObjectContextAdapter)Db).ObjectContext;
                    result.Status = objectContext.Translate<FormStatus>(reader).ToList();//(reader, 0, MergeOption.AppendOnly);

                    reader.NextResult();
                    result.FormType = objectContext.Translate<FormType>(reader).ToList();//, "Set2", MergeOption.AppendOnly);

                    reader.NextResult();
                    result.Centers = objectContext.Translate<SygmaCenter>(reader).ToList();//, "Set3", MergeOption.AppendOnly);
                }
                finally
                {
                    Db.Database.Connection.Close();
                }

                return result;
            }
		}
	}
}