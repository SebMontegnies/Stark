using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using WebApp.Models;

namespace WebApp.DataGenerator
{
	public class ConsultationGenerator
	{
		public static List<MedicalConsultation> Create()
		{
			return new Faker<MedicalConsultation>()
				.RuleFor(m => m.Age, f => f.Random.Int(50, 80))
				.RuleFor(m=>m.BloodOxygenationRate, f=>f.Random.Int(50,100))
				.RuleFor(m=>m.Heartbeat,f=>f.Random.Int(70,160))
				.RuleFor(m=>m.Temperature, f=>f.Random.Double(35,39))
				.RuleFor(m=>m.Weight , f=>f.Random.Double(60,120))
				.Generate(5);
		}
	}
}
