using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using MOOCParsersLib;
using MOOCParsersLib.HTMLParsers;
using MOOCParsersLib.HTMLParsers.CourseraHtmlParser;
using MOOCParsersLib.APIParsers;

namespace YourMOOC
{
    public partial class MainPage : ContentPage
    {
        private string CurrentTag { get; set; } = "";
        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();

        public MainPage()
        {
            InitializeComponent();

            CoursesList.ItemTemplate = new DataTemplate(typeof(CourseCell));
            TagsPicker.SelectedItem = "Выберите один из тэгов";
            SetBasicContent();
        }

        private async void SetBasicContent()
        {
            try
            {
                Courses.Add(new Course(
                        "Пожалуйста, подождите...",
                        "",
                        "",
                        Color.LightGray
                        )
                    );

                CoursesList.ItemsSource = Courses;

                MainHtmlParser<List<Course>> parser;

                parser = new MainHtmlParser<List<Course>>(
                    new CourseraHtmlParser(),
                    new CourseraHtmlParserSettings("courses")
                    );

                List<Course> list = new List<Course>();
                list = await parser.StartParsing();

                List<Course> list1 = new List<Course>();
                list1 = await MainApiParser.StartParsing("");

                foreach (var course in list1)
                    list.Add(course);

                list.Sort();
                
                Courses.Clear();
                foreach (var course in list)
                    Courses.Add(course);

                CoursesList.ItemsSource = Courses;
            }
            catch(Exception ex1)
            {
                Courses.Clear();
                Courses.Add(new Course(
                        "Что-то пошло не так!",
                        ex1.Message,
                        "",
                        Color.Pink
                        )
                    );
                CoursesList.ItemsSource = Courses;
            }
        }

        private async void SearchCourses_Tapped(object sender, EventArgs e)
        {
            try
            {
                //что происходит при поиске
                Courses.Clear();

                Courses.Add(new Course(
                        "Пожалуйста, подождите...",
                        "",
                        "",
                        Color.LightGray
                        )
                    );

                CoursesList.ItemsSource = Courses;

                MainHtmlParser<List<Course>> parser;

                parser = new MainHtmlParser<List<Course>>(
                    new CourseraHtmlParser(),
                    new CourseraHtmlParserSettings($"search?query={SearchEntry.Text.ToLower()}")
                    );

                List<Course> list = new List<Course>();

                do
                {
                    list = await parser.StartParsing();
                } while (list.Count == 0);

                List<Course> list1 = new List<Course>();
                list1 = await MainApiParser.StartParsing(SearchEntry.Text.ToLower());

                foreach (var course in list1)
                    list.Add(course);

                list.Sort();
                Courses.Clear();

                foreach (var course in list)
                    Courses.Add(course);

                if (list.Count == 0)
                {
                    Courses.Add(new Course(
                        "",
                        "Нет данных",
                        "",
                        Color.LightPink
                        )
                    );
                }

                CoursesList.ItemsSource = Courses;
            }
            catch(Exception ex2)
            {
                Courses.Clear();
                Courses.Add(new Course(
                        "Что-то пошло не так!",
                        ex2.Message,
                        "",
                        Color.Pink
                        )
                    );
                CoursesList.ItemsSource = Courses;
            }
            
        }

        private void Results_Refreshing(object sender, EventArgs e)
        {
            //что происходит при обновлении
            CoursesList.IsRefreshing = false;
        }

        private void TagsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CurrentTag = TagsPicker.SelectedItem.ToString();
                SearchEntry.Text = CurrentTag;
                SearchCourses_Tapped(sender, e);
                TagsPicker.SelectedItem = "Выберите один из тэгов";
            }
            catch { }
        }
    }
}
