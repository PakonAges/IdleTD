using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        
    }

    public class Pool : MonoMemoryPool<Bullet>
    {
        
    }
}