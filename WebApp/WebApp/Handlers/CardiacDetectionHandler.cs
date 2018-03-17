﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using AspNetCore.Http.Extensions;

namespace WebApp.Handlers
{
	public class CardiacDetectionHandler
	{
		public static string Detect()
		{
			using (var client = new HttpClient())
			{
				var scoreRequest = new
				{
					Inputs = new Dictionary<string, List<Dictionary<string, string>>>() {
						{
							"input1",
							new List<Dictionary<string, string>>(){new Dictionary<string, string>(){
									{
										"class", "1"
									},
									{
										"age", "45"
									},
									{
										"heartbeat", "100"
									},
									{
										"sp02", "70"
									},
									{
										"height", "175"
									},
									{
										"weight", "120"
									},
									{
										"sex", "1"
									},
									{
										"chol", "4"
									},
									{
										"temp", "38"
									},
								}
							}
						},
					},
					GlobalParameters = new Dictionary<string, string>()
					{
					}
				};

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "+/yacIXKwg/gPh+mqVerS/j0WDLK1DcsY/FLVpj3VeuCapvcdMy4EELtzIZfUAK1eA68NvEj7UaSkF4QvJr4oQ==");
				client.BaseAddress = new Uri("https://ussouthcentral.services.azureml.net/workspaces/23533c5facbe443ca4e19c86e7493546/services/89cd6441d6d344509ecbf5e05cdc9433/execute?api-version=2.0&format=swagger");

				HttpResponseMessage response = client.PostAsJsonAsync("", scoreRequest).Result;

				string result;
				if (response.IsSuccessStatusCode)
				{
					result = response.Content.ReadAsStringAsync().Result;
				}
				else
				{
					result = response.Content.ReadAsStringAsync().Result;
				}

				return result;
			}
		}
	}
}