using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace atm.services.models
{
    public class CenterLocation : ILocation
    {
        /// <summary>
        /// Row Identifier of the Center record
        /// </summary>
        public int CenterId { get; set; }

        /// <summary>
        /// Description of the Center record
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Description2 of the center
        /// </summary>
        public string Description2 { get; set; }

        /// <summary>
        /// SygmaCenterNo of the center
        /// </summary>
        public int SygmaCenterNo { get; set; }

        /// <summary>
        /// SyscoHouseNo of the center
        /// </summary>
        public int SyscoHouseNo { get; set; }

        /// <summary>
        /// CadecSiteKey of the center
        /// </summary>
        public int? CadecSiteKey { get; set; }

        /// <summary>
        /// RegionID of the center
        /// </summary>
        public int RegionID { get; set; }

        /// <summary>
        /// LocationTypeID of the center
        /// </summary>
        public int LocationTypeID { get; set; }

        /// <summary>
        /// SameCityAsOffice of the center
        /// </summary>
        public bool SameCityAsOffice { get; set; }

        /// <summary>
        /// InProduction of the center
        /// </summary>
        public bool InProduction { get; set; }

        /// <summary>
        /// Active status of the center
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Longitude of the center's address
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Latitude of the center's address
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// EnableStopMove of the center permission
        /// </summary>
        public byte EnableStopMove { get; set; }

    }
}