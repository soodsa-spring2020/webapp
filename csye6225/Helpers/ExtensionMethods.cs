using System.Collections.Generic;
using System.Linq;
using csye6225.Models;

namespace csye6225.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<AccountModel> WithoutPasswords(this IEnumerable<AccountModel> users) {
            return users.Select(x => x.WithoutPassword());
        }

        public static AccountModel WithoutPassword(this AccountModel user) {
            user.password = null;
            return user;
        }
    }
}