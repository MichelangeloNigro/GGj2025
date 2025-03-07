using System;
using UnityEngine;
using UnityEngine.Rendering;

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
    private float startingX;
    private Rigidbody rigidBody;
    public TrailRenderer trail;
    public GameObject particle;
    public state state=state.Position;
    //attenzione la layer mask viene contata in binario, perch� unity xd 
    public LayerMask maskFloor;
    public bool overRideWait;
    [TextArea]
    public string description;
    public int aroma;
    public int flavor;
    public int faith;
    public int shape;
    public int colourstat;
    public Color color;
    private float minPLayTime = 5f;
    private float timecur = 0;
    public float bounceFactor=1;
    public float maxLife;
    public float life;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        life= maxLife;
    }
    
    public void setColorTrail(Color color)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(color, 0f), // Start color (solid red)
                new GradientColorKey(color, 1f) // End color (same red)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(0.8f, 0f), // Fully opaque at the start
                new GradientAlphaKey(0.3f, 1f) // Fully opaque at the end
            }
        );
        trail.colorGradient = gradient;
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
        //rigidBody.AddForce(-transform.forward, ForceMode.Force);
        transform.position += (transform.right)* Time.deltaTime;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player")&&(state==state.Playing||state==state.End))
        {
            var contact = collision.GetContact(0);
            GetComponent<AudioSource>().Play();
            collision.rigidbody.AddForce(-contact.normal * bounceFactor, ForceMode.Impulse);
        }
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
                if (transform.eulerAngles.x <= 315)
                {
                    moveToBorder();
                    Debug.Log("moving");
                    break;
                }
                Debug.Log("throw");
                trail.gameObject.SetActive(true);
                particle.SetActive(true);
                rigidBody.AddForce(-transform.right * throwForce * throwMultiplier, ForceMode.Impulse);
                timecur = 0;
                state = state.Playing;
                break;
            case state.Playing:
                timecur += Time.deltaTime;
                if (rigidBody.linearVelocity.magnitude < 0.4&&timecur>minPLayTime)
                {
                    rigidBody.linearVelocity = Vector3.zero;
                    state = state.End;
                    break;
                }
                //in media un turno usa 2 circa di vita 
                life -= (rigidBody.linearVelocity.magnitude/10000);
                rigidBody.mass -= (rigidBody.linearVelocity.magnitude / 100000);
                var scale = (100 * life) / maxLife;
                transform.localScale = new Vector3(scale,scale,scale);
                break;
            case state.End:
                if (rigidBody.linearVelocity.magnitude >1)
                {
                    state = state.Playing;

                }
                break;
        }
    }
}
