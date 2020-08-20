using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SERP.Core.Entities.Entity.Core.Transaction
{
    [Table("FeeDiscount",Schema = "TransactionSch")]
    public class FeeDiscountModel :Base<int>
    {
        [Required(ErrorMessage ="Please enter the discount name.")]
        [Display(Name ="Discount Name", Prompt ="Enter Display Name")]
        public string  Name { get; set; }

        [Required(ErrorMessage ="Please enter the discount code")]
        [Display(Name="Discount Code", Prompt ="Enter Discount Code")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Please enter description")]
        [Display(Name = "Discount Description", Prompt = "Enter Discount Description")]
        public string Description { get; set; } = string.Empty;
        public string DiscountType { get; set; } = string.Empty;

        [DataType(DataType.Currency)]
        [Display(Name = "Discount Value", Prompt = "Enter discount value")]
        public decimal DiscountValue { get; set; } = default;
        public int DependentOnParticular { get; set; } = default;
        public int SessionId { get; set; }
    }
}
