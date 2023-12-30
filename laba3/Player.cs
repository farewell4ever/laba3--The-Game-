using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3
{

    [Serializable]
    class Player
    {
        public string Name { get; set; }
        public int CurrentRoom { get; set; }

        public Player(string name, int currentRoom)
        {
            Name = name;
            CurrentRoom = currentRoom;
        }
    }

}
