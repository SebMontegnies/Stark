using System.Collections.Generic;

namespace WebApp.Models
{
	public class Patient
	{
		public string Name { get; set; }
		public Gender Gender { get; set; }
		public string Photo { get; set; }
		public MedicalConsultation Consultation { get; set; }
	}
}
