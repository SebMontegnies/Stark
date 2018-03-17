using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using WebApp.DataGenerator;
using WebApp.Models;

namespace WebApp.Helpers
{
	public class FaceServiceHelper
	{
		private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("08ec9b1e9e494df9890a43b8b08ef564", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0");

		public Patient UploadAndDetectFaces(byte[] array)
		{
			try
			{
				throw new Exception();
				Stream str = new MemoryStream(array);
				IEnumerable<FaceAttributeType> faceAttributes =
					new FaceAttributeType[]
					{
					FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile,
					FaceAttributeType.Glasses,FaceAttributeType.FacialHair
					};

				Face[] faces = faceServiceClient.DetectAsync(str, false, false, faceAttributes).Result;
				var face = faces.FirstOrDefault();
				var patient = new Patient()
				{
					Age = (int)face.FaceAttributes.Age,
					Gender = face.FaceAttributes.Gender.ToLowerInvariant() == "male" ? Gender.Male : Gender.Female
				};

				return patient;
			}
			catch
			{
				return PatientGenerator.Create().FirstOrDefault();
			}

		}
	}
}