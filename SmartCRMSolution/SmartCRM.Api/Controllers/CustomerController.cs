using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Mvc;
using SmartCRM.Api.Infrastructures.Controller;
using SmartCRM.Api.ViewModels.Contacts;
using SmartCRM.Core.Filters;
using SmartCRM.Service.Contacts;
using System.Threading.Tasks;

namespace SmartCRM.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : CoreController
    {
        #region Private Methods

        private readonly IMapper _mapper;
        private readonly IContactService _contactService;
        private readonly IConfiguration _configuration;

        #endregion

        #region ctor

        public CustomerController(IMapper mapper,
            IConfiguration configuration,
            IContactService contactService
            )
        {
            _mapper = mapper;
            _contactService = contactService;
            _configuration = configuration;
        }

        #endregion

        #region ctor

        //POST api/Customer/GetCustomerByFilter 
        [Route("GetCustomerByFilter")]
        [HttpPost]
        public ActionResult GetCustomerByFilter(ContactSearchFilter filter)
        {
            var currentUser = CurrentLoginUser;

            var customers = _contactService.GetListByFilter(filter);

            var model = new ContactListViewModel
            {
                Contacts = customers,
                SearchFilter = filter
            };

            return Ok(new
            {
                model = model
            });
        }

        #endregion

        #region GetById

        // GET api/customer/customer-details/5
        [HttpGet]
        [Route("details/{customerId:int}/{title:string}")]
        public async Task<IActionResult> GetcustomerDetails(int customerId)
        {
            var contact = _contactService.GetDetailsById(customerId);

            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        #endregion       
    }
}
