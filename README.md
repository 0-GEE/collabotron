# Collabotron

This is a simple server-client based system designed to 
streamline the process of creating collaborative beatmaps in osu!. 
It is built using Python (Flask) for the server and C# (WPF) for the client.


## For Users
Unfortunately, getting the whole Collabotron system up and running isn't the most friendly towards those who 
do not have experience with web technologies. To set it all up you'll first need to spin up the server which can 
be achieved using any web server software which supports WSGI along with a machine with open ports (VPS or PC 
with port forwarding work). Alternatively, you can run the Flask development server directly without caring about
setting up an actual production server (although I feel legally required to inform you that this may not be a good idea).

As for starting up the client, it's as simple as double-clicking the executable. Just make sure you have the
server up and running, and the osu! Editor open to your desired beatmap beforehand.

## For Contributors
I would first like to apologize for the atrocity that is subjecting your eyes to my code. If you actually want to 
work on this project after you've recovered, you should make sure that you have the NuGet packages
``Newtonsoft.Json`` and ``Prism.Wpf`` installed as they are dependencies.

As for environment I recommend using Visual Studio 2019 (or later) as your IDE with the .NET desktop development
workload installed. Of course you also need .NET itself (``net5.0-windows`` and yes I know this is outdated you
should be able to change the target framework yourself when you work on it). If you have changes you want to
contribute just open a PR and I'll (probably) get around to checking it when I have time.
