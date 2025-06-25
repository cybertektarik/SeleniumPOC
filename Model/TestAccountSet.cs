using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumProject.Model
{
    public class TestAccountUser
    {
        public string Username { get; set; } = string.Empty;
    }

    public class TestAccountSet
    {
        public string DefaultUrl { get; set; } = string.Empty;
        public Dictionary<string, TestAccountUser> Users { get; set; } = new();
    }

}
