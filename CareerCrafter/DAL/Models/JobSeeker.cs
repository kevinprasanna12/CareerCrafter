using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DAL.Models
{
    public class JobSeeker
    {
        public int JobSeekerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public Resume Resume { get; set; }
        public ICollection<Application> Applications { get; set; }
    }
}
