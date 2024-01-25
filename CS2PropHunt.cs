using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using System;
using System.Drawing;
using System.Reflection;

namespace CS2PropHunt
{
    public class CS2PropHunt : BasePlugin
    {
        public override string ModuleName => "CS2 Prop Hunt Plugin";

        public override string ModuleVersion => "0.0.2";

        public List<string> models = new List<string>();

        int spawnTerroristOffset = 0;

        DateTime hideTime = DateTime.Now;
        bool teleportedPlayers = false;

        CDynamicProp CreateProp(string model, Vector position)
        {
            CDynamicProp prop = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
            prop.DispatchSpawn();
            prop.NoGhostCollision = true;
            prop.Collision.CollisionGroup = 0;
            prop.Teleport(position, new QAngle(0, 0, 0), new Vector(0, 0, 0));
            prop.SetModel(model);

            return prop;
        }

        public override void Load(bool hotReload)
        {

            models.Clear();
            models.Add("models/props/de_inferno/claypot03.vmdl");

            RegisterListener<CounterStrikeSharp.API.Core.Listeners.OnMapStart>(ev =>
            {
                spawnTerroristOffset = 0;
                Server.ExecuteCommand("mp_give_player_c4 0");
                models.Clear();

            });

            RegisterListener<Listeners.OnEntitySpawned>(entity =>
            {
                if (entity.DesignerName == "prop_physics_multiplayer")
                {
                    var prop = new CPhysicsPropMultiplayer(entity.Handle);

                    // Until i fix it it doesnt work
                    //var getModel = new VirtualFunctionWithReturn<IntPtr,string>(GETMODELSIGNATURE);
                    //string model = getModel.Invoke(entity.Handle);
                    //Console.WriteLine(model);

                    var GETMODELSIGNATURE = "\\x40\\x53\\x48\\x83\\xEC\\x20\\x48\\x8B\\x41\\x30\\x48\\x8B\\xD9\\x48\\x8B\\x48\\x08\\x48\\x8B\\x01\\x2A\\x2A\\x2A\\x48\\x85";

                    var getModel = new VirtualFunctionWithReturn<IntPtr, string>(GETMODELSIGNATURE);
                    string model = getModel.Invoke(entity.Handle);

                    if (!models.Contains(model))
                    {
                        models.Add(model);
                        Console.WriteLine(model);
                    }

                    
                }
                if(entity.DesignerName == "func_buyzone")
                {
                    entity.Remove();
                }

                if (entity.DesignerName == "info_player_terrorist")
                {
                    var spawn = new CInfoPlayerTerrorist(entity.Handle);

                    if (Server.MapName == "de_mirage")
                    {
                        spawn.Teleport(new Vector(1766 - spawnTerroristOffset, 660, -160), new QAngle(0, 0, 0), new Vector(0, 0, 0));
                    }
                    if (Server.MapName == "de_inferno")
                    {
                        spawn.Teleport(new Vector(454, 384 , 136), new QAngle(0, 0, 0), new Vector(0, 0, 0));
                    }
                    spawnTerroristOffset += 30;


                }

            });

            RegisterEventHandler<EventRoundStart>((ev,@info) =>
            {
                hideTime = DateTime.Now.AddMinutes(1);
                teleportedPlayers = false;
                props.Clear();

                foreach (var player in Utilities.GetPlayers())
                {
                    if(player.Team == CsTeam.CounterTerrorist)
                    {
                        PropSpawner(player);
                    }
                }

                var offset = -20;

                CreateProp("models/dev/dev_cube.vmdl", new Vector(1235 + offset, 543, -240));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1235 + offset, 543, -210));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1235 + offset, 543, -180));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1235 + offset, 543, -150));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1235 + offset, 543, -120));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1235 + offset, 543, -90));

                CreateProp("models/dev/dev_cube.vmdl", new Vector(1265 + offset, 543, -240));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1265 + offset, 543, -210));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1265 + offset, 543, -180));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1265 + offset, 543, -150));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1265 + offset, 543, -120));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1265 + offset, 543, -90));

                CreateProp("models/dev/dev_cube.vmdl", new Vector(1295 + offset, 543, -240));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1295 + offset, 543, -210));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1295 + offset, 543, -180));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1295 + offset, 543, -150));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1295 + offset, 543, -120));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1295 + offset, 543, -90));

                CreateProp("models/dev/dev_cube.vmdl", new Vector(1325 + offset, 543, -240));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1325 + offset, 543, -210));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1325 + offset, 543, -180));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1325 + offset, 543, -150));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1325 + offset, 543, -120));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1325 + offset, 543, -90));

                CreateProp("models/dev/dev_cube.vmdl", new Vector(1355 + offset, 543, -240));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1355 + offset, 543, -210));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1355 + offset, 543, -180));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1355 + offset, 543, -150));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1355 + offset, 543, -120));
                CreateProp("models/dev/dev_cube.vmdl", new Vector(1355 + offset, 543, -90));

                return HookResult.Continue;
            },HookMode.Post);

            RegisterListener<Listeners.OnTick>(() =>
            {
                int offs = 0;
                var players = Utilities.GetPlayers();
                foreach (var player in players)
                {
                    if (hideTime.CompareTo(DateTime.Now) > 0) {
                        player.PrintToCenter("Hiding time: " + hideTime.Subtract(DateTime.Now).ToString("mm\\:ss"));
                    } else if (!teleportedPlayers)
                    {
                        if (player.Pawn.IsValid)
                        {
                            if (player.Team == CsTeam.Terrorist)
                            {
                                if (Server.MapName == "de_mirage")
                                {
                                    player.Pawn.Value.Teleport(new Vector(1316, -421 + offs, -103), new QAngle(0, -180, 0), new Vector(0, 0, 0));
                                }
                                if (Server.MapName == "de_inferno")
                                {
                                    player.Pawn.Value.Teleport(new Vector(670 - offs, 494, 136), new QAngle(0, 0, 0), new Vector(0, 0, 0));
                                }
                                offs += 30;
                            }
                        }
                    }
                    if (!player.Pawn.IsValid) continue;
                    
                    bool found = false;
                    foreach (var item in props)
                    {

                        if (item.playerId == player.SteamID)
                        {
                            if (!player.Pawn.IsValid) continue;
                            var off = offset(player.Pawn.Value.AbsOrigin, new Vector(0, 0, 0));
                            if (true || item.lastPlayerPos.X != off.X || item.lastPlayerPos.Z != off.Z || item.lastPlayerPos.Y != off.Y || item.weirdStuff) { 
                                item.Teleport(off, /*new QAngle(item.prop.AbsRotation.X, player.Pawn.Value.AbsRotation.Y, item.prop.AbsRotation.Z)*/ new QAngle(0, player.Pawn.Value.AbsRotation.Y, 0));
                                //item.weirdStuff = !(item.lastPlayerPos2.X == item.lastPlayerPos.X && item.lastPlayerPos2.Z == item.lastPlayerPos.Z && item.lastPlayerPos2.Y == item.lastPlayerPos.Y);
                                item.weirdStuff = false;
                            }                       
                            else
                            {
                                if (CalculateDistance(item.prop.AbsOrigin, player.Pawn.Value.AbsOrigin) > 20)
                                {
                                    item.weirdStuff = true;
                                    item.lastPlayerPos2 = player.Pawn.Value.AbsOrigin;
                                }
                                    
                                //item.Teleport(off);

                            }
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        if (!player.Pawn.IsValid) continue;
                        player.Pawn.Value.ShadowStrength = 0;
                        player.Pawn.Value.Collision.BoundingRadius = 5f;
                        player.Pawn.Value.Render = Color.FromArgb(0, 0, 0, 0);

                        
                    }
                    else
                    {
                        if (!player.Pawn.IsValid) continue;
                        player.Pawn.Value.ShadowStrength = 1;

                        player.Pawn.Value.Render = Color.FromArgb(255, 0, 0, 0);
                    }

                }
                if(hideTime.CompareTo(DateTime.Now) <= 0 && !teleportedPlayers)
                {
                    teleportedPlayers = true;
                }
            });
        }
        private float CalculateDistance(Vector point1, Vector point2)
        {
            float dx = point2.X - point1.X;
            float dy = point2.Y - point1.Y;
            float dz = point2.Z - point1.Z;

            return (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        Vector offset(Vector a, Vector b)
        {
            return new Vector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        class SpecialProp
        {
            public CDynamicProp prop;
            public ulong playerId;
            CS2PropHunt plugin;
            public bool weirdStuff = false;
            public Vector lastPlayerPos = new Vector(0, 0, 0);
            public Vector lastPlayerPos2 = new Vector(0, 0, 0);
            public int modelID = 0;

            public SpecialProp(CS2PropHunt plugin, CDynamicProp prop, ulong userId, int modelId)
            {
                this.prop = prop;
                playerId = userId;
                this.plugin = plugin;
                this.modelID = modelId;
            }
            public void Teleport(Vector position, QAngle angle, Vector velocity)
            {
                if (!prop.IsValid)
                {
                    plugin.props.Remove(this);
                    return;
                }
                lastPlayerPos = position;
                prop.Teleport(position, angle, velocity);
            }
            public void Teleport(Vector position)
            {
                Teleport(position, prop.AbsRotation, new Vector(0, 0, 0));
            }
            public void Teleport(Vector position, QAngle angle)
            {
                Teleport(position, angle, new Vector(0, 0, 0));
            }
        }

        List<SpecialProp> props = new List<SpecialProp>();

        [ConsoleCommand("spawn_prop", "i love props")]
        public void spawnProp(CCSPlayerController? player, CommandInfo command)
        {
            PropSpawner(player);
        }
        public void PropSpawner(CCSPlayerController? player)
        {
            if (player == null)
            {
                return;
            }

            foreach (var item in props)
            {

                if (item.playerId == player.SteamID)
                {
                    if (!player.Pawn.IsValid) continue;
                    return;
                    break;
                }
            }

            // Spawn
            CDynamicProp prop = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
            prop.DispatchSpawn();
            prop.Teleport(player.Pawn.Value.AbsOrigin, new QAngle(0, 0, 0), new Vector(0, 0, 0));
            prop.Globalname = "test_prop";
            var modelId = Random.Shared.Next(0, models.Count - 1);
            prop.SetModel(models[modelId]);
            /*if (Server.MapName == "de_mirage")
            {
                prop.SetModel("models/props_junk/plasticcrate01a.vmdl");
            }
            if (Server.MapName == "de_inferno")
            {
                prop.SetModel("models/generic/planter_kit_01/pk01_planter_09_cressplant_breakable_b.vmdl");
            }*/
            prop.Collision.CollisionGroup = 2;

            props.Add(new SpecialProp(this, prop, player.SteamID, modelId));

            player.RemoveItemByDesignerName("weapon_c4");
            player.RemoveWeapons();
            player.Pawn.Value.Health = 1;

        }

        [ConsoleCommand("remove_prop", "i hate props")]
        public void PropRemover(CCSPlayerController? player, CommandInfo command)
        {
            if (player == null)
            {
                return;
            }

            foreach (var item in props)
            {
                if (item.playerId == player.SteamID)
                {
                    item.prop.Remove();
                    props.Remove(item);

                    player.GiveNamedItem("weapon_knife");
                    return;
                }
            }

        }
    }
}