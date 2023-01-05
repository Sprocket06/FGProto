using Chroma.ContentManagement;
using Chroma.Diagnostics.Logging.Sinks;
using FGProto.CommanderExtensions;

namespace FGProto;

public class GameCore : Game
{
    public static Log Log { get; private set; } = LogManager.GetForCurrentAssembly();
    public static IContentProvider? ContentProvider;
    private readonly DebugConsole _console;
    
    public GameCore() : base(new(false, false))
    {
        FixedTimeStepTarget = 60;
        Log = LogManager.GetForCurrentAssembly();
        _console = new DebugConsole(this.Window);
        Log.SinkTo(new CommanderSink(_console));
        SceneManager.SetActiveScene<GameScene>();
    }

    protected override void Draw(RenderContext context)
    {
        SceneManager.ActiveScene?.Draw(context);
        _console.Draw(context);
    }

    protected override void Update(float delta)
    {
        SceneManager.ActiveScene?.Update(delta);
        _console.Update(delta);
    }

    protected override void FixedUpdate(float delta)
    {
        SceneManager.ActiveScene?.FixedUpdate(delta);
    }

    protected override void LoadContent()
    {
        ContentProvider = Content;
        SceneManager.ActiveScene?.LoadStep();
    }

    protected override void MouseMoved(MouseMoveEventArgs e)
    {
        SceneManager.ActiveScene?.MouseMoved(e);
    }

    protected override void MousePressed(MouseButtonEventArgs e)
    {
        SceneManager.ActiveScene?.MousePressed(e);
    }

    protected override void MouseReleased(MouseButtonEventArgs e)
    {
        SceneManager.ActiveScene?.MouseReleased(e);
    }

    protected override void WheelMoved(MouseWheelEventArgs e)
    {
        SceneManager.ActiveScene?.WheelMoved(e);
    }

    protected override void KeyPressed(KeyEventArgs e)
    {
        _console.KeyPressed(e);
        SceneManager.ActiveScene?.KeyPressed(e);
    }

    protected override void KeyReleased(KeyEventArgs e)
    {
        SceneManager.ActiveScene?.KeyReleased(e);
    }

    protected override void TextInput(TextInputEventArgs e)
    {
        _console.TextInput(e);
        SceneManager.ActiveScene?.TextInput(e);
    }
}