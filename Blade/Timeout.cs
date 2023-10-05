namespace Blade;

public class Timeout : GameObject
{
    private readonly Action action;
    public int Milliseconds;
    private int elapsed = 0;
    private readonly int msPerFrame;


    public Timeout(Action action, int milliseconds)
    {
        this.action = action;
        Milliseconds = milliseconds;
        msPerFrame = 1000 / EngineFlags.FrameRate;
    }

    public Timeout(int milliseconds, Action action)
    {
        this.action = action;
        Milliseconds = milliseconds;
        msPerFrame = 1000 / EngineFlags.FrameRate;
    }

    public override void Update()
    {
        base.Update();
        elapsed += msPerFrame;
        if (elapsed >= Milliseconds)
        {
            action();
            elapsed = 0;
        }
    }

}