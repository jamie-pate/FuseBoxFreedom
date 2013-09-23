FuseBoxFreedom
==============

Break out of fusebox XML. Convert to cfc cfscript based circuits.

Tired of dealing with crusty old Fusebox XML files?
Wish you didn't have to deal with massive parsed files that unroll your fuseactions
and generally try their best to defeat the JVM?

FuseBoxFreedom attempts to replace fusebox with a lightweight cfc based drop-in.
It converts circuit.xml.cfm files to circuit.cfc cfscript and handles requests like fusebox did,
but without the clunky parser getting in the way.

The re-parser is written in C# (Linq to XML) because it was the quickest way I could think of to rewrite XML,
but the output should (eventually) run in Railo/Coldfusion 9+ environments.

* Still very early, any contributions would be welcome.

Usage:
* Convert a fusebox XML circuit.xml.cfm to a cfcscript circuit.cfc
* * FuseboxFreedom.exe \path\to\circuit.xml.cfm \path\to\circuit.cfc
* or: Convert many circuit.xml.cfm circuits to circuit.cfc:
* * FuseboxFreedom.exe \path\to\,\other\path\to\[,..\otherpath\to] circuit.xml.cfm circuit.cfc [watch]
* specify 'watch' to continue running and process any specified files when they are changed

GOOD NEWS EVERYBODY!
----

FuseBoxFreedom increased the speed of my Fusebox 4.1 XML.CFM project 5x!

That's right. Page loads went down from minimum 1200ms to minimum 250ms! This is not even including parse time.


NOTES:
---
Offers backwards compatibility with a direct conversion,
but it's a pretty loose match that fits the way my site is set up.
Prefuse is only run once per circuit, no post fuse, no events, lexicons etc.
Apparently I never needed most of that cruft. It also does partial conversion,
so You can start using it on a few circuits without fixing the entire site at once.

The output is pure cfscript which is much more aesthetically pleasing than tag soup.
It also has the advantage that cfscript runs faster in modern cf engines. (cf9, railo 3/4)

I used a clean room design, based mostly from my expectation and understanding of the XML grammar behavior,
so there is no attachment/reliance on any previous fusebox code base.

Reasoning:
---
After I finally spent many hours digging into the fusebox 4.1 transformer and parser (that I'd inherited and been running for 5 years)
and tried to monkey with them to overcome the "too much code" errors that Railo was encountering (caused by firehosing fuse action code (and probably too many nested do tags on my part)) into the parsed files.

The code for fb4.1 is ... less than maintable.
Combine that with the fact that flat parsed files may be a good idea for a dumb interpreter,
but modern cf runs on the JVM and that makes big wads of code the opposite of good!
(Lots of classes suck up RAM, jit time goes through the roof, 
hot spots don't get the attention they deserve, etc etc)

With a looming deadline I had to make a choice, Hack on the WETT (Write everything twelve times) imperative
transform/parse code, or replace the whole thing completely. 12 hours later I had something that works, 5x
faster than the transform/parsed version of the same page. This was a regular 1200ms request processing time, shaved down to 90ms!

TL;DR
---
If anyone else out there is feeling trapped by an old site running fusebox, This is for you.
And if you encounter any quirks during conversion, or bugs in the runtime, feel free to send a pull request.
