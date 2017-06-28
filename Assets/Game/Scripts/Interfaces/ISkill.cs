using System;
using UnityEngine;

public interface ISkill {

	void Activate ();

	void SetSkill(Action<string,string, int> skillParam);

}
