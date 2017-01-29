using UnityEngine;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class Requests : MonoBehaviour {
	private ScrollableList newList;
	private Text namePanel;
	private Text intersectPanel;
	private Text ETAPanel;
	private Text LLocation;
	private Text RLocation;
	private Text LDistance;
	private Text RDistance;

	//Timer
	int startTime = 0;
	int restSeconds = 0;
	int countDownSeconds = 0;
	bool started;

	void Awake(){
		startTime = (int)Time.time;
	}

	void Start () {
		namePanel = GameObject.Find ("RouteNamePanel").GetComponent<Text>();
		intersectPanel = GameObject.Find ("IntersectionPanel").GetComponent<Text>();
		ETAPanel = GameObject.Find ("ETAPanel").GetComponent<Text>();

		LLocation = GameObject.Find ("LLocation").GetComponent<Text>();
		LDistance = GameObject.Find ("LDistance").GetComponent<Text>();

		RLocation = GameObject.Find ("RLocation").GetComponent<Text>();
		RDistance = GameObject.Find ("RDistance").GetComponent<Text>();

		newList = FindObjectOfType<ScrollableList>();
		//MakeRequest ("Stop1");
	}

	void Update(){
		if (started){
			Timer ();
		}
	}
	void Timer(){
		restSeconds = countDownSeconds - ((int)Time.time - startTime);
		//display messages or whatever here --&gt;do stuff based on your timer
		if (restSeconds == 60) {
			print ("One Minute Left");
		}
		if (restSeconds == 0) {
			print ("Time is Over");
			//do stuff here
		}
		int roundedRestSeconds = Mathf.CeilToInt(restSeconds);
		int displaySeconds = roundedRestSeconds % 60;
		int displayMinutes = roundedRestSeconds / 60; 
		string text = String.Format ("{0:00}:{1:00}", displayMinutes, displaySeconds); 

		ETAPanel.text = "ETA: " + text;
	}

	public void MakeRequest(string newReq) {
		started = true;
		countDownSeconds = (int)UnityEngine.Random.Range (250, 600);
		int parseInt = 0;
		parseInt = int.Parse(Regex.Replace(newReq, "[^0-9]+", string.Empty));

		WWWForm form = new WWWForm();
		var headers = form.headers;
		headers["Content-Type"]="application/json";

		string url = "https://enigmatic-peak-90131.herokuapp.com/api/stops/"+ parseInt;

		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
	}

	IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		// check for errors 
		if (www.error == null)
		{
			//Debug.Log("WWW Ok!: " + www.text);
			int count = 0; 
			List<string> myTimes = new List<string> ();

			var currStop = JObject.Parse(www.text);
			string name = (string)currStop ["name"];
			string atS = (string)currStop ["at_street"];
			string onS = (string)currStop ["on_street"];
			var nbLocation = currStop ["nearby"];

			var times = currStop["times_weekday"];

			foreach (var entry in times) {
				count++;
				myTimes.Add ((string)entry["time"]);
			}
			foreach (var entry in nbLocation){
				if ((bool)entry ["left"]) {
					LLocation.text = "Location: " + (string)entry["name"];
					LDistance.text = "Distance: " + (string)entry["distance"] + "km";
				} else {
					RLocation.text = "Location: " + (string)entry["name"];
					RDistance.text = "Distance: " + (string)entry["distance"] + "km";
				}
			}

			namePanel.text = "Route: "+name;
			intersectPanel.text = "Intersection: "+onS+" and "+atS;
			newList.BuildList (count, myTimes);

		} else {
			Debug.Log("WWW Error: "+ www.error);
		} 
	} 
	public int getSecondsLeft(int hours,int minutes ,int seconds) {
		//Create Desired time
		DateTime target = new DateTime(0,0,0,hours,minutes,seconds);

		//Get the current time
		DateTime now = System.DateTime.Now;

		//Convert both to seconds
		int targetMin = target.Hour * 60 * 60 + target.Minute*60 + target.Second;
		int nowMin = now.Hour * 60 * 60 + now.Minute * 60 + now.Second;

		//Get the difference in seconds
		int diff = targetMin - nowMin;

		return diff;
	}
}
