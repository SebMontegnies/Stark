using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.ViewModels
{
	public class GeneralInformationViewModel
	{
		public Gender Gender { get; set; }
		public int Age { get; set; }
		public bool HasChildren { get; set; }
		public int Heartbeat { get; set; }
		public decimal Temperature { get; set; }
		public decimal BloodoOxygenationRate { get; set; }
	}
}
