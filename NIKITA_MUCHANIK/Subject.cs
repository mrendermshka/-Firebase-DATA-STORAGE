using System;
namespace NIKITA_MUCHANIK
{
    public class Subject
    {
        public string Id { get; set; } // Assuming you have an ID field in your Firebase database
        public string Name { get; set; }
        public string Code { get; set; }

        public Subject()
        {
            // Default constructor is required for FireSharp to work
        }

        public Subject(string id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }
    }
}
