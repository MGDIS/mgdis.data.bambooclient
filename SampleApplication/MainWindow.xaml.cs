using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using MGDIS.Data.BambooClient;
using SampleApplication.Commands;
using SampleApplication.Entities;
using SampleApplication.Queries;

namespace SampleApplication
{
    /// <summary>
    /// Main window
    /// </summary>
    /// <remarks>No MVVM, no styles, just a sample application</remarks>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Teacher> Teachers = null;
        public ObservableCollection<Course> Courses = null;
        public ObservableCollection<Student> Students = null;

        public ObservableCollection<List<string>> Results = new ObservableCollection<List<string>>();

        public MainWindow()
        {
            InitializeComponent();
            Teachers = new ObservableCollection<Teacher>(new GetTeachers().Execute() as List<Teacher>);
            Courses = new ObservableCollection<Course>(new GetCourses().Execute() as List<Course>);
            Students = new ObservableCollection<Student>(new GetStudents().Execute() as List<Student>);
            lstTeachers.DataContext = Teachers;
            lstCourses.DataContext = Courses;
            lstStudents.DataContext = Students;
            lstResults.ItemsSource = Results;
        }

        private void btnSampleRequest_Click(object sender, RoutedEventArgs e)
        {
            txtRequest.Text = SampleRequest.GetNext();
        }

        private void btnCreateTeacher_Click(object sender, RoutedEventArgs e)
        {
            object Result = new CreateTeacher(
                new Entities.Teacher(
                    Guid.NewGuid().ToString("N"),
                    txtTeacherFirstName.Text,
                    txtTeacherLastName.Text,
                    txtTeacherPhoneExtension.Text)).Execute();
            Teachers.Add(Result as Teacher);
        }

        private void btnCreateCourse_Click(object sender, RoutedEventArgs e)
        {
            object Result = new CreateCourse(
                new Entities.Course(
                    Guid.NewGuid().ToString("N"),
                    txtCourseDescription.Text,
                    TimeSpan.FromHours(double.Parse(txtCourseDuration.Text)))).Execute();
            Courses.Add(Result as Course);
        }

        private void btnCreateStudent_Click(object sender, RoutedEventArgs e)
        {
            object Result = new CreateStudent(
                new Entities.Student(
                    Guid.NewGuid().ToString("N"),
                    txtStudentFirstName.Text,
                    txtStudentLastName.Text)).Execute();
            Students.Add(Result as Student);
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BambooConnection Connection = new BambooConnection(PersistenceEngine.Engine);
                BambooCommand Command = new BambooCommand(txtRequest.Text, Connection);
                Connection.Open();

                using (IDataReader Reader = Command.ExecuteReader())
                {
                    GridView DynamicGridView = new GridView();
                    for (int i = 0; i < Reader.FieldCount; i++)
                    {
                        GridViewColumn GridColumn = new GridViewColumn();
                        GridColumn.Header = Reader.GetName(i);
                        GridColumn.DisplayMemberBinding = new Binding(string.Format("[{0}]", i));
                        DynamicGridView.Columns.Add(GridColumn);
                    }
                    lstResults.View = DynamicGridView;

                    Results.Clear();
                    while (Reader.Read())
                    {
                        List<string> DataLine = new List<string>();
                        for (int i = 0; i < Reader.FieldCount; i++)
                            DataLine.Add(Reader.GetString(i));
                        Results.Add(DataLine);
                    }
                }
            }
            catch (Exception excep)
            {
                MessageBox.Show("Chances are that the request is incorrect or contains currently unsupported features. Use the [Sample request...] button to generate supported SQL requests, and check https://github.com/MGDIS/mgdis.data.bambooclient for more information about features to come." + Environment.NewLine + Environment.NewLine + excep.ToString(), "Bamboo ADO.NET Provider Error");
            }
        }

        private void btnTeacherCourseRelation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Associations are not implemented yet, as the Bamboo ADO.NET Provider does not support joins for now. More on https://github.com/MGDIS/mgdis.data.bambooclient", "Undefined behaviour");
        }

        private void btnStudentCourseRelation_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Associations are not implemented yet, as the Bamboo ADO.NET Provider does not support joins for now. More on https://github.com/MGDIS/mgdis.data.bambooclient", "Undefined behaviour");
        }
    }
}
