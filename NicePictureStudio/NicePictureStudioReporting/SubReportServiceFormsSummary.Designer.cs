namespace NicePictureStudioReporting
{
    partial class SubReportServiceFormsSummary
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.InstanceReportSource instanceReportSource1 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.InstanceReportSource instanceReportSource2 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.InstanceReportSource instanceReportSource3 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.InstanceReportSource instanceReportSource4 = new Telerik.Reporting.InstanceReportSource();
            Telerik.Reporting.InstanceReportSource instanceReportSource5 = new Telerik.Reporting.InstanceReportSource();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SubReportServiceFormsSummary));
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector1 = new Telerik.Reporting.Drawing.DescendantSelector();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector2 = new Telerik.Reporting.Drawing.DescendantSelector();
            this.subReportPHServices1 = new NicePictureStudioReporting.SubReportPHServices();
            this.subReportEquipmentServices1 = new NicePictureStudioReporting.SubReportEquipmentServices();
            this.subReportOutputServices1 = new NicePictureStudioReporting.SubReportOutputServices();
            this.subReportLocationServices1 = new NicePictureStudioReporting.SubReportLocationServices();
            this.subReportOutsource1 = new NicePictureStudioReporting.SubReportOutsource();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.subPhotoGraphReport = new Telerik.Reporting.SubReport();
            this.subEquipmentReport = new Telerik.Reporting.SubReport();
            this.subOutputReport = new Telerik.Reporting.SubReport();
            this.subLocationReport = new Telerik.Reporting.SubReport();
            this.subOutsourceReport = new Telerik.Reporting.SubReport();
            this.shape1 = new Telerik.Reporting.Shape();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.textBox4 = new Telerik.Reporting.TextBox();
            this.shape2 = new Telerik.Reporting.Shape();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this.subReportPHServices1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subReportEquipmentServices1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subReportOutputServices1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subReportLocationServices1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subReportOutsource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // subReportPHServices1
            // 
            this.subReportPHServices1.Name = "SubReportPHServices";
            // 
            // subReportEquipmentServices1
            // 
            this.subReportEquipmentServices1.Name = "SubReportEquipmentServices";
            // 
            // subReportOutputServices1
            // 
            this.subReportOutputServices1.Name = "SubReportOutputServices";
            // 
            // subReportLocationServices1
            // 
            this.subReportLocationServices1.Name = "SubReportLocationServices";
            // 
            // subReportOutsource1
            // 
            this.subReportOutsource1.Name = "SubReportOutsource";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(3.5D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox1,
            this.textBox2,
            this.subPhotoGraphReport,
            this.subEquipmentReport,
            this.subOutputReport,
            this.subLocationReport,
            this.subOutsourceReport,
            this.shape1,
            this.textBox3,
            this.textBox4,
            this.shape2});
            this.detail.Name = "detail";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000078558921814D), Telerik.Reporting.Drawing.Unit.Inch(3.9418537198798731E-05D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2000000476837158D), Telerik.Reporting.Drawing.Unit.Inch(0.20000004768371582D));
            this.textBox1.Value = "=Fields.Id";
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(2.5D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9000002145767212D), Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D));
            this.textBox2.Value = "=Fields.ServiceNetPrice";
            // 
            // subPhotoGraphReport
            // 
            this.subPhotoGraphReport.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D), Telerik.Reporting.Drawing.Unit.Inch(0.60000008344650269D));
            this.subPhotoGraphReport.Name = "subPhotoGraphReport";
            instanceReportSource1.Parameters.Add(new Telerik.Reporting.Parameter("ServiceFormId", "=Fields.Id"));
            instanceReportSource1.ReportDocument = this.subReportPHServices1;
            this.subPhotoGraphReport.ReportSource = instanceReportSource1;
            this.subPhotoGraphReport.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.3000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            // 
            // subEquipmentReport
            // 
            this.subEquipmentReport.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D), Telerik.Reporting.Drawing.Unit.Inch(0.90007895231246948D));
            this.subEquipmentReport.Name = "subEquipmentReport";
            instanceReportSource2.Parameters.Add(new Telerik.Reporting.Parameter("ServiceFormId", "=Fields.Id"));
            instanceReportSource2.ReportDocument = this.subReportEquipmentServices1;
            this.subEquipmentReport.ReportSource = instanceReportSource2;
            this.subEquipmentReport.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.3000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            // 
            // subOutputReport
            // 
            this.subOutputReport.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D), Telerik.Reporting.Drawing.Unit.Inch(1.2001577615737915D));
            this.subOutputReport.Name = "subOutputReport";
            instanceReportSource3.Parameters.Add(new Telerik.Reporting.Parameter("ServiceFormId", "=Fields.Id"));
            instanceReportSource3.ReportDocument = this.subReportOutputServices1;
            this.subOutputReport.ReportSource = instanceReportSource3;
            this.subOutputReport.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.3000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            // 
            // subLocationReport
            // 
            this.subLocationReport.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D), Telerik.Reporting.Drawing.Unit.Inch(1.8003154993057251D));
            this.subLocationReport.Name = "subLocationReport";
            instanceReportSource4.Parameters.Add(new Telerik.Reporting.Parameter("ServiceFormId", "=Fields.Id"));
            instanceReportSource4.ReportDocument = this.subReportLocationServices1;
            this.subLocationReport.ReportSource = instanceReportSource4;
            this.subLocationReport.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.3000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            // 
            // subOutsourceReport
            // 
            this.subOutsourceReport.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D), Telerik.Reporting.Drawing.Unit.Inch(1.5002366304397583D));
            this.subOutsourceReport.Name = "subOutsourceReport";
            instanceReportSource5.Parameters.Add(new Telerik.Reporting.Parameter("ServiceFormId", "=Fields.Id"));
            instanceReportSource5.ReportDocument = this.subReportOutsource1;
            this.subOutsourceReport.ReportSource = instanceReportSource5;
            this.subOutsourceReport.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.3000001907348633D), Telerik.Reporting.Drawing.Unit.Inch(0.30000004172325134D));
            // 
            // shape1
            // 
            this.shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.29999995231628418D), Telerik.Reporting.Drawing.Unit.Inch(2.1003944873809814D));
            this.shape1.Name = "shape1";
            this.shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.2999997138977051D), Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D));
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.6000001430511475D), Telerik.Reporting.Drawing.Unit.Inch(2.5D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.69999980926513672D), Telerik.Reporting.Drawing.Unit.Inch(0.20000012218952179D));
            this.textBox3.Value = "ราคารวม :";
            // 
            // textBox4
            // 
            this.textBox4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D), Telerik.Reporting.Drawing.Unit.Inch(3.9339065551757812E-05D));
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.69999980926513672D), Telerik.Reporting.Drawing.Unit.Inch(0.20000012218952179D));
            this.textBox4.Value = "ประเภทการบริการ : ";
            // 
            // shape2
            // 
            this.shape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D), Telerik.Reporting.Drawing.Unit.Inch(0.20011813938617706D));
            this.shape2.Name = "shape2";
            this.shape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            this.shape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(5.2999997138977051D), Telerik.Reporting.Drawing.Unit.Inch(0.299999862909317D));
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "NicePictureStudioReporting.Properties.Settings.NicePictureStudioDB";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@ServicesId", System.Data.DbType.String, "=Parameters.ServicesId.Value")});
            this.sqlDataSource1.SelectCommand = resources.GetString("sqlDataSource1.SelectCommand");
            // 
            // SubReportServiceFormsSummary
            // 
            this.DataSource = this.sqlDataSource1;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "SubReportServiceFormsSummary";
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(1D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            reportParameter1.Name = "ServicesId";
            reportParameter1.Text = "ServicesId";
            reportParameter1.Value = "9";
            this.ReportParameters.Add(reportParameter1);
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.Table), "Normal.TableNormal")});
            styleRule2.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule2.Style.Color = System.Drawing.Color.Black;
            styleRule2.Style.Font.Name = "Tahoma";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableHeader")});
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector1});
            styleRule3.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule3.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule3.Style.Font.Name = "Tahoma";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            descendantSelector2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Normal.TableBody")});
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector2});
            styleRule4.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule4.Style.Font.Name = "Tahoma";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Inch(6D);
            ((System.ComponentModel.ISupportInitialize)(this.subReportPHServices1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subReportEquipmentServices1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subReportOutputServices1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subReportLocationServices1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subReportOutsource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.SubReport subPhotoGraphReport;
        private Telerik.Reporting.SubReport subEquipmentReport;
        private Telerik.Reporting.SubReport subOutputReport;
        private Telerik.Reporting.SubReport subLocationReport;
        private Telerik.Reporting.SubReport subOutsourceReport;
        private SubReportPHServices subReportPHServices1;
        private SubReportEquipmentServices subReportEquipmentServices1;
        private SubReportOutputServices subReportOutputServices1;
        private SubReportLocationServices subReportLocationServices1;
        private SubReportOutsource subReportOutsource1;
        private Telerik.Reporting.Shape shape1;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.TextBox textBox4;
        private Telerik.Reporting.Shape shape2;
    }
}