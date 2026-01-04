using System.Collections;
using UnityEngine;


public class Util : MonoBehaviour
{
    public static float GRAVITY = -9.81f;

    public static IEnumerator DestroyAfterTime(float delay, GameObject gameObject)
    {
        yield return new WaitForSeconds(delay);

        ObjectDestroy(gameObject);
    }


    public static void ObjectDestroy(GameObject gameObject)
    {
        if(gameObject.TryGetComponent(out PoolableObject pool))
        {
            pool.ReturnToPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
