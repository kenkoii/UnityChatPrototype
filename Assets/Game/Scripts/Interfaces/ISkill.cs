using System;
using UnityEngine;

public interface ISkill {

	void Activate (GameObject entity);

	void SetSkill(Action<string, int> skillParam);

}
