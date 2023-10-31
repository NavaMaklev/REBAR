using Microsoft.AspNetCore.Mvc;
using REBAR.Models;
using REBAR.Services;

namespace REBAR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly MenuService _menuService;

        public MenuController(MenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public ActionResult<Menu> GetMenu()
        {
            var menu = _menuService.CreateMenu();
            if (menu == null)
            {
                return NotFound();
            }
            return Ok(menu);
        }
    }

}
