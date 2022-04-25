using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace GodotModules
{
    public abstract class Command
    {
        public static readonly List<Command> Instances = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(x => typeof(Command).IsAssignableFrom(x) && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<Command>()
                .ToList();

        public string[] Aliases { get; set; }

        public bool IsMatch(string cmd) 
        {
            var cmdMatchesAlias = false;
            if (Aliases != null)
                cmdMatchesAlias = Aliases.Contains(cmd);

            return cmdMatchesAlias || GetType().Name.Replace("Command", "").ToLower() == cmd;
        }

        public abstract void Run(string[] args);
    }
}
