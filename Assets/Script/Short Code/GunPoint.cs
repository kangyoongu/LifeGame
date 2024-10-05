using UnityEngine;

public class GunPoint : MonoBehaviour
{
    public static Transform pos;
    private void Awake()
    {
        pos = transform;
    }
}
