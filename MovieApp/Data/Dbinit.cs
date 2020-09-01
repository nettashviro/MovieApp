using MovieApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Data
{
    public class Dbinit
    {
        public static void init(MovieAppContext _context)
        {
            // Look for any accounts.
            if (!_context.Account.Any())
            {
                _context.Account.AddRange(
                  new List<Account>() {
                        new Account
                        {
                            // User Admin
                             Username ="Admin",
                             Password = "Admin",
                             Email = "admin@gmail.com",
                             Type = Account.UserType.Admin                        },
                         new Account
                         {
                            // User Customer
                             Username ="Customer",
                             Password = "Customer",
                             Email = "customer@gmail.com",
                             Type = Account.UserType.Customer,
                         }
                   }
              );
            }

            _context.SaveChanges();
        }
    }
}
