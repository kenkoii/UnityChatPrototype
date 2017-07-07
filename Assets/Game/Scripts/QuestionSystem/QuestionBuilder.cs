using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PapaParse.Net;
using System.Linq;

public static class QuestionBuilder
{
	private static string questionAnswer;
	private static string questionString;
	private static List<string> questionsDone = new List<string> ();
	private static List<Question> questionList = new List<Question> ();
	private static List<string> wrongChoices = new List<string> ();
	private static List<int> wrongChoicesDone = new List<int> ();
	public static List<Dictionary<string,System.Object>> parsedData = new List<Dictionary<string,System.Object>> ();

	public static void PopulateQuestion (string questionName)
	{
		questionList.Clear ();
		//parsedData = CSVParser.ParseCSV (questionName);
		parsedData = getParsedCSV (questionName);

		if (questionName == "wordchoice" || questionName == "slotmachine") {
			for (int listIndex = 0; listIndex < parsedData.Count - 1; listIndex++) {
				questionList.Add (new Question (parsedData [listIndex] ["Definition"].ToString (),
					parsedData [listIndex] ["Answer"].ToString (), 0));
				wrongChoices.Add(parsedData[listIndex]["Choices"].ToString());
			}
		} else {
			for (int listIndex = 0; listIndex < parsedData.Count - 1; listIndex++) {
				Debug.Log (parsedData [listIndex] ["Definition"].ToString () + "/" +
				parsedData [listIndex] ["Answer"].ToString ());
				questionList.Add (new Question (parsedData [listIndex] ["Definition"].ToString (),
					parsedData [listIndex] ["Answer"].ToString (), 0));
			}
		}
	}

	public static Question GetQuestion ()
	{
		QuestionChecker (true);
		Question questionGot = new Question (questionString, questionAnswer, 0);
		return questionGot;
	}

	private static void QuestionChecker (bool initRandom)
	{
		if (initRandom) {
			QuestionRandomizer ();
			return;
		}
		if (questionsDone.Contains (questionString)) {
			QuestionRandomizer ();
			return;
		}
		questionsDone.Add (questionString);
	}

	public static string GetRandomChoices ()
	{
		int randomnum = UnityEngine.Random.Range (0, wrongChoices.Count);
		while (wrongChoicesDone.Contains (randomnum)) {
			randomnum = UnityEngine.Random.Range (0, wrongChoices.Count);
		}
		string wrongChoice = wrongChoices [randomnum];
		return wrongChoice;
	}

	private static void QuestionRandomizer ()
	{
		int randomize = UnityEngine.Random.Range (0, questionList.Count);
		questionAnswer = questionList [randomize].answer.ToUpper ().ToString ();
		questionString = questionList [randomize].question;
		QuestionChecker (false);
	}

	public static List<Dictionary<string,System.Object>> getParsedCSV (string csv)
	{
		int csvHeaderLines = 1;
		TextAsset csvData = Resources.Load (csv) as TextAsset;
		Result parsed = Papa.parse (csvData.ToString ());
		List<List<string>> rows = parsed.data;
		List<string> csvHeader = new List<string>();
		List<Dictionary<string,System.Object>> csvParsedData = new List<Dictionary<string,System.Object>>();
		int csvLineIndex = 0;

		for (int listIndex = 0; listIndex < rows.Count; listIndex++) {
			csvParsedData.Add(new Dictionary<string,object>());
			for (int subListIndex = 0; subListIndex < rows [listIndex].Count; subListIndex++) {
				if (listIndex < csvHeaderLines) {
					csvHeader.Add (rows [listIndex] [subListIndex]);
				} else {
					//NON HEADER BELOW 
					csvParsedData [csvLineIndex].Add (csvHeader[subListIndex],rows[listIndex][subListIndex]);
					if (subListIndex.Equals (rows [listIndex].Count-1)) {
						csvLineIndex += 1;
					}
				}
			}
		}
		return csvParsedData;
	}
}
