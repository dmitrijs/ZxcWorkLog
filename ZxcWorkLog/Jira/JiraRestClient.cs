using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ZxcWorkLog.Jira
{
    public class JiraRestClient
    {
        private string m_BaseUrl;
        private string m_Username;
        private string m_Password;

        public JiraRestClient(string baseUrl, string username, string password)
        {
            m_BaseUrl = baseUrl + (baseUrl.EndsWith("/") ? "" : "/") + "rest/api/latest/";
            m_Username = username;
            m_Password = password;
        }

        public string RunQuery(
            JiraResource resource,
            string argument = null,
            string data = null,
            string method = "GET")
        {
            string url = string.Format("{0}{1}/", m_BaseUrl, resource.ToString());

            if (argument != null)
            {
                url = string.Format("{0}?{1}", url, argument);
            }

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = method;
/*

            if (data != null)
            {
                using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
                {
                    writer.Write(data);
                }
            }
*/

            string base64Credentials = GetEncodedCredentials();
            request.Headers.Add("Authorization", "Basic " + base64Credentials);

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            string result = string.Empty;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        public IEnumerable<JiraIssue> FindIssues(string jql)
        {
            var reponse = RunQuery(JiraResource.search, "jql=" + jql.Replace(" ", "+") + "&maxResults=50");
            dynamic stuff = JsonConvert.DeserializeObject(reponse);

            var issues = new List<JiraIssue>();
            foreach (var issue in stuff["issues"])
            {
                issues.Add(new JiraIssue((string)issue["key"], (string)issue["fields"]["summary"], (string)issue["fields"]["status"]["name"]));
            }
            return issues;
        }

        private string GetEncodedCredentials()
        {
            string mergedCredentials = string.Format("{0}:{1}", m_Username, m_Password);
            byte[] byteCredentials = UTF8Encoding.UTF8.GetBytes(mergedCredentials);
            return Convert.ToBase64String(byteCredentials);
        }
    }

    public enum JiraResource
    {
        project,
        search
    }
}
