using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* User Model */
public class User {
	public string username;
	public int life;
	public int gp;

	public User() {
	}

	public User(string username, int life, int gp) {
		this.username = username;
		this.life = life;
		this.gp = gp;
	}

	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result["username"] = username;
		result ["life"] = life;
		result ["gp"] = gp;

		return result;
	}
}
