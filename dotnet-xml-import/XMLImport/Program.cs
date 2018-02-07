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
                "f39cc495-2640-4f55-a39c-07b8d49692f0",
                "34bd681443474c3fbb13d361ab9a04c6-a7c40152960f41ef98cc3ecd4c1dea28-f3963186a3a94ca0a19b327a29d5a2b7");

            // Get the project
            var project = client.Projects.Get("Website");

            // Load the XML file into an XElement
            XElement doc = XElement.Load(@"C:\Sites\indesign-import-to-contensis\dotnet-xml-import\XMLImport\finalXML.xml");
            // Navigate the XElement to create the stories
            IEnumerable<Story> stories = (from x in doc.Descendants("Stories").Elements("Story")
                                          select new Story
                                          {
                                              Title = x.Element("StoryHeading").Value,
                                              Introduction = x.Element("StoryIntroduction").Value,
                                              StoryBody = x.Element("StoryBody").Elements()
                                          }).ToList();
            // Local testing in console
            //Console.WriteLine("Story title: " + stories.First().Title);
            //Console.WriteLine("Story Intro: " + stories.First().Introduction);
            //Console.WriteLine("Story Body: " + stories.First().StoryBody);
            //Console.ReadLine();

            // Iterate through stories and call entry creator
            foreach (var story in stories)
            {
                CreateEntryFromXML(project, story);
            }
        }

        private static string ElementCleaner(XElement el)
        {
            string s = "";
            switch (el.Name.LocalName)
            {
                case "StoryText":
                    s = "<p>" + el.Value + "</p>";
                    break;
                case "StorySubHeading":
                    s = "<h2>" + el.Value + "</h2>";
                    break;
            }
            return s;
        }

        // Create entries 
        private static void CreateEntryFromXML(Project project, Story story)
        {
            // Set-up a new story entry
            var storyEntry = project.Entries.New("story");

            var composer = new ComposedField();
            string html = "";
            foreach (XElement el in story.StoryBody)
            {
                html = html + ElementCleaner(el);
            }
            byte[] bytes = Encoding.Default.GetBytes(html);
            html = Encoding.UTF8.GetString(bytes);

            composer.Add(new ComposedFieldItem("storyMarkup", html));
            storyEntry.Set("storyComposer", composer);


            // Set each field value
            storyEntry.Set("title", story.Title);
            storyEntry.Set("subtitle", story.Introduction);

            // Save and publish the story
            storyEntry.Save();
            storyEntry.Publish();
            Console.WriteLine($"Saved story {storyEntry.Get("title")}");
        }
    }
}