using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIScript : MonoBehaviour {

    public GameObject Bact;
    float startTime = 0;

    public float growthSpeed = 8;
    public float breedSpeed = 2;
    public float lifeTime = 5;
    public bool Party;
    Color PartyColor;
    Color currentColor;

    float currentGrowthSpeed;
    float currentBreedSpeed;
    float currentLifeTime;
    bool currentMode;

	// Use this for initialization
	void Start () {
        currentGrowthSpeed = growthSpeed;
        currentBreedSpeed = breedSpeed;
        currentLifeTime = lifeTime;
        PartyColor = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
        if (startTime + .01 < Time.time && startTime != 0)
            ClearAll();

        if(currentGrowthSpeed != growthSpeed)
        {
            GameObject[] AllBact = GameObject.FindGameObjectsWithTag("Bacteria");
            for (int i = 0; i < AllBact.Length; i++)
            {
                AllBact[i].GetComponent<BacteriaAI>().growthSpeed = growthSpeed;
            }
            Bact.GetComponent<BacteriaAI>().growthSpeed = growthSpeed;
            currentGrowthSpeed = growthSpeed;
        }

        if (currentBreedSpeed != breedSpeed)
        {
            GameObject[] AllBact = GameObject.FindGameObjectsWithTag("Bacteria");
            for (int i = 0; i < AllBact.Length; i++)
            {
                AllBact[i].GetComponent<BacteriaAI>().breedSpeed = breedSpeed;
            }
            Bact.GetComponent<BacteriaAI>().breedSpeed = breedSpeed;
            currentBreedSpeed = breedSpeed;
        }

        if (currentLifeTime != lifeTime)
        {
            GameObject[] AllBact = GameObject.FindGameObjectsWithTag("Bacteria");
            for (int i = 0; i < AllBact.Length; i++)
            {
                AllBact[i].GetComponent<BacteriaAI>().totalLifeTime = lifeTime;
            }
            Bact.GetComponent<BacteriaAI>().totalLifeTime = lifeTime;
            currentLifeTime = lifeTime;
        }

        //Bact.renderer.sharedMaterial.color = Color.Lerp(Color.red, Color.green, Time.deltaTime * 2);

        //print("( " + Mathf.Sin(Mathf.PI * (Time.time / 10)) / 1.25f + ", " + Mathf.Sin(Mathf.PI * (Time.time / 10) + 2 * (Mathf.PI / 3)) / 1.25f + ", " + Mathf.Sin(Mathf.PI * (Time.time / 10) + 4 * (Mathf.PI / 3)) / 1.25f + ")");
        if(Party && currentMode ==  false)
        {
            //Bact.renderer.sharedMaterial.color = new Color(Mathf.Sin(Mathf.PI * (Time.time / 3)) * 1.5f - .5f, Mathf.Sin(Mathf.PI * (Time.time / 3) + 2 * (Mathf.PI / 3)) * 1.5f - .5f, Mathf.Sin(Mathf.PI * (Time.time / 3) + 4 * (Mathf.PI / 3)) * 1.5f - .5f);
            //ColorChanger();
            //Bact.renderer.material.color = PartyColor;
            //print(10 * Time.deltaTime);
            GameObject[] AllBact = GameObject.FindGameObjectsWithTag("Bacteria");
            for (int i = 0; i < AllBact.Length; i++)
            {
                AllBact[i].renderer.material.color = new Color(Random.Range(.1f, 1), Random.Range(.1f, 1), Random.Range(.1f, 1));
            }
            Bact.renderer.material.color = new Color(Random.Range(.1f, 1), Random.Range(.1f, 1), Random.Range(.1f, 1));
            currentMode = true;
        }
        else if(Party == false && currentMode)
        {

            GameObject[] AllBact = GameObject.FindGameObjectsWithTag("Bacteria");
            for (int i = 0; i < AllBact.Length; i++)
            {
                AllBact[i].renderer.material.color = new Color(1, 1, 1);
            }
            Bact.renderer.material.color = new Color(1,1,1);
            currentMode = false;
        }
	}

    public void ClearAll()
    {
        GameObject[] AllBact = GameObject.FindGameObjectsWithTag("Bacteria");
        GameObject[] AllHex = GameObject.FindGameObjectsWithTag("HexTile");

        for (int i = 0; i < AllHex.Length; i++)
        {
            AllHex[i].GetComponent<HexProperties>().tileTaken = false;
        }

        for (int i = 0; i < AllBact.Length; i++)
        {
            Destroy(AllBact[i]);
        }

        if(startTime + .01 < Time.time && startTime != 0)
        {
            startTime = 0;
        }
        else
        {
            startTime = Time.time;
        }
    }

    public void GrowthSpeed()
    {
        growthSpeed = GameObject.Find("Growth Speed Slider").GetComponent<Slider>().value*7+8;
    }

    public void BreedingFrequency()
    {
        breedSpeed = GameObject.Find("Breeding Frequency Slider").GetComponent<Slider>().value*2+2;
    }

    public void LifeTime()
    {
        lifeTime = GameObject.Find("Total Life Time Slider").GetComponent<Slider>().value;
    }

    public void PartyTime()
    {
        Party = GameObject.Find("Party Toggle").GetComponent<Toggle>().isOn;
    }

    void ColorChanger()
    {
        Color colorA = Color.red;
        Color colorB = Color.green;
        Color colorC = Color.blue;

        if (PartyColor == colorA)
        {
            currentColor = colorB;
        }
        else if (PartyColor == colorB)
        {
            currentColor = colorC;
        }
        else if (PartyColor == colorC)
        {
            currentColor = colorA;
        }

        PartyColor = Color.Lerp(PartyColor, currentColor, Time.deltaTime * .25f);
    }
}
