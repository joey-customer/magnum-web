using System;
using System.IO;
using Magnum.Api.Models;

namespace Magnum.Consoles.Barcodes.Commons
{
	public abstract class ImageGeneratorBase : IImageGenerator
	{
		public string TemplateFile { get; set; }
		public string WipDir { get; set; }

        public void Setup()
		{
			//Common stub setup can be added here
			CustomSetup();
		}

        public void Cleanup()
		{
			//Common stub cleanup can be added here
			CustomCleanup();
		}

        public abstract MemoryStream RenderToStream(BaseModel data);
        
		public void RenderToFile(BaseModel data, string fileName)
		{
			MemoryStream ms = RenderToStream(data);
			SaveToFile(ms, fileName);

			ms.Close();
			ms.Dispose();
		}

		protected abstract void CustomSetup();
		protected abstract void CustomCleanup();
		protected abstract void SaveToFile(MemoryStream ms, string fileName);	
    }
}