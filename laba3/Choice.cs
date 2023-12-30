using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3
{
    [Serializable]
    class Choice
    {
        public string Description { get; set; }
        public int NextRoom { get; set; }

        public Choice(string description, int nextRoom)
        {
            Description = description;
            NextRoom = nextRoom;
        }
    }
}
