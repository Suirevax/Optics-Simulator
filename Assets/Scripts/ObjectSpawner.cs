using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] GameObject meniscusLens;
    [SerializeField] GameObject LaserSource;

    LensGenerator SpawnLens()
    {
        GameObject lens = Instantiate(meniscusLens, Vector3.zero, Quaternion.Euler(Vector3.zero));
        return lens.GetComponent<LensGenerator>();
    }
    public void SpawnMeniscusLens()
    {
        var lensgenerator = SpawnLens();
        lensgenerator.lensSize = new Vector2(1, 1);
        lensgenerator.handlerFirstPoint = new Vector2(-0.5f, 0.5f);
        lensgenerator.handlerSecondPoint = new Vector2(0.5f, 0.5f);
        lensgenerator.handlerFirstPoint2 = new Vector2(-0.5f, 0.5f);
        lensgenerator.handlerSecondPoint2 = new Vector2(0.5f, 0.5f);
    }
    
    public void SpawnLaserSource()
    {
        Instantiate(LaserSource, Vector3.zero, Quaternion.Euler(new Vector3(0,0,90)));
    }

    public void SpawnDefaultLens()
    {
        var lensgenerator = SpawnLens();
        lensgenerator.handlerFirstPoint = Vector2.zero;
        lensgenerator.handlerSecondPoint = Vector2.zero;
        lensgenerator.handlerFirstPoint2 = Vector2.zero;
        lensgenerator.handlerSecondPoint2 = Vector2.zero;
    }

    public void SpawnBiconvex()
    {
        var lensgenerator = SpawnLens();
        lensgenerator.lensSize = new Vector2(2,0);
        lensgenerator.handlerFirstPoint = new Vector2(-0.5f, 0.5f);
        lensgenerator.handlerSecondPoint = new Vector2(0.5f, 0.5f);
        lensgenerator.handlerFirstPoint2 = new Vector2(-0.5f, -0.5f);
        lensgenerator.handlerSecondPoint2 = new Vector2(0.5f, -0.5f);
    }
}
