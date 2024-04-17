using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quiz.Models;

namespace Quiz.Brokers.UserBroker
{
    internal interface IUserBroker
    {
        Task<bool> RegisterUserAsync(TgUser user);
        Task<Role?> LoginAsync(long tgId);
    }
}
