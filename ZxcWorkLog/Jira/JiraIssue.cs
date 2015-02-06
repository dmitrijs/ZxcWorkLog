namespace ZxcWorkLog.Jira
{
    public class JiraIssue
    {
        public string Id { get; private set; }
        public string Summary { get; private set; }
        public string Status { get; private set; }

        public JiraIssue(string id, string summary, string status)
        {
            Id = id;
            Summary = summary;
            Status = status;
        }
    }
}
