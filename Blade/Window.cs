using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SkiaSharp;

namespace Blade;
public class Window : GameWindow {
    GRGlInterface grgInterface;
    GRContext grContext;
    SKSurface surface;
    SKCanvas canvas;
    GRBackendRenderTarget renderTarget;

    public Window(string title, int width, int height) : base(new GameWindowSettings {
        UpdateFrequency = 60
    },
     new NativeWindowSettings {
         Title = title,
         Flags = ContextFlags.ForwardCompatible | ContextFlags.Debug,
         Profile = ContextProfile.Core,
         StartFocused = true,
         WindowBorder = WindowBorder.Fixed,
         Size = new Vector2i(width, height)
     }) {
        VSync = VSyncMode.Off;
    }

    protected override void OnLoad() {
        base.OnLoad();
        grgInterface = GRGlInterface.Create();
        grContext = GRContext.CreateGl(grgInterface);
        renderTarget = new GRBackendRenderTarget(ClientSize.X, ClientSize.Y, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
        surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
        canvas = surface.Canvas;
    }


    protected override void OnUnload() {
        surface.Dispose();
        renderTarget.Dispose();
        grContext.Dispose();
        grgInterface.Dispose();
        base.OnUnload();
    }

    protected override void OnUpdateFrame(FrameEventArgs args) {
        base.OnUpdateFrame(args);
        ScreenManager.CurrentScreen.Update();
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e) {
        base.OnKeyDown(e);
        ScreenManager.CurrentScreen.OnKeyDown(e);
    }

    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);
        canvas.Clear(SKColors.SlateGray);
        ScreenManager.CurrentScreen.Draw(canvas, 0, 0);
        canvas.Flush();
        SwapBuffers();
        GC.Collect();
    }
}