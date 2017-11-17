using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompareHeight : MonoBehaviour {

	public Text text;
	public GameObject a;
	public GameObject b;

	public Board board;

	protected Vector3 diff = Vector3.zero;

	protected float length = 4f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (b.transform.position.magnitude > 0.2f) {

			diff = a.transform.position - (b.transform.position - new Vector3(0, 0.01f, 0));
			length = diff.magnitude;

			text.text = length.ToString () + " , Active";
		} else {
			length = 4f;
			text.text = length.ToString() + " , Not active";
		}

		length = length < 2f ? 2f : length;
		length = length > 6f ? 6f : length;
		board.height = length - 4.0f;
	}
}
