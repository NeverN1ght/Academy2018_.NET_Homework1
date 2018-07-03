using System;
using System.Collections.Generic;
using System.Text;

namespace Academy2018_.NET_Homework1.Entities
{
    public class Address
    {
        public int Id { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string Zip { get; set; }

        public int UserId { get; set; }
    }
}
