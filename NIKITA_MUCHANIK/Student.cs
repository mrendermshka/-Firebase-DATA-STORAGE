using System;
namespace NIKITA_MUCHANIK
{
    public class Student
    {
        public string Id { get; set; } // Assuming you have an ID field in your Firebase database
        public string Name { get; set; }
        public int Age { get; set; }
        public string Grade { get; set; }

        public Student()
        {
            // Default constructor is required for FireSharp to work
        }

        public Student(string id, string name, int age, string grade)
        {
            Id = id;
            Name = name;
            Age = age;
            Grade = grade;
        }
    }
}
