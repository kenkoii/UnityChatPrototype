using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* User Model */
public class User {
	public string gameName;
	public int life;
	public int gp;

	public User() {
	}

	public User(string gameName, int life, int gp) {
		this.gameName = gameName;
		this.life = life;
		this.gp = gp;
	}

	public Dictionary<string, System.Object> ToDictionary() {
		Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
		result ["gameName"] = gameName;
		result ["life"] = life;
		result ["gp"] = gp;

		return result;
	}
}
