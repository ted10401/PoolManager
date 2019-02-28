using UnityEngine;
using System.Collections.Generic;
using JSLCore.Pool;

public class UnitTest_PoolManager : MonoBehaviour
{
    [System.Serializable]
    public class SimpleClass
    {
        public int x;
        public int y;
    }


    public GameObject gameObjectReference1;
    public GameObject gameObjectReference2;
    private Queue<GameObject> m_recycleGameObjects = new Queue<GameObject>();

    public AudioSource audioSourceReference1;
    public AudioSource audioSourceReference2;
    private Queue<AudioSource> m_recycleAudioSources = new Queue<AudioSource>();

    public Mesh meshReference1;
    public Mesh meshReference2;
    [SerializeField] private List<Mesh> m_recycleMeshes = new List<Mesh>();

    public SimpleClass simpleClassReference;
    [SerializeField] private List<SimpleClass> m_recycleSimpleClasses = new List<SimpleClass>();

    private void Awake()
    {
        meshReference1 = new Mesh();
        meshReference1.name = "Mesh 1";

        meshReference2 = new Mesh();
        meshReference2.name = "Mesh 2";

        simpleClassReference = new SimpleClass();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_recycleGameObjects.Enqueue(PoolManager.Instance.Get(gameObjectReference1));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m_recycleGameObjects.Enqueue(PoolManager.Instance.Get(gameObjectReference2));
        }

        if (Input.GetKey(KeyCode.Alpha3) && m_recycleGameObjects.Count > 0)
        {
            m_recycleGameObjects.Dequeue().Recycle();
        }

        if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            PoolManager.Instance.Clear(gameObjectReference1);
            PoolManager.Instance.Clear(gameObjectReference2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PoolManager.Instance.Destroy(gameObjectReference1);
            PoolManager.Instance.Destroy(gameObjectReference2);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_recycleAudioSources.Enqueue(PoolManager.Instance.Get(audioSourceReference1));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            m_recycleAudioSources.Enqueue(PoolManager.Instance.Get(audioSourceReference2));
        }

        if (Input.GetKey(KeyCode.E) && m_recycleAudioSources.Count > 0)
        {
            m_recycleAudioSources.Dequeue().Recycle();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            PoolManager.Instance.Clear(audioSourceReference1);
            PoolManager.Instance.Clear(audioSourceReference2);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            PoolManager.Instance.Destroy(audioSourceReference1);
            PoolManager.Instance.Destroy(audioSourceReference2);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            m_recycleMeshes.Add(PoolManager.Instance.Get(meshReference1));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            m_recycleMeshes.Add(PoolManager.Instance.Get(meshReference2));
        }

        if (Input.GetKey(KeyCode.D) && m_recycleMeshes.Count > 0)
        {
            m_recycleMeshes[0].Recycle();
            m_recycleMeshes.RemoveAt(0);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            PoolManager.Instance.Clear(meshReference1);
            PoolManager.Instance.Clear(meshReference2);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            PoolManager.Instance.Destroy(meshReference1);
            PoolManager.Instance.Destroy(meshReference2);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            m_recycleSimpleClasses.Add(PoolManager.Instance.Get(simpleClassReference));
        }

        if (Input.GetKey(KeyCode.X) && m_recycleSimpleClasses.Count > 0)
        {
            m_recycleSimpleClasses[0].Recycle();
            m_recycleSimpleClasses.RemoveAt(0);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            PoolManager.Instance.Clear(simpleClassReference);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            PoolManager.Instance.Destroy(simpleClassReference);
        }
    }
}
