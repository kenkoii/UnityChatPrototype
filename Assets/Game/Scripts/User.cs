using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class User {
	public string username;
	public string message;
	public long timeStamp;

	public User() {
	}

	public User(string username, string message, long timeStamp) {
		this.username = username;
		this.message = message;
		this.timeStamp = timeStamp;
	}

	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["username"] = username;
		result["message"] = message;
		result["timestamp"] = timeStamp;

		return result;
	}
}
