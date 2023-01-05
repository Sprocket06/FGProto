using System.Runtime.Serialization;
using Color = System.Drawing.Color;

namespace FGProto.Scenes;

public class GameScene : Scene
{
    public Vector2 StickPos = new();
    public List<Vector2> InputBuffer = new();
    private Vector2 inputDisplayPos = new(50, 50);
    private bool punch = false;
    private KeyCode punchBtn = KeyCode.U;
    private Texture testSprite;
    
    private List<Vector2[]> punchMoves = new()
    {
        new []{ new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1)},
        new []{ new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)},
        new []{ new Vector2(1, 0)}
    };

    private List<String> punchMoveNames = new() {"623P", "236P", "6P"};

    public GameScene()
    {
        //testSprite = GameCore.ContentProvider?.Load<Texture>("fg-test-sprite1.png");
    }

    public override void LoadStep()
    {
        testSprite = GameCore.ContentProvider?.Load<Texture>("fg-test-sprite1.png");
    }
    
    public override void Draw(RenderContext context)
    {
        context.Clear(Color.White);
        context.DrawTexture(testSprite, new(100, 100));
        drawStickDisplay(context);
    }

    public override void FixedUpdate(float delta)
    {
        Vector2 lastState = StickPos;
        StickPos = Vector2.Zero;
        if (Keyboard.IsKeyDown(KeyCode.A))
        {
            StickPos.X += -1;
        }

        if (Keyboard.IsKeyDown(KeyCode.D))
        {
            StickPos.X += 1;
        }

        if (Keyboard.IsKeyDown(KeyCode.S))
        {
            StickPos.Y += 1;
        }

        if (Keyboard.IsKeyDown(KeyCode.W))
        {
            StickPos.Y += -1;
        }
        
        InputBuffer.Add(StickPos);
        bool lastPunch = punch;
        punch = Keyboard.IsKeyDown(punchBtn);

        if (!lastPunch && punch)
        {
            // it's input scanning time;
            Vector2[] recentInputFrames = new Vector2[16];
            bool validInput = false;
            int index = 0;
            for (int i = 0; i < 16; i++)
            {
                //GameCore.Log.Info(InputBuffer[^(i+1)]);
            }
            foreach(Vector2[] moveInput in punchMoves)
            {
                int nextExpectedInputIndex = 0;
                bool forgivenessUsed = false;
                Vector2 lastSeen;
                for (int i = 0; i < 16; i++)
                {
                    Vector2 expected = moveInput[^(nextExpectedInputIndex+1)];
                    if (i > 0)
                    {
                        if (InputBuffer[^(i + 1)] == InputBuffer[^i]) continue;
                    }
                    if (InputBuffer[^(i+1)] != expected)
                    {
                        if (!forgivenessUsed)
                        {
                            forgivenessUsed = true;
                            continue;
                        }

                        break;
                    }

                    nextExpectedInputIndex++;
                    if (nextExpectedInputIndex >= moveInput.Length)
                    {
                        validInput = true;
                        break;
                    }
                }

                if (validInput) break;
                index++;
            }

            if (validInput)
            {
                GameCore.Log.Info(punchMoveNames[index]);
            }
        }
    }
    
    private void drawStickDisplay(RenderContext ctx)
    {
        Vector2 sPos = (StickPos == Vector2.Zero) ? inputDisplayPos : inputDisplayPos + (Vector2.Normalize(StickPos) * 25);
        ctx.Circle(ShapeMode.Fill, inputDisplayPos, 32, Color.DarkGray);
        ctx.Circle(ShapeMode.Fill, sPos, 5, Color.DarkRed); 
    }

    public override void KeyPressed(KeyEventArgs e)
    {
        //if (e.KeyCode == punchBtn) punch = true;
    }

    public override void KeyReleased(KeyEventArgs e)
    {
        //if (e.KeyCode == punchBtn) punch = false;
    }
}