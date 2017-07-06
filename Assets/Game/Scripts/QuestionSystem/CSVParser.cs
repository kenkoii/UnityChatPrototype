using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CSVParser {
	public static string csvObject = "";
	public static List<string> csvHeader = new List<string>();
	private static string csvLine = "";
	public static List<string> csvGotData = new List<string> ();

	public static List<string> ParseCSV(string csv){
		char fieldSeperator = ',';
		char lineSeperater = '\n';
		int csvHeaderLines = 1;
		TextAsset csvData = Resources.Load (csv) as TextAsset;
		string[] csvDataBundle = csvData.text.Split (lineSeperater);
		for (int rowIndex = 0; rowIndex < csvDataBundle.Length; rowIndex++) {
			string[] fields = csvDataBundle[rowIndex].Split (fieldSeperator);
			for (int columnIndex = 0; columnIndex < fields.Length; columnIndex++) {
				if (rowIndex < csvHeaderLines) {
					csvHeader.Add(fields[columnIndex]);
				} else {
					csvObject = columnIndex == fields.Length-1? fields [columnIndex] : fields [columnIndex]+",";
					csvLine = columnIndex == fields.Length-1 ? "":csvLine + csvHeader [rowIndex] + ":" + csvObject;
					Debug.Log (csvHeader [columnIndex] + ":" + csvObject);
					//csvLine = "";
					csvObject = "";
					//csvGotData.Add (csvHeader[rowIndex]+":"+csvObject);

				}
			}
		}
		return csvGotData;
	}

}
