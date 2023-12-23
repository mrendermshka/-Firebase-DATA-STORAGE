using System;
using Gtk;
using Newtonsoft;
using FireSharp;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using System.Collections.Generic;
using Newtonsoft.Json;
using NIKITA_MUCHANIK;

public partial class MainWindow : Gtk.Window
{

    private TreeView treeView;
    private Button retrieveStudentsButton;
    private Button retrieveGroupsButton;
    private Button retrieveSubjectsButton;
    private ListStore listStore;
    private Button addStudentButton;

    public static IFirebaseConfig ifc;
    private static FirebaseClient client;

    private Button addGroupButton;
    private Button addSubjectButton;

    public MainWindow() : base("GTK# GridView Example")
    {
        SetDefaultSize(600, 400);
        DeleteEvent += (o, args) => Application.Quit();

        ifc = new FirebaseConfig()
        {
            BasePath = "https://to-do-list-5c2b2-default-rtdb.firebaseio.com/",
            AuthSecret = "AIzaSyD35thUhEaf8cSUb4ejJP_KzZ1DYdA"
        };

        // Create buttons
        retrieveStudentsButton = new Button("Retrieve Students");
        retrieveStudentsButton.Clicked += RetrieveStudentsButton_Clicked;

        retrieveGroupsButton = new Button("Retrieve Groups");
        retrieveGroupsButton.Clicked += RetrieveGroupsButton_Clicked;

        retrieveSubjectsButton = new Button("Retrieve Subjects");
        retrieveSubjectsButton.Clicked += RetrieveSubjectsButton_Clicked;

        // Buttons for adding new items
        addStudentButton = new Button("Add Student");
        addStudentButton.Clicked += AddStudentButton_Clicked;

        addGroupButton = new Button("Add Group");
        addGroupButton.Clicked += AddGroupButton_Clicked;

        addSubjectButton = new Button("Add Subject");
        addSubjectButton.Clicked += AddSubjectButton_Clicked;

        // Create a vertical box to hold the buttons and the tree view
        var vBox = new VBox();
        vBox.PackStart(retrieveStudentsButton, false, false, 0);
        vBox.PackStart(retrieveGroupsButton, false, false, 0);
        vBox.PackStart(retrieveSubjectsButton, false, false, 0);

        treeView = new TreeView();
        vBox.PackStart(treeView, true, true, 0);

        // Create columns for the tree view
        string[] columnNames = { "ID", "Name", "Age" };
        for (int i = 0; i < columnNames.Length; i++)
        {
            var renderer = new CellRendererText();
            var column = new TreeViewColumn(columnNames[i], renderer, "text", i);
            treeView.AppendColumn(column);
        }

        // Create a ListStore to hold the data
        listStore = new ListStore(typeof(string), typeof(string), typeof(string));

        // Set the ListStore as the model for the TreeView
        treeView.Model = listStore;

        // Add the buttons for adding new items
        vBox.PackStart(addStudentButton, false, false, 0);
        vBox.PackStart(addGroupButton, false, false, 0);
        vBox.PackStart(addSubjectButton, false, false, 0);

        Add(vBox); // Add the vertical box to the window

        ShowAll();
    }

    private void AddStudentButton_Clicked(object sender, EventArgs e)
    {
        // Show the Add Student Dialog
        ShowAddStudentDialog();
    }

    private void AddGroupButton_Clicked(object sender, EventArgs e)
    { 
        ShowAddGroupDialog();
    }

    private void AddSubjectButton_Clicked(object sender, EventArgs e)
    {
        ShowAddSubjectDialog();
    }

    private void ShowAddStudentDialog()
    {
        CreateAddStudentWindow();
    }
    private void ShowAddGroupDialog()
    {
        CreateAddGroupWindow();
    }
    private void ShowAddSubjectDialog()
    {
        CreateAddSubjectWindow();
    }


