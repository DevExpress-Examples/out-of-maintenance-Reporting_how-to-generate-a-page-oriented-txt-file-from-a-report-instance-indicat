Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports DevExpress.XtraPrinting
Imports System.Diagnostics

Namespace WindowsFormsApplication1

    Public Partial Class Form1
        Inherits Form

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub button1_Click(ByVal sender As Object, ByVal e As EventArgs)
            Dim report As XtraReport1 = New XtraReport1()
            report.CreateDocument()
            Using writer As StreamWriter = File.CreateText("FINALOUTPUT.txt")
                For Each page As Page In report.Pages
                    Dim ps As PrintingSystem = New PrintingSystem()
                    ps.Begin()
                    ps.Graph.PageUnit = GraphicsUnit.Document
                    ps.Graph.Modifier = BrickModifier.Detail
                    For Each brick As BrickBase In CType(page.InnerBricks(0), CompositeBrick).InnerBricks
                        If TypeOf brick Is Brick Then
                            Dim newBrick As Brick = TryCast(brick.Clone(), Brick)
                            newBrick.PrintingSystem = ps
                            ps.Graph.DrawBrick(CType(brick, Brick))
                        End If
                    Next

                    ps.End()
                    Dim ms As MemoryStream = New MemoryStream()
                    ps.ExportToText(ms, New TextExportOptions() With {.TextExportMode = TextExportMode.Text, .Separator = " "})
                    ms.Position = 0
                    Using reader As StreamReader = New StreamReader(ms)
                        writer.Write(reader.ReadToEnd())
                    End Using

                    writer.Write("----------------PAGE BREAK----------------" & Environment.NewLine)
                Next
            End Using

            Call Process.Start("FINALOUTPUT.txt")
        End Sub
    End Class
End Namespace
