using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laba3
{
    [Serializable]
    class Room
    {
        public string Description { get; set; }
        public List<Choice> Choices { get; set; }
        public bool IsSolved { get; set; }

        public Room(string description)
        {
            Description = description;
            Choices = new List<Choice>();
            IsSolved = false;
        }

        public void AddChoice(string description, int nextRoom)
        {
            Choices.Add(new Choice(description, nextRoom));
        }

    }
}
