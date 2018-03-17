using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AspNetCore.Http.Extensions;
using WebApp.Models;

namespace WebApp.Handlers
{
    public class DiabetesDetectionHandler
    {

	    static async Task Diabetes()
	    {
		    using (var client = new HttpClient())
		    {
			    var scoreRequest = new
			    {
				    Inputs = new Dictionary<string, StringTable>() {
					    {
						    "input1",
						    new StringTable()
						    {
							    ColumnNames = new string[] {"Number of times pregnant", "Plasma glucose concentration a 2 hours in an oral glucose tolerance test", "Diastolic blood pressure (mm Hg)", "Triceps skin fold thickness (mm)", "2-Hour serum insulin (mu U/ml)", "Body mass index (weight in kg/(height in m)^2)", "Diabetes pedigree function", "Age (years)", "Class variable (0 or 1)"},
							    Values = new string[,] {  { "1", "80", "72", "35", "0", "33.6", "0.300", "20", "0" } }
						    }
					    },
				    }
			    };

			    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "HwgRcg4xmeojLv2XR+HUAqwlycanM2qSqqO8QYULH9J2VzeEGrWtaHq7UET2G/J7w+jD9ccO2PpG6w8g3E1MwA==");
			    HttpResponseMessage response = await client.PostAsJsonAsync("https://ussouthcentral.services.azureml.net/workspaces/23533c5facbe443ca4e19c86e7493546/services/90da378ee107449ab26e57e4334967d6/execute?api-version=2.0&details=true", scoreRequest);

			    if (response.IsSuccessStatusCode)
			    {
				    string result = await response.Content.ReadAsStringAsync();
			    }
			    else
			    {
				    string responseContent = await response.Content.ReadAsStringAsync();
			    }
		    }
	    }
	}
}
