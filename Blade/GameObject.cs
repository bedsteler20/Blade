using OpenTK.Windowing.Common;
using SkiaSharp;

namespace Blade;


public class GameObject : SKDrawable {
    public List<GameObject> Children { get; } = new();
    public virtual void OnKeyDown(KeyboardKeyEventArgs e) { }
    private bool hasSetup = false;
    public virtual void Update() {
        if (!hasSetup) {
            Setup();
            hasSetup = true;
        }
        foreach (var child in Children) {
            child.Update();
        }
    }

    protected virtual void Setup() { }
}
