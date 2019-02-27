using UnityEngine;
using System.Collections.Generic;
using JSLCore.Pool;

public class UnitTest : MonoBehaviour
{
    public GameObject gameObjectReference;
    private Queue<GameObject> m_recycleGameObjects = new Queue<GameObject>();

    private void Update()
    {
        if(Input.GetKey(KeyCode.Alpha1))
        {
            m_recycleGameObjects.Enqueue(PoolManager.Instance.Get(gameObjectReference));
        }

        if (Input.GetKey(KeyCode.Alpha2) && m_recycleGameObjects.Count > 0)
        {
            PoolManager.Instance.Recycle(m_recycleGameObjects.Dequeue());
        }
    }
}
