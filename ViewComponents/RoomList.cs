using MasterDetailes_HotelManagement_CoreApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MasterDetailes_HotelManagement_CoreApp.ViewComponents
{
    public class RoomList : ViewComponent
    {
        public IViewComponentResult Invoke(List<Reservation> data)
        {

            ViewBag.Count = data.Count;
            ViewBag.Total = data.Sum(i => i.Total_Cost);

            return View(data);
        }

    }
}
