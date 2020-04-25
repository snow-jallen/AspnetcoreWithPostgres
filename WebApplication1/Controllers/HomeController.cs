using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly PostgresContext context;

        public HomeController(IConfiguration configuration, PostgresContext context)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            try
            {
                using (var conn = new NpgsqlConnection(configuration["ConnectionString"]))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO todo (title, iscomplete) VALUES (@title, @iscomplete)";
                        //cmd.Parameters.AddWithValue("id", 1);
                        cmd.Parameters.AddWithValue("title", $"new item @ {DateTime.Now}");
                        cmd.Parameters.AddWithValue("iscomplete", false);
                        cmd.ExecuteNonQuery();
                    }

                    // Retrieve all rows
                    using (var cmd = new NpgsqlCommand("SELECT * FROM todo", conn))
                    {
                        var adapter = new NpgsqlDataAdapter(cmd);
                        var todoItems = new DataSet();
                        adapter.Fill(todoItems);
                        foreach (DataRow row in todoItems.Tables[0].Rows)
                            ViewData["Message"] += $"id= {row["id"]}; title={row["title"]}; isComplete={row["iscomplete"]}<br/>";
                    }
                    
                }
            }
            catch(Exception ex)
            {
                ViewData["Message"] = ex.Message;
            }
            
            return View();
        }

        public IActionResult Contact()
        {

            try
            {
                var items = context.Todo;
                ViewData["Message"] = $"I found {items.Count()} items, the first of which is {items.First().Title}";
                return View();
            }
            catch (Exception ex)
            {
                ViewData["Message"] = ex.Message;
                return View();
            }
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
    }
}
