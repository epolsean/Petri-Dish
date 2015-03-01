using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BacteriaAI : MonoBehaviour {

    private List<int> openSlots;
    private Collider[] nearbyObjects;

    private float storedFood = 0;

    public float totalLifeTime = 5;
    float lifeTime = 0;

    private float idleTimer = 0;

    private Job move;
    private Job eat;
    private Job rest;
    private Job breed;
    private Job death;

    public GameObject Bact;
    public float growthSpeed = 8;
    public float breedSpeed = 2;

    float storedEnergy = 10;

    public enum State
    {
        IDLE,   // Switches between eating, resting, and moving jobs
        BREED,  // Splits into two identical Bacteria
        DEATH   // Destroys the Bacteria at the end of it's life
    }

    private State _state;
    public State state
    {
        get
        {
            return _state;
        }
        set
        {
            ExitState(_state);
            _state = value;
            EnterState(_state);
        }
    }

    void Awake()
    {
        openSlots = new List<int>();

        GameObject[] AllBact = GameObject.FindGameObjectsWithTag("Bacteria");
        for(int i = 0; i < AllBact.Length; i++)
        {
            if (AllBact[i].transform.position == this.gameObject.transform.position && AllBact[i].gameObject != this.gameObject)
            {
                AllBact[i].GetComponent<BacteriaAI>().ExitState(State.IDLE);
                AllBact[i].GetComponent<BacteriaAI>().ExitState(State.BREED);
                AllBact[i].GetComponent<BacteriaAI>().ExitState(State.DEATH);
                Destroy(AllBact[i].gameObject);
            }
        }
        
    }

    void EnterState(State stateEntered)
    {
        switch (stateEntered)
        {
        case State.IDLE:

            float randomSelect = UnityEngine.Random.Range(0, 2);

            switch ((int)randomSelect)
            {
            case 0:
                move = new Job(Moving(), true);
                break;

            case 1:
                eat = new Job(Eating(), true);
                break;

            case 2:
                rest = new Job(Resting(), true);
                break;
            }

            /*
            if ()
            */

            break;
        case State.BREED:

            breed = new Job(Breeding(), true);

            break;
        case State.DEATH:

            death = new Job(Death(), true);

            break;
        }
    }

    void ExitState(State stateExited)
    {
        switch(stateExited)
        {
        case State.IDLE:

            if (move != null) move.kill();
            if (eat != null) eat.kill();
            if (rest != null) rest.kill();

            break;
        case State.BREED:

            if (breed != null) breed.kill();

            break;
        case State.DEATH:

            if (death != null) death.kill();

            break;
        }
    }

    IEnumerator Moving()
    {
        DetectOpenTiles();

        Collider[] takenTile = Physics.OverlapSphere(this.transform.position, .1f, 1 << 8);
        
        if (openSlots.Count > 0)
        {
            float randomSlot = UnityEngine.Random.Range(0, openSlots.Count);
            this.transform.position = nearbyObjects[openSlots[(int)randomSlot]].transform.position + new Vector3(0, 0.07f, 0);
            nearbyObjects[openSlots[(int)randomSlot]].GetComponent<HexProperties>().tileTaken = true;
        }

        takenTile[0].GetComponent<HexProperties>().tileTaken = false;

        openSlots.Clear();

        storedFood--;

        yield return null;
    }

    IEnumerator Eating()
    {
        storedFood++;

        yield return null;
    }

    IEnumerator Resting()
    {
        yield return null;
    }

    IEnumerator Breeding()
    {
        ExitState(State.IDLE);

        DetectOpenTiles();

        if (openSlots.Count > 0)
        {
            float randomSlot = UnityEngine.Random.Range(0, openSlots.Count);
            Instantiate(Bact, nearbyObjects[openSlots[(int)randomSlot]].transform.position + new Vector3(0, 0.07f, 0), this.transform.rotation);
            nearbyObjects[openSlots[(int)randomSlot]].GetComponent<HexProperties>().tileTaken = true;
        }

        openSlots.Clear();

        yield return null;
    }

    IEnumerator Death()
    {
        Collider[] takenTile = Physics.OverlapSphere(this.transform.position, .1f, 1 << 8);
        takenTile[0].GetComponent<HexProperties>().tileTaken = false;

        ExitState(State.BREED);
        ExitState(State.IDLE);

        Destroy(this.gameObject);

        death.kill();

        yield return null;
    }

	// Update is called once per frame
	void Update () {

        if (totalLifeTime <= lifeTime)
            EnterState(State.DEATH);
        else
        {
            if (idleTimer % growthSpeed == 0)
            {
                if (storedFood >= breedSpeed)
                {
                    EnterState(State.BREED);
                    storedFood = 0;
                }
                else
                    EnterState(State.IDLE);
            }
            idleTimer++;

            lifeTime += Time.deltaTime;
        }
	}

    void DetectOpenTiles()
    {
        nearbyObjects = Physics.OverlapSphere(this.transform.position, .75f, 1 << 8);
        for (int i = 0; i < nearbyObjects.Length; i++)
        {
            if (nearbyObjects[i].GetComponent<HexProperties>().tileTaken == false)
            {
                openSlots.Add(i);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bacteria")
        {
            other.GetComponent<BacteriaAI>().ExitState(State.IDLE);
            other.GetComponent<BacteriaAI>().ExitState(State.BREED);
            other.GetComponent<BacteriaAI>().ExitState(State.DEATH);
            Destroy(other.gameObject);
        }
    }
}
