using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Ticketingtool.Models;
using Microsoft.EntityFrameworkCore;
using System;

public class JiraTicketingToolController : Controller
{
    private readonly ApplicationDbContext _context;

    public JiraTicketingToolController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index1()
    {
        try
        {
            // get all distinct descriptions where issuetype = Initiative from the database using stored procedure
            var initiatives = _context.JiraTaskDetails
                .FromSqlRaw("EXECUTE sp_GetInitiativeList")
                .AsEnumerable()
                .Select(j => new SelectListItem { Value = j.description, Text = j.description })
                .ToList();

            // create a select list for the description dropdown
            var descriptionSelectList = new SelectList(initiatives, "Value", "Text");

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
}
