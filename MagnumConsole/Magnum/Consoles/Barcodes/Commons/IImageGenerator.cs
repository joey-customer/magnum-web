using System;
using System.IO;
using Magnum.Api.Models;

namespace Magnum.Consoles.Barcodes.Commons
{
	public interface IImageGenerator
	{
        void Setup();
        MemoryStream RenderToStream(BaseModel data);
        void RenderToFile(BaseModel data, string fileName);
        void Cleanup();
    }
}
