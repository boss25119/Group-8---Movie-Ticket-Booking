using FilmTicketReservation.Data;
using FilmTicketReservation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace FilmTicketReservation.Controllers
{
    public class OrdersController : Controller
    {

        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            var orders = from o in _context.OrderDetails
                         select o;

            return View(orders);

        }
        //GET
        public IActionResult Create(int? id)
        {


            ViewBag.Movies = _context.Movies.Select(v => new SelectListItem
            {
                Text = v.Title,
                Value = v.Id.ToString(),

            });

            var Seat = Enum.GetValues(typeof(Seat))
                .Cast<Seat>()
                .Select(v => v.ToString())
                .ToList();
            ViewBag.MovieSeat = new SelectList(Seat);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("MovieId, MovieTitle, MoviePrice,Seat,Quantity, TotalPrice")] OrderDetail orderDetail)
        {
            Movie movie = new Movie();
            movie.Id = orderDetail.MovieId;
            movie.Title = orderDetail.MovieTitle;
            movie.Price = orderDetail.MoviePrice;
            orderDetail.TotalPrice = orderDetail.MoviePrice * orderDetail.Quantity;

            var movieSeat = Enum.GetValues(typeof(Seat))
                .Cast<Seat>()
                .Select(v => v.ToString());

            
            orderDetail.Seat.ToString();
            _context.Add(orderDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Views.Thankyou));
        }

        public IActionResult Thankyou()
        {
            return View();
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrderDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // GET: Orders/Delete/5
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.OrderDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.OrderDetails.FindAsync(id);
            _context.OrderDetails.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.OrderDetails.Any(e => e.Id == id);
        }
    }






}
       


