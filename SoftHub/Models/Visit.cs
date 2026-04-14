using System;

namespace SoftHub.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}