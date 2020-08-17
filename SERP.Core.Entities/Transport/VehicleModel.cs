using SERP.Core.Entities.Entity.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Core.Entities.Transport
{
    [Table("Vehicle", Schema = "Transport")]
    public class VehicleModel: Base<int>
    {
        [Required(ErrorMessage ="Vehicle name is required.")]
        [DataType(DataType.Text)]
        [DisplayName("Vehicle Name")]
        [MaxLength(100)]
        public string VehicleName { get; set; }
        public string VehicleType { get; set; }

        [Required(ErrorMessage = "Vehicle number is required.")]
        [DataType(DataType.Text)]
        [DisplayName("Vehicle Number")]
        [MaxLength(100)]
        public string VehicleNumber { get; set; }
        public string VehicleDeviceId { get; set; }
        public int SessionId { get; set; }
    }
}
