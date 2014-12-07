namespace ZxcWorkLog.Jira
{
    public class JiraIssue
    {
        public string Id { get; private set; }
        public string Summary { get; private set; }

        public JiraIssue(string id, string summary)
        {
            Id = id;
            Summary = summary;
        }
    }
}
