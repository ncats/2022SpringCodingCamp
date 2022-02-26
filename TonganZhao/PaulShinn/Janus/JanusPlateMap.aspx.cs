using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PlateLibrary;
using CoMaWebApplication;
using PlateLibrary.Compounds;
using CompoundStore;
using DevExpress.Spreadsheet;
using FOTSLibrary;
using CoMaWebApplication.AppCode;
using Instruments;

public partial class JanusPlateMap : System.Web.UI.Page, ISearchPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.InputTextBox.Height = ASPxSpreadsheet1.Height = 550;
        Master.InputTextBox.Width = 400;
        if (!IsPostBack)
        {
            string orderID = Request.QueryString["StoreOrderID"];
            Master.InputTextBox.Text = Helper.GetStoreOrderToString(orderID);

            cboDilutionFactor.DataSource = JanusJob.DilutionPoints;
            cboDilutionFactor.DataBind();
            cboDilutionFactor.SelectedIndex = 1;
            cboInstrument.DataSource = JanusJob.Instruments;
            cboInstrument.DataBind();
            LoadWellNumbers();
        }
    }

    private void LoadWellNumbers()
    {
        int dilutionFactor = Convert.ToInt16(cboDilutionFactor.Value);
        JanusJob.TransferType transferType = cboTransferType.Text == "Dilutions" ? JanusJob.TransferType.Dilution : JanusJob.TransferType._1to1;
        cboStartingWell.DataSource = JanusJob.GetWells(transferType, dilutionFactor);        
        cboStartingWell.DataBind();
        cboStartingWell.SelectedIndex = 0;
    }  
   
    public int Submit(List<string> items)
    {
        ASPxSpreadsheet1.CreateSheet("Sheet1");
        ASPxSpreadsheet1.RemoveSheets(new string[] { "Janus", "Platemap", "FX" });       
         
        JanusJob.TransferType transferType = cboTransferType.Text == "Dilutions" ? JanusJob.TransferType.Dilution : JanusJob.TransferType._1to1;

        List<Plate> sourcePlates = ExtensionMethods.ParseStoreOrderPlates(items.ToListArray());
        int transferVolume = Convert.ToInt16(txtTransferVol.Text);
        int dilutionFactor = Convert.ToInt16(cboDilutionFactor.Value);
        string instrument = cboInstrument.Text;

        JanusJob job = new JanusJob(sourcePlates, transferType, instrument, dilutionFactor, transferVolume, cboStartingWell.Text, chkSortPlatemap.Checked);
        ASPxSpreadsheet1.WriteDataTable(job.InstrumentTable);
        
        ASPxSpreadsheet1.WriteDataTable(job.PlatemapTable, false);
        ASPxSpreadsheet1.RemoveSheet("Sheet1");
        string filename = "Janus Plate Map " + DateTime.Now.ToString("MM-dd-yy-hhmmss") + ".xls";
        ASPxSpreadsheet1.DownloadSpreadsheet(Page, filename);
        return 0;
    }

    protected void cboStartingWell_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
    {
        LoadWellNumbers();
    }
}