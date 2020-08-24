using System.Collections.Generic;
using KeyVaultSecret.Models;

namespace KeyVaultSecret.Repositories
{
    interface IContactRepository
    {
        Contact GetContact(int id);

        int AddContact(Contact contact);

        bool DeleteContact(int id);

        List<Contact> GetContacts();
    }
}
