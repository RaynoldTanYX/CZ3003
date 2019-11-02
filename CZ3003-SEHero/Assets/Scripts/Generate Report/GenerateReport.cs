using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using sharpPDF;
using sharpPDF.Enumerators;
using SimpleJSON;

public class GenerateReport : MonoBehaviour
{
    internal string attacName = "StudentReport.pdf";

    public string jsonURL;

    // Use this for initialization
    IEnumerator Start()
    {
        //yield return StartCoroutine(CreatePDF(jsonURL));
        Debug.Log("Processing Data, Please Wait");

        WWW _www = new WWW(jsonURL);
        yield return _www;
        if (_www.error == null)
        {
            CreatePDF(jsonURL);
        }
        else
        {
            Debug.Log("Opps something went wrong.");
        }

    }

    // Update is called once per frame
    public IEnumerator CreatePDF(string jsonURL)
    {
        pdfDocument myDoc = new pdfDocument("Student Report", "Me", false);
        pdfPage myFirstPage = myDoc.addPage();

        // Create PDF
        // Debug.Log ( "Continue to create PDF");
        myFirstPage.addText("Student Report", 10, 730, predefinedFont.csHelveticaOblique, 30, new pdfColor(predefinedColor.csOrange));

        /*Table's creation*/
        pdfTable myTable = new pdfTable();
        //Set table's border
        myTable.borderSize = 1;
        myTable.borderColor = new pdfColor(predefinedColor.csDarkBlue);

        /*Add Columns to a grid*/
        myTable.tableHeader.addColumn(new pdfTableColumn("world_id", predefinedAlignment.csRight, 120));
        myTable.tableHeader.addColumn(new pdfTableColumn("level_id", predefinedAlignment.csCenter, 120));
        myTable.tableHeader.addColumn(new pdfTableColumn("score", predefinedAlignment.csLeft, 150));

        // Extract Data
        ScoresData ScoreObject = new ScoresData();
        ScoreObject = JsonUtility.FromJson<ScoresData>(jsonURL);
        Debug.Log(ScoreObject.scores);

        foreach (ScoreList x in ScoreObject.scores)
        {
            if (x.username.Equals("a"))
            {
                Debug.Log(x.username);
                Debug.Log(x.level_id);
                Debug.Log(x.world_id);

                pdfTableRow myRow = myTable.createRow();
                myRow[0].columnValue = x.world_id.ToString();
                myRow[1].columnValue = x.level_id.ToString();
                myRow[2].columnValue = x.score.ToString();
                myTable.addRow(myRow);
                myRow = null;
            }
        }

        /*Set Header's Style*/
        myTable.tableHeaderStyle = new pdfTableRowStyle(predefinedFont.csCourierBoldOblique, 12, new pdfColor(predefinedColor.csBlack), new pdfColor(predefinedColor.csLightOrange));
        /*Set Row's Style*/
        myTable.rowStyle = new pdfTableRowStyle(predefinedFont.csCourier, 8, new pdfColor(predefinedColor.csBlack), new pdfColor(predefinedColor.csWhite));
        /*Set Alternate Row's Style*/
        myTable.alternateRowStyle = new pdfTableRowStyle(predefinedFont.csCourier, 8, new pdfColor(predefinedColor.csBlack), new pdfColor(predefinedColor.csLightYellow));
        /*Set Cellpadding*/
        myTable.cellpadding = 10;
        /*Put the table on the page object*/
        myFirstPage.addTable(myTable, 5, 700);

        myDoc.createPDF(attacName);
        myTable = null;

        yield return null;
    }
}
