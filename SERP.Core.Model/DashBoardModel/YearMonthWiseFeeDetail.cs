

namespace SERP.Core.Model.DashBoardModel
{
    public class YearMonthWiseFeeDetail
    {
        public decimal PayableAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal FineAmount { get; set; }
        public decimal DueAmount { get; set; }
        public string MonthName { get; set; }
        public int Year { get; set; }
    }
}
