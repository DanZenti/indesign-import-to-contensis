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
            var client = ManagementClient.Create("**ADD CMS URL HERE**",
                "**ADD THE CLIENT ID FROM YOUR CONTENSIS PROJECT**",
                "**ADD THE SHARED SECRET FROM YOUR CONTENSIS PROJECT**");

            // Get the project
            var project = client.Projects.Get("Website");

            // Load the XML file into an XElement
            XElement doc = XElement.Load(@"C:\Training\ManagementAPI\dotnet-xml-import\XMLImport\booklet.xml");
            // Navigate the XElement to create the stories
            IEnumerable<Story> stories = (from x in doc.Descendants("Stories").Elements("Story")
                                            select new Story
                                            {
                                                Title = x.Element("SectionHeading").Value,
                                                Introduction = x.Element("SectionIntroduction").Value,
                                                FeatureImage = x.Element("FeatureImage").Attribute("href").Value
                                            }).ToList();
            // Local testing in console
            //Console.WriteLine("Story title: " + stories.First().Title);
            //Console.WriteLine("Story Intro: " + stories.First().Introduction);
            //Console.WriteLine("Feature Image Href: " + stories.First().FeatureImage);
            //Console.ReadLine();

            // Iterate through stories and call entry creator
            foreach (var story in stories)
            {
                CreateEntryFromXML(project, story);
            }
        }

        // Create entries 
        private static void CreateEntryFromXML(Project project, Story story)
        {
            // Set-up a new story entry
            var storyEntry = project.Entries.New("story");

            // Set each field value
            storyEntry.Set("title", story.Title);
            storyEntry.Set("subtitle", story.Introduction);
            storyEntry.Set("featureImage", story.FeatureImage);

            // Save and publish the story
            storyEntry.Save();
            storyEntry.Publish();
            Console.WriteLine($"Saved story {storyEntry.Get("title")}");
        }
    }
}