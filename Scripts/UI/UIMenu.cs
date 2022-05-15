using Godot;

namespace GodotModules
{
    public class UIMenu : Node
    {
        [Export] public readonly NodePath NodePathBtnPlay;

        private SceneManager _sceneManager;

        public void PreInit(SceneManager sceneManager) 
        {
            _sceneManager = sceneManager;
        }

        public override void _Ready() => GetNode<Button>(NodePathBtnPlay).GrabFocus();

        private async void _on_Play_pressed() => await _sceneManager.ChangeScene(Scene.Game);
        private async void _on_Multiplayer_pressed() => await _sceneManager.ChangeScene(Scene.GameServers);
        private async void _on_Options_pressed() => await _sceneManager.ChangeScene(Scene.Options);
        private async void _on_Mods_pressed() => await _sceneManager.ChangeScene(Scene.Mods);
        private async void _on_Credits_pressed() => await _sceneManager.ChangeScene(Scene.Credits);
        private void _on_Quit_pressed() => GetTree().Notification(MainLoop.NotificationWmQuitRequest);
        private void _on_Discord_pressed() => OS.ShellOpen("https://discord.gg/866cg8yfxZ");
        private void _on_GitHub_pressed() => OS.ShellOpen("https://github.com/GodotModules/GodotModulesCSharp");
    }
}
