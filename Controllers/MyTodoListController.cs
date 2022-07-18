using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyTodoList.Data;
using MyTodoList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTodoList.Controllers
{
    public class MyTodoListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MyTodoListController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index() 
        {
            /*var todoList = await _context.TodoLists.ToListAsync();
            return View(todoList);*/

            IQueryable<TodoList> items = from i in _context.TodoLists orderby i.Id select i;
            List<TodoList> todoList = await items.ToListAsync();
            return View(todoList);


        }

        //Get /todo/create
        public IActionResult Create() => View();

        //POST /todo/create
        [HttpPost]
        public async Task<ActionResult> Create(TodoList item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();

                TempData["save"] = "The item has Been added!";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        //GET /todo/edit/5
        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await _context.TodoLists.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        //POST /todo/edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();

                TempData["edit"] = "The item has been updated!";

                return RedirectToAction("Index");
            }
            return View(item);
        }

        //GET /todo/delete/5
        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await _context.TodoLists.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "The item does not exist!";
            }
            else {
                _context.TodoLists.Remove(item);
                await _context.SaveChangesAsync();

                TempData["delete"] = "The item has been deleted!";
            }
            return RedirectToAction("Index");
        }

    }
}
