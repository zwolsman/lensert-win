using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Lensert
{
    public static class LensertClient
    {
        private const string API_URL = "https://lensert.com/upload";

        private static readonly HttpClient _httpClient;
        private static readonly JavaScriptSerializer _javaScriptSerializer;
        
        static LensertClient()
        {
            _httpClient = new HttpClient();
            _javaScriptSerializer = new JavaScriptSerializer();
        }

        public static async Task<string> UploadImageAsync(Image bitmap)
        {
            var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var streamContent = new StreamContent(memoryStream);
            streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");

            var multipartDataContent = new MultipartFormDataContent
            {
                {streamContent, "shot", "screenshot.png"}
            };

            var responseMessage = await _httpClient.PostAsync(API_URL, multipartDataContent);
            if (!responseMessage.IsSuccessStatusCode)
                return null;

            var responseString = await responseMessage.Content.ReadAsStringAsync();
            var json = _javaScriptSerializer.Deserialize<dynamic>(responseString);

            streamContent.Dispose();
            multipartDataContent.Dispose();
            if (json["result"] == ":(")
            {
                Console.WriteLine("Error!");
                Console.WriteLine(json["error"]["code"]);
                Console.WriteLine(json["error"]["message"]);
            }

            return json["link"];
        }
    }
}
