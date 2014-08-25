using UnityEngine;
using System.Collections;
using System.Linq;

public class CardDataReader : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		// Get all tiles in the database that match the height
		CardInfo[] cardInfos = Database.Instance.GetEntries<CardInfo>().ToArray();
		foreach(CardInfo info in cardInfos)
		{
			Debug.Log ("------ ------" + info.CardNo);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
