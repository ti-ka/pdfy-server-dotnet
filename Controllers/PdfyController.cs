using System.IO;
using Microsoft.AspNetCore.Mvc;
using WkWrap.Core;

namespace PdfServer.Controllers
{
    public class PdfyController : Controller
    {
        public static FileInfo HtmlToPdfBinary;

        [HttpPost("pdfy")]
        public IActionResult Index([FromBody] PayloadModel model)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(new
                {
                    Error = "The model is not formatted properly. " +
                            "Please set headers as application/json and provide proper model."
                });
            }
            
            var converter = new HtmlToPdfConverter(HtmlToPdfBinary);
            var pdfBytes = converter.ConvertToPdf(model.html);
            return File(pdfBytes, "application/pdf", "export.pdf");
        }
        
        [HttpGet("pdfy")]
        public IActionResult Index(string html)
        {
            var converter = new HtmlToPdfConverter(HtmlToPdfBinary);
            var pdfBytes = converter.ConvertToPdf(html);
            return File(pdfBytes, "application/pdf", "export.pdf");
        }

        public class PayloadModel
        {
            public string html;
        }
    }
}