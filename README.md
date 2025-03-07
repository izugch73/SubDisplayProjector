# SubDisplayProjector

A Unity component to project the main display content onto a secondary display, such as a sub-display with a fixed resolution (e.g., 1920×1080). This component supports various adjustment modes and can account for the Safe Area on mobile devices.

---

## Feature Overview

- **Multiple Adjustment Modes**
    - **WholeScreen**: Renders the entire main display on the sub-display, scaling the content to fit.
    - **HeightBasis**: Scales based on the height. The left and right edges may be cropped or filled with black bars.
    - **WidthBasis**: Scales based on the width. The top and bottom may be cropped or filled with black bars.

- **Safe Area Based Adjustment**
    - When enabled (`isBasedSafeArea` is true), the sub-display is scaled according to the device's Safe Area.
    - The script calculates the offset between the Safe Area center and the screen center, applying it (with scaling) to correctly position the content.
    - Note that **not** all areas of the safe area will necessarily be rendered. Depending on the value of `AdjustMode`, areas within the safe area may be cropped.

- **Singleton Access**
    - The component implements a singleton pattern, allowing easy access via `SubDisplayProjector.Instance`.

---

## Usage

### Setup

1. **Place the prefab**
    - Place SubDisplayProjector.prefab in the prefab directory in your scene.

2. **Configure in the Inspector**
    - **AdjustMode**: Choose one of the adjustment modes (WholeScreen, HeightBasis, WidthBasis).
    - **Is Based Safe Area**: Enable this flag if you want to account for the device's Safe Area (e.g., to avoid notches or curved screen edges).

### Runtime Behavior

- You can reapply adjustments at runtime using:
    - `Apply(AdjustMode adjustMode, bool isBasedSafeArea)`: Allows runtime updating of the adjustment mode and Safe Area flag.

### Example

```csharp
// Accessing the singleton instance and reapplying with new settings:
SubDisplayProjector.Instance.Apply(AdjustMode.HeightBasis, true);
```

### Rendering

- The `OnGUI()` method calls `Graphics.Blit` to update the RenderTexture, ensuring that the latest display content is projected onto the sub-display.

---

This component is designed to handle varying device resolutions and Safe Area configurations, ensuring that the content is properly scaled and centered for projection onto a fixed sub-display resolution.


# Compatible Unity Versions
2022.3.53f1

# Install
Add following url to package manager:
https://github.com/izugch73/SubDisplayProjector.git?path=Assets/Plugins/SubDisplayProjector#1.2.0

# License
MIT