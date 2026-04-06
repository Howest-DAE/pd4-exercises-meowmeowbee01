using UnityEngine;

public class Billboarding : MonoBehaviour
{

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;
        transform.rotation = Quaternion.LookRotation(forward);
    }
}
