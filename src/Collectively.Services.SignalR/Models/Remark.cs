using System;
using System.Collections.Generic;

namespace Collectively.Services.SignalR.Models
{
    public class Remark
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int Rating { get; set; }
        public bool Resolved { get; set; }
        public string Status { get; set; }
        public IList<File> Photos { get; set; }
        public IList<Vote> Votes { get; set; }
    }
}