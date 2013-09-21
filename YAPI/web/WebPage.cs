
namespace htmlRetrieval
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class WebPage
    {
        public WebPage()
        {
        }
        public void FromFile(string path)
        {
            FromFile(path, Encoding.UTF8);
        }
        public void FromFile(string path, Encoding enc)
        {
            if (!System.IO.File.Exists(path))
            {
                string pathl = CombineLocalFile(path);
                if (!System.IO.File.Exists(pathl))
                {
                    RequestErrorString = string.Format("File: '{0}' not found.", path);
                    ErrorsInRequest = true;
                    return;
                }
                path = pathl;
            }
            try
            {
                Address = path;
                System.IO.StreamReader sr = new System.IO.StreamReader(path, enc);
                Html = sr.ReadToEnd();
                sr.Close();
            }
            catch (Exception e) { ErrorsInRequest = true; RequestErrorString = string.Format("File: '{0}' can't read: '{1}'", path, e.Message); Html = null; }
        }

        static string CombineLocalFile(string path)
        {
            string p = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            p = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(p), path);
            return p;
        }

        public WebPage(string url)
        {
            Address = url;
        }

        const int readbuff = 4096;

        public bool ErrorsInRequest;
        public string RequestErrorString { get; set; }

        public string Address { get; set; }
        public string RequestEtag { get; set; }
        public string ResponseEtag { get; set; }
        public bool NotModified { get; set; }
        private string HtmlData;

        public string Html
        {
            get
            {
                if (HtmlData == null)
                {
                    bool NModified; string respoetag;
                    HtmlData = SiteRequest(Address, RequestEtag, out ErrorsInRequest, out respoetag, out NModified);
                    ResponseEtag = respoetag;
                    if (NModified)
                    {
                        NotModified = true;
                        HtmlData = "";
                    }
                    if (ErrorsInRequest)
                    {
                        RequestErrorString = HtmlData;
                        HtmlData = null;
                    }
                }
                return HtmlData;
            }
            set
            {
                HtmlData = value;
            }
        }


        public static string SiteRequest(string URL, string CacheTag, out bool error, out string etag, out bool NotModified)
        {
            etag = ""; error = true; NotModified = false;

            List<byte> array = new List<byte>();
            try
            {
                System.Net.HttpWebRequest wr = System.Net.WebRequest.Create(URL) as System.Net.HttpWebRequest;
                wr.Headers.Clear();
                wr.UserAgent = "Mozilla/5.0 (CeRN; U; WindowsMobile 6.5.3; ru; rv:1.9.2.6) Gecko/20100625";
                //wr.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:14.0) Gecko/20100101 Firefox/14.0 YB/6.9";
                wr.Headers.Add("Accept-Language", "ru-ru,ru;q=0.8,en-us;q=0.5,en;q=0.3");
                //wr.Headers.Add("Accept-Charset", "windows-1251,utf-8;q=0.7,*;q=0.7");
                wr.Accept = "Accept: text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                //wr.Connection = "keep-alive";
                if (CacheTag != null)
                {
                    wr.Headers.Add("If-None-Match", CacheTag);
                    wr.Headers.Add("Cache-Control", "max-age=0");
                }
                System.Net.HttpWebResponse resp = null;
                resp = wr.GetResponse() as System.Net.HttpWebResponse;

                //if (resp.StatusCode == HttpStatusCode.NotModified)
                //{
                //    NotModified = true;
                //    return "";
                //}
                //if (resp.StatusCode != HttpStatusCode.OK)
                //{
                //    return @"request to " + URL + ", status code:" + resp.StatusCode + @" ""}";
                //}
                if (resp.Headers["Etag"] != null)
                {
                    etag = resp.Headers["Etag"];
                }
                System.IO.Stream receiveStream = resp.GetResponseStream();

                Encoding encode = Encoding.UTF8;
                //try
                //{
                //    encode = System.Text.Encoding.GetEncoding(resp.CharacterSet);
                //}
                //catch { }

                byte[] read = new byte[readbuff];
                int count = int.MaxValue;
                while (count > 0)
                {
                    count = receiveStream.Read(read, 0, readbuff);
                    if (count == readbuff)
                    {
                        array.AddRange(read);
                    }
                    else if (count != 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            array.Add(read[i]);
                        }
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1);
                    }
                }
                resp.Close();

                error = false;
                byte[] tar = array.ToArray();
                string response = encode.GetString(tar, 0, tar.Length); GC.SuppressFinalize(array);
                return response;
            }
            catch (WebException wex)
            {
                if (wex.Status != WebExceptionStatus.Success && wex.Status != WebExceptionStatus.ProtocolError)
                {
                    return string.Format("msg:'{0}' url: {1} ", wex.Message, URL);
                }
                else
                    if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotModified)
                    {
                        error = false;
                        NotModified = true;
                    }
                return "";
            }
            catch (Exception e)
            {
                return string.Format("msg:'{0}' url: {1} ", e.Message, URL);
            }
        }
    }
}
