using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String : MonoBehaviour {
    public Color c1; 
    public Transform StartPoint;
	public Transform EndPoint;
	public Transform StarPoint;
	public AudioClip Sound1;
	public AudioClip Sound2;
	public GameObject NotesParticle;

    private Vector3 TouchPos;
    private float ReleaseDistance;
	private bool isStarSelected = false;
    const float ReleaseThreshold = 1f;

    private bool isHoldingString = false;

    // Use this for initialization
    void Start () { 
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Self-Illumin/VertexLit")); 
		lineRenderer.SetWidth(0.1f, 0.1f);

        lineRenderer.SetColors(c1, c1);
        lineRenderer.material.color = c1;
    }
	
	// Update is called once per frame
	void Update () {
		LineRenderer lineRenderer = GetComponent<LineRenderer>(); 

        // No interaction points
        var notouchPoints = new Vector3[2];
        notouchPoints[0] = StartPoint.position;
        notouchPoints[1] = EndPoint.position;


        var v3 = Input.mousePosition;
        TouchPos = Camera.main.ScreenToWorldPoint(v3);
        TouchPos.z = 0f; 

		if (Input.GetMouseButton(0)) {
			ReleaseDistance = Mathf.Abs(TouchPos.y - StartPoint.position.y) / ReleaseThreshold;
			TouchPos.y = TouchPos.y + Random.Range(-ReleaseDistance*0.05f, ReleaseDistance*0.05f);
			
			if(Vector3.Distance(StarPoint.position, TouchPos) < 0.3f && !isHoldingString ){
				if(Input.GetMouseButtonDown(0)){
					if(!isStarSelected){
						isStarSelected = true;
						StarPoint.transform.localScale = new Vector3(0.25f, 0.25f, StarPoint.transform.localScale.z);
					}else{
						isStarSelected = false;
						StarPoint.transform.localScale = new Vector3(0.17f, 0.17f, StarPoint.transform.localScale.z);
					}
				}
			} else if (isTouchString(TouchPos) || isHoldingString) {
                isHoldingString = true;
				if(!isStarSelected){
					var touchPoints = new Vector3[3];
                	touchPoints[0] = StartPoint.position;
					touchPoints[1] = TouchPos;
					touchPoints[2] = EndPoint.position;
					
					lineRenderer.SetVertexCount(3);
					
					if (isGrabString(TouchPos))
					{
                        lineRenderer.SetPositions(touchPoints);
					}
					else
					{
						// Out of range sound
						isHoldingString = false;
						AudioSource audi = transform.GetComponent<AudioSource>();
						audi.volume = ReleaseDistance;
						if(!isStarSelected){
							GetComponent<AudioSource>().clip = Sound1;
						}else{
							GetComponent<AudioSource>().clip = Sound2;
						}
						audi.Play();
						PopupManager.Instance.num++;
						if(NotesParticle != null){
							var par = Instantiate(NotesParticle, TouchPos + new Vector3(0f, 0f, -2f), Quaternion.identity);
							Destroy(par, 1.0f);
						}
					}
				} else{
					var touchPoints = new Vector3[4];
					touchPoints[0] = StartPoint.position;
					touchPoints[1] = StarPoint.position;
					touchPoints[2] = TouchPos;
					touchPoints[3] = EndPoint.position;
					
					lineRenderer.SetVertexCount(4);
					
					if (isGrabString(TouchPos))
					{
					    lineRenderer.SetPositions(touchPoints);
					}
					else
					{
						// Out of range sound
						isHoldingString = false;
						AudioSource audi = transform.GetComponent<AudioSource>();
						audi.volume = ReleaseDistance;
						if(!isStarSelected){
							GetComponent<AudioSource>().clip = Sound1;
						}else{
							GetComponent<AudioSource>().clip = Sound2;
						}
						audi.Play();
						PopupManager.Instance.num++;
						if(NotesParticle != null){
							var par = Instantiate(NotesParticle, TouchPos + new Vector3(0f, 0f, -2f), Quaternion.identity);
							Destroy(par, 1.0f);
						}
					}
				}
            }
            // Out of range autorelease refresh line.
            else
            {
                lineRenderer.SetVertexCount(2);
				
				lineRenderer.SetPositions(notouchPoints);
            }
        }
        // Release mouse sound
        else if (Input.GetMouseButtonUp(0) && isHoldingString)
        {
            isHoldingString = false;
            AudioSource audi = transform.GetComponent<AudioSource>();
            audi.volume = ReleaseDistance;
			
			if(!isStarSelected){
				GetComponent<AudioSource>().clip = Sound1;
			}else{
				GetComponent<AudioSource>().clip = Sound2;
			}
			audi.Play();
			PopupManager.Instance.num++;
			if(NotesParticle != null){
				var par = Instantiate(NotesParticle, TouchPos + new Vector3(0f, 0f, -2f), Quaternion.identity);
				Destroy(par, 1.0f);
			}
		}
		else
		{
			lineRenderer.SetVertexCount(2);
            lineRenderer.SetPositions(notouchPoints);
        }
    }

    private bool isTouchString(Vector3 TouchPos)
    {
		if (!isStarSelected) {
			if (TouchPos.x > StartPoint.position.x && TouchPos.x < EndPoint.position.x && TouchPos.y < StartPoint.position.y + 0.2f && TouchPos.y > StartPoint.position.y - 0.2f) {
				return true;
			}
		} else {
			
			if (TouchPos.x > StarPoint.position.x && TouchPos.x < EndPoint.position.x && TouchPos.y < StarPoint.position.y + 0.2f && TouchPos.y > StarPoint.position.y - 0.2f) {
				return true;
			}
		}

        return false;
    }

    private bool isGrabString(Vector3 TouchPos)
    {
		if (!isStarSelected) {
			if (TouchPos.x > StartPoint.position.x && TouchPos.x < EndPoint.position.x && TouchPos.y < StartPoint.position.y + 1f && TouchPos.y > StartPoint.position.y - 1f) {
				return true;
			}
		} else {
			if (TouchPos.x > StarPoint.position.x && TouchPos.x < EndPoint.position.x && TouchPos.y < StarPoint.position.y + 1f && TouchPos.y > StarPoint.position.y - 1f) {
				return true;
			}
		}

        return false;
    }
}
