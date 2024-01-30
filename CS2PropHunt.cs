using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Utils;
using System.Drawing;
using System.Runtime.InteropServices;

namespace CS2PropHunt
{
    public class CS2PropHunt : BasePlugin
    {
        public override string ModuleName => "CS2 Prop Hunt Plugin";

        public override string ModuleVersion => "0.1.1";

        public List<string> models = new List<string>();

        int spawnTerroristOffset = 0;

        DateTime hideTime = DateTime.Now;
        bool teleportedPlayers = false;
        Vector[] infernoSpawns = new Vector[] {
            new Vector(940, 1404, 157),
                        new Vector(940, 1364, 157),
                        new Vector(940, 1334, 157),
                        new Vector(940, 1304, 157),
                        new Vector(1000, 2915, 192),
                        new Vector(474, -354, 132),
                        new Vector(504, -354, 132),
                        new Vector(534, -354, 132),
                        new Vector(564, -354, 132),
                        new Vector(1850, 871, 192),
                        new Vector(1820, 871, 192),
                        new Vector(1710, 871, 192),

                    };
        Vector[] mirageSpawns = new Vector[] {
                        new Vector(1766, 660, -160), // oob t spawn
                        new Vector(1736, 660, -160),
                        new Vector(1706, 660, -160),
                        new Vector(1676, 660, -160),
                        new Vector(806, -2244, 232), // Palace A site
                        new Vector(776, -2244, 232),
                        new Vector(746, -2244, 232),
                        new Vector(1007, -957, 64), // OOB t spawn but cool
                        new Vector(30, -994, -23), // 2 trees at mid
                        new Vector(-1990, -905, 8), // i like oob

                    };
        Vector[] officeSpawns = new Vector[] {
            new Vector(-73,-1268,-180),
            new Vector(-43,-1268,-180),
            new Vector(-33,-1268,-180),
            new Vector(1180,-511,-120),
            new Vector(1180,-411,-120),
            new Vector(-2052,-1430,-250),
            new Vector(-583,420,-323),
            new Vector(-1237,-2910,-355),
            new Vector(-1207,-2910,-355),
            new Vector(-1167,-2910,-355),

        };

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
            Server.PrintToChatAll("Prop hunt not set. Please restart round.");

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
                    var GETMODELSIGNATURE_LINUX = "\\x55\\x48\\x89\\xE5\\x53\\x48\\x89\\xFB\\x48\\x83\\xEC\\x08\\x48\\x8B\\x47\\x30";
                    
