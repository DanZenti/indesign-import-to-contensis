using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace StoryImport
{
    public class Story
    {
        public string Title { get; set; }
        public string Introduction { get; set; }
        public IEnumerable<XElement> StoryBody { get; set; }
        public string FeatureImage { get; set; }
    }
}