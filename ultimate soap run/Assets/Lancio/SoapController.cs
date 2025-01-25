using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

enum state
{
    Waiting,
    Position,
    Rotation,
    Strenght,
    Moving
}
public class SoapController : MonoBehaviour
{
    private float strenght;
    private float soapLeft;
    private Vector2 startingPoint;
    private float startingRotation;
    private List<Tuple<Vector2,float>> startingPointsANDrotation = new List<Tuple<Vector2, float>>();
    private string name;
    private float throwForce;
    public GameObject stecca;
    public int throwMultiplier;
    public float startingX;
    private Rigidbody rigidBody;
    public GameObject trail;


    state state=state.Waiting;
    //attenzione la layer mask viene contata in binario, perch� unity xd 
    public LayerMask maskFloor;
    //private bool isChoosingPoisition;
    //private bool isChoosingRotation;
    //private bool isChoosingtrenght;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = state.Position;
        rigidBody = GetComponent<Rigidbody>();
        //rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePosition;

        //state = state.Rotation;
        rigidBody.constraints = RigidbodyConstraints.FreezePositionY|RigidbodyConstraints.FreezeRotationZ|RigidbodyConstraints.FreezeRotationX;
    }
    public void ChoosePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000000000f, maskFloor))
        {
            transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }

        //Vector3 mouseScreenPos = Input.mousePosition;
        //mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        //Debug.Log($"Mouse World Position: {mouseWorldPos}");
        ////Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ////Debug.Log(mousePos);
        //transform.position = new Vector3(mouseWorldPos.x, transform.position.y,mouseWorldPos.z);
        if (Input.GetMouseButtonDown(0))
        {
            // startingPoint= mouseWorldPos;
            rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePosition;

            state = state.Rotation;
        }
    }
    public void rotation()
    {
        //Calcoliamo la posizione della saponetta sull schermo
        Vector3 centre = Camera.main.WorldToScreenPoint(transform.position);
        //creaiamo un vettore che va da saponetta a mouse
        Vector3 vectorLook = Input.mousePosition - centre;
        //calcolaiamo l'angolo in gradi del vettore che abbiamo trovato (iimaginiamo un grafico cartesiano dove la saponetta � il centro e il mouse il punto, con l'atangente troviamo l'angolo che deriva dal vettore 0,0 e mouse.x,mouse.y)
        float angle = -Mathf.Atan2(vectorLook.y, vectorLook.x) * Mathf.Rad2Deg;
        //applichiamo la rotazione alla y per rotarla
        transform.rotation = Quaternion.Euler(-90, angle, 0);
     
        if (Input.GetMouseButtonDown(0))
        {
            startingRotation = angle;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            state = state.Strenght;
        }
    }
    public void ChooseStrenght(float startx) {
        float normalizedMouseX = Math.Abs((Input.mousePosition.x - Screen.width / 2) / (Screen.width / 2))*2;
        //Debug.Log(normalizedMouseX);
        throwForce = Math.Clamp(startx * normalizedMouseX, -1000, startx);

        // Debug.Log(normalizedMouseX);
        // Debug.Log(normalizedMouseX);
        //Vector3 mouseScreenPos = Input.mousePosition;
        //mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        //Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        //Debug.Log(Math.Min(startingX, mouseWorldPos.x));
        stecca.transform.localPosition=new Vector3(Math.Clamp(startx * normalizedMouseX,-1000,startx),  stecca.transform.localPosition.y, stecca.transform.localPosition.z);
        if (Input.GetMouseButtonDown(0))
        {
            stecca.gameObject.SetActive(false);

            state = state.Moving; 
        }
    }
 public void moveToBorder()
    {
        rigidBody.AddForce(-transform.forward, ForceMode.Force);
        transform.position += (transform.right)* Time.deltaTime;
    }
    void Update()
    {
        switch (state)
        {

            case state.Position:
                ChoosePosition();
                break;
            case state.Rotation:
                rotation();
                startingX = stecca.transform.localPosition.x;
                break;
            case state.Strenght:
                stecca.gameObject.SetActive(true);
                ChooseStrenght(startingX);
                break;
            case state.Waiting:
                break;
            case state.Moving:
                rigidBody.constraints = RigidbodyConstraints.None;

                if (transform.eulerAngles.x<=320)
                {
                    moveToBorder();
                    Debug.Log("moving");
                    break;
                }
                Debug.Log("throw");
              trail.SetActive(true);
                rigidBody.AddForce(-transform.right * throwForce * throwMultiplier, ForceMode.Impulse);
                state = state.Waiting;
                break;
        }
    }
}
