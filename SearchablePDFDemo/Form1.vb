Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Data
Imports Atalasoft.Imaging
Imports Atalasoft.Ocr
Imports Atalasoft.Ocr.GlyphReader
Imports Atalasoft.Imaging.ImageProcessing
Imports Atalasoft.Imaging.ImageProcessing.Document
Imports Atalasoft.Imaging.Codec
Imports WinDemoHelperMethods.WinDemoHelperMethods

Namespace SearchablePDFDemo
    ''' <summary>
    ''' Summary description for Form1.
    ''' </summary>
    Public Class Form1 : Inherits System.Windows.Forms.Form
        Private WithEvents btnSelectImage As System.Windows.Forms.Button
        Private WithEvents btnProcessImage As System.Windows.Forms.Button
        Private groupBox1 As System.Windows.Forms.GroupBox
        Private lblFilename As System.Windows.Forms.Label
        Private lblProgress As System.Windows.Forms.Label
        Private chkAutoInvertText As System.Windows.Forms.CheckBox
        Private chkFixPageOrientation As System.Windows.Forms.CheckBox
        Private chkCropBorders As System.Windows.Forms.CheckBox
        Private chkRemoveLines As System.Windows.Forms.CheckBox
        ''' <summary>
        ''' Required designer variable.
        ''' </summary>
        Private components As System.ComponentModel.Container = Nothing

        Private _filename As String
        Private _engine As GlyphReaderEngine
        Private _pdfTrans As PdfTranslator
        Private openFileDialog1 As System.Windows.Forms.OpenFileDialog
        Private saveFileDialog1 As System.Windows.Forms.SaveFileDialog
        Friend WithEvents BtnAbout As System.Windows.Forms.Button
        Private _pageNum As Integer
        Shared Sub New()
            'load the OCR resources
            Dim loader As GlyphReaderLoader = New GlyphReaderLoader()

            HelperMethods.PopulateDecoders(RegisteredDecoders.Decoders)

        End Sub
        Public Sub New()
            If CheckLicenseFile() Then
                InitializeComponent()
                _engine = New GlyphReaderEngine
                _pdfTrans = New PdfTranslator
                _engine.Translators.Add(_pdfTrans)
                AddHandler _engine.PageProgress, AddressOf _engine_PageProgress
                AddHandler _engine.DocumentProgress, AddressOf _engine_DocumentProgress
                AddHandler _engine.ImageTransformation, AddressOf _engine_ImageTransformation
                AddHandler _engine.ImageSendOff, AddressOf _engine_ImageSendOff
            End If
        End Sub

        ''' <summary>
        ''' Clean up any resources being used.
        ''' </summary>
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not components Is Nothing Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

#Region "Windows Form Designer generated code"
        ''' <summary>
        ''' Required method for Designer support - do not modify
        ''' the contents of this method with the code editor.
        ''' </summary>
        Private Sub InitializeComponent()
            Me.btnSelectImage = New System.Windows.Forms.Button
            Me.btnProcessImage = New System.Windows.Forms.Button
            Me.groupBox1 = New System.Windows.Forms.GroupBox
            Me.chkRemoveLines = New System.Windows.Forms.CheckBox
            Me.chkCropBorders = New System.Windows.Forms.CheckBox
            Me.chkFixPageOrientation = New System.Windows.Forms.CheckBox
            Me.chkAutoInvertText = New System.Windows.Forms.CheckBox
            Me.lblFilename = New System.Windows.Forms.Label
            Me.lblProgress = New System.Windows.Forms.Label
            Me.openFileDialog1 = New System.Windows.Forms.OpenFileDialog
            Me.saveFileDialog1 = New System.Windows.Forms.SaveFileDialog
            Me.BtnAbout = New System.Windows.Forms.Button
            Me.groupBox1.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnSelectImage
            '
            Me.btnSelectImage.Location = New System.Drawing.Point(8, 16)
            Me.btnSelectImage.Name = "btnSelectImage"
            Me.btnSelectImage.Size = New System.Drawing.Size(80, 23)
            Me.btnSelectImage.TabIndex = 0
            Me.btnSelectImage.Text = "Select Image"
            '
            'btnProcessImage
            '
            Me.btnProcessImage.Location = New System.Drawing.Point(8, 184)
            Me.btnProcessImage.Name = "btnProcessImage"
            Me.btnProcessImage.Size = New System.Drawing.Size(152, 23)
            Me.btnProcessImage.TabIndex = 1
            Me.btnProcessImage.Text = "Generate Searchable PDF"
            '
            'groupBox1
            '
            Me.groupBox1.Controls.Add(Me.chkRemoveLines)
            Me.groupBox1.Controls.Add(Me.chkCropBorders)
            Me.groupBox1.Controls.Add(Me.chkFixPageOrientation)
            Me.groupBox1.Controls.Add(Me.chkAutoInvertText)
            Me.groupBox1.Location = New System.Drawing.Point(8, 48)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(272, 128)
            Me.groupBox1.TabIndex = 2
            Me.groupBox1.TabStop = False
            Me.groupBox1.Text = "Pre-processing Options"
            '
            'chkRemoveLines
            '
            Me.chkRemoveLines.Location = New System.Drawing.Point(16, 96)
            Me.chkRemoveLines.Name = "chkRemoveLines"
            Me.chkRemoveLines.Size = New System.Drawing.Size(104, 24)
            Me.chkRemoveLines.TabIndex = 3
            Me.chkRemoveLines.Text = "Remove Lines"
            '
            'chkCropBorders
            '
            Me.chkCropBorders.Location = New System.Drawing.Point(16, 72)
            Me.chkCropBorders.Name = "chkCropBorders"
            Me.chkCropBorders.Size = New System.Drawing.Size(104, 24)
            Me.chkCropBorders.TabIndex = 2
            Me.chkCropBorders.Text = "Crop Borders"
            '
            'chkFixPageOrientation
            '
            Me.chkFixPageOrientation.Checked = True
            Me.chkFixPageOrientation.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkFixPageOrientation.Location = New System.Drawing.Point(16, 48)
            Me.chkFixPageOrientation.Name = "chkFixPageOrientation"
            Me.chkFixPageOrientation.Size = New System.Drawing.Size(128, 24)
            Me.chkFixPageOrientation.TabIndex = 1
            Me.chkFixPageOrientation.Text = "Fix Page Orientation"
            '
            'chkAutoInvertText
            '
            Me.chkAutoInvertText.Location = New System.Drawing.Point(16, 24)
            Me.chkAutoInvertText.Name = "chkAutoInvertText"
            Me.chkAutoInvertText.Size = New System.Drawing.Size(104, 24)
            Me.chkAutoInvertText.TabIndex = 0
            Me.chkAutoInvertText.Text = "Auto Invert Text"
            '
            'lblFilename
            '
            Me.lblFilename.AutoSize = True
            Me.lblFilename.Location = New System.Drawing.Point(96, 24)
            Me.lblFilename.Name = "lblFilename"
            Me.lblFilename.Size = New System.Drawing.Size(37, 13)
            Me.lblFilename.TabIndex = 3
            Me.lblFilename.Text = "(none)"
            '
            'lblProgress
            '
            Me.lblProgress.AutoSize = True
            Me.lblProgress.Location = New System.Drawing.Point(8, 216)
            Me.lblProgress.Name = "lblProgress"
            Me.lblProgress.Size = New System.Drawing.Size(51, 13)
            Me.lblProgress.TabIndex = 4
            Me.lblProgress.Text = "Progress:"
            '
            'saveFileDialog1
            '
            Me.saveFileDialog1.DefaultExt = "pdf"
            Me.saveFileDialog1.Filter = "Searchable PDF|*.pdf"
            '
            'BtnAbout
            '
            Me.BtnAbout.Location = New System.Drawing.Point(205, 184)
            Me.BtnAbout.Name = "BtnAbout"
            Me.BtnAbout.Size = New System.Drawing.Size(75, 23)
            Me.BtnAbout.TabIndex = 5
            Me.BtnAbout.Text = "About ..."
            Me.BtnAbout.UseVisualStyleBackColor = True
            '
            'Form1
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 238)
            Me.Controls.Add(Me.BtnAbout)
            Me.Controls.Add(Me.lblProgress)
            Me.Controls.Add(Me.lblFilename)
            Me.Controls.Add(Me.groupBox1)
            Me.Controls.Add(Me.btnProcessImage)
            Me.Controls.Add(Me.btnSelectImage)
            Me.Name = "Form1"
            Me.Text = "Image to Searchable PDF"
            Me.groupBox1.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
#End Region

        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread()> _
        Shared Sub Main()
            Application.Run(New Form1)
        End Sub

        Private Sub _engine_ImageTransformation(ByVal sender As Object, ByVal e As OcrImagePreprocessingEventArgs)
            'this is where we do things to the image that we want to be viewed in the searchable PDF
            'but change the size or coordinates of the image
            If chkCropBorders.Checked Then
                If e.ImageIn.PixelFormat = PixelFormat.Pixel1bppIndexed Then
                    'only apply if source image is bitonal
                    Dim ab As AdvancedBorderRemovalCommand = New AdvancedBorderRemovalCommand
                    Dim res As ImageResults = ab.Apply(e.ImageIn)
                    e.ImageOut = res.Image
                End If
            End If
        End Sub

        Private Sub _engine_ImageSendOff(ByVal sender As Object, ByVal e As OcrImagePreprocessingEventArgs)
            Dim image As AtalaImage = e.ImageIn
            If e.ImageIn.PixelFormat <> PixelFormat.Pixel1bppIndexed Then
                'threshold the image here (OCR engine will automatically threshold otherwise)
                Dim threshold As AdaptiveThresholdCommand = New AdaptiveThresholdCommand
                Dim res As ImageResults = threshold.Apply(image)
                image = res.Image

            End If
            'this is where we cleanup the document that is OCR'ed but not included in the searchable PDF
            If chkAutoInvertText.Checked Then

                Dim at As AutoInvertTextCommand = New AutoInvertTextCommand
                Dim res As ImageResults = at.Apply(image)
                If (Not res.IsImageSourceImage) AndAlso Not image Is e.ImageIn Then
                    image.Dispose()
                End If
                image = res.Image
            End If

            'this is where we cleanup the document that is OCR'ed but not included in the searchable PDF
            If chkRemoveLines.Checked Then
                Dim at As LineRemovalCommand = New LineRemovalCommand
                Dim res As ImageResults = at.Apply(image)
                If (Not res.IsImageSourceImage) AndAlso Not image Is e.ImageIn Then
                    image.Dispose()
                End If
                image = res.Image
            End If
            If Not image Is e.ImageIn Then
                e.ImageOut = image
            End If
        End Sub

        Private Sub _engine_DocumentProgress(ByVal sender As Object, ByVal e As OcrDocumentProgressEventArgs)
            If e.Stage = OcrDocumentStage.BeginPage Then
                _pageNum += 1 ' increment the current page count
            End If
        End Sub

        Private Sub _engine_PageProgress(ByVal sender As Object, ByVal e As OcrPageProgressEventArgs)
            lblProgress.Text = e.Stage.ToString() & " Page " & _pageNum & ": " & e.Progress & "%..."
            lblProgress.Refresh()
        End Sub

        Private Sub btnSelectImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelectImage.Click
            Me.openFileDialog1.Filter = HelperMethods.CreateDialogFilter(True)

            If Me.openFileDialog1.ShowDialog(Me) = DialogResult.OK Then
                _filename = openFileDialog1.FileName
                lblFilename.Text = _filename
            End If
        End Sub

        Private Sub btnProcessImage_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnProcessImage.Click
            saveFileDialog1.Filter = "Portable Document Format (PDF)|*.pdf"

            If saveFileDialog1.ShowDialog(Me) = DialogResult.OK Then
                _engine.Initialize()
                Try
                    _pageNum = 0
                    Me.Cursor = Cursors.WaitCursor
                    _engine.PreprocessingOptions.AutoRotate = chkFixPageOrientation.Checked
                    _pdfTrans.AutoPageRotation = chkFixPageOrientation.Checked
                    Dim fs As FileSystemImageSource = New FileSystemImageSource(New String() {_filename}, True)
                    _engine.Translate(fs, "application/pdf", saveFileDialog1.FileName)
                    System.Diagnostics.Process.Start(saveFileDialog1.FileName)
                Catch ex As Exception
                    MessageBox.Show(Me, ex.ToString())
                Finally
                    _engine.ShutDown()
                    Me.Cursor = Cursors.Default
                End Try
            End If
        End Sub

#Region "Check for license code"

        Private Function CheckGRLicense() As Boolean
            Try
                Dim gr As GlyphReaderEngine = New GlyphReaderEngine   ' does not throw
                gr.Initialize() ' will throw on no license
                gr.Dispose()
                Return True
            Catch e1 As AtalasoftLicenseException
                Return False
            End Try
        End Function

        Private Function CheckLicenseFile() As Boolean
            ' Make sure a license for DotImage and OCR exist.
            Try
                Dim img As AtalaImage = New AtalaImage
                img.Dispose()
            Catch ex1 As Atalasoft.Imaging.AtalasoftLicenseException
                LicenseCheckFailure(ex1.Message)
                Return False
            End Try

            If AtalaImage.Edition <> LicenseEdition.Document Then
                LicenseCheckFailure("This demo requires a Document Imaging License." & Constants.vbCrLf & "Your current license is for '" & AtalaImage.Edition.ToString() & "'.")
                Return False
            End If

            Try
                Dim t As TranslatorCollection = New TranslatorCollection
            Catch e1 As AtalasoftLicenseException
                LicenseCheckFailure("This demo requires an OCR license.")
                Return False
            End Try

            If CheckGRLicense() Then
                Return True
            Else
                LicenseCheckFailure("GlyphReader is not licensed on your system.  Please request an evaluation license for it before running this demo.")
                Return False
            End If
        End Function

        Private Sub LicenseCheckFailure(ByVal message As String)
            AddHandler Load, AddressOf LoadFailure
            If MessageBox.Show(Me, message & Constants.vbCrLf & Constants.vbCrLf & "Would you like to request an evaluation license?", "License Required", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
                Dim asm As System.Reflection.Assembly = System.Reflection.Assembly.Load("Atalasoft.dotImage")
                If Not asm Is Nothing Then
                    Dim version As String = asm.GetName().Version.ToString(2)

                    ' Locate the activation utility.
                    Dim path As String = ""
                    Dim key As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("Software\Atalasoft\dotImage\" & version)
                    If Not key Is Nothing Then
                        path = Convert.ToString(key.GetValue("AssemblyBasePath"))
                        If Not path Is Nothing AndAlso path.Length > 5 Then
                            path = path.Substring(0, path.Length - 3) & "AtalasoftToolkitActivation.exe"
                        Else
                            path = System.IO.Path.GetFullPath("..\..\..\..\..\AtalasoftToolkitActivation.exe")
                        End If

                        key.Close()
                    End If

                    If System.IO.File.Exists(path) Then
                        System.Diagnostics.Process.Start(path)
                    Else
                        MessageBox.Show(Me, "We were unable to location the DotImage activation utility." & Constants.vbCrLf & "Please run it from the Start menu shortcut.", "File Not Found")
                    End If
                Else
                    MessageBox.Show(Me, "Unable to load the DotImage assembly.", "Load Error")
                End If
            End If
        End Sub

        Private Sub LoadFailure(ByVal sender As Object, ByVal e As EventArgs)
            Application.Exit()
        End Sub
#End Region

        Private Sub BtnAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnAbout.Click
            Dim aboutBox As AtalaDemos.AboutBox.About = New AtalaDemos.AboutBox.About("About Atalasoft Searchable PDF Demo", "Searchable PDF Demo")
            aboutBox.Description = "This demo uses our OCR engine to convert an input image (single or multi-page) into a searchable PDF using a GlyphReaderEngine and our PdfTranslator class." & vbCrLf & vbCrLf & _
                                   "Pre-Processing options (deskewing, border removal, text inversion and line removal) are also provided." & vbCrLf & vbCrLf & _
                                   "This winforms application is fairly bare-bones, but the concepts covered can easily be applied to your console app, windows service, web service, or can even be used server-side in your ASP.NET or Silverlight web application."
            aboutBox.ShowDialog()
        End Sub
    End Class
End Namespace
