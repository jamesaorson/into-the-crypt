using Common.Behaviors;
using Komodo.Core.ECS.Entities;
using Komodo.Core.Engine.Input;
using Komodo.Core;
using System;
using System.Collections.Generic;

namespace Komodo
{
    public static class Startup
    {
        public static Game Game { get; private set; }
        [STAThread]
        static void Main()
        {
            using (Game = new Game())
            {
                SetupInputs(CreateInputMap());
                new Entity(Game).AddComponent(new RootBehavior());
                Game.Run();
            }
        }

        static Dictionary<string, List<Inputs>> CreateInputMap()
        {
            return new Dictionary<string, List<Inputs>>
            {
                ["interact"] = new List<Inputs>
                {
                    Inputs.KeyEnter
                },
                ["left"] = new List<Inputs>
                {
                    Inputs.KeyA
                },
                ["quit"] = new List<Inputs>
                {
                    Inputs.KeyEscape
                },
                ["right"] = new List<Inputs>
                {
                    Inputs.KeyD
                },
                ["sprint"] = new List<Inputs>
                {
                    Inputs.KeyLeftShift,
                    Inputs.KeyRightShift,
                },
                ["up"] = new List<Inputs>
                {
                    Inputs.KeyW
                },
                ["down"] = new List<Inputs>
                {
                    Inputs.KeyS
                },
                ["camera_left"] = new List<Inputs>
                {
                    Inputs.KeyLeft
                },
                ["camera_right"] = new List<Inputs>
                {
                    Inputs.KeyRight
                },
                ["camera_zoom_in"] = new List<Inputs>
                {
                    Inputs.KeyE
                },
                ["camera_zoom_out"] = new List<Inputs>
                {
                    Inputs.KeyQ
                },
                ["toggle_debug"] = new List<Inputs>
                {
                    Inputs.KeyF1
                },
            };
        }

        static void SetupInputs(Dictionary<string, List<Inputs>> inputMap)
        {
            foreach (var pair in inputMap)
            {
                foreach (var input in pair.Value)
                {
                    InputManager.AddInputMapping(pair.Key, input);
                }
            }
        }
    }
}
