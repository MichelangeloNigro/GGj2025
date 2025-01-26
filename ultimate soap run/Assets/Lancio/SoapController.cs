using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public enum state
{
    Position,
    Rotation,
    Strenght,
    Waiting,
    Playing,
    Moving,
    End
}
public class SoapController : MonoBehaviour
{
    private float strenght;
    private float throwForce;
    public GameObject stecca;
    public int throwMultiplier;
    public float startingX;
    private Rigidbody rigidBody;
    public GameObject trail;
    public state state=state.Position;
    //attenzione la layer mask viene contata in binario, perch� unity xd 
    public LayerMask maskFloor;
    public bool overRideWait;
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
    }
    public void ChoosePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000000000f, maskFloor))
        {
            transform.position = new Vector3(hit.point.x, hit.point.y+(GetComponent<MeshCollider>().bounds.size.y), hit.point.z);
        }

       
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
        transform.rotation = Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z);
     
        if (Input.GetMouseButtonDown(0))
        {
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;

            //startingRotation = angle;
            state = state.Strenght;
        }
    }
    public void ChooseStrenght(float startx) {
        float normalizedMouseX = Math.Abs((Input.mousePosition.x - Screen.width / 2) / (Screen.width / 2))*2;
        throwForce = Math.Clamp(startx * normalizedMouseX, -1000, startx);
        stecca.transform.localPosition=new Vector3(Math.Clamp(startx * normalizedMouseX,-1000,startx),  stecca.transform.localPosition.y, stecca.transform.localPosition.z);
        if (Input.GetMouseButtonDown(0))
        {
            stecca.gameObject.SetActive(false);
            strenght = Math.Clamp(startx * normalizedMouseX, -1000, startx);
            if (overRideWait)
            {
                trail.SetActive(true);
                rigidBody.AddForce(-transform.right * throwForce * throwMultiplier, ForceMode.Impulse);
                rigidBody.constraints = RigidbodyConstraints.None;
                rigidBody.isKinematic = false;
                state = state.End;

                return;
            }
            state = state.Waiting; 
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
                rigidBody.constraints = RigidbodyConstraints.FreezeAll;
                break;
            case state.Moving:
                rigidBody.constraints = RigidbodyConstraints.None;
                if (transform.eulerAngles.x <= 320)
                {
                    moveToBorder();
                    Debug.Log("moving");
                    break;
                }
                Debug.Log("throw");
                trail.SetActive(true);
                rigidBody.AddForce(-transform.right * throwForce * throwMultiplier, ForceMode.Impulse);
                state = state.Playing;
                break;
            case state.Playing:
                //if (rigidBody.linearVelocity.magnitude < 1)
                //{
                //    rigidBody.linearVelocity = Vector3.zero;
                //    state = state.End;
                //    break;
                //}
                break;
            case state.End:
                break;
        }
    }
}
