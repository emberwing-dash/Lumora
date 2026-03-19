using UnityEngine;
using TMPro;
using System.Collections;

public class NewtonSimulate2 : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI displayText;
    public float typingSpeed = 0.03f;

    [Header("Seesaw Setup")]
    public GameObject seesaw;

    public GameObject carObj;
    public GameObject truckObj;

    public Rigidbody carRB;
    public Rigidbody truckRB;

    public Transform carDropPoint;
    public Transform truckDropPoint;

    [Header("Race Setup")]
    public GameObject raceObjects;
    public Rigidbody car;
    public Rigidbody truck;

    public Transform carStart;
    public Transform truckStart;

    public float carSpeed = 10f;
    public float truckSpeed = 5f;

    private int stage = 0;
    private int lastStage = -1;
    private Coroutine typingCoroutine;

    void Start()
    {
        seesaw.SetActive(true);

        carObj.SetActive(false);
        truckObj.SetActive(false);
        raceObjects.SetActive(false);

        StartTyping("Car and Truck\n\nPress button to begin");
    }

    void Update()
    {
        if (stage == lastStage) return;
        lastStage = stage;

        // 🚗 Stage 1 → Car drop
        if (stage == 1)
        {
            StartTyping("Car is lighter\n\nWatch what happens");

            DropCar();
        }

        // 🚛 Stage 2 → Truck drop
        else if (stage == 2)
        {
            StartTyping("Truck is heavier\n\nObserve the difference");

            DropTruck();
        }

        // 🏁 Stage 3 → Race setup
        else if (stage == 3)
        {
            seesaw.SetActive(false);
            raceObjects.SetActive(true);

            SetupRace();

            StartTyping("Now observe motion\n\nCar vs Truck");
        }

        // 🏎️ Stage 4 → Race
        else if (stage == 4)
        {
            StartTyping("Car accelerates faster\n\nIt reaches first");

            StartRace();
        }
    }

    // 🚗 Drop Car
    void DropCar()
    {
        carObj.SetActive(true);

        carObj.transform.position = carDropPoint.position;

        carRB.linearVelocity = Vector3.zero;
        carRB.angularVelocity = Vector3.zero;

        carRB.isKinematic = false;
        carRB.useGravity = true;
    }

    // 🚛 Drop Truck
    void DropTruck()
    {
        truckObj.SetActive(true);

        truckObj.transform.position = truckDropPoint.position;

        truckRB.linearVelocity = Vector3.zero;
        truckRB.angularVelocity = Vector3.zero;

        truckRB.isKinematic = false;
        truckRB.useGravity = true;
    }

    // 🏁 Setup Race
    void SetupRace()
    {
        car.position = carStart.position;
        truck.position = truckStart.position;

        car.linearVelocity = Vector3.zero;
        truck.linearVelocity = Vector3.zero;

        car.isKinematic = false;
        truck.isKinematic = false;
    }

    // 🏎️ Start Race
    void StartRace()
    {
        car.linearVelocity = Vector3.forward * carSpeed;
        truck.linearVelocity = Vector3.forward * truckSpeed;
    }

    // ✨ TYPEWRITER
    void StartTyping(string msg)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(msg));
    }

    IEnumerator TypeText(string msg)
    {
        displayText.text = "";

        foreach (char c in msg)
        {
            displayText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void OnButtonPressed()
    {
        stage++;
    }
}