using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PopupManager : MonoBehaviour {
	public int num = 0;
	public int nextNum = 1;
	public List<Sprite> Toys;

	private static PopupManager instance = null;
	public static PopupManager Instance
	{
		get
		{
			return instance;
		}
	}
	private void Awake(){
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		instance = this;
		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (num == nextNum) {
			// Show a random toy.
			int i = Random.Range(0, Toys.Count);
			var a = new GameObject();
			a.AddComponent(typeof(SpriteRenderer));
			a.AddComponent(typeof(PolygonCollider2D));
			a.AddComponent(typeof(DragableObj));

			a.GetComponent<SpriteRenderer>().sprite = Toys[i];
			a.transform.position = new Vector3(Random.Range(-10f, 10f), Random.Range(5.2f, -3.6f), 0f);
			float randSize = Random.Range(0.8f, 1.2f);
			a.transform.localScale = new Vector3(randSize, randSize, 1f);


			// Renew
			nextNum++;
			num = 0;
		}
		if (num > nextNum) {
			num = 0;
		}
	}
}
