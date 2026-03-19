using UnityEngine;
using TMPro;
using System.Collections;

public class NewtonSimulate : MonoBehaviour
{
    [Header("Select Law")]
    public bool firstLaw;
    public bool secondLaw;
    public bool thirdLaw;

    [Header("Objects")]
    public Rigidbody obj1;
    public Rigidbody obj2;

    [Header("VR Hands")]
    public GameObject leftHand;
    public GameObject rightHand;

    [Header("UI")]
    public TextMeshProUGUI displayText;

    [Header("Settings")]
    public float motionSpeed = 2f;
    public float pushForce = 5f;
    public float typingSpeed = 0.03f;

    private int stage = 0;
    private int lastStage = -1;
    private bool buttonPressed = false;
    private bool hasStarted = false;
    private bool canPressButton = false;

    private float enterTime = 0f;
    private float inputBlockTime = 1.5f;

    private Coroutine typingCoroutine;

    private Vector3 obj1StartPos;
    private Vector3 obj2StartPos;

    private Quaternion obj1StartRot;
    private Quaternion obj2StartRot;

    void Start()
    {
        obj1StartPos = obj1.transform.position;
        obj2StartPos = obj2.transform.position;

        obj1StartRot = obj1.transform.rotation;
        obj2StartRot = obj2.transform.rotation;

        SetObjectsStill();

        // ❌ disable hands initially
        if (leftHand) leftHand.SetActive(false);
        if (rightHand) rightHand.SetActive(false);
    }

    void ResetObjects()
    {
        // 👉 must disable kinematic first
        obj1.isKinematic = false;
        obj2.isKinematic = false;

        // Reset transform
        obj1.transform.position = obj1StartPos;
        obj2.transform.position = obj2StartPos;

        obj1.transform.rotation = obj1StartRot;
        obj2.transform.rotation = obj2StartRot;

        // Reset physics
        obj1.linearVelocity = Vector3.zero;
        obj2.linearVelocity = Vector3.zero;

        obj1.angularVelocity = Vector3.zero;
        obj2.angularVelocity = Vector3.zero;

        // 👉 stop movement again
        obj1.isKinematic = true;
        obj2.isKinematic = true;
    }

    // ================= TRIGGER START =================
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (hasStarted) return;

        hasStarted = true;
        enterTime = Time.time;

        ResetObjects();

        StartCoroutine(StartIntro());
    }

    IEnumerator StartIntro()
    {
        yield return StartCoroutine(TypeText(GetStartText()));
        yield return new WaitForSeconds(0.5f);

        canPressButton = true;
    }

    void Update()
    {
        if (!hasStarted) return;

        if (stage == lastStage) return;
        lastStage = stage;

        if (firstLaw)
        {
            FirstLawFlow();
        }
        else if (secondLaw)
        {
            SecondLawFlow();
        }
        else if (thirdLaw)
        {
            StartTyping("3rd Law coming soon...");
        }
    }

    // ================= FIRST LAW =================
    void FirstLawFlow()
    {
        // ❌ ensure hands OFF
        if (leftHand) leftHand.SetActive(false);
        if (rightHand) rightHand.SetActive(false);

        if (stage == 1)
        {
            SetObjectsStill();
            StartTyping("1st Law of Motion\n\nA body at rest remains at rest");
        }
        else if (stage == 2)
        {
            SetObjectsMoving();
            StartTyping("A body in motion remains in motion\n\nunless acted upon by an external force");
        }
        else if (stage == 3)
        {
            StartTyping("External force applied!\n\nWatch the interaction");
            ApplyCollisionForce();
        }
        else if (stage == 4)
        {
            StartTyping("Now push that boulder");
        }
    }

    // ================= SECOND LAW =================
    void SecondLawFlow()
    {
        // ✅ enable hands
        if (leftHand) leftHand.SetActive(true);
        if (rightHand) rightHand.SetActive(true);

        if (stage == 1)
        {
            StartTyping("2nd Law of Motion\n\nForce depends on mass and acceleration");
        }
        else if (stage == 2)
        {
            StartTyping("Heavier objects need more force\n\nto move the same way");
        }
        else if (stage == 3)
        {
            StartTyping("Place car and truck on seesaw\n\nSee their weight difference");
        }
    }

    // ================= COMMON =================

    string GetStartText()
    {
        if (firstLaw) return "1st Law of Motion\n\nPress button to begin";
        if (secondLaw) return "2nd Law of Motion\n\nPress button to begin";
        if (thirdLaw) return "3rd Law of Motion\n\nPress button to begin";

        return "Press button to begin";
    }

    void SetObjectsStill()
    {
        obj1.linearVelocity = Vector3.zero;
        obj2.linearVelocity = Vector3.zero;

        obj1.isKinematic = true;
        obj2.isKinematic = true;
    }

    void SetObjectsMoving()
    {
        obj1.isKinematic = false;
        obj2.isKinematic = false;

        obj1.linearVelocity = Vector3.forward * motionSpeed;
        obj2.linearVelocity = Vector3.back * motionSpeed;
    }

    void ApplyCollisionForce()
    {
        obj1.isKinematic = false;
        obj2.isKinematic = false;

        obj1.linearVelocity = Vector3.forward * motionSpeed;

        StartCoroutine(WaitAndPush());
    }

    IEnumerator WaitAndPush()
    {
        yield return new WaitForSeconds(1f);
        obj2.AddForce(Vector3.forward * pushForce, ForceMode.Impulse);
    }

    // ================= TYPEWRITER =================
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

    // ================= BUTTON =================
    public void OnButtonPressed()
    {
        if (Time.time < enterTime + inputBlockTime) return;
        if (!hasStarted) return;
        if (!canPressButton) return;
        if (buttonPressed) return;

        buttonPressed = true;
        stage++;

        Invoke(nameof(ResetButton), 0.4f);
    }

    void ResetButton()
    {
        buttonPressed = false;
    }
}