using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVParser {
	private string questionData;
	private string answerData;

	public List<string> GetQuestions(){
		char lineSeperater = '\n'; // It defines line seperate character
		char fieldSeperator = ',';
		TextAsset csvFile;
		int index = 0;
		int fieldindexer = 0;
		csvFile = Resources.Load ("wingquestion") as TextAsset;
		List<string> questions = new List<string>();
		string[] records = csvFile.text.Split (lineSeperater);
		foreach (string record in records) {
			string[] fields = record.Split (fieldSeperator);


			index += 1;
			if (index > 4) {
				foreach (string field in fields) {

					fieldindexer = fieldindexer + 1;
					switch (fieldindexer) {
					case 1:
						questionData = field;
						break;
					case 2:
						answerData = field;
						break;
					case 3:
						questions.Add (questionData + "]" + answerData);
						break;
					default:
						if (fieldindexer == 5) {
							fieldindexer = 0;
						}
						break;
					}


				}
			}
		}
		return questions;
	}

}
