using Blade.Colors;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;

namespace Blade;
public class Window : GameWindow {
    GRGlInterface grgInterface;
    GRContext grContext;
    SKSurface surface;
    SKCanvas canvas;
    GRBackendRenderTarget renderTarget;

    public static Window CurrentWindow { get; private set; }

    private int minHeight;
    private int minWidth;

    public int Width => ClientSize.X;
    public int Height => ClientSize.Y;

    public Window(string title, int width, int height) : base(new GameWindowSettings {
        UpdateFrequency = EngineFlags.FrameRate
    },
     new NativeWindowSettings {
         Title = title,
         Flags = ContextFlags.ForwardCompatible | ContextFlags.Debug,
         Profile = ContextProfile.Core,
         StartFocused = true,
         WindowBorder = WindowBorder.Resizable,
         Size = new Vector2i(width, height),
         MinimumSize = new Vector2i(width, height)
     }) {
        VSync = VSyncMode.Off;
        CurrentWindow = this;
        minWidth = width;
        minHeight = height;
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

    protected override void OnResize(ResizeEventArgs e) {
        base.OnResize(e);
        renderTarget.Dispose();
        surface.Dispose();
        canvas.Dispose();
        renderTarget = new GRBackendRenderTarget(ClientSize.X, ClientSize.Y, 0, 8, new GRGlFramebufferInfo(0, (uint)SizedInternalFormat.Rgba8));
        surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
        canvas = surface.Canvas;
    }

    protected override void OnJoystickConnected(JoystickEventArgs e) {
        base.OnJoystickConnected(e);
    }

    protected override void OnUpdateFrame(FrameEventArgs args) {
        base.OnUpdateFrame(args);
        JoystickInput();
        ScreenManager.CurrentScreen.Update();
    }

    private void JoystickInput() {
        foreach (var joystick in JoystickStates) {
            foreach (XInputMapping mapping in Enum.GetValues(typeof(XInputMapping))) {
                if (joystick == null) continue;
                if (joystick.IsButtonDown((int)mapping)) {
                    ScreenManager.CurrentScreen.OnJoystickButtonDown(mapping);
                }
                if (joystick.IsButtonPressed((int)mapping)) {
                    ScreenManager.CurrentScreen.OnJoystickButtonPressed(mapping);
                }
            }

        }
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e) {
        base.OnKeyDown(e);
        ScreenManager.CurrentScreen.OnKeyDown(e);
    }

    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);
        canvas.Clear(Catppuccin.Base);
        ScreenManager.CurrentScreen.Draw(canvas, 0, 0);
        canvas.Flush();
        SwapBuffers();
        if (EngineFlags.CollectGarbageAfterFrame) GC.Collect();
    }
}