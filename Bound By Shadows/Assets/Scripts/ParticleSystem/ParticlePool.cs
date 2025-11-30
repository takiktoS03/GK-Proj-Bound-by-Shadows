//using UnityEngine;
//using System.Collections.Generic;

//public class ParticlePool
//{
//    private readonly Stack<GameObject> pool = new Stack<GameObject>();
//    private readonly Material material;
//    private readonly Sprite sprite;

//    public ParticlePool(Material mat, Sprite spr, int prewarmCount) 
//    {
//        material = mat;
//        sprite = spr;

//        for (int i = 0; i < prewarmCount; ++i)
//            pool.Push(CreateNewParticleObject());
//    }

//    private GameObject CreateNewParticleObject() 
//    {
//        GameObject go = new GameObject("Particle");
//        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();

//        sr.material = material;
//        sr.sprite = sprite;

//        go.SetActive(false);
//        return go;
//    }

//    public GameObject Get()
//    {
//        if(pool.Count == 0)
//            pool.Push(CreateNewParticleObject());

//        GameObject obj = pool.Pop();
//        obj.SetActive(true);
//        return obj;
//    }

//    public void Release(GameObject obj)
//    {
//        obj.SetActive(false);
//        pool.Push(obj);
//    }
//}
