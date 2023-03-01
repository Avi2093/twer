using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Ticketingtool.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Ticketingtool.Controllers
{
    // This is the controller for the Jira ticketing tool.
    public class JiraTicketingToolController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JiraTicketingToolController(ApplicationDbContext context)
        {
            // Set the context for the controller.
            _context = context;
        }

        // This action method retrieves a list of initiatives from the database and populates the dropdowns for the index view.
        public IActionResult Index()
        {
            try
            {
                // Get all distinct descriptions where issuetype = Initiative from the database.
                var initiatives = _context.JiraTaskDetails
                    .Where(j => j.issuetype == "Initiative")
                    .Select(j => j.description)
                    .Distinct()
                    .ToList();

                // Create a select list for the description dropdown.
                var descriptionSelectList = new SelectList(initiatives);

                // Create a view model for the index view.
                var viewModel = new JiraTicketingToolViewModel
                {
                    InitiativeSelectList = descriptionSelectList,
                    ProjectSelectList = new SelectList(new List<SelectListItem>()),
                    EpicSelectList = new SelectList(new List<SelectListItem>()) // Initialize the third dropdown with an empty list.
                };

                // Render the index view with the view model.
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // If an exception occurs, render the error view with a message.
                var errorViewModel = new ErrorViewModel { Message = "An error occurred while processing your request. Please try again later." };
                return View("Error", errorViewModel);
            }
        }

        // This action method retrieves a list of projects for a given initiative description and populates the project dropdown.
        public IActionResult GetProjects(string description)
        {
            try
            {
                // Get all distinct jiraProjectName values for the selected description.
                var projects = _context.JiraTaskDetails
                    .Where(j => j.description == description)
                    .Select(j => new SelectListItem
                    {
                        Value = j.jiraProjectName,
                        Text = j.jiraProjectName
                    })
                    .Distinct()
                    .ToList();

                // Create a select list for the project dropdown.
                var projectSelectList = new SelectList(projects, "Value", "Text");

                // Return the select list as JSON.
                return Json(projectSelectList);
            }
            catch (Exception ex)
            {
                // If an exception occurs, return a bad request with a message.
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }

        //This action method retrieves a list of epics for a given initiative description and project and populates the epic dropdown.
        public IActionResult GetEpics(string description, string project)
        {
            try
            {
                // Get the LOB value for the selected initiative description.
                var lob = _context.JiraTaskDetails
                    .Where(j => j.description == description && j.issuetype == "Initiative")
                    .Select(j => j.LOB)
                    .FirstOrDefault();

                // Get all distinct epic descriptions for the selected LOB and project where issuetype = Epic.
                var epics = _context.JiraTaskDetails
                    .Where(j => j.LOB == lob && j.jiraProjectName == project && j.issuetype == "Epic")
                    .Select(j => new SelectListItem
                    {
                        Value = j.description,
                        Text = j.description
                    })
                    .Distinct()
                    .ToList();

                // Create a select list for the epic dropdown.
                var epicSelectList = new SelectList(epics, "Value", "Text");

                // Return the select list as JSON.
                return Json(epicSelectList);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }
        
        // This action method retrieves a list of tasks for a given epic description and populates the task dropdown.
        public IActionResult GetTasks(string description, string project)
        {
            try
            {
                // Get the parent key for the selected epic.
                var parentKey = _context.JiraTaskDetails
                    .Where(j => j.description == description && j.jiraProjectName == project)
                    .Select(j => j.issueKey)
                    .FirstOrDefault();

                // Get all distinct task descriptions for the selected parent key, project, and issuetype = Story.
                var tasks = _context.JiraTaskDetails
                    .Where(j => j.parentKey == parentKey && j.jiraProjectName == project && j.issuetype == "Task")
                    .Select(j => new SelectListItem
                    {
                        Value = j.description,
                        Text = j.description
                    })
                    .Distinct()
                    .ToList();

                // Create a select list for the task dropdown.
                var taskSelectList = new SelectList(tasks, "Value", "Text");

                // Return the select list as JSON.
                return Json(taskSelectList);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }


    }
}
