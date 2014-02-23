using System;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Collections.Specialized;
using System.IO;
using Windows7.DesktopIntegration.Interop;

namespace FuseboxFreedom {
    public partial class GUI : Form {
        public GUI() {
            InitializeComponent();
        }
        private Windows7.DesktopIntegration.Interop.ITaskbarList3 tbl;
        private void GUI_Load(object sender, EventArgs earg) {

            tbl = (Windows7.DesktopIntegration.Interop.ITaskbarList3)new Windows7.DesktopIntegration.Interop.CTaskbarList();
            var sd = FuseboxFreedom.Properties.Settings.Default;
            var sstring = sd.project;
            dgv_paths.AutoGenerateColumns = false;
            dgv_paths.DataSource = new BindingList<PathSetting>();
            if (!String.IsNullOrWhiteSpace(sstring)) {
                XDocument sdoc = XDocument.Parse(sstring);
                LoadSettingsXML(sdoc);
            }
            if (sd.recent != null) {
                btn_load.DropDownItems.AddRange(
                    sd.recent.OfType<string>()
                        .Select(s => MakeRecent(s)).ToArray()
                );
            }
            Program.WatchNotify += new EventHandler<Program.WatcherNotifyEventArgs>(Program_WatchNotify);

        }

        void Program_WatchNotify(object sender, Program.WatcherNotifyEventArgs e) {
            if (watcherPaths.ContainsKey((FileSystemWatcher)sender)) {
                PathSetting ps = watcherPaths[(FileSystemWatcher)sender];
                ps.processed = e.processed;
                ps.exception = e.ex;
                ps.Update();
            }
            if (e.progress != null || e.ex != null) {
                TBPFLAG progressflag = e.progress ?? false ? TBPFLAG.TBPF_INDETERMINATE : TBPFLAG.TBPF_NOPROGRESS;
                if (e.ex != null) {
                    progressflag = TBPFLAG.TBPF_ERROR;
                }
                this.BeginInvoke((MethodInvoker)delegate {
                    HideProgress(progressflag == TBPFLAG.TBPF_NOPROGRESS);
                    if (progressflag != TBPFLAG.TBPF_NOPROGRESS) {
                        tbl.SetProgressState(this.Handle, progressflag);
                        if (progressflag == TBPFLAG.TBPF_ERROR) {
                            tbl.SetProgressValue(this.Handle, 100, 100);
                        }
                    }
                });
            }
        }

        private ToolStripMenuItem MakeRecent(string path) {
            ToolStripMenuItem mnu = new ToolStripMenuItem(path);
            mnu.Click += new EventHandler(mnu_load_item_Click);
            return mnu;
        }

        void AddRecent(string path) {
            ToolStripMenuItem mnu = btn_load.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(i=>i.Text == path);
            if (mnu != null) {
                btn_load.DropDownItems.Remove(mnu);
            }
            btn_load.DropDownItems.Add(MakeRecent(path));
        }

        void mnu_load_item_Click(object sender, EventArgs e) {
            LoadSettingsFile(((ToolStripMenuItem)sender).Text);
        }
        private void GUI_FormClosing(object sender, FormClosingEventArgs e) {
            try {
                var sd = FuseboxFreedom.Properties.Settings.Default;
                sd.project = SaveSettingsXML().ToString();
                sd.recent = new StringCollection();
                sd.recent.AddRange(
                    btn_load.DropDownItems.Cast<ToolStripMenuItem>()
                            .Select(btn => btn.Text).ToArray()
                );
                sd.Save();
            } catch (Exception ex) {

                var dr = new ThreadExceptionDialog(ex).ShowDialog();
                if (dr != System.Windows.Forms.DialogResult.Abort) {
                    e.Cancel = true;
                }
            }
        }

        public class PathSetting : INotifyPropertyChanged {
            public PathSetting() { }
            public PathSetting(string p) {
                this.path = p;
            }
            public string path { get; set; }
            public Image status {
                get {
                    if (this.exception != null) {
                        return FuseboxFreedom.Properties.Resources.error;
                    } else if (processed) {
                        return FuseboxFreedom.Properties.Resources.check2;
                    } else {
                        return FuseboxFreedom.Properties.Resources.empty;
                    }
                }
            }
            public Exception exception;
            public bool processed = false;

