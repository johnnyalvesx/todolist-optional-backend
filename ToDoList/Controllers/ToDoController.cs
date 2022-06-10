using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Infrastructure;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoContext context;

        public ToDoController(ToDoContext context)
        {
            this.context = context;
        }
        public async Task<ActionResult> Index()
        {
            IQueryable<TodoList> items = from i in context.ToDoList orderby i.Id select i;

            List<TodoList> todoList = await items.ToListAsync();

            return View(todoList);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TodoList item)
        {
           if(ModelState.IsValid)
            {
                context.Add(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "A tarefa foi adicionada com sucesso!";

                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<ActionResult> Edit(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TodoList item)
        {
            if (ModelState.IsValid)
            {
                context.Update(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "A tarefa foi editada com sucesso!";

                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<ActionResult> Delete(int id)
        {
            TodoList item = await context.ToDoList.FindAsync(id);
            if (item == null)
            {
                TempData["Error"] = "A tarefa não existe.";
            }
            else
            {
                context.ToDoList.Remove(item);
                await context.SaveChangesAsync();

                TempData["Success"] = "A tarefa foi deletada.";
            }

            return RedirectToAction("Index");
        }
    }
}
