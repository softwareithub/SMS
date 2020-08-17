using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.FrontOffice
{
    [Table("VisitorBook", Schema = "Master")]
    public class VisitorBook : Base<int>
    {
        [Required(ErrorMessage = "Please Enter Person Name")]
        [Display(Prompt = "Enter Person Name")]

        public string PersonName { get; set; }
        [Required(ErrorMessage = "Please enter number of person")]
        [Display(Prompt = "Enter Number of Person")]
        public int NumberOfPerson { get; set; }
        [Required(ErrorMessage = "Please enter Phone Number")]
        [Display(Prompt = "Enter Person Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PersonPhone { get; set; }
        [Required(ErrorMessage = "Please enter Person Address")]
        [Display(Prompt = "Enter Person Address")]
        public string PersonAddress { get; set; }
        [Required(ErrorMessage = "Please enter To Whome To Meet")]
        [Display(Prompt = "Enter Whoom To Meet")]
        public string ToWhoomToMeet { get; set; }
        public string Purpose { get; set; }
        public string AppointmentNumber { get; set; }
        public DateTime InDateTime { get; set; } = DateTime.Now;
        public DateTime? OutDateTime { get; set; } = default;
        public int SessionId { get; set; }
    }
}
