using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Question{
	public string question;
	public string answer;
	public string synonym;
	public string antonym;
	public QuestionSystemEnums.QuestionType questionType;
	public QuestionSystemEnums.SelectionType selectionType;

	public Question(string question, string answer ,string synonym ,string antonym,
		QuestionSystemEnums.QuestionType questionType,QuestionSystemEnums.SelectionType selectionType){
		this.question = question;
		this.synonym = synonym;
		this.antonym = antonym;
		this.answer = answer;
		this.questionType = questionType;
		this.selectionType = selectionType;
	}

}
