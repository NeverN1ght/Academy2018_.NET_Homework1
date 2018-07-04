using System;
using System.Collections.Generic;
using System.Text;

namespace Academy2018_.NET_Homework1.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Body { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public int Likes { get; set; }
    }
}