            public void Update() {
                var h = PropertyChanged;
                if (h != null) {
                    h(this, new PropertyChangedEventArgs("status"));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            public void Notify(bool processed, Exception ex) {
                this.processed = processed;
                this.exception = ex;
                Update();
            }
        }

        void LoadSettingsFile(string path = null) {
            if (String.IsNullOrWhiteSpace(path)) {
                if (fd_load.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
                    return;
                }
                path = fd_load.FileName;
            }
            LoadSettingsXML(XDocument.Load(path));
            AddRecent(path);
        }

        void LoadSettingsXML(XDocument sdoc) {
            watchblock = true;
            XElement s = sdoc.Root;
            txt_root.Text = s.Attr("root");
            txt_input.Text = s.Attr("input");
            txt_output.Text = s.Attr("output");
            chk_watch.Checked = s.Attr("watch", false);
            var ds = ((BindingList<PathSetting>)dgv_paths.DataSource);
            ds.Clear();
            foreach (string path in s.Element("paths").Elements("path").Select(e => e.Value)) {
                ds.Add(new PathSetting(path));
            }
            watchblock = false;
            if (chk_watch.Checked) {
                Watch();
            }
        }

        void SaveSettingsFile(string path = null) {
            if (String.IsNullOrWhiteSpace(path)) {
                if (fd_save.ShowDialog() != System.Windows.Forms.DialogResult.OK) {
                    return;
                }
                path = fd_save.FileName;
            }
            SaveSettingsXML().Save(path);
            AddRecent(path);
        }


        XDocument SaveSettingsXML() {
            XDocument sdoc = new XDocument();
            XElement s = new XElement((XName)System.Reflection.Assembly.GetAssembly(typeof(GUI)).GetName().Name);
            s.Add(new XAttribute("root", txt_root.Text));
            s.Add(new XAttribute("input", txt_input.Text));
            s.Add(new XAttribute("output", txt_output.Text));
            s.Add(new XAttribute("watch", chk_watch.Checked));
            var ds = ((BindingList<PathSetting>)dgv_paths.DataSource);
            s.Add(new XElement("paths", ds.Select(ps => new XElement("path", ps.path))));
            sdoc.Add(s);
            return sdoc;
        }

        private void btn_load_ButtonClick(object sender, EventArgs e) {
            LoadSettingsFile();
        }

        private void btn_save_Click(object sender, EventArgs e) {
            SaveSettingsFile();
        }

        private void toolStripButton1_Click(object sender, EventArgs e) {
            if (fd_folder.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                txt_root.Text = fd_folder.SelectedPath;
            }
        }

        private IEnumerable<PathSetting> pathSettings {
            get {
                return ((BindingList<PathSetting>)dgv_paths.DataSource);
            }
        }
        private IEnumerable<string> paths {
            get {
                return pathSettings
                    .Select(p => System.IO.Path.Combine(txt_root.Text, p.path));
            }
        }

        Dictionary<FileSystemWatcher, PathSetting> watcherPaths = new Dictionary<FileSystemWatcher, PathSetting>();
        FileSystemWatcher WatchProgress(PathSetting ps) {
            string p = System.IO.Path.Combine(txt_root.Text, ps.path);
            FileSystemWatcher w = Program.Watch(p, txt_input.Text);
            watcherPaths[w] = ps;
            return w;
        }
        private void Watch() {
            Program.input_file = txt_input.Text;
            Program.output_filename = txt_output.Text;
            watcherPaths.Clear();
            Program.watchers = pathSettings.Select(p => WatchProgress(p)).ToArray();
            chk_watch.Checked = true;
        }

        private void Unwatch() {
            if (Program.watchers != null) {
                foreach (var w in Program.watchers) {
                    w.Dispose();
                }
                Program.watchers = null;
                chk_watch.Checked = false;
            }
        }

        private void btn_process_Click(object sender, EventArgs e) {
            IEnumerable<PathSetting> _pathSettings = pathSettings;
            pb_converting.Maximum = _pathSettings.Count();
            pb_converting.Value = 0;
            HideProgress(false);
            tbl.SetProgressState(this.Handle, Windows7.DesktopIntegration.Interop.TBPFLAG.TBPF_NORMAL);
            foreach (PathSetting ps in _pathSettings) {

                tbl.SetProgressValue(this.Handle, (ulong)pb_converting.Value, (ulong)pb_converting.Maximum);
                pb_converting.Value++;
                try {
                    string path = System.IO.Path.Combine(txt_root.Text, ps.path);
                    Program.Convert(path, txt_input.Text, txt_output.Text);
                    ps.exception = null;
                    ps.processed = true;

                } catch (Exception ex) {
                    ps.exception = ex;
                    ps.processed = true;
                }
                ps.Update();
            }
            HideProgress(true);
            pb_converting.Value = pb_converting.Maximum;
        }

        private void txt_root_TextChanged(object sender, EventArgs e) {
            Unwatch();
        }

        private void dgv_paths_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.ColumnIndex > -1) {
                if (dgv_paths.Columns[e.ColumnIndex].DataPropertyName == Status.DataPropertyName) {
                    PathSetting ps = pathSettings.ElementAt(e.RowIndex);
                    if (ps.exception != null) {
                        new ThreadExceptionDialog(ps.exception).Show();
                    }
                }
            }
        }

        bool watchblock = false;
        private void chk_watch_CheckedChanged(object sender, EventArgs e) {
            if (watchblock) {
                return;
            }
            watchblock = true;
            try {
                if (!chk_watch.Checked) {
                    Unwatch();
                } else {
                    Watch();
                }
            } finally {
                watchblock = false;
            }

        }

        private void dgv_paths_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
            Unwatch();
        }

        private void txt_input_TextChanged(object sender, EventArgs e) {
            Unwatch();
        }

        private void txt_output_TextChanged(object sender, EventArgs e) {
            Unwatch();
        }

        private void ProgressHideTimer_Tick(object sender, EventArgs e) {
            ProgressHideTimer.Enabled = false;
            tbl.SetProgressState(this.Handle, TBPFLAG.TBPF_NOPROGRESS);
        }

        void HideProgress(bool hide) {
            ProgressHideTimer.Enabled = false;
            ProgressHideTimer.Enabled = hide;
        }
    }
}
