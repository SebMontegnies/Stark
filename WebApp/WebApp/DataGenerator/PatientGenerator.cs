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
				.RuleFor(p => p.Gender, f => f.PickRandom<Gender>())
				.RuleFor(p => p.Photo, f => f.Image.People(640, 480, true))
				.RuleFor(p=>p.Age , f=>f.Random.Int(1,99))
				.Generate(5);
		}
	}
}
