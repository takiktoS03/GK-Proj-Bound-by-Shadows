using UnityEngine;
using UnityEngine.Rendering.Universal;

/**
 * Skrypt umożliwiający płynne podążanie obiektu za wskazanym celem z zadanym offsetem.
 * Stosowany m.in. do świateł, efektów lub obiektów pomocniczych.
 *
 * @author Filip Kudła
 */
public class Follow : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    public float followSpeed = 10f;

    private void Update()
    {
        if (objectToFollow == null) return;

        transform.position = Vector3.Lerp(
            transform.position,
            objectToFollow.position + offset,
            Time.deltaTime * followSpeed
        );
    }
}
