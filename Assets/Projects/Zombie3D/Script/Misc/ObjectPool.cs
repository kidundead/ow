using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool
{

    protected List<GameObject> objects;
    protected List<Transform> transforms;
    protected List<float> createdTime;
    protected float life;
    protected bool hasAnimation = false;
    protected bool hasParticleEmitter = false;
    protected GameObject folderObject;
    public void Init(string poolName, GameObject prefab, int initNum, float life)
    {
        objects = new List<GameObject>();
        transforms = new List<Transform>();
        createdTime = new List<float>();
        this.life = life;

        folderObject = new GameObject(poolName);

        
        for (int i = 0; i < initNum; i++)
        {

            GameObject obj = Object.Instantiate(prefab) as GameObject;
            objects.Add(obj);
            transforms.Add(obj.transform);
            createdTime.Add(0f);

            obj.active = false;
            obj.transform.parent = folderObject.transform;



            if (obj.GetComponent<UnityEngine.Animation>() != null)
            {
                hasAnimation = true;
            }
            if (obj.GetComponent<ParticleEmitter>() != null)
            {
                hasParticleEmitter = true;
            }
            obj.SetActiveRecursively(false);
        }

    }


    public GameObject CreateObject(Vector3 position, Quaternion rotation)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].active)
            {

                objects[i].SetActiveRecursively(true);
                transforms[i].position = position;
                objects[i].transform.rotation = rotation;
                /*
                objects[i].active = true;
                transforms[i].position = position;
                objects[i].transform.rotation = Quaternion.LookRotation(rotation);
                
                //objects[i].animation.Stop();

                if (hasAnimation)
                {
                    objects[i].animation.Play();
                }
                if (hasParticleEmitter)
                {
                    ParticleEmitter pe = objects[i].particleEmitter;
                    pe.emit = true;
                    pe.Emit();
                }
                */

                createdTime[i] = Time.time;
                return objects[i];
            }

        }

        GameObject obj = Object.Instantiate(objects[0]) as GameObject;
        objects.Add(obj);
        transforms.Add(obj.transform);
        createdTime.Add(0f);
        obj.name = objects[0].name;
        obj.transform.parent = folderObject.transform;
        
        if (obj.GetComponent<UnityEngine.Animation>() != null)
        {
            hasAnimation = true;
        }
        if (obj.GetComponent<ParticleEmitter>() != null)
        {
            hasParticleEmitter = true;
        }
        obj.SetActiveRecursively(true);


        return obj;
    }


    public GameObject CreateObject(Vector3 position, Vector3 lookAtRotation)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].active)
            {

                objects[i].SetActiveRecursively(true);
                transforms[i].position = position;
                objects[i].transform.rotation = Quaternion.LookRotation(lookAtRotation);
                /*
                if (hasAnimation)
                {
                    objects[i].animation.Play();
                }
                if (hasParticleEmitter)
                {
                    ParticleEmitter pe = objects[i].particleEmitter;
                    pe.emit = true;
                    pe.Emit();
                }
                */

                createdTime[i] = Time.time;
                return objects[i];
            }
                    
        }

        GameObject obj = Object.Instantiate(objects[0]) as GameObject;
        objects.Add(obj);
        transforms.Add(obj.transform);
        createdTime.Add(0f);
        obj.name = objects[0].name;
        obj.transform.parent = folderObject.transform;

        if (obj.GetComponent<UnityEngine.Animation>() != null)
        {
            hasAnimation = true;
        }
        if (obj.GetComponent<ParticleEmitter>() != null)
        {
            hasParticleEmitter = true;
        }
        obj.SetActiveRecursively(true);


        return obj;
    }

    public void AutoDestruct()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            if ((objects[i].active))
            {
                if (Time.time - createdTime[i] > life)
                {
                    objects[i].SetActiveRecursively(false);

                }
            }
        }

    }

    public GameObject DeleteObject(GameObject obj)
    {
        obj.SetActiveRecursively(false);
        return obj;
    }





}
