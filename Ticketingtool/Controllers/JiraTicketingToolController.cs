using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Ticketingtool.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ticketingtool.Controllers
{

    public class JiraTicketingToolController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JiraTicketingToolController(ApplicationDbContext context)
        {
            _context = context;
        }

         public IActionResult Index()
        {
            try
            {
                // get all distinct descriptions where issuetype = Initiative from the database
                var initiatives = _context.JiraTaskDetails
                    .Where(j => j.issuetype == "Initiative")
                    .Select(j => j.description)
                    .Distinct()
                    .ToList();

                // create a select list for the description dropdown
                var descriptionSelectList = new SelectList(initiatives);

                // create a view model for the index view
                var viewModel = new JiraTicketingToolViewModel
                {
                    DescriptionSelectList = descriptionSelectList,
                    ProjectSelectList = new SelectList(new List<SelectListItem>())
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                var errorViewModel = new ErrorViewModel { Message = "An error occurred while processing your request. Please try again later." };
                return View("Error", errorViewModel);
            }
        }
        public IActionResult GetProjects(string description)
        {
            try
            {
                // get all distinct jiraProjectName values for the selected description
                var projects = _context.JiraTaskDetails
                    .Where(j => j.description == description)
                    .Select(j => j.jiraProjectName)
                    .Distinct()
                    .ToList();

                // create a select list for the project dropdown
                var projectSelectList = new SelectList(projects);

                // return the select list as JSON
                return Json(projectSelectList);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }
        public IActionResult GetEpics(string description, string project)
        {
            try
            {
                // get all distinct epic descriptions for the selected description and project where issuetype = Epic
                var epics = _context.JiraTaskDetails
                    .Where(j => j.description == description && j.jiraProjectName == project && j.issuetype == "Epic")
                    .Select(j => j.description)
                    .Distinct()
                    .ToList();

                // create a select list for the epic dropdown
                var epicSelectList = new SelectList(epics);

                // return the select list as JSON
                return Json(epicSelectList);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }
    }
}
