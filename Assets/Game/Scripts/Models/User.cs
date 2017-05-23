using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* User Model */
public class User {
	public string username;
	public int life;

	public User() {
	}

	public User(string username, int life) {
		this.username = username;
		this.life = life;
	}

	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["username"] = username;
		result ["life"] = life;

		return result;
	}
}
