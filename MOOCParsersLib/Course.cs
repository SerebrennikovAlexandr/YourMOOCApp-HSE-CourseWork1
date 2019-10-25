using System;
using Xamarin.Forms;

namespace MOOCParsersLib
{
    public class Course : IComparable<Course>
    {
        public string Name { get; set; }
        public string Source { get; set; }
        public string SourсeRef { get; set; }
        public ImageSource Picture { get; set; }
        public string Rating { get; set; } = "";
        public string EnrolledPeopleAmount { get; set; } = "";
        public Color Color { get; set; }

        public Course(string name, string source, string sourceref, Color color)
        {
            Name = name;
            Source = source;
            SourсeRef = sourceref;
            Color = color;
        }

        public Course(string name, string source, string sourceref, string picture, string rating, string people, Color color)
        {
            Name = name;
            Source = source;
            SourсeRef = sourceref;
            Picture = new UriImageSource
            {
                CachingEnabled = true,
                Uri = new Uri(picture)
            };
            Rating = rating;
            EnrolledPeopleAmount = people;
            Color = color;
        }

        public Course(string name, string source, string sourceref, ImageSource picture, string rating, string people, Color color)
        {
            Name = name;
            Source = source;
            SourсeRef = sourceref;
            Picture = picture;
            Rating = rating;
            EnrolledPeopleAmount = people;
            Color = color;
        }

        public int CompareTo(Course c)
        {
            if (c.Rating != Rating) return c.Rating.CompareTo(Rating);
            else if (c.EnrolledPeopleAmount.Contains("K") && EnrolledPeopleAmount.Contains("K")) return c.EnrolledPeopleAmount.CompareTo(EnrolledPeopleAmount);
            else if (c.EnrolledPeopleAmount.Contains("K")) return 1;
            else if (EnrolledPeopleAmount.Contains("K")) return -1;
            else return c.EnrolledPeopleAmount.CompareTo(EnrolledPeopleAmount);
        }
    }
}
