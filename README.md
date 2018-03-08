# Sleipnir
A graph editor framework leveraging the unity asset store asset [Odin](http://sirenix.net/odininspector "Odin")

(pronounced “SLAYP-neer”; “The Sliding One”)

![Gif of the graph editor](example.gif "")

THIS PROJECT IS STILL WIP
The main reason I created this was to have a visual way to build and modify graph hierarchies
and still use the power of Odin inspectors within my graph editor.

# Install

* Make sure you have Odin installed into your unity project
* Copy the Sleipnir folder to your Assets folder, and wait for unity to compile
* Right click in the project listing inside unity and choose Sleipnir -> Graph
* This will generate a scriptable object that acts as the data source for a graph
* Double click the asset that was created and a graph editor will open up
* Happy graph editing

# Customization

Sleipnir defines an interface for plugging Data into each node in the graph

You will need to define a C# script that inherits from this interface and then you will beable to
fill the data porition of a node with a instance of that object.


# TODO / Outstanding Features

* Horizontal + Vertical graph layout types
* Node unlocking status and hierarchial based unlocking
* Functions for easily laying out a real-time graph based on the data
* Visual styling upgrades to feel more polished

If you would like to help out please submit PR's and or reach out in the DevDog discord where Odin is discussed


