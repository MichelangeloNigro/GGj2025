using System;
using Unity.Mathematics;
using UnityEngine;

public class TrailGenerator : MonoBehaviour
{
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float interval = 0.5f;
    
    [Serializable] 
    public enum PlayerColor
    {
        Blue,
        Red
    };

    [SerializeField]
    private PlayerColor playerColor;

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
            case PlayerColor.Red :
                return Color.red;
            case PlayerColor.Blue:
                return Color.blue;
        }
        
        return Color.black;
    }
}
