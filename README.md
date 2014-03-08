FuseBoxFreedom
==============

Break out of fusebox XML. Convert to cfc cfscript based circuits.

Tired of dealing with crusty old Fusebox XML files?
Wish you didn't have to deal with massive parsed files that unroll your fuseactions
and generally try their best to defeat the JVM?

FuseBoxFreedom attempts to replace fusebox with a lightweight cfc based **partial** drop-in.
It converts circuit.xml.cfm files to circuit.cfc cfscript and handles requests like fusebox did,
but without the clunky parser getting in the way. There are configuration options to block specific
methods (circuit.fuseAction) from executing when processing a request. The handleRequest method wil
return false if the request was blocked, or the circuit requested has not been converted.

The re-parser is written in C# (Linq to XML)
but the output should run in Railo/Coldfusion 9+ environments.
In the future this could be converted to a cfc or xslt or some combination and process
the circuit cfcs the same way fusebox does for maximum compatibility.


* Still very early, any contributions would be welcome.

Usage:
* Convert a fusebox XML circuit.xml.cfm to a cfcscript circuit.cfc
* * FuseboxFreedom.exe (no arguments) will open GUI mode
* * FuseboxFreedom.exe \path\to\circuit.xml.cfm \path\to\circuit.cfc
* or: Convert many circuit.xml.cfm circuits to circuit.cfc:
* * FuseboxFreedom.exe \path\to\,\other\path\to\[,..\otherpath\to] circuit.xml.cfm circuit.cfc [watch]
* specify 'watch' to continue running and process any specified files when they are changed

GUI MODE:
---
Gui mode will run a windows form interface that allows visual configuration. Choose your project root and add some paths to the path list. Check the **watch for changes** checkbox to start watching for changes or the **process** button to immediately convert all circuits to cfc.

The GUI settings *should* save your settings across loads. Sometimes it seems to lose the persistent settings so it's best to save a backup of your settings if they are complicated.

NOTES:
---
Offers backwards compatibility with a direct conversion,
but it's a pretty loose match that fits the way my site is set up.
Prefuse is only run once per circuit, no post fuse, no events, lexicons etc.
Apparently I never needed most of that cruft. It also does partial conversion,
so You can start using it on a few circuits without fixing the entire site at once.

The output is pure cfscript which is much more aesthetically pleasing than tag soup.
~~~It also has the advantage that cfscript reportedly runs faster in modern cf engines. (cf9, railo 3/4)~~~


I used a clean room design, based mostly from my expectation and understanding of the XML grammar behavior,
so there is no attachment/reliance on any previous fusebox code base.

Performance currently hovers near the parsed circuit.xml performance level (a little slower even) for small circuits.
For **large** circuits however, fbfreedom is a clear winner so far. More accurate benchmarking and some (any) optimization
will hopefully prove fbfreedom faster in the long term.

Reasoning:
---
After I finally spent many hours digging into the fusebox 4.1 transformer and parser (that I'd inherited and been running for 5 years)
and tried to monkey with them to overcome the "too much code" errors that Railo was encountering (caused by unrolling fuse action code (and probably too many nested do tags on my part)) into the parsed files.

The code for fb4.1 is ... less than maintable.
Combine that with the fact that flat parsed files may be a good idea for a dumb interpreter,
but modern cf runs on the JVM and that makes big wads of code the opposite of good! (especially with 'good' performance/caching settings)

With a looming deadline I had to make a choice, Hack on the WETT (Write everything twelve times) imperative
transform/parse code, or replace the whole thing completely. 12 hours later I had something that 'works for me'


TL;DR
---
If anyone else out there is feeling trapped by an old site running fusebox, This is for you. Large circuits running fuseboxFreedom seem to be slightly faster than parsed fusebox 4.1 code with 'best' performance settings enabled.
And if you encounter any quirks during conversion, or bugs in the runtime, feel free to send a pull request.

TODO:
---
Add proper unit tests.
Fix scope issues
Find a better way to distribute scope? (unit tests will help).
Proper benchmarking tests.
Actual performance optimizations! (there are none yet).
