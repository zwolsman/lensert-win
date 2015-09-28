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
    class LensertClient
    {
        private const string API_URL = "http://lensert.com/api/v2/";

        private readonly HttpClient _httpClient;
        private readonly JavaScriptSerializer _javaScriptSerializer;

        public string Username { get; }
        public string Password { get; }
        
        public LensertClient(string username, string password)
        {
            Username = username;
            Password = password;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(API_URL)
            };

            _javaScriptSerializer = new JavaScriptSerializer();
        }

        public async Task<bool> Login()
        {
            var multipartDataContent = new MultipartFormDataContent
            {
                {new StringContent(Username), "username"},
                {new StringContent(Password), "password"}
            };

            var responseMessage = await _httpClient.PostAsync("user/login", multipartDataContent);
            if (responseMessage.IsSuccessStatusCode)
            {
                var token = responseMessage.Headers.GetValues("X-Lensert-Token").FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Logged in..!");
                    _httpClient.DefaultRequestHeaders.Add("X-Lensert-Token", token);

                    //LoggedIn?.Invoke(this, EventArgs.Empty);
                    return true;
                }
            }

            multipartDataContent.Dispose();

            Console.WriteLine("Not a valid login!");
            return false;
        }

        public async Task<string> UploadImageAsync(Image bitmap)
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

            var responseMessage = await _httpClient.PostAsync("shot/upload", multipartDataContent);
            if (!responseMessage.IsSuccessStatusCode)
                return false;

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
