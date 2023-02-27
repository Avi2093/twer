namespace Ticketingtool.Models
{
    public class JiraTaskDetail
    {
      
        public string issuetype { get; set; }
        public string description { get; set; }
        public string jiraProjectName { get; set; }
        public string reporter { get; set; }

        public string jiraProjectKey { get; set; }
        public string LOB { get; set; }
        public string issueKey { get; set; }
        // add other properties here
    }

}
