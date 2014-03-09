#define FUSE_ACTION_COMMENT
#define HELP_COMMENTS
//#define COMMENTS
//#define FULL_COMMENT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace FuseboxFreedom {
    /// <summary>
    /// Convert an xml fusebox circuit to a cfscript cfc
    /// TODO: convert an entire project
    /// TODO: copy fbFreedom.cfc to cfc path
    /// TODO: read fusebox.xml.cfm to output fbFreedomSettings.cfc
    /// </summary>
    class Program {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        static System.Threading.ManualResetEvent wait_exit = new System.Threading.ManualResetEvent(false);
        public static FileSystemWatcher[] watchers = null;
        public static string output_filename = null;
        public static string input_file = null;
        public static event EventHandler<WatcherNotifyEventArgs> WatchNotify;
        [STAThread]
        static void Main(string[] args) {
            if (args.Length == 1 || args.Contains("/?") || args.Contains("--help")) {
                string file = typeof(Program).Assembly.Location;
                file = Path.GetFileName(file);
                Console.WriteLine("Usage: {0} \\path\\to\\circuit.xml.cfm \\path\\to\\circuit.cfc",
                    file);
                Console.WriteLine("Converts a fusebox XML circuit.xml.cfm to a cfcscript circuit.cfc");
                Console.WriteLine("\t or: {0} \\path\\to\\,\\other\\path\\to\\[..\\other\topath\\to] circuit.xml.cfm circuit.cfc [watch]",
                     file);
                Console.WriteLine("Convert many circuit.xml.cfm circuits to circuit.cfc");
                Console.WriteLine("\t specify 'watch' to continue running and process any specified files when they are changed");
                Console.WriteLine("\t{0} --help, {0} /?: show this message", file);
                Console.ReadKey();
            }
            if (args.Length == 0) {
                ShowWindow(Process.GetCurrentProcess().MainWindowHandle, 0);
                System.Windows.Forms.Application.EnableVisualStyles();
                try {
                    System.Windows.Forms.Application.Run(new GUI());
                } catch (Exception ex) {
                    new System.Windows.Forms.ThreadExceptionDialog(ex).ShowDialog();
                }
            }
            else if (args.Length == 2) {
                Convert(args[0], args[1]);
            } else {
                string[] paths = args[0].Split(',');
                foreach (string path in paths) {
                    Convert(path, args[1], args[2]);
                }
                if (args.Length > 3 && args[3] == "watch") {
                    Console.WriteLine("Watching:\n\t{0}", String.Join("\n\t",paths.Select(p=>Path.Combine(p, args[1]))));
                    input_file = args[1];
                    output_filename = args[2];
                    watchers = paths.Select(p=>Watch(p, args[1])).ToArray();
                    //wait forever!
                    wait_exit.WaitOne();
                }
            }
                
        }

        public static FileSystemWatcher Watch(string path, string from) {
            //path = Path.GetFullPath(path);
            FileSystemWatcher w = new FileSystemWatcher(path);
            w.Changed += new FileSystemEventHandler(w_Notify);
            w.Renamed += new RenamedEventHandler(w_Renamed);
            w.Disposed += new EventHandler(w_Disposed);
            w.Error += new ErrorEventHandler(w_Error);
            w.IncludeSubdirectories = false;
            w.EnableRaisingEvents = true;
            return w;
        }

        static void w_Renamed(object sender, RenamedEventArgs e) {
            if (e.Name == input_file) {
                string path = ((FileSystemWatcher)sender).Path;
                var h = Program.WatchNotify;
                if (h != null) {
                    h(sender, new WatcherNotifyEventArgs(true, null, true));
                } else {
                    Console.Write("File {0}", e.FullPath);
                }
                try {
                    Convert(e.FullPath, Path.Combine(path, output_filename));
                    if (h != null) {
                        h(sender, new WatcherNotifyEventArgs(true, null, false));
                    } else {
                        Console.WriteLine(" -> " + output_filename);
                    }
                } catch (Exception ex) {
                    if (h != null) {
                        h(sender, new WatcherNotifyEventArgs(true, ex));
                    } else {
                        Console.WriteLine("\t{0}\n{1}", ex.GetType().Name, ex.Message);
                    }
                }
            }
        }

        static void w_Disposed(object sender, EventArgs e) {
            var h = Program.WatchNotify;
            if (h != null) {
                h(sender, new WatcherNotifyEventArgs(false, null));
            } else {
                Console.WriteLine("Watcher Disposed!");
            }
            RemoveWatcher((FileSystemWatcher)sender);
        }

        static void w_Error(object sender, ErrorEventArgs e) {
            var h = Program.WatchNotify;
            if (h != null) {
                h(sender, new WatcherNotifyEventArgs(true, e.GetException()));
            } else {
                Console.WriteLine("Error: {0} : {1}",
                                  e.GetException().GetType().Name,
                                  e.GetException().Message);
            }
        }

        static void w_Notify(object sender, FileSystemEventArgs e) {
            if (e.Name == input_file) {
                
            var h = Program.WatchNotify;
                Console.Write("File {0} {1}", e.FullPath, e.ChangeType);
                switch (e.ChangeType) {
                    case WatcherChangeTypes.Deleted:

                        if (h != null) {
                            h(sender, new WatcherNotifyEventArgs(false, null, true));
                        } else {
                            Console.WriteLine("\t Watcher removed");
                        }
                        RemoveWatcher((FileSystemWatcher)sender);
                        break;
                    case WatcherChangeTypes.Changed:

                        string path = ((FileSystemWatcher)sender).Path;
                        try {
                            Convert(e.FullPath, Path.Combine(path, output_filename));
                            if (h != null) {
                                h(sender, new WatcherNotifyEventArgs(true, null, false));
                            } else {
                                Console.WriteLine(" -> " + output_filename);
                            }
                        } catch (Exception ex) {
                            if (h != null) {
                                h(sender, new WatcherNotifyEventArgs(true, ex));
                            } else {
                                Console.WriteLine("\t{0}\n{1}", ex.GetType().Name, ex.Message);
                            }
                        }
                        break;
                }
                Console.WriteLine();
            }
        }

        static void RemoveWatcher(FileSystemWatcher w) {
            lock (wait_exit) {
                w.EnableRaisingEvents = false;
                watchers = watchers.Except(new FileSystemWatcher[] {w}).ToArray();
                if (watchers.Length == 0) {
                    wait_exit.Set();
                }
            }
        }

        public static void Convert(string path, string from, string to) {
            Convert(Path.Combine(path, from),
                    Path.Combine(path, to));
        }

        static void Convert(string from, string to) {
            XDocument doc = XDocument.Load(from, LoadOptions.SetLineInfo);
            StreamWriter sw = new StreamWriter(to);
            try {
                sw.Write(String.Join("\n", doc.Elements("circuit").SelectMany(c => WriteCircuit(c))));
            } catch (Exception ex) {
                throw new XmlException("Error converting " + from + " : " + ex.Message, ex);
            }
            sw.Close();
        }

        /// <summary>
        /// Write a cfc circuit from a circuit.xml.cfm. 
        /// Not completely accurate as far as fusebox behavior
        /// but this is the way i think it should work
        /// prefuseaction becomes init()
        /// postfuseaction raises an exception
        /// </summary>
        /// <param name="circuit">circuit node to transform.</param>
        /// <returns></returns>
        static IEnumerable<string> WriteCircuit(XElement circuit) {
            return new string[] {
                "component hint=\"Generated by FuseboxFreedom http://www.github.com/jamie-pate/FuseBoxFreedom\" output=\"false\"{",
#if COMMENTS
                "//Handle all the boilerplate",
#endif
#if HELP_COMMENTS
                tab("//IMPORTANT: Do not update this file directly if there is still a circuit.xml.cfm file in the same directory"),
                tab("//This file was generated automatically with " + Path.GetFileName(typeof(Program).Assembly.Location)),
                tab("//Use "),
                tab("//\t " + Path.GetFileName(typeof(Program).Assembly.Location) + " path/to/this/circuit circuit.xml.cfm circuit.cfc watch"),
                tab("//to automatically propagate changes from the circuit.xml.cfm to the circuit.cfc"),
                tab("//If the circuit.xml.cfm source file has been removed, it is probably safe to ignore these instructions"),
#endif
                tab("this.fb = createObject('component', 'cfcs.fbFreedom').init(this, variables)"),
                
                tab(CheckSupportedAccess(circuit))
                }
                .Concat(circuit.Elements("prefuseaction").SelectMany(pfa=>tab(WriteFA(pfa, "init"))))
                .Concat(circuit.Elements("fuseaction").SelectMany(fa=>tab(WriteFA(fa))))
                .Concat(circuit.Elements("postfuseactioon").SelectMany(pfa=>tab(NotImplemented(pfa))))
                .Concat(new string[] {
                "}"
                });
        }

        private static IEnumerable<string> NotImplemented(XElement elem) {
            throw new NotImplementedException(line(elem));
        }

        static string tab(string str) {

            return String.Join("\n", str.Split('\n').Select(s => String.IsNullOrWhiteSpace(s) ? s : "\t" + s));
        }
        static IEnumerable<string> tab(IEnumerable<string> strings) {
            return strings.Select(s=>tab(s));
        }

        static string comment(string str) {
            return String.Join("\n", str.Split('\n').Select(s => "//" + s));
        }

        static IEnumerable<string> comment(XElement elem, string output) {
            return new string[] {
#if COMMENTS
                comment(elem),
#endif
                output
            };
        }

        static IEnumerable<string> comment(XElement elem, IEnumerable<string> output, bool empty = false) {
#if COMMENTS
            return new string[] {comment(elem, empty)}.Concat(output);
#else
            return output;
#endif
        }
        static string comment(XElement elem, bool empty = false) {
#if FULL_COMMENT
            empty = false;
#endif
            XElement oelem = elem;
            if (empty) {
                elem = new XElement(elem);
                elem.ReplaceAll(elem.Attributes());
            }
            return comment(line(elem, oelem));
        }

        static string line(XElement elem, XElement elem2 = null) {
            if (elem2 == null) {
                elem2 = elem;
            }
            return ((IXmlLineInfo)elem2).LineNumber + " : " + elem.ToString();
        }
        static IEnumerable<string> CFFunction(string name, IEnumerable<string> lines) {
            return new string[] {
#if FUSE_ACTION_COMMENT
                    comment("fuseAction"),
#endif
                    String.Format("function {0}() {{", name)
                }.Concat(tab(lines))
                .Concat(new string[]{"}",""});
        }
        
        static string CheckSupportedAccess(XElement elem) {
            string access_level = elem.Attr("access");
            if (!new string[] {"", "public", "internal"}.Contains(access_level)) {
                throw new NotImplementedException("access levels other than public, internal for " + line(elem));
            }
            switch (elem.Name.ToString()) {
                case "circuit":
                    return String.Format("this.fb.access_level = \"{0}\";", access_level);
                case "fuseaction":
                    return String.Format("this.fb.acl[\"{0}\"] = \"{1}\";", elem.Attr("name"), access_level);
                case "prefuseaction":
                    return String.Format("this.fb.acl[\"init\"] = \"{0}\";", access_level);
                default:
                    throw new NotImplementedException("Access levels not implemented for " + line(elem));

            }
        }

        static IEnumerable<string> WriteFA(XElement fuseAction, string methodName = null) {
            return comment(fuseAction,
                new string[] {
                    CheckSupportedAccess(fuseAction)
                }.Concat(
                    CFFunction(methodName ?? fuseAction.Attribute("name").Value,
                              fuseAction.Elements().SelectMany(e => comment(e, FuseElement(e), true)))
                )
                , true);
        }

        static IEnumerable<string> FuseElements(XElement elem) {
            return elem.Elements().SelectMany(e => comment(e, FuseElement(e), true));
        }

        static bool Truthy(string b, bool emptyValue = false) {
            switch (b.ToLower()) {
                case "false":
                case "no":
                case "0":
                    return false;
                case "":
                    return emptyValue;
                default:
                    return true;
            }
        }

        static IEnumerable<string> WriteOutput(XElement elem, XName attrName, params string[] lines) {
            XAttribute name = elem.Attribute(attrName);
            if (name == null) {
                return lines.SelectMany(l=>OverWrite(elem, name, String.Format("writeOutput({0})", l)));
            } else {
                return OverWrite(elem, name, lines);
            }
        }

        static IEnumerable<string> OverWrite(XElement elem, XName attrName, params string[] lines) {
            XAttribute name = elem.Attribute(attrName);
            return OverWrite(elem, name, lines);
        }
        
        static IEnumerable<string> OverWrite(XElement elem, XAttribute name, params string[] lines) {
            if (Truthy(elem.Attr("overwrite"), true)) {
                return lines.Select(l=>Assign(name, l));
            } else {
                if (name == null) {
                    throw new NotSupportedException("Cannot not overwrite when name attribute is not present: " + line(elem));
                }
                return new string[] {
                   String.Format("if (not isdefined(\"{0}\")) {{",
                        name.Value)
                }.Concat(tab(lines.Select(l=>Assign(name, l))))
                .Concat(new string[] {
                    "}"
                });
            }
        }

        /// <summary>
        /// remove extra hash marks from #expression#.
        /// If # is detected inside expression, return "#expres##sion#"
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static string DeHash(string str, bool quote = true) {
            string chopped = str.Length > 2 ? str.Substring(1, str.Length - 2) : str;
            if (str.StartsWith("#") && str.EndsWith("#") && !chopped.Contains('#')) {
                return chopped;
            } else if (quote) {
                return "\"" + str + '\"';
            } else {
                return str;
            }
        }

        static string Assign(XAttribute name, string value) {
            return name != null ?
                String.Format("{0} = {1};",
                    name.Value, value) :
                String.Format("{0};",
                    value);
        }

        static IEnumerable<string> FuseElement(XElement elem) {
            switch (elem.Name.ToString()) {
                case "xfa":
                    return new string[] {
                            String.Format("this.fb.set_xfa(\"{0}\",\"{1}\", {2});",
                            elem.Attr("name"), elem.Attr("value"), Truthy(elem.Attr("overwrite"), true))
                    };
                case "set":
                    return OverWrite(elem, "name", DeHash(elem.Attr("value")));
                case "if":
                    XElement t = elem.Element("true");
                    XElement f = elem.Element("false");
                    List<string> result = new List<string>();
                    if (t != null || f != null) {
                        string fmt = t == null ? "not({0})" : "{0}";
                        result.Add(String.Format("if ({0}) {{",
                            String.Format(fmt,
                                DeHash(elem.Attr("condition"), false))));
                        if (t != null) {
                            result.AddRange(tab(comment(
                                t,
                                FuseElements(t),
                                true)));
                        }
                        if (t != null && f != null) {
                            result.Add("} else {");
                        }
                        if (f != null) {
                            result.AddRange(tab(comment(
                                f,
                                FuseElements(f),
                                true)));
                        }
                        result.Add("}");
                        return result;
                    } else {
                        //<if><stuff/></if> is illegal right?
                        throw new NotImplementedException("Empty if: " + line(elem));
                    }
                case "relocate":
                    //<cflocation url="#xfa.cart###nocart" addtoken="no">
                    return new string[] {
                        String.Format("location({0}, {1})", DeHash(elem.Attr("url")), false)
                    };
                case "instantiate":
                    return OverWrite(elem, "object",
                        String.Format("this.fb.instantiate(\"{0}\")",
                            elem.Attr("class")));

                case "invoke":
                    XAttribute mc = elem.Attribute("methodcall");
                    if (mc == null) {
                        throw new NotImplementedException("Only methodcall style invoke is supported " + line(elem));
                    }
                    return OverWrite(elem, "returnvariable",
                        String.Format("this.fb.instantiate('{0}')\n\t.{1}",
                            elem.Attr("class"), mc.Value));

                case "do":
                    return WriteOutput(elem, "contentvariable",
                                    String.Format("this.fb.do('{0}', {1})", elem.Attr("action"), true));
                case "include":
                    return WriteOutput(elem, "contentvariable", String.Format("this.fb.include(\"{0}\")",
                            elem.Attr("template")));
                default:
                    throw new NotImplementedException(line(elem));
                
            }
            
        }
        public class WatcherNotifyEventArgs : EventArgs {
            public WatcherNotifyEventArgs(bool processed, Exception ex, bool? progress = null) {
                this.processed = processed;
                this.ex = ex;
                this.progress = progress;
            }
            public bool? progress { get; private set; }
            public bool processed { get; private set; }
            public Exception ex { get; private set; }
        }

    }
    static class XElementExtensions {
        public static string Attr(this XElement elem, XName name) {
            XAttribute attr = elem.Attribute(name);
            if (attr != null) {
                return attr.Value;
            } else {
                return "";
            }
        }

        public static bool Attr(this XElement elem, XName name, bool defaultValue) {
            bool value;
            return bool.TryParse(elem.Attr(name),out value) ? value : defaultValue;
        }
    }

}
