# Collabotron

This is a simple server-client based system designed to 
streamline the process of creating collaborative beatmaps in osu!.
It is built using Python (Flask) for the server and C# (WPF) for the client.

Collabotron speeds up the collab mapping process by automating all the overhead
of digging the .osu file out of the beatmap directory, finding your collabmate(s),
sending the file to them, and them having to place it in their beatmap directories.


## For Users
Unfortunately, getting the whole Collabotron system up and running isn't the most friendly towards those who 
do not have experience with web technologies. To set it all up you'll first need to spin up the server which can 
be achieved using any web server software which supports WSGI along with a machine with open ports (VPS or PC 
with port forwarding work). Alternatively, you can run the Flask development server directly without caring about
setting up an actual production server (although I feel legally required to inform you that this may not be a good idea).

As for starting up the client, it's as simple as double-clicking the executable. Just make sure you have the
server up and running, and the osu! Editor open to your desired beatmap beforehand.

Once everything is set up, you first need to enter the access code to your beatmapping session
(This should be agreed upon and configured by the session 'owner' prior to startup by
modifying the server's ``.env`` file.). This action will also prompt you to enter the 
path to your osu! Songs folder if this is the first time you are using Collabotron on your computer.
Be sure to include the FULL path to the Songs folder. When authenticating, it is important to ensure
that you and your collabmates authenticate in the order that you intend to have when mapping.

Once you have authenticated, a status message will be displayed in the application indicating
whether or not you are the "host" of the collab session. Here, we defined "host" to mean
the one who currently posesses the privilege to upload their beatmap to the server, thus
syncing the beatmap contents of all other collabmates.

Once you are finished your part, just click the "Finish Mapping" button to upload your part
to the server and sync the beatmaps of all of your collabmates. Note that this will
also transfer your host privileges away to whoever is next in line to map. From then on,
you will wait until it is your turn to map again. Whenever the host uploads their part,
your .osu file will be synced with theirs. Simply reload the Editor (Ctrl + L) to get osu!
to read the updated map into the Editor.

If you wish to leave the session but do not wish to close the client, just hit the "Stop Session" button.

## For Contributors
I would first like to apologize for the atrocity that is subjecting your eyes to my code. If you actually want to 
work on this project after you've recovered, you should make sure that you have the NuGet packages
``Newtonsoft.Json`` and ``Prism.Wpf`` installed as they are dependencies.

As for environment I recommend using Visual Studio 2019 (or later) as your IDE with the .NET desktop development
workload installed. Of course you also need .NET itself (``net5.0-windows`` and yes I know this is outdated you
should be able to change the target framework yourself when you work on it). If you have changes you want to
contribute just open a PR and I'll (probably) get around to checking it when I have time.
