using SamrtCRM.Data.Models;
using SmartCRM.Core.Filters;
using SmartCRM.Service.Encriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static SmartCRM.Core.Utilities.AppConstants;

namespace SmartCRM.Service.Contacts
{
    public class ContactService : IContactService
    {
        private readonly SmartCRMDbContext db;

        public ContactService(SmartCRMDbContext db)
        {
            this.db = db;
        }
        public IEnumerable<Contact> GetListByFilter(ContactSearchFilter filter)
        {
            List<Contact> contactList = new List<Contact>();

            var query = db.Contact
                            .Where(f =>
                            (f.Status != StatusConstants.Deleted) &&
                            (filter.UserRoleId == null || filter.UserRoleId == 0 || f.Users.Any(a => a.RoleId == filter.UserRoleId)) &&
                            (filter.SearchTerm == string.Empty || f.FirstName.Contains(filter.SearchTerm.Trim())
                                                               || f.Email.Contains(filter.SearchTerm.Trim())
                                                               || f.CellPhone.Contains(filter.SearchTerm.Trim())));
            filter.TotalCount = query.Count();

            //sorting 
            Func<Contact, object> OrderByStringField = null;

            switch (filter.SortColumn)
            {
                case "FirstName":
                    OrderByStringField = p => p.FirstName;
                    break;
                case "Email":
                    OrderByStringField = p => p.Email;
                    break;
                case "CellPhone":
                    OrderByStringField = p => p.CellPhone;
                    break;
                default:
                    OrderByStringField = p => p.FirstName;
                    break;
            }
            //end sorting  

            var finalQuery = filter.SortDirection == "ASC" ? query.OrderBy(OrderByStringField) : query.OrderByDescending(OrderByStringField);

            contactList = finalQuery.Skip((filter.PageNumber - 1) * filter.PageSize)
                                        .Take(filter.PageSize)
                                        .AsParallel()
                                        .ToList();
            return contactList;
        }

        public Contact GetDetailsById(int id)
        {
            var filteredListing = new Contact();

            filteredListing = db.Contact.FirstOrDefault(d => d.Id == id);

            return filteredListing;
        }

        public bool Add(Contact contact)
        {
            try
            {
                db.Contact.Add(contact);
                db.SaveChanges();

                var randomPass = EncryptionService.GenerateRandomPassword(8);
                var salt = EncryptionService.CreateRandomSalt();
                var passwordHash = EncryptionService.HashPassword(randomPass, salt);

                var newUser = new User
                {
                    ContactId = contact.Id,
                    PasswordHash = passwordHash,
                    PasswordSalt = salt,
                    RoleId = UserRoleConstants.Customer,
                    Username = contact.Username,
                    Status = StatusConstants.Active,
                    CreatedDateUtc = contact.CreatedDate
                };

                db.User.Add(newUser);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(Contact contact)
        {
            try
            {
                var upateContact = db.Contact.FirstOrDefault(d => d.Id == contact.Id);

                if (upateContact == null)
                    return false;

                upateContact.FirstName = contact.FirstName;
                upateContact.LastName = contact.LastName;
                upateContact.Email = contact.Email;
                upateContact.CellPhone = contact.CellPhone;
                upateContact.Address = contact.Address;

                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Remove(Contact contact)
        {
            try
            {
                var removeContact = db.Contact.FirstOrDefault(d => d.Id == contact.Id);

                if (removeContact == null)
                    return false;
                removeContact.Status = contact.Status;
                db.SaveChanges();

                var removeUser = db.User.FirstOrDefault(d => d.ContactId == contact.Id);

                if (removeUser == null)
                    return false;
                removeUser.Status = contact.Status;

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
