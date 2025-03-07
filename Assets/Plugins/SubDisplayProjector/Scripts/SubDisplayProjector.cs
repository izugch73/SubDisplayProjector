using UnityEngine;
using UnityEngine.UI;

namespace SubDisplayProjector
{
    public class SubDisplayProjector : MonoBehaviour
    {
        private SubDisplayProjector()
        {
        }

        private static SubDisplayProjector _instance;

        public static SubDisplayProjector Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<SubDisplayProjector>(true);
                return _instance;
            }
        }

        /**
         * Adjustment Modes
         * WidthBasis: Render based on the width. The top and bottom may be cropped or black bars added.
         * HeightBasis: Render based on the height. The left and right may be cropped or black bars added.
         * WholeScreen: Render the entire screen with black bars added.
         */
        [SerializeField] private AdjustMode adjustMode = AdjustMode.WholeScreen;

        /**
         * Safe Area Based Adjustment
         * If true, the safe area is considered when adjusting the display size.
         */
        [SerializeField] private bool isBasedSafeArea = false;

        [SerializeField] private RawImage subDisplay;

        private RenderTexture _subDisplayRenderTexture;
        private int _connectedDisplaysCount = 1;

        private void Start()
        {
            DontDestroyOnLoad(this);
            InitializeRenderTexture();

            if (Display.displays.Length > 1)
            {
                // Another display is already connected
                Apply(AdjustMode.WholeScreen, true);
            }
        }

        private void Update()
        {
            if (Display.displays.Length == _connectedDisplaysCount) return;
            _connectedDisplaysCount = Display.displays.Length;

            // Observe new display's connected
            if (_connectedDisplaysCount > 1)
            {
                Display.displays[_connectedDisplaysCount - 1].Activate();
                Apply(AdjustMode.WholeScreen, true);
            }
        }

        /// <summary>
        /// Adjust the display size based on the specified mode.
        /// </summary>
        public void Apply(AdjustMode adjustMode, bool isBasedSafeArea)
        {
            if (Display.displays.Length < 2) return;

            this.adjustMode = adjustMode;
            this.isBasedSafeArea = isBasedSafeArea;
            var displaySize = GetAnotherScreenResolution(Display.displays.Length - 1);
            AdjustDisplaySize(displaySize);
        }

        private static Vector2 GetAnotherScreenResolution(int displayId)
        {
            if (Application.isEditor || Display.displays.Length < 2) return new Vector2(1920, 1080);

            var display2 = Display.displays[displayId];
            var width = display2.systemWidth;
            var height = display2.systemHeight;

            return new Vector2(width, height);
        }

        private void InitializeRenderTexture()
        {
            _subDisplayRenderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
            subDisplay.texture = _subDisplayRenderTexture;
        }

        private void AdjustDisplaySize(Vector2 size)
        {
            float baseWidth = Screen.width;
            float baseHeight = Screen.height;
            var scale = 1f;

            if (isBasedSafeArea)
            {
                var safeArea = Screen.safeArea;

                // Calculate the offset from the center of the screen
                var screenCenter = new Vector2(Screen.width, Screen.height) / 2f;
                var offset = safeArea.center - screenCenter;

                // Calculate the scale based on the safe area
                var scaleWidth = size.x / safeArea.width;
                var scaleHeight = size.y / safeArea.height;

                scale = adjustMode switch
                {
                    AdjustMode.HeightBasis => scaleHeight,
                    AdjustMode.WidthBasis => scaleWidth,
                    _ => Mathf.Min(scaleWidth, scaleHeight)
                };

                subDisplay.rectTransform.anchoredPosition = -offset * scale;
            }
            else
            {
                var scaleWidth = size.x / baseWidth;
                var scaleHeight = size.y / baseHeight;

                scale = adjustMode switch
                {
                    AdjustMode.HeightBasis => scaleHeight,
                    AdjustMode.WidthBasis => scaleWidth,
                    _ => Mathf.Min(scaleWidth, scaleHeight)
                };
            }

            var newSizeDelta = new Vector2(baseWidth * scale, baseHeight * scale);
            subDisplay.rectTransform.sizeDelta = newSizeDelta;
        }

        private void OnGUI()
        {
            Graphics.Blit(null, _subDisplayRenderTexture);
        }
    }

    public enum AdjustMode
    {
        WholeScreen,
        HeightBasis,
        WidthBasis
    }
}