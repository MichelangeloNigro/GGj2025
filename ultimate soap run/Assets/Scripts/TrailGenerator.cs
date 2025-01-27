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
    celeste,
    Pink,
    Black
};
public class TrailGenerator : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float interval = 0.5f;
    
    [SerializeField]
    public PlayerColor playerColor;
    public AudioSource bubble;

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
        bubble.Play();
        newParticle = Instantiate(particlePrefab, transform.position, quaternion.identity);
        newParticle.GetComponent<ParticleSystem>().startColor = SetColor(); 
    }

    private Color SetColor()
    {
        switch (playerColor)
        {
            
            case PlayerColor.Red:
                return new Color(0.93f, 0.22f, 0.19f, 1);
            case PlayerColor.Blue:
                return new Color(0.46f, 0.71f, 0.87f, 1);
            case PlayerColor.Green:
                return new Color(0.50f, 0.78f, 0.48f, 1);
                break;
            case PlayerColor.Yellow:
            return new Color(0.87f, 0.70f, 0.45f, 1);
                break;
            case PlayerColor.Purple:
                return new Color(0.70f, 0.49f, 0.78f, 1);
                break;
            case PlayerColor.Black:
                return Color.black;
                break;
            case PlayerColor.Pink:
                return new Color(0.87f, 0.36f, 0.67f, 1);
                break;
            case PlayerColor.celeste:
                return new Color(0.47f, 0.95f, 0.93f, 1);
                break;
        }

        return Color.black;
    }
}
