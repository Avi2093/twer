using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ticketingtool.Models
{
    public class JiraTicketingToolViewModel
    {
        public string SelectedProject { get; set; }
        public SelectList ProjectSelectList { get; set; }

        public string SelectedReporter { get; set; }
        public SelectList ReporterSelectList { get; set; }

        public string SelectedDescription { get; set; }
        public SelectList DescriptionSelectList { get; set; }
    }
}
