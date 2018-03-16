using System.Collections.Generic;
using Bogus;
using WebApp.Models;

namespace WebApp.DataGenerator
{
	public class PatientGenerator
	{
		public static List<Patient> Create()
		{
			return new Faker<Patient>("fr")
				.RuleFor(p => p.Name, f => f.Person.FirstName)
				.RuleFor(p=>p.Gender , f=>f.PickRandom<Gender>())
				.Generate(5);
		}
	}
}
