namespace EmailRequestApp.Controllers;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmailRequestApp.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using Dapper.Contrib;

public class PersonController : Controller
{
	private readonly ILogger<PersonController> _logger;
	private readonly string _connectionString;

	public PersonController(ILogger<PersonController> logger)
	{
		_logger = logger;

		var configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json")
			.Build();
		_connectionString = configuration.GetConnectionString("DefaultConnection");
	}

	// private async Task<List<Person>> GetPeople()
	// {


	// 	return Task<people>;
	// }

	//GET: People
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
		// return View();
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
		// var lastId = connection.Query<int>("SELECT MAX(Id) FROM EmailTable").AsList()[0] + 1;
		// var person = new Person { Id = lastId, Name = name, Email = email };
		var person = new Person { Name = name, Email = email };
		//I have issues with the Id here. Dapper does not automatically assign the Id, and the [Key] attribute did not work for some reason
		// connection.Execute("INSERT INTO EmailTable (Id, Name, Email) VALUES (@Id, @Name, @Email)", person);
		connection.Execute("INSERT INTO EmailTable (Name, Email) VALUES (@Name, @Email)", person);
	}

	// [HttpGet]
	// public IActionResult GetPeople()
	// {

	// }
}