using SamrtCRM.Data.Models;
using SmartCRM.Core.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCRM.Service.Contacts
{
    public interface IContactService
    {
        IEnumerable<Contact> GetListByFilter(ContactSearchFilter filter);

        Contact GetDetailsById(int id);
        bool Add(Contact contact);
        bool Update(Contact contact);
        bool Remove(Contact contact);
    }
}
