using System;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public enum PlayerColor
{
    Blue,
    Red,
    Green,
    Yellow,
    Purple,
    Black,
    Pink,
};
public class TrailGenerator : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float interval = 0.5f;
    
 

    [SerializeField]
    public PlayerColor playerColor;

    private float time;

    private void Start()
    {
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;

        while (time >= interval)
        {
            SpawnTrail();
            time -= interval;
        }
    }

    private void SpawnTrail()
    {
        GameObject newParticle;
        newParticle = Instantiate(particlePrefab, transform.position, quaternion.identity);
        newParticle.GetComponent<ParticleSystem>().startColor = SetColor(); 
    }

    private Color SetColor()
    {
        switch (playerColor)
        {
            case PlayerColor.Red:
                return Color.red;
            case PlayerColor.Blue:
                return Color.blue;
            case PlayerColor.Green:
                return Color.green;
                break;
            case PlayerColor.Yellow:
                return Color.yellow;
                break;
            case PlayerColor.Purple:
                return new Color(0.43f,0,0.404f,1);
                break;
            case PlayerColor.Black:
                return Color.black;
                break;
            case PlayerColor.Pink:
                return new Color(1,0.7f,0.976f,1);
                break;
        }

        return Color.black;
    }
}
