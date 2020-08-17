using SERP.Core.Entities.Entity.Core;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SERP.Core.Entities.Transport
{
    [Table("Stopage",Schema = "Transport")]
    public class StopageModel:Base<int>
    {
        [Required(ErrorMessage ="Stopage name is required.")]
        [DisplayName("Stopage Name")]
        [MaxLength(200)]
        public string StopageName { get; set; }
        [Required(ErrorMessage ="Location is required.")]
        public string Latituide { get; set; }
        [Required(ErrorMessage = "Location is required.")]
        public string Longitude { get; set; }
        public string PLaceAddress { get; set; }
        public string PlaceId { get; set; }
        public int SessionId { get; set; }
    }
}
