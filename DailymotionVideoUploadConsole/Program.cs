using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailymotionVideoUploadConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileInfo();
        }
        
        private static void FileInfo()
        {
            DirectoryInfo DI = new DirectoryInfo(@"C:\Users\Onur\Downloads\dmvideolar\");
            FileInfo[] DirectoryInfoList = DI.GetFiles();//dosyanın bulunduğu klasördeki dosyaların listesi.

            foreach (var item in DI.GetFiles())
            {
                Console.WriteLine("Klasör --> " + @"C:\Users\Onur\Downloads\dmvideolar\" + " - Dosya Adı -->" + item.Name + " - Yükleniyor...");
                //return item.DirectoryName + "\\" + item.Name;

                var vLocalVideoPath = item.DirectoryName + "\\" + item.Name;

                var vVideoName = ValueClipping(item.Name);

                var vVideoUrl = UploadUrl();
                var vAcilanUrl = UploadVideo(vVideoUrl, vLocalVideoPath);
                var vVideoId = CreateVideo(vAcilanUrl);
                PublishVideo(vVideoId, vVideoName, vVideoName, "example tag 1,example tag 2,example tag 3");
                FileMove(item.Name);
            }
        }

        private static void FileMove(string sUploadedVideoPath)
        {
            File.Move(@"C:\Users\Onur\Downloads\dmvideolar\"+ sUploadedVideoPath + "", @"C:\Users\Onur\Downloads\yuklenendmvideolar\" + sUploadedVideoPath + "");//Dosya kopyalanmaz komple taşınır.
            Console.WriteLine("Klasör --> " + "Video yüklenenler klasörüne taşındı." + " - Dosya Adı -->" + sUploadedVideoPath + " - Yükleme Tamamlandı" + "\n");
        }

        private static string ValueClipping(string sValue)
        {
            var vValue = sValue
                .Replace(".mp4", "")
                .Replace(".avi", "")
                .Replace(".m4v", "")
                .Replace(".flv", "")
                .Replace(".wmv", "")
                .Replace(".3gp", "")
                .Replace(".mov", "")
                .Replace(".mpeg", "");
            return vValue;
        }

        public static string UploadUrl()
        {
            var client = new RestClient("https://api.dailymotion.com/file/upload");
            client.Options.Timeout = -1;
            var request = new RestRequest();
            request.AddHeader("Authorization", "Bearer ZmYDYikYV2FcIy86O1N4Si4WMCIgHjsiN1UwJn56AV1V");
            RestResponse response = client.Execute(request);
            Console.WriteLine(" - Durum -->" + " - (2) =Upload Url= işlemi tamamlandı...");

            ApiObject obj = JsonConvert.DeserializeObject<ApiObject>(response.Content);
            return obj.upload_url;
        }

        public static string UploadVideo(string sVideoUrl,string sLocalVideoPath)
        {
            var client = new RestClient(sVideoUrl);
            client.Options.Timeout = -1;
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddFile("file", @""+sLocalVideoPath);
            RestResponse response = client.Execute(request);
            Console.WriteLine(" - Durum -->" + " - (3) =Video Upload= işlemi tamamlandı...");

            ApiObject obj = JsonConvert.DeserializeObject<ApiObject>(response.Content);
            return obj.url;
        }

        public static string CreateVideo(string sAcilanUrl)
        {
            var client = new RestClient("https://api.dailymotion.com/me/videos");
            client.Options.Timeout = -1;
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", "Bearer ZmYDYikYV2FcIy86O1N4Si4WMCIgHjsiN1UwJn56AV1V");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("url", sAcilanUrl);
            RestResponse response = client.Execute(request);
            Console.WriteLine(" - Durum -->" + " - (4) =Create= işlemi tamamlandı...");

            ApiObject obj = JsonConvert.DeserializeObject<ApiObject>(response.Content);
            
            return obj.id;
        }

        public static void PublishVideo(string sVideoId, string sVideoTitle, string sVideoDescription, string sVideoTags)
        {
            var client = new RestClient("https://api.dailymotion.com/video/" + sVideoId);
            client.Options.Timeout = -1;
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", "Bearer ZmYDYikYV2FcIy86O1N4Si4WMCIgHjsiN1UwJn56AV1V");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("published", "true");
            request.AddParameter("title", sVideoTitle);
            request.AddParameter("description", sVideoDescription);
            request.AddParameter("tags", sVideoTags);
            request.AddParameter("channel", "tv");
            request.AddParameter("is_created_for_kids", "false");
            RestResponse response = client.Execute(request);
            Console.WriteLine(" - Durum -->" + " - (5) =Publish= işlemi tamamlandı...");
        }
    }
}
