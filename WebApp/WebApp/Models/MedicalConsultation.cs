using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
	public class MedicalConsultation
	{
		public double Weight { get; set; }
		public double Temperature { get; set; }
		public int Heartbeat { get; set; }
		public int BloodOxygenationRate { get; set; }
	}
}
