using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace atm.services.models
{
    public class Driver
    {
        public Driver()
        {
        }

        public Driver(DataRowView row)
        {
            DriverId = (int)row["DriverId"];
            DriverName = row["DriverName"].ToString();
            SygmaCenterNo = (int)row["SygmaCenterNo"];
        }

        [Column("DriverId")] public int DriverId { get; set; }
        [Column("DriverName")] public string DriverName { get; set; }
        [Column("SygmaCenterNo")] public int SygmaCenterNo { get; set; }
    }

    public class DriverHelper : Driver
    {
        public DriverHelper()
        {
        }

        public DriverHelper(DataRowView row) : base(row)
        {
        }
    }
}