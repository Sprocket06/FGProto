namespace FGProto.Scenes.SceneManagement;

public static class SceneManager
{
    private static readonly List<Scene> _scenes = new();

    public static Scene? ActiveScene { get; private set; }

    public static void SetActiveScene<T>() where T : Scene, new()
    {
        GameCore.Log.Info($"Loading scene... {typeof(T).Name}");
        if (!_scenes.Exists(x => x is T))
            _scenes.Add(new T());

        ActiveScene = _scenes.First(x => x is T);
    }

    public static void SetActiveScene(Scene scene)
    {
        GameCore.Log.Info($"Loading scene... {scene.GetType().Name}");
        if (!_scenes.Contains(scene))
            _scenes.Add(scene);

        ActiveScene = scene;
    }
}