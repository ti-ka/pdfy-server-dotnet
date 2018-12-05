using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using PdfServer.Controllers;
using WkWrap.Core;

namespace PdfServer
{
    public static class PdfConverterSetup
    {
        public static void Setup(IConfiguration configuration)
        {
            // Note: This could also been done on the constructor of the Controller
            // Here, we can do it only once.
            
            // 1. Check configuration
            var path = configuration["PdfConverter"];
            if (string.IsNullOrEmpty(path)){
                throw new ApplicationException("Error: Please define 'PdfConverter' in app settings.");
            }
            
            // 2. Check binary
            PdfyController.HtmlToPdfBinary = new FileInfo(path);
            if (!PdfyController.HtmlToPdfBinary.Exists)
            {
                throw new ApplicationException("Error: The PDF Converter binary does not exist in the path: " + path);
            }
            
            // 3. Check if the converter works
            var converter = new HtmlToPdfConverter(PdfyController.HtmlToPdfBinary);
            var pdfBytes = converter.ConvertToPdf("Hello there, convert me.");
            if (pdfBytes == null)
            {
                throw new ApplicationException("Error: The PDF Converter is not working correctly. Please check.");
            }
            
            // 4. All pass:
            Console.WriteLine("All checks passed. Starting PDFy Server. 👍");
        }
    }
}