using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    internal class UserAnswer
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int AnswerId { get; set; }

    }
}
