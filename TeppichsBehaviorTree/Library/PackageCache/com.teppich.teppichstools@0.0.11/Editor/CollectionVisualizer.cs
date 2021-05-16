using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[ExecuteAlways]
public class CollectionVisualizer<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private bool showBezier;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        foreach (T thing in Collector<T>.collection)
        {
            Vector3 thisPosition  = transform.position;
            Vector3 thingPosition = thing.transform.position;

            if (!showBezier)
                Handles.DrawAAPolyLine(thisPosition, thingPosition);
            else
            {
                float halfHeight = (thisPosition.y - thingPosition.y) * 0.5f;

                Vector3 tangentOffset = Vector3.up * halfHeight;

                Handles.DrawBezier(thisPosition, thingPosition, thisPosition - tangentOffset,
                                   thingPosition + tangentOffset, Color.white, EditorGUIUtility.whiteTexture, 1f);
            }
        }
    }
#endif
}