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
