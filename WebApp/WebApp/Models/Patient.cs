using System.Collections.Generic;

namespace WebApp.Models
{
	public class Patient
	{
		public string Name { get; set; }
		public Gender Gender { get; set; }
		public List<MedicalConsultation> Consultations { get; set; }
	}
}
