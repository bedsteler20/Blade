using OpenTK.Windowing.Common;
using SkiaSharp;

namespace Blade;


public class GameObject : SKDrawable {
    public List<GameObject> Children { get; } = new();
    public virtual void OnKeyDown(KeyboardKeyEventArgs e) { }

    public virtual void Update() {
        foreach (var child in Children) {
            child.Update();
        }
    }
}