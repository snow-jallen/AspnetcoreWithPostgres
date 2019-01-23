using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            try
            {
                using (var conn = new NpgsqlConnection("Server=144.17.24.208;Port=5432;Database=postgres;User Id=postgres;"))
                {
                    conn.Open();
                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO todo (id, title, iscomplete) VALUES (@id, @title, @iscomplete)";
                        cmd.Parameters.AddWithValue("id", 1);
                        cmd.Parameters.AddWithValue("title", "new item");
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
            ViewData["Message"] = "Your contact page.";

            return View();
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
