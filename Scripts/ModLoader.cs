using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using MoonSharp.Interpreter;
using MoonSharp.VsCodeDebugger;

namespace Game
{
    public class ModLoader
    {
        private static Dictionary<string, Mod> Mods = new Dictionary<string, Mod>();
        private static MoonSharpVsCodeDebugServer DebugServer { get; set; }
        private static string PathMods { get; set; }


        public static void Init()
        {
            FindModsPath();
            
            DebugServer = new MoonSharpVsCodeDebugServer(); // how does this work in action?
            DebugServer.Start();
        }

        private static Script GetModScriptTemplate()
        {
            var script = new Script();

            var luaPlayer = new Godot.File();
            luaPlayer.Open("res://Scripts/Lua/Player.lua", Godot.File.ModeFlags.Read);

            script.DoString(luaPlayer.GetAsText());

            return script;
        }

        private static void FindModsPath()
        {
            string pathExeDir;

            if (Godot.OS.HasFeature("standalone")) // check if game is exported
                // set to exported release dir
                pathExeDir = $"{Directory.GetParent(Godot.OS.GetExecutablePath()).FullName}";
            else
                // set to project dir
                pathExeDir = Godot.ProjectSettings.GlobalizePath("res://");

            PathMods = Path.Combine(pathExeDir, "Mods");
        }

        public static void LoadAll()
        {
            Directory.CreateDirectory(PathMods);

            var mods = Directory.GetDirectories(PathMods);

            foreach (var mod in mods)
            {
                var modScript = GetModScriptTemplate();
                var files = Directory.GetFiles(mod);

                var pathInfo = $"{mod}/info.json";
                var pathScript = $"{mod}/script.lua";
                if (!File.Exists(pathInfo) || !File.Exists(pathScript))
                    continue;

                var modInfo = JsonConvert.DeserializeObject<ModInfo>(File.ReadAllText(pathInfo));

                try
                {
                    modScript.DoFile(pathScript);

                    UpdatePlayer(modScript);

                    Mods.Add(modInfo.Name, new Mod {
                        ModInfo = modInfo,
                        Script = modScript
                    });

                    DebugServer.AttachToScript(modScript, modInfo.Name);
                }
                catch (ScriptRuntimeException e)
                {
                    Godot.GD.Print(e.DecoratedMessage);
                }
            }
        }

        private static void UpdatePlayer(Script script)
        {
            var resPlayer = script.Globals.Get("Player");
            if (resPlayer != null)
            {
                var player = resPlayer.Table;
                var resHealth = player.Get("health");

                if (resHealth != null)
                    Master.Player.SetHealth((int)resHealth.Number);
            }
        }
    }

    public struct Mod 
    {
        public ModInfo ModInfo { get; set; }
        public Script Script { get; set; }
    }

    public struct ModInfo
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public string Version { get; set; }
    }
}