namespace EmailRequestApp.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmailRequestApp.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib;
using Microsoft.Extensions.Options;

public class PersonController : Controller
{
	private readonly ILogger<PersonController> _logger;
	private readonly string? _connectionString;

	public PersonController(ILogger<PersonController> logger, IOptions<MyOptions> options)
	{
		_logger = logger;

		try
		{
			_connectionString = options.Value.ConnectionString;
		}
		catch
		{
			if (options.Value.ConnectionString == null)
			{
				_logger.LogError("Connection string is null");
			}
		}
	}

	public IActionResult Index()
	{	
		using var connection = new SqlConnection(_connectionString);

		var people = connection.Query<Person>("SELECT * FROM EmailTable");
		if (people == null)
		{
			return View(new List<Person>{
				new Person { Name = "No people found", 
							Email = "No people found" 
							}});
		}

		return View(people);
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}

	[HttpPost]
	public void PostPerson(string name, string email)
	{
		using var connection = new SqlConnection(_connectionString);

		var person = new Person { Name = name, Email = email };

		connection.Execute("INSERT INTO EmailTable (Name, Email) VALUES (@Name, @Email)", person);
	}

}