using SERP.Core.Entities.Entity.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SERP.Core.Entities.LibraryManagement
{
	[Table("LibrarySetting", Schema = "LibraryManagement")]
	public class LibrarySetting : Base<int>
	{
		[Required(ErrorMessageResourceType = typeof(ValidaionMessage), ErrorMessageResourceName = "LibrarySettingUserType")]
		public string UserType { get; set; }
		[Required(ErrorMessage ="Issue Days is required")]
		public int IssueDays { get; set; }
		[Required(ErrorMessage ="Re Issue Time is required.")]
		public int ReIssueTime { get; set; }
		[Required(ErrorMessage ="Fine amount is required.")]
		public decimal FineAmount { get; set; }
		[Required(ErrorMessage ="Fine amount fixed is required.")]
		public int IsFineAmountFixed { get; set; }
		public string FineAmountCalulateOn { get; set; }
		public int AvailableNotification { get; set; }
		public int ReturnDateNotification { get; set; }

	}
}
	
