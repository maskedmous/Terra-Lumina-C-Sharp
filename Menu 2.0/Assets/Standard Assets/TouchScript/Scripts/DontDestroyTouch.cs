using UnityEngine;
using System.Collections;

public class DontDestroyTouch : MonoBehaviour
{
	public void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
