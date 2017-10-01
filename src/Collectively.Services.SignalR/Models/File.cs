using System;

namespace Collectively.Services.SignalR.Models
{
    public class File
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Url { get; set; }
        public string Metadata { get; set; }
        public DateTime CreatedAt { get; set; }        
    }
}