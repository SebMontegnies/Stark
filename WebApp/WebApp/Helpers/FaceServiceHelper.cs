using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace WebApp.Helpers
{
	public class FaceServiceHelper
	{
		private readonly IFaceServiceClient faceServiceClient = new FaceServiceClient("08ec9b1e9e494df9890a43b8b08ef564", "https://westcentralus.api.cognitive.microsoft.com/face/v1.0");

		public Face UploadAndDetectFaces(string imageFilePath)
		{
			// The list of Face attributes to return.
			IEnumerable<FaceAttributeType> faceAttributes =
				new FaceAttributeType[] { FaceAttributeType.Gender, FaceAttributeType.Age, FaceAttributeType.Smile, FaceAttributeType.Glasses, FaceAttributeType.FacialHair };

			// Call the Face API.
			try
			{
				using (Stream imageFileStream = new FileStream(imageFilePath, FileMode.Open))
				{
					Face[] faces = faceServiceClient.DetectAsync(imageFileStream, returnFaceId: true, returnFaceLandmarks: false, returnFaceAttributes: faceAttributes).Result;
					return faces.FirstOrDefault();
				}
			}
			// Catch and display Face API errors.
			catch (FaceAPIException f)
			{
				return new Face();
			}
			// Catch and display all other errors.
			catch (Exception e)
			{
				return new Face();
			}
		}
	}
}