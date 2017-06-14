using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class SelectLetterIcon : MonoBehaviour, IQuestion {
	public string questionString; 
	private Action<int,int> onResult;
	private string questionData = "";
	private string answerData = "";
	public string questionAnswer; 
	private static int answerindex = 1;
	//private int selectionlistcount = 13;ß∫
	private GameObject[] answerlist = new GameObject[13];
	private GameObject[] selectionlist = new GameObject[13];
	public static List<Question> questionlist;
	public List<string> answerIdentifier;
	private static List<string> questionsDone = new List<string>();
	public string answerwrote;
	public float timeDuration;
	private static int round = 1;
	private int letterno;
	//Properties
	public string QuestionAnswer{
		get{ 
			return questionAnswer;
		}
	}
	public List<string> QuestionsDone{
		get{ return questionsDone; 
		}
		set{ questionsDone = value;
		}
	}
	public void Activate(GameObject entity,float timeduration,Action<int,int> Result){
		round = 1;
		answerindex = 1;
		SelectLetterEvent sle = new SelectLetterEvent ();
		sle.Currentround = 1;
		sle.CorrectAnswers = 0;
		NextRound (round);
		QuestionController qc = new QuestionController ();
		qc.OnResult = Result;
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="round">Round.</param>
	public void NextRound(int round){
		if (round == 1) {
			answerindex = 1;
		}
	//		Debug.Log (GetCSV("https://docs.google.com/spreadsheets/d/19cKJ0YqMbNQWQmW_ZuEj4h9AHeyC_-H899MRE1F3rkw/edit#gid=0"));
		questionlist = new List<Question> ();

		PopulateQuestionList ();
		int randomize = UnityEngine.Random.Range (0, questionlist.Count);
		questionAnswer = questionlist [randomize].answer.ToUpper().ToString();
		questionString = questionlist [randomize].question;
		while (questionsDone.Contains (questionString)) {
			randomize = UnityEngine.Random.Range (0, questionlist.Count);
			questionAnswer = questionlist [randomize].answer.ToUpper().ToString();
			questionString = questionlist [randomize].question;
			if (!questionsDone.Contains (questionString)) {
				break;
				}
			} 
		questionsDone.Add (questionString);
		GameObject questionInput = Resources.Load ("Prefabs/inputContainer") as GameObject;
		GameObject questionModal = GameObject.Find("SelectLetterIconModal");
		for (int i = 0; i < questionAnswer.Length; i++) {
			GameObject input = Instantiate (questionInput) as GameObject; 
			input.transform.SetParent (questionModal.transform.GetChild (1).
				transform.GetChild (0).GetChild (0).transform, false);
			input.name = "input" + (i + 1);
			input.GetComponent<Button>().onClick.AddListener (() => {
				GameObject.Find("SelectLetterIcon").GetComponent<SelectLetterEvent>().AnswerOnClick();
			});
			answerlist [i] = input;
			input.transform.GetChild (0).GetComponent<Text> ().text = "";
		}
		ShuffleAlgo ();
		SelectLetterEvent sle = new SelectLetterEvent ();
		sle.GetAnswer (questionlist[randomize].answer);
		questionModal.transform.GetChild (0).GetComponent<Text> ().text = questionString;
	
	}
	public void PopulateQuestionList(){

		CSVParser cs = new CSVParser ();
		List<string> databundle = cs.GetQuestions ("wingquestion");
		int i = 0;
		foreach(string questions in databundle ){
		string[] splitter = databundle[i].Split (']');	

			questionData = splitter [0];
			answerData = splitter [1];
			//if ((i % 2)==0) {
				questionlist.Add (new Question (questionData, answerData, 0));

			//}

			i+=1;
		}


	}

	public void ShuffleAlgo ()
	{
		int[] RandomExist = new int[questionAnswer.Length];
		string temp = questionAnswer;

		letterno = 0;
		int randomnum = 0;      
		for (int z = 0; z < temp.Length; z++) {
			randomnum = UnityEngine.Random.Range (1, selectionlist.Length);        
			RandomExist [letterno] = randomnum;
			while (true) {
				int index = Array.IndexOf (RandomExist, randomnum);
				if (index > -1) {
					randomnum = UnityEngine.Random.Range (1, selectionlist.Length);
				} else {
					break;
				}
			}
			for (int i = 0; i < selectionlist.Length; i++) {
				if (randomnum == i) {
					
					GameObject.Find ("Letter" + i).GetComponent<Image> ().
					transform.GetChild (0).GetComponent<Text> ().text = temp [letterno].ToString ().ToUpper ();       
				}			
			}
			RandomExist [letterno] = randomnum;
			letterno = letterno + 1;

		}

		for (int f = 1; f < 13; f++) {
			string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			int randomnum2 = UnityEngine.Random.Range (1, 26);
			int index = Array.IndexOf (RandomExist, f);
			if (index > -1) {

			} else {
				GameObject.Find ("Letter" + f).GetComponent<Image> ().transform.GetChild (0).GetComponent<Text> ().text = alphabet [randomnum2].ToString ().ToUpper ();
			}
		}

	}
}
