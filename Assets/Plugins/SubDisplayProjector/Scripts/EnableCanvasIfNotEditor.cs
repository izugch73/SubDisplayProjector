using UnityEngine;

namespace SubDisplayProjector
{
    // `SubDisplayProjector` renders to an external display via a Canvas.
    // This Canvas is always visible in the Scene view of the Editor, so the Canvas's GameObject is disabled in the prefab.
    // This script enables the Canvas's GameObject when not in the Editor.
    public class EnableCanvasIfNotEditor : MonoBehaviour
    {
        [SerializeField] private Canvas _targetCanvas = null!;

        private void Awake()
        {
            if (!Application.isEditor) _targetCanvas.gameObject.SetActive(true);
        }
    }
}
