using Microsoft.AspNetCore.Mvc;
using REBAR.Models;
using REBAR.Services;
using System.Xml.Linq;

namespace REBAR.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchAccountsController : ControllerBase
    {
        private readonly BranchAccountService _branchAccountService;

        public BranchAccountsController(BranchAccountService branchAccountService)
        {
            _branchAccountService = branchAccountService;
        }

        [HttpGet]
        public ActionResult<List<BranchAccount>> Get() => _branchAccountService.Get();

        [HttpGet("{branchId}", Name = "GetBranchAccount")]
        public ActionResult<BranchAccount> Get(Guid branchId)
        {
            var branchAccount = _branchAccountService.Get(branchId);

            if (branchAccount == null)
            {
                return NotFound();
            }

            return branchAccount;
        }
        [HttpPost("closeAccount/{branchId}")]
        public ActionResult CloseAccount(Guid branchId)
        {
            try
            {
                var report = _branchAccountService.CloseAccountForToday(branchId);
                if (report == null)
                    return BadRequest("banch not exist");
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<BranchAccount> Create(BranchAccount branchAccount)
        {
            branchAccount.BranchId = Guid.NewGuid();
            _branchAccountService.Create(branchAccount);
            return CreatedAtRoute("GetBranchAccount", new { branchId = branchAccount.BranchId.ToString() }, branchAccount);
        }

        [HttpPut("{branchId}")]
        public IActionResult Update(Guid branchId, BranchAccount branchAccountIn)
        {
            var branchAccount = _branchAccountService.Get(branchId);

            if (branchAccount == null)
            {
                return NotFound();
            }

            _branchAccountService.Update(branchId, branchAccountIn);
            return NoContent();
        }

        [HttpDelete("{branchId}")]
        public IActionResult Delete(Guid branchId)
        {
            var branchAccount = _branchAccountService.Get(branchId);

            if (branchAccount == null)
            {
                return NotFound();
            }

            _branchAccountService.Remove(branchAccount.BranchId);
            return NoContent();
        }
    }

}
