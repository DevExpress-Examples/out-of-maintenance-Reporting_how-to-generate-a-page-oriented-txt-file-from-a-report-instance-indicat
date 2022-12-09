using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Native;
using DevExpress.XtraReports.UI;
using System.Diagnostics;
using DevExpress.Drawing;

namespace WindowsFormsApplication1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            XtraReport1 report = new XtraReport1();
            report.CreateDocument();

            using (StreamWriter writer = File.CreateText("FINALOUTPUT.txt")) {
                foreach (Page page in report.Pages) {
                    PrintingSystemBase ps = new PrintingSystemBase();
                    ps.Begin();
                    ps.Graph.PageUnit = DXGraphicsUnit.Document;
                    ps.Graph.Modifier = BrickModifier.Detail;
                    foreach (BrickBase brick in ((CompositeBrick)page.InnerBricks[0]).InnerBricks)
                        if (brick is Brick) {
                            Brick newBrick = brick.Clone() as Brick;
                            newBrick.PrintingSystem = ps;
                            ps.Graph.DrawBrick((Brick)brick);
                        }

                    ps.End();

                    MemoryStream ms = new MemoryStream();
                    ps.ExportToText(ms, new TextExportOptions() { TextExportMode = TextExportMode.Text, Separator = " ", Encoding = Encoding.UTF8 });

                    ms.Position = 0;

                    using (StreamReader reader = new StreamReader(ms)) {
                        writer.Write(reader.ReadToEnd());
                    }

                    writer.Write("----------------PAGE BREAK----------------" + Environment.NewLine);
                }
            }

            Process.Start("FINALOUTPUT.txt");
        }
    }
}
