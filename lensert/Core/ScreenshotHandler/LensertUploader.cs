﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Lensert.Core.ScreenshotHandler
{
    internal sealed class LensertUploader : BaseImageUploader
    {
        private const string API_URL = "https://lensert.com/upload";

        private static readonly HttpClient _httpClient;
        private static readonly JavaScriptSerializer _javaScriptSerializer;

        static LensertUploader()
        {
            _httpClient = new HttpClient();
            _javaScriptSerializer = new JavaScriptSerializer();
        }

        protected override async Task<string> UploadAsync(Image screenshot)
        {
            using (var memoryStream = new MemoryStream())
            {
                screenshot.Save(memoryStream, ImageFormat.Png);
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
}