                    var getModel = new VirtualFunctionWithReturn<IntPtr, string>(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? GETMODELSIGNATURE_LINUX : GETMODELSIGNATURE);
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
                        if (spawnTerroristOffset > mirageSpawns.Length-1) spawnTerroristOffset = 0;
                        spawn.Teleport(mirageSpawns[spawnTerroristOffset], new QAngle(0, 0, 0), new Vector(0, 0, 0));
                    }
                    if (Server.MapName == "de_inferno")
                    {
                        if (spawnTerroristOffset > infernoSpawns.Length - 1) spawnTerroristOffset = 0;
                        spawn.Teleport(infernoSpawns[spawnTerroristOffset], new QAngle(0, 0, 0), new Vector(0, 0, 0));
                    }
                    if (Server.MapName == "cs_office")
                    {
                        if (spawnTerroristOffset > officeSpawns.Length - 1) spawnTerroristOffset = 0;
                        spawn.Teleport(officeSpawns[spawnTerroristOffset], new QAngle(0, 0, 0), new Vector(0, 0, 0));
                    }
                    spawnTerroristOffset += 1;


                }

            });
            RegisterEventHandler<EventRoundStart>((ev,@info) =>
            {
                hideTime = DateTime.Now.AddMinutes(1);
                teleportedPlayers = false;
                props.Clear();


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

                foreach (var player in Utilities.GetPlayers())
                {
                    try
                    {
                        if (player.Team == CsTeam.CounterTerrorist)
                        {
                            PropSpawner(player,true);

                        }
                    } catch (Exception e) { 
                        // Shut the f up
                    }
                }

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

                        if (player.TeamNum == 2)
                        {
                            player.RemoveWeapons();
                        }

                    } else if (!teleportedPlayers)
                    {
                        if (player.Pawn.IsValid)
                        {
                            if (player.TeamNum == 2)
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

                                player.GiveNamedItem(CounterStrikeSharp.API.Modules.Entities.Constants.CsItem.P90);
                                player.GiveNamedItem(CounterStrikeSharp.API.Modules.Entities.Constants.CsItem.Knife);
                                player.GiveNamedItem(CounterStrikeSharp.API.Modules.Entities.Constants.CsItem.USP);
                            }
                            if(player.TeamNum == 3)
                            {
                                player.RemoveWeapons();
                            }
                        }
                    }
                    if (!player.Pawn.IsValid) continue;
                    
                    bool found = false;
                    foreach (var item in props)
                    {

                        if (item.playerId == player.SteamID)
                        {
                            if (player.Team == CsTeam.Spectator)
                            {
                                item.prop.Remove();
                                props.Remove(item);
                                break;
                            }
                            if (!player.Pawn.IsValid) continue;
                            var off = offset(player.Pawn.Value.AbsOrigin, new Vector(0, 0, 0));
                            if (!item.Frozen ) {//|| item.lastPlayerPos.X != off.X || item.lastPlayerPos.Z != off.Z || item.lastPlayerPos.Y != off.Y || item.weirdStuff) { 
                                item.Teleport(off, /*new QAngle(item.prop.AbsRotation.X, player.Pawn.Value.AbsRotation.Y, item.prop.AbsRotation.Z)*/ new QAngle(0, player.Pawn.Value.AbsRotation.Y, 0));
                                //item.weirdStuff = !(item.lastPlayerPos2.X == item.lastPlayerPos.X && item.lastPlayerPos2.Z == item.lastPlayerPos.Z && item.lastPlayerPos2.Y == item.lastPlayerPos.Y);
                                item.weirdStuff = false;
                                player.GravityScale = 1;
                            }                       
                            else
                            {
                                if (CalculateDistance(item.prop.AbsOrigin, player.Pawn.Value.AbsOrigin) > 5 || true)
                                {
                                    player.Pawn.Value.Teleport(item.prop.AbsOrigin, new QAngle(IntPtr.Zero), new Vector(0,0,0));
                                }
                                player.GravityScale = 0;

                                //item.Teleport(off);

                            }
                            if (hideTime.CompareTo(DateTime.Now) <= 0) player.PrintToCenter("Swaps left: " + item.Swaps + ", You are " + (item.Frozen ? "" : "not ") + "frozen.");
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

            HookEntityOutput("prop_dynamic", "OnTakeDamage", (CEntityIOOutput output, string name, CEntityInstance activator, CEntityInstance caller, CVariant value, float delay) =>
            {
                caller.Remove();

                return HookResult.Continue;
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
            public int Swaps = 2;
            public int DecoysLeft = 3;
            public bool Frozen = false;

            public SpecialProp(CS2PropHunt plugin, CDynamicProp prop, ulong userId, int modelId)
            {
                this.prop = prop;
                playerId = userId;
                this.plugin = plugin;
                modelID = modelId;
                Swaps = 2;
                Frozen = false;
                DecoysLeft = 3;
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

        [ConsoleCommand("spawn_decoy", "Spawn a fake prop at your legs")]
        public void spawnDecoy(CCSPlayerController? player, CommandInfo command)
        {
            SpecialProp? foundProp = null;
            foreach (var item in props)
            {

                if (item.playerId == player.SteamID)
                {
                    foundProp = item; break;
                }

            }
            if (foundProp == null) return;
            if(foundProp.DecoysLeft > 0)
            {
                foundProp.DecoysLeft--;
                CDynamicProp prop = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
                prop.DispatchSpawn();
                prop.Teleport(foundProp.prop.AbsOrigin, foundProp.prop.AbsRotation, foundProp.prop.AbsVelocity);
                prop.Globalname = "test_prop";
                prop.SetModel(models[foundProp.modelID]);
                prop.Collision.CollisionGroup = 2;
                player.PrintToChat("Decoys Left: " + foundProp.DecoysLeft);

            } else
            {
                player.PrintToChat("No decoys!");
            }
        }

        [ConsoleCommand("swap_prop", "Swap prop for another prop (infinite times when hiding time, after that only 2 times)")]
        public void spawnProp(CCSPlayerController? player, CommandInfo command)
        {
            PropSpawner(player);
        }
        public void PropSpawner(CCSPlayerController? player, bool allowCreate = false)
        {
            if (player == null)
            {
                return;
            }
            var modelId = Random.Shared.Next(0, models.Count - 1);

            foreach (var item in props)
            {

                if (item.playerId == player.SteamID)
                {
                    if (!player.Pawn.IsValid) continue;
                    var canSwap = hideTime.CompareTo(DateTime.Now) > 0;
                    if (!canSwap)
                    {
                        if (item.Swaps > 0)
                        {
                            canSwap = true;
                            item.Swaps--;
                        }
                        
                    }
                    if (canSwap)
                    {
                        if(models.Count > 1)
                        while (item.modelID == modelId) modelId = Random.Shared.Next(0, models.Count - 1);

                        item.prop.SetModel(models[modelId]);
                        item.modelID = modelId;
                    }
                    
                    return;
                }
            }
            if (allowCreate || player.Team == CsTeam.CounterTerrorist)
            {
                // Spawn
                CDynamicProp prop = Utilities.CreateEntityByName<CDynamicProp>("prop_dynamic");
                prop.DispatchSpawn();
                prop.Teleport(player.Pawn.Value.AbsOrigin, new QAngle(0, 0, 0), new Vector(0, 0, 0));
                prop.Globalname = "test_prop";
                prop.SetModel(models[modelId]);
                /*if (Server.MapName == "de_mirage")
                {
                    prop.SetModel("models/props_junk/plasticcrate01a.vmdl");
                }
                if (Server.MapName == "de_inferno")
                {
                    prop.SetModel("models/generic/planter_kit_01/pk01_planter_09_cressplant_breakable_b.vmdl");
                }*/
                prop.Collision.CollisionGroup = 2; // best is 2

                props.Add(new SpecialProp(this, prop, player.SteamID, modelId));

                player.RemoveItemByDesignerName("weapon_c4");
                player.RemoveWeapons();
                player.GiveNamedItem(CounterStrikeSharp.API.Modules.Entities.Constants.CsItem.Knife);
                player.Pawn.Value.Health = 1;
            }

        }

        [ConsoleCommand("freeze_prop", "Freeze prop")]
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
                    item.Frozen = !item.Frozen;
                    return;
                }
            }

        }
    }
}