using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace SnifferVideo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: GetPreview
        public string GetPreview()
        {
            string url = Request.QueryString["txtUrl"];
            string result = string.Empty;

            Uri uri = new Uri(url);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sReader = new StreamReader(stream, Encoding.UTF8);
            result = sReader.ReadToEnd();

            //(?<="url":").*?(?=")
            //(?<="vid=").*?(?=?)

            result = Regex.Match(result, "(?<=vid=).*?(?=&)").Value;

            //至此已经得到视频vid
            string previewUrl = $"https://v.qq.com/iframe/preview.html?vid={result}&amp;auto=1";
            //接下来调用NetCut和WebBrowser捕获previewUrl的响应内容,得到mp4真实地址,在推送到前台下载
            return result;
        }
    }
}