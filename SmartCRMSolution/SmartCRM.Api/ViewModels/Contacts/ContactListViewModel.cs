using SamrtCRM.Data.Models;
using SmartCRM.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartCRM.Api.ViewModels.Contacts
{
    public class ContactListViewModel
    {
        public IEnumerable<Contact> Contacts { get; set; }
        public ContactSearchFilter SearchFilter { get; set; }
    }
}
