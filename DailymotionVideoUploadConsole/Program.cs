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
        public static string Bearer = "Bearer eGFeQC0jEkx1CkNxJEtLQQgMAC8HSgQvFUVYBXMIUCga";
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
                Console.WriteLine("Klasör --> " + @"C:\Users\Onur\Downloads\dmvideolar\" + " - Dosya Adı -->" + item.Name + " - Yükleme işlemi başladı...");
                //return item.DirectoryName + "\\" + item.Name;

                var vLocalVideoPath = item.DirectoryName + "\\" + item.Name;

                var vVideoName = ValueClipping(item.Name);
                var vVideoUrl = UploadUrl();
                System.Threading.Thread.Sleep(1000);
                var vAcilanUrl = UploadVideo(vVideoUrl, vLocalVideoPath);
                System.Threading.Thread.Sleep(1000);
                var vVideoId = CreateVideo(vAcilanUrl);
                System.Threading.Thread.Sleep(1000);
                PublishVideo(vVideoId, vVideoName, vVideoName,"example tag 1,example tag 2,example tag 3");
                System.Threading.Thread.Sleep(1000);
                FileMove(item.Name);
            }
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
            request.AddHeader("Authorization", Bearer);
            RestResponse response = client.Execute(request);
            ApiObject obj = JsonConvert.DeserializeObject<ApiObject>(response.Content);
            if ((int)response.StatusCode == 200)
            {
                Console.WriteLine("Durum --> " + " - (2) *UploadUrl* işlemi tamamlandı...");
            }
            else
            {
                Root Errorobj = JsonConvert.DeserializeObject<Root>(response.Content);
                Console.WriteLine("Durum --> " + " - (2) *UploadUrl* işleminde Hata --> " + Errorobj.error.code + " - " + Errorobj.error.message);
            }
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
            ApiObject obj = JsonConvert.DeserializeObject<ApiObject>(response.Content);
            if ((int)response.StatusCode == 200)
            {
                Console.WriteLine("Durum --> " + " - (3) *UploadVideo* işlemi tamamlandı...");
            }
            else
            {
                Root Errorobj = JsonConvert.DeserializeObject<Root>(response.Content);
                Console.WriteLine("Durum --> " + " - (3) *UploadVideo* işleminde Hata --> " + Errorobj.error.code + " - " + Errorobj.error.message);
            }
            return obj.url;
        }

        public static string CreateVideo(string sAcilanUrl)
        {
            var client = new RestClient("https://api.dailymotion.com/me/videos");
            client.Options.Timeout = -1;
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", Bearer);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("url", sAcilanUrl);
            RestResponse response = client.Execute(request);
            ApiObject obj = JsonConvert.DeserializeObject<ApiObject>(response.Content);
            if ((int)response.StatusCode == 200)
            {
                Console.WriteLine("Durum --> " + " - (4) *CreateVideo* işlemi tamamlandı...");
            }
            else
            {
                Root Errorobj = JsonConvert.DeserializeObject<Root>(response.Content);
                Console.WriteLine("Durum --> " + " - (4) *CreateVideo* işleminde Hata --> " + Errorobj.error.code + " - " + Errorobj.error.message);
            }
            return obj.id;
            
        }

        public static void PublishVideo(string sVideoId, string sVideoTitle, string sVideoDescription, string sVideoTags)
        {
            var client = new RestClient("https://api.dailymotion.com/video/" + sVideoId);
            client.Options.Timeout = -1;
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddHeader("Authorization", Bearer);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("published", "true");
            request.AddParameter("title", sVideoTitle);
            request.AddParameter("description", sVideoDescription);
            request.AddParameter("tags", sVideoTags);
            request.AddParameter("channel", "tv");
            request.AddParameter("is_created_for_kids", "false");
            RestResponse response = client.Execute(request);
            if ((int)response.StatusCode == 200)
            {
                Console.WriteLine("Durum --> " + " - (5) *PublishVideo* işlemi tamamlandı...");
            }
            else
            {
                Root Errorobj = JsonConvert.DeserializeObject<Root>(response.Content);
                Console.WriteLine("Durum --> " + " - (5) *PublishVideo* işleminde Hata --> " + Errorobj.error.code + " - " + Errorobj.error.message);
            }
        }

        private static void FileMove(string sUploadedVideoPath)
        {
            File.Move(@"C:\Users\Onur\Downloads\dmvideolar\" + sUploadedVideoPath + "", @"C:\Users\Onur\Downloads\yuklenendmvideolar\" + sUploadedVideoPath + "");//Dosya kopyalanmaz komple taşınır.
            Console.WriteLine("Durum --> " + sUploadedVideoPath + " - Video 'Yüklenenler' klasörüne taşındı - Yükleme Tamamlandı.");
        }
    }
}
