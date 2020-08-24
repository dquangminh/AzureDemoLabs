using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KeyVaultSecret.Models;
using KeyVaultSecret.Repositories;

namespace KeyVaultSecret.Services
{
    public class ContactService
    {
        private ContactRepository contactRepository;
        
        public ContactService()
        {
            contactRepository = new ContactRepository();
        }
        public List<Contact> GetContacts()
        {
            var contacts = contactRepository.GetContacts();

            return contacts;
        }

        public Contact GetContact(int id)
        {
            var contact = contactRepository.GetContact(id);

            return contact;
        }

        public int AddContact(Contact contact)
        {
            contactRepository.AddContact(contact);

            return contact.Id;
        }

        public bool DeleteContact(int id)
        {
            var success = contactRepository.DeleteContact(id);

            return success;
        }
    }
}