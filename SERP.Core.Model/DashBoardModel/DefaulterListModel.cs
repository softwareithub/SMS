using System;


namespace SERP.Core.Model.DashBoardModel
{
    public class DefaulterListModel
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal DueAmount { get; set; }
        public DateTime DateOfDeposit { get; set; }
    }
}
