using System;
using System.Collections.Generic;
using log4net;
using SqlToGraphiteInterfaces;

namespace SqlToGraphitePlugin_HttpPing
{
    public class HttpPing : PluginBase
    {
        [Help("metirc namespace path")]
        public string Path { get; set; }

        [Help("Address of the proxy server (leave blank to bypass)")]
        public string Proxy { get; set; }

        [Help("Uri of the destination to http get")]
        public string Uri { get; set; }

        public override string Name { get; set; }
        
        public override string ClientName { get; set; }

        public override string Type { get; set; }

        public HttpPing()
        {
        }

        public HttpPing(ILog log, Job job, IEncryption encryption)
            : base(log, job, encryption)
        {
        }

        private string HttpGet(string uri)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(uri);
            if (!string.IsNullOrEmpty(Proxy))
            {
                req.Proxy = new System.Net.WebProxy(Proxy, false); //true means no proxy    
            }
            System.Net.WebResponse resp = req.GetResponse();
            var sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }


        public override IList<IResult> Get()
        {
            var rtn = new List<IResult>();
            try
            {
                Log.Debug(string.Format("HTTP-Get {0} ", Uri));                
                var start = DateTime.Now;
                var data = this.HttpGet(Uri);
                var timetaken = DateTime.Now.Subtract(start);
                var value = Convert.ToInt32(timetaken.TotalMilliseconds);
                rtn.Add(new Result(value, Name, DateTime.Now, Path));
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                rtn.Add(new Result(-1, Name, DateTime.Now, Path));
            }

            return rtn;
        }
    }
}
