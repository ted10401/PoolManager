using UnityEngine;
using System.Collections.Generic;
using JSLCore.Pool;

public class UnitTest : MonoBehaviour
{
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
            PoolManager.Instance.Recycle(m_recycleGameObjects.Dequeue());
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_recycleAudioSources.Enqueue(PoolManager.Instance.Get(audioSourceReference1));
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            m_recycleAudioSources.Enqueue(PoolManager.Instance.Get(audioSourceReference2));
        }

        if (Input.GetKey(KeyCode.Alpha6) && m_recycleAudioSources.Count > 0)
        {
            PoolManager.Instance.Recycle(m_recycleAudioSources.Dequeue());
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            m_recycleMeshes.Add(PoolManager.Instance.Get(meshReference1));
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            m_recycleMeshes.Add(PoolManager.Instance.Get(meshReference2));
        }

        if (Input.GetKey(KeyCode.Alpha9) && m_recycleMeshes.Count > 0)
        {
            PoolManager.Instance.Recycle(m_recycleMeshes[0]);
            m_recycleMeshes.RemoveAt(0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            m_recycleSimpleClasses.Add(PoolManager.Instance.Get(simpleClassReference));
        }

        if (Input.GetKey(KeyCode.W) && m_recycleSimpleClasses.Count > 0)
        {
            PoolManager.Instance.Recycle(m_recycleSimpleClasses[0]);
            m_recycleSimpleClasses.RemoveAt(0);
        }
    }
}
