﻿using System;
using UnityEngine;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using System.Threading;

public class MainController : MonoBehaviour {
	static Animator anim;

	void Start () {}
	
	public void TranslateController(){
		InputField inputText = GameObject.Find("/Canvas/InputText").GetComponent<InputField>();
		Debug.Log("Input text: " + inputText.text);
		// Sending HTTP Request
		StartCoroutine(getTranslate(inputText.text.Replace(' ', '@')));
	}

	private IEnumerator getTranslate(String text){
		string request = "http://0.0.0.0:8080/translate/"+ text;
		Debug.Log(request);
    	WWW req = new WWW(request);
    	yield return req;
		// HTTP response recieved
		InputField translateText = GameObject.Find("/Canvas/TranslateText").GetComponent<InputField>();
		translateText.text = req.text;
		anim = this.transform.Find("Aj").gameObject.GetComponent<Animator>();
		//anim = GetComponent<Animator>();
		string[] tokens = req.text.ToLower().Split(' ');
		StartCoroutine(waitedAnimate(tokens));
	}
	
	IEnumerator waitedAnimate(string[] tokens) {
		Text displayWord = GameObject.Find("/Canvas/DisplayWord").GetComponent<Text>();
		for (var i=0; i<tokens.Length;++i){
			if (tokens[i].Equals(".") || tokens[i].Equals("question") || tokens[i].Equals("neutral")
				|| tokens[i].Equals("q") || tokens[i].Equals("exclamation")) continue;
			Debug.Log("Setting Trig: " + tokens[i]);
			displayWord.text = tokens[i];
			anim.SetTrigger(tokens[i]);
			yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
		 }
	}

}	