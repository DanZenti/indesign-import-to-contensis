# Indesign to contensis

An example of bringing tagged content from an Adobe Indesign document into Contensis. This uses the Management API for Contensis.

## What it contains

- An example Indesign file - a basic magazine with multiple stories with tagged items.
- DTD file to define the Indesign XML structure. 
- An example of the XML file. 
- A .NET project utilising the Contensis Management API to read the XML and create Content Entries in Contensis.

## You will also need to create a Content Type in Contensis

Before you run the .NET script and bring the content from the XML file into Contensis you will need a Content Type called 'Story'. Follow the details below to create the Content Type and correct fields to match the XML data.

## Get started

A quickstart tutorial is being created for this repository on www.zengenti.com/blogs

A local copy of this repository can be made using the git clone command:

git clone https://github.com/DanZenti/indesign-import-to-contensis.git
The dependencies will need to be resolved using Nuget.

The Contensis Management API package is a Nuget dependendency which can be found at https://www.nuget.org/packages/Zengenti.Contensis.Management/

## Other notes
...