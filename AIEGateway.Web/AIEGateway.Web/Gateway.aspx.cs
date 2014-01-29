using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Http;
using System.Net;
using System.IO;
using AIEGateway.Web.DataAccess;

namespace AIEGateway.Web
{
    public partial class Gateway : System.Web.UI.Page
    {
        private Repository _repository = new Repository();


        protected void Page_Load(object sender, EventArgs e)
        {
            Execute();
        }

        private void Execute()
        {
            //hdnTemperature.Value = Request["temperature"];
            //hdnWetDry.Value = Request["wetdry"];

            var temperature = Request["temperature"];
            var wetDry = Request["wetdry"];

            if (string.IsNullOrEmpty(temperature) && string.IsNullOrEmpty(wetDry))
                return;
            
            var message = (wetDry == "wet") ? "ALERT: Moisture detected" : "ALERT: Temperature threshold exceeded";

            var channels = _repository.GetRegisteredDevices();

            foreach (var channel in channels)
            {
                if (string.IsNullOrEmpty(channel))
                    continue;

                var postData = String.Format("{0} 'temperature': '{1}', 'msg': '{2}', 'channel': '{3}' {4}", "{", temperature, message, channel, "}").Replace("'", "\"");
                SubmitPostRequest(postData);
            }
        }



        private string SubmitPostRequest(string postData)
        {
         // variables to store parameter values
            string url = "https://aiemobileservice.azure-mobile.net/tables/equipment_incident";

            // create the POST request
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.Headers.Add("X-ZUMO-APPLICATION", "NYuUVUztAwEXJQZxOFbppximTExpoh26");
            webRequest.ContentType = "application/json; charset=utf-8";
            webRequest.ContentLength = postData.Length;

            // POST the data
            using (StreamWriter requestWriter2 = new StreamWriter(webRequest.GetRequestStream()))
            {
                requestWriter2.Write(postData);
            }

            //  This actually does the request and gets the response back
            HttpWebResponse resp = (HttpWebResponse)webRequest.GetResponse();

            string responseData = string.Empty;

            using (StreamReader responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                // dumps the HTML from the response into a string variable
                responseData = responseReader.ReadToEnd();
            }
            return responseData;
        }

    }
}