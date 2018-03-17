using System.Collections.Generic;

namespace WebApp.Models
{
	public class Patient
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Gender Gender { get; set; }
		public string Photo { get; set; }
		public int Age { get; set; }

		public MedicalConsultation Consultation { get; set; }
	}
}
