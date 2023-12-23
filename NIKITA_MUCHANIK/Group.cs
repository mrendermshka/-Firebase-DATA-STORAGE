using System;
namespace NIKITA_MUCHANIK
{
    public class Group
    {
        public string Id { get; set; } // Assuming you have an ID field in your Firebase database
        public string Name { get; set; }
        public string Description { get; set; }

        public Group()
        {
            // Default constructor is required for FireSharp to work
        }

        public Group(string id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
    }
}
