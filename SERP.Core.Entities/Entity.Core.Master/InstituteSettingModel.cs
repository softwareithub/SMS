using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SERP.Core.Entities.Entity.Core.Master
{
    public class InstituteSettingModel:Base<int>
    {
		public int InstituteId { get; set; }
		[Display(Prompt ="Enter Registration number")]
		public string RegistrationNumber { get; set; }
		[Display(Prompt ="Enter Affliated by ")]
		public string AffliatedBy { get; set; }
		[Display(Prompt ="Enter Affliation Number")]
		public string AffliationNumber { get; set; }
		[Display(Prompt ="Enter Date of Eastblishment")]
		public DateTime DateOfEastablishment { get; set; }
		[Display(Prompt ="Enter Affliation Board")]
		public string Board { get; set; }
		[Display(Prompt ="Enter Detail about School/College/Institute")]
		public string AboutUsDetail { get; set; }
		public string PlaceAddress { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public int Advertise { get; set; }
		
	}
}
