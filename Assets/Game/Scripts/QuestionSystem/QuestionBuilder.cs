using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestionBuilder {
	private static string questionData;
	private static string answerData;
	private static string replacedAsset;
	private static string questionAnswer;
	private static string questionString;
	private static List<string> questionsDone = new List<string> ();
	private static List<Question> questionList = new List<Question>();
	private static List<string> wrongChoices = new List<string> ();
	private static List<int> wrongChoicesDone = new List<int> ();

	public static void PopulateQuestion(string questionName){
		List<string> databundle = new List<string>();
		questionList.Clear ();

		switch(questionName){
		case "SelectChangeTyping":
			databundle = CSVParser (questionName);
			int i = 0;
			foreach (string questions in databundle) {
				string[] splitter = databundle [i].Split (']');	
				string questionData = splitter [0];
				string answerData = splitter [1];
				i += 1;
				questionList.Add (new Question (questionData, answerData, 0));
			}
			break;
		case "wordchoice":
			databundle = CSVParser ("wordchoice");
			int j = 0;
			int randomnum = UnityEngine.Random.Range (1, 3);
			foreach (string questions in databundle) {
				string[] splitQuestion = databundle [j].Split (']');	

				questionData = splitQuestion [0];	
				string synonymData = splitQuestion [1];
				string antonymData = splitQuestion [2];
				wrongChoices.Add (splitQuestion [3]);
				if (questionData.Length > 1) {
					switch (randomnum) {
					case 1:
						questionList.Add (new Question (questionData, synonymData, 3));
						break;
					case 2:
						questionList.Add (new Question (questionData, antonymData, 3));
						break;
					}
				}
				j += 1;
			}
			break;
		case "slotmachine":
			 databundle = CSVParser ("slotmachine");
			 i = 0;
			 randomnum = UnityEngine.Random.Range (1, 3);
			foreach(string questions in databundle ){
				string[] splitter = databundle [i].Split (']');
				questionData = splitter [0];
				string synonymData = splitter [1];
				string antonymData = splitter [2];
				antonymData.Remove (antonymData.Length - 1);
				if (questionData.Length > 1) {
					switch (randomnum) {
					case 1:
						questionList.Add (new Question (questionData, synonymData, 3));
						break;
					case 2:
						questionList.Add (new Question (questionData, antonymData, 3));
						break;
					}
				}
				i+=1;
			}
			break;
		}
	}

	public static Question GetQuestion(){
		QuestionChecker (true);
		Question questionGot = new Question(questionString,questionAnswer,0);
		return questionGot;
	}

	private static void QuestionChecker(bool initRandom){
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

	public static string GetRandomChoices(){
		int randomnum = UnityEngine.Random.Range (0,wrongChoices.Count);
		while (wrongChoicesDone.Contains (randomnum)) {
			randomnum = UnityEngine.Random.Range (0, wrongChoices.Count);
		}
		string wrongChoice = wrongChoices [randomnum];
		return wrongChoice;
	}

	private static void QuestionRandomizer(){
		int randomize = UnityEngine.Random.Range (0, questionList.Count);
		questionAnswer = questionList [randomize].answer.ToUpper ().ToString ();
		questionString = questionList [randomize].question;
		QuestionChecker (false);
	}
		
	public static List<string> CSVParser(string resource){
		char lineSeperater = '\n'; // It defines line seperate character
		char fieldSeperator = ',';
		TextAsset csvFile;

		int index = 0;
		int fieldindexer = 0;

		csvFile = Resources.Load (resource) as TextAsset;
		List<string> questions = new List<string> ();
		switch (resource) {
		case "SelectChangeTyping":

			string[] records = csvFile.text.Split (lineSeperater);
			foreach (string record in records) {

				index += 1;
				if (index > 5) {
					string replacee = "";
					//char[] charhelper = new char[record.Length];
					int numberOfComma = 0;
					foreach (char c in record) {
						if (c == ',') {
							numberOfComma += 1;
						}
					}
					string[] fields = record.Split (fieldSeperator);
					if (numberOfComma > 3) {
						numberOfComma = numberOfComma - 3;
						int j = 0;
						foreach (char c in record) {
							if (c == ',') {
								if (numberOfComma == 0) {
									replacee = replacee + c;
								} else {
									replacee = replacee + ']';
									numberOfComma -= 1;
								}


							} else {
								replacee = replacee + c;
							}


							j++;
						}
						fields = replacee.Split (fieldSeperator);
					} 
					foreach (string field in fields) {
						fieldindexer = fieldindexer + 1;

						switch (fieldindexer) {
						case 1:
							foreach (char c in field) {
								if (c == ']') {
									questionData = questionData + ',';
								} else {
									questionData = questionData + c;
								}
							}
							break;
						case 2:
							answerData = field;
							break;
						case 3:
							questions.Add (questionData + "]" + answerData);
							questionData = "";
							break;
						default:
							if (fieldindexer == 4) {
								fieldindexer = 0;
							}
							break;
						}


					}
				}
			}
			break;

		case "wordchoice":
			string[] wordChoiceRecord = csvFile.text.Split (lineSeperater);
			foreach (string record in wordChoiceRecord) {
				string[] fields = record.Split (fieldSeperator);
				index += 1;
				if (index > 1) {
					foreach (string field in fields) {
						fieldindexer += 1;
						switch (fieldindexer) {
						case 1:
							questionData = field;
							break;
						case 2:
							answerData = field;
							break;
						case 3:
							answerData = answerData + "]" + field;
							break;
						case 4:
							answerData = answerData + "]" + field;
							questions.Add (questionData + "]" + answerData);
							questionData = "";
							fieldindexer = 0;
							break;
						}
					}
				}

			}
			break;
		case "slotmachine":
			string[] slotMachineRecord = csvFile.text.Split (lineSeperater);
			foreach (string record in slotMachineRecord) {
				string[] fields = record.Split (fieldSeperator);
				index += 1;
				if (index > 1 && index<27) {
					foreach (string field in fields) {
						fieldindexer += 1;
						switch (fieldindexer) {
						case 1:
							questionData = field;
							break;
						case 2:
							answerData = field;
							break;
						case 3:
							field.TrimEnd (' ');

							string removeSpace = field.Remove(field.Length - 1);
							answerData = answerData + "]" + removeSpace;

							questions.Add (questionData + "]" + answerData);
							questionData = "";
							fieldindexer = 0;
							break;
						}
					}
				}

			}
			break;
		}
		return questions;
	}

}
