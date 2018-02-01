using System;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Zengenti.Contensis.Management;
using StoryImport;


namespace ParsingXml
{
    class Program
    {
        // The mapping between the taxonomy node name in the CSV and the taxonomy key to save
        private static readonly Dictionary<string, string> StoryTypeTaxonomyKeys = new Dictionary<string, string>
        {
            {"News", "0/24/27"},
            {"Article", "0/24/28"}
        };


        static void Main(string[] args)
        {

            // Create the management client
            var client = ManagementClient.Create("https://cms-danb-dev.cloud.contensis.com",
                "c89ff901-0237-4eb2-8812-7481f477ae1b",
                "5fcff60262344f6688c30eaf2cb60310-876433afb06a424eb622f89285466b54-7eb4d019b89f45caae3097cb20a10c4c");

            // Get the project
            //var project = client.Projects.Get("Website");

            XElement doc = XElement.Load(@"C:\Training\ManagementAPI\dotnet-xml-import\XMLImport\booklet.xml");
            IEnumerable<Story> stories = (from x in doc.Descendants("Stories").Elements("Story")
                                            select new Story
                                            {
                                                Title = x.Element("SectionHeading").Value,
                                                Introduction = x.Element("SectionIntroduction").Value,
                                                FeatureImage = x.Element("FeatureImage").Attribute("href").Value
                                            }).ToList();
            Console.WriteLine("Story title: " + stories.First().Title);
            Console.WriteLine("Story Intro: " + stories.First().Introduction);
            Console.WriteLine("Feature Image Href: " + stories.First().FeatureImage);
            Console.ReadLine();

            //foreach (var story in stories)
            //{
            //    CreateEntryFromXML(project, story);
            //}
        }

        private static void CreateEntryFromXML(Project project, Story story)
        {
            // Set-up a new movie entry
            var storyEntry = project.Entries.New("story");

            // Set each field value
            storyEntry.Set("title", story.Title);
            storyEntry.Set("subtitle", story.Introduction);
            storyEntry.Set("featureImage", story.FeatureImage);

            // Save and publish the movie
            storyEntry.Save();
            storyEntry.Publish();
            Console.WriteLine($"Saved story {storyEntry.Get("title")}");
        }
    }
}