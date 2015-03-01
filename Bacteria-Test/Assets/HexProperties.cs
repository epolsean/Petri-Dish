using UnityEngine;
using System.Collections;

public class HexProperties : MonoBehaviour {

    private GameObject highlightedTile;
    public bool tileTaken;

    public GameObject Bact;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if ((Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor) && Input.GetMouseButtonUp(0) && highlightedTile != null && tileTaken == false)
        {
            Instantiate(Bact, highlightedTile.transform.position + new Vector3(0, 0.07f, 0), Bact.transform.rotation);
            tileTaken = true;
        }
        else if(Application.platform == RuntimePlatform.Android && Input.GetTouch(0).phase == TouchPhase.Ended && highlightedTile != null && tileTaken == false)
        {
            Instantiate(Bact, highlightedTile.transform.position + new Vector3(0,0.07f,0), Bact.transform.rotation);
            tileTaken = true;
        }
        else if (Application.platform == RuntimePlatform.Android && Input.GetKeyUp(KeyCode.LeftControl) && highlightedTile != null && tileTaken == false)
        {
            Instantiate(Bact, highlightedTile.transform.position + new Vector3(0, 0.07f, 0), Bact.transform.rotation);
            tileTaken = true;
        }

        //if(Event.current.mousePosition. == )
	}

    void OnMouseOver()
    {
        highlightedTile = this.gameObject;
    }

    void OnMouseExit()
    {
        highlightedTile = null;
    }
}