    private void CreateAddStudentWindow()
    {
        Window addStudentWindow = new Window("Add Student");
        addStudentWindow.SetDefaultSize(300, 200);

        // Create a vertical box for the new window
        var vBox = new VBox();

        // Entry fields for adding a new student
        Entry studentIdEntry = new Entry();

        Entry studentNameEntry = new Entry();

        Entry studentAgeEntry = new Entry();

        // Button to add a new student
        var confirmButton = new Button("Add Student");
        Student student = new Student
        {
            Age = int.Parse(studentAgeEntry.Text),
            Name = studentNameEntry.Text,
            Id = studentIdEntry.Text
        };
        confirmButton.Clicked += (s, e) => AddStudentToFirebase(int.Parse(studentAgeEntry.Text), studentAgeEntry.Text, studentIdEntry.Text);

        // Add entry fields and the button to the vertical box
        vBox.PackStart(studentIdEntry, false, false, 0);
        vBox.PackStart(studentNameEntry, false, false, 0);
        vBox.PackStart(studentAgeEntry, false, false, 0);
        vBox.PackStart(confirmButton, false, false, 0);

        addStudentWindow.Add(vBox);
        addStudentWindow.ShowAll();
    }

    private void AddStudentToFirebase(int age, string txt, string txt2)
    {
        Student student = new Student
        {
            Age = age,
            Name = txt,
            Id = txt2
        };
        client = new FireSharp.FirebaseClient(ifc);
        SetResponse response = client.Set($"Student/{student.Id}", student);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("Student added successfully!");
        }
        else
        {
            Console.WriteLine($"Error: {response.Body}");
        }
    }

    private void CreateAddGroupWindow()
    {
        Window addGroupWindow = new Window("Add Group");
        addGroupWindow.SetDefaultSize(300, 200);

        // Create a vertical box for the new window
        var vBox = new VBox();

        // Entry fields for adding a new group
        Entry groupIdEntry = new Entry();

        Entry groupNameEntry = new Entry();

        Entry groupDescriptionEntry = new Entry();

        // Button to add a new group
        var confirmButton = new Button("Add Group");

        confirmButton.Clicked += (s, e) => AddGroupToFirebase(groupIdEntry.Text, groupNameEntry.Text,groupDescriptionEntry.Text);

        // Add entry fields and the button to the vertical box
        vBox.PackStart(groupIdEntry, false, false, 0);
        vBox.PackStart(groupNameEntry, false, false, 0);
        vBox.PackStart(groupDescriptionEntry, false, false, 0);
        vBox.PackStart(confirmButton, false, false, 0);

        addGroupWindow.Add(vBox);
        addGroupWindow.ShowAll();
    }

    private void AddGroupToFirebase(string txt, string txt1, string txt2)
    {
        Group group = new Group
        {
            Id = txt,
            Name = txt1,
            Description = txt2
        };
        client = new FireSharp.FirebaseClient(ifc);
        SetResponse response = client.Set($"Group/{group.Id}", group);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("Group added successfully!");
        }
        else
        {
            Console.WriteLine($"Error: {response.Body}");
        }
    }

    private void CreateAddSubjectWindow()
    {
        Window addSubjectWindow = new Window("Add Subject");
        addSubjectWindow.SetDefaultSize(300, 200);

        // Create a vertical box for the new window
        var vBox = new VBox();

        // Entry fields for adding a new subject
        Entry subjectIdEntry = new Entry();

        Entry subjectNameEntry = new Entry();

        Entry subjectCodeEntry = new Entry();

        // Button to add a new subject
        var confirmButton = new Button("Add Subject");
        confirmButton.Clicked += (s, e) => AddSubjectToFirebase(subjectIdEntry.Text, subjectNameEntry.Text, subjectCodeEntry.Text);

        // Add entry fields and the button to the vertical box
        vBox.PackStart(subjectIdEntry, false, false, 0);
        vBox.PackStart(subjectNameEntry, false, false, 0);
        vBox.PackStart(subjectCodeEntry, false, false, 0);
        vBox.PackStart(confirmButton, false, false, 0);

        addSubjectWindow.Add(vBox);
        addSubjectWindow.ShowAll();
    }

    private void AddSubjectToFirebase(string a, string b, string c)
    {
        Subject subject = new Subject
        {
            Id = a,
            Name = b,
            Code = c
        };
        client = new FireSharp.FirebaseClient(ifc);
        SetResponse response = client.Set($"Subject/{subject.Id}", subject);
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("Subject added successfully!");
        }
        else
        {
            Console.WriteLine($"Error: {response.Body}");
        }
    }

    private void RetrieveStudentsButton_Clicked(object sender, EventArgs e)
    {
        // Fetch students from Firebase
        var students = RetrieveStudentsFromFirebase();

        // Update the TreeView with student data
        UpdateTreeView(students, "Id", "Name", "Age");
    }

    private void RetrieveGroupsButton_Clicked(object sender, EventArgs e)
    {
        // Fetch groups from Firebase
        var groups = RetrieveGroupsFromFirebase();

        // Update the TreeView with group data
        UpdateTreeView(groups, "Id", "Name", "Description");
    }

    private void RetrieveSubjectsButton_Clicked(object sender, EventArgs e)
    {
        // Fetch subjects from Firebase
        var subjects = RetrieveSubjectsFromFirebase();

        // Update the TreeView with subject data
        UpdateTreeView(subjects, "Id", "Name", "Code");
    }

    private IEnumerable<NIKITA_MUCHANIK.Student> RetrieveStudentsFromFirebase()
    {
        try
        {
            client = new FireSharp.FirebaseClient(ifc);
            FirebaseResponse res = client.Get(@"Student/");

            if (res.Body == "null")
            {
                // No data found
                return new List<NIKITA_MUCHANIK.Student>();
            }

            Dictionary<string, NIKITA_MUCHANIK.Student> dataStudent = JsonConvert.DeserializeObject<Dictionary<string, NIKITA_MUCHANIK.Student>>(res.Body.ToString());
            return dataStudent.Values;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving students: {ex.Message}");
            return new List<NIKITA_MUCHANIK.Student>();
        }
    }

    // Similar changes for RetrieveGroupsFromFirebase and RetrieveSubjectsFromFirebase methods


    private IEnumerable<NIKITA_MUCHANIK.Group> RetrieveGroupsFromFirebase()
    {
        try
        {
            client = new FireSharp.FirebaseClient(ifc);
            FirebaseResponse res = client.Get(@"Group/");

            if (res.Body == "null")
            {
                // No data found
                return new List<NIKITA_MUCHANIK.Group>();
            }

            Dictionary<string, NIKITA_MUCHANIK.Group> dataGroup = JsonConvert.DeserializeObject<Dictionary<string, NIKITA_MUCHANIK.Group>>(res.Body.ToString());
            return dataGroup.Values;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving groups: {ex.Message}");
            return new List<NIKITA_MUCHANIK.Group>();
        }
    }



    private IEnumerable<NIKITA_MUCHANIK.Subject> RetrieveSubjectsFromFirebase()
    {
        try
        {
            client = new FireSharp.FirebaseClient(ifc);
            FirebaseResponse res = client.Get(@"Subject/");

            if (res.Body == "null")
            {
                // No data found
                return new List<NIKITA_MUCHANIK.Subject>();
            }

            Dictionary<string, NIKITA_MUCHANIK.Subject> dataSubject = JsonConvert.DeserializeObject<Dictionary<string, NIKITA_MUCHANIK.Subject>>(res.Body.ToString());
            return dataSubject.Values;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving subjects: {ex.Message}");
            return new List<NIKITA_MUCHANIK.Subject>();
        }
    }


    private void UpdateTreeView<T>(IEnumerable<T> data, params string[] propertyNames)
    {
        // Clear existing data
        listStore.Clear();

        // Add new data to the ListStore
        foreach (var item in data)
        {
            var values = new List<string>();
            foreach (var propertyName in propertyNames)
            {
                var property = typeof(T).GetProperty(propertyName);
                if (property != null)
                {
                    var value = property.GetValue(item);
                    // Check if the property value is not null before converting to string
                    values.Add(value?.ToString() ?? "");
                }
                else
                {
                    // Handle unexpected types or missing properties
                    Console.WriteLine($"Error: Property {propertyName} not found on type {typeof(T)}");
                }
            }

            if (values.Count == propertyNames.Length)
            {
                listStore.AppendValues(values.ToArray());
            }
            else
            {
                // Handle unexpected types or missing properties
                Console.WriteLine($"Error: Unable to retrieve values for item of type {typeof(T)}");
            }
        }
    }
}
