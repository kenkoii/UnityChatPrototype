using System.Collections.Generic;
public interface IRPCDicObserver
{
	void OnNotify (Dictionary<string, System.Object> dicValue);
}

