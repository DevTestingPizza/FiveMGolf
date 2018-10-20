using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveMGolf
{
    public class Gfx : BaseScript
    {
        public static Scaleform Scale { get; private set; }
        public static Scaleform Scale2 { get; private set; }

        private static int currentHole = 1;

        private static int currentClubIndex = 0;
        private static int currentShotTypeIndex = 0;
        private static int currentShot = 2;
        private static int ball = 0;
        private static int currentClubObj = 0;

        private const int golfball_estimate_shot_blip_sprite = 390;
        private const int hole_flag_blip_sprite = 358;

        private readonly uint golfball_model_hash = (uint)GetHashKey("prop_golf_ball");
        private readonly uint golfball_tee_model_hash = (uint)GetHashKey("prop_golf_tee");


        public static readonly List<Hole> holes = new List<Hole>()
        {
            #region hole1
            new Hole(
                id: 1,
                par: 5,
                distance: 531,
                mapZoom: 880,
                mapAngle: 280,
                mapCenter: new Vector3(-1225f, 85f, 62.88f),
                spawn: new Vector3(-1367.2f, 176.44f, 58.01f),
                holeCoords: new Vector3(-1114.121f, 220.789f, 63.78f),
                teeCoords: new Vector3(-1370.93f, 173.98f, 57.01f)
                ),
            #endregion

            #region hole2
            new Hole(
                id: 2,
                par: 4,
                distance: 436,
                mapZoom: 850,
                mapAngle: 90,
                mapCenter: new Vector3(-1224f, 245f, 59f),
                spawn: new Vector3(-1367.2f, 176.44f, 58.01f),
                holeCoords: new Vector3(-1322.07f, 158.77f, 56.69f),
                teeCoords: new Vector3(-1107.26f, 157.15f, 62.04f)
                ),
            #endregion

            #region hole3
            new Hole(
                id: 3,
                par: 3,
                distance: 436,
                mapZoom: 850,
                mapAngle: 90,
                mapCenter: new Vector3(-1237.35f, 181.96f, 62.54f),
                spawn: new Vector3(-1367.2f, 176.44f, 58.01f),
                holeCoords: new Vector3(-1237.419f, 112.988f, 56.086f),
                teeCoords: new Vector3(-1312.97f, 125.64f, 56.39f)
                ),
            #endregion

            #region hole4
            new Hole(
                id: 4,
                par: 4,
                distance: 436,
                mapZoom: 850,
                mapAngle: 90,
                mapCenter: new Vector3(-1237.35f, 181.96f, 62.54f),
                spawn: new Vector3(-1367.2f, 176.44f, 58.01f),
                holeCoords: new Vector3(-1096.541f, 7.848f, 49.63f),
                teeCoords: new Vector3(-1218.56f, 107.48f, 57.04f)
                ),
            #endregion

            #region hole5
            new Hole(
                id: 5,
                par: 4,
                distance: 436,
                mapZoom: 850,
                mapAngle: 90,
                mapCenter: new Vector3(-1237.35f, 181.96f, 62.54f),
                spawn: new Vector3(-1367.2f, 176.44f, 58.01f),
                holeCoords: new Vector3(-957.386f, -90.412f, 39.161f),
                teeCoords: new Vector3(-1098.15f, 69.5f, 53.09f)
                ),
            #endregion

            #region hole6
            new Hole(
                id: 6,
                par: 3,
                distance: 436,
                mapZoom: 850,
                mapAngle: 90,
                mapCenter: new Vector3(-1237.35f, 181.96f, 62.54f),
                spawn: new Vector3(-1367.2f, 176.44f, 58.01f),
                holeCoords: new Vector3(-1103.516f, -115.163f, 40.444f),
                teeCoords: new Vector3(-987.7f, -105.42f, 39.59f)
                ),
            #endregion

            #region hole7
            new Hole(
                id: 7,
                par: 4,
                distance: 436,
                mapZoom: 850,
                mapAngle: 90,
                mapCenter: new Vector3(-1237.35f, 181.96f, 62.54f),
                spawn: new Vector3(-1367.2f, 176.44f, 58.01f),
                holeCoords: new Vector3(-1290.632f, 2.754f, 49.217f),
                teeCoords: new Vector3(-1117.793f, -104.069f, 40.8406f)
                ),
            #endregion

            #region hole8
            new Hole(
                id: 8,
                par: 5,
                distance: 436,
                mapZoom: 850,
                mapAngle: 90,
                mapCenter: new Vector3(-1237.35f, 181.96f, 62.54f),
                spawn: new Vector3(-1367.2f, 176.44f, 58.01f),
                holeCoords: new Vector3(-1034.944f, -83.144f, 42.919f),
                teeCoords: new Vector3(-1272.63f, 38.4f, 48.75f)
                ),
            #endregion

            #region hole9
            new Hole(
                id: 9,
                par: 4,
                distance: 436,
                mapZoom: 850,
                mapAngle: 90,
                mapCenter: new Vector3(-1237.35f, 181.96f, 62.54f),
                spawn: new Vector3(-1367.2f, 176.44f, 58.01f),
                holeCoords: new Vector3(-1294.775f, 83.51f, 53.804f),
                teeCoords: new Vector3(-1138.381f, 0.60467f, 47.98225f)
                ),
            #endregion
        };


        private static readonly List<KeyValuePair<string, int>> GolfClubsData = new List<KeyValuePair<string, int>>()
        {
            new KeyValuePair<string, int>("collision_hmhne2", 1),   // D
            new KeyValuePair<string, int>("collision_34g1vu", 3),   // 3W
            new KeyValuePair<string, int>("collision_34g1vw", 5),   // 5W
            new KeyValuePair<string, int>("collision_34g1vy", 9),   // 3I
            new KeyValuePair<string, int>("collision_34g1vz", 10),  // 4I
            new KeyValuePair<string, int>("collision_7u9nbd5", 11), // 5I
            new KeyValuePair<string, int>("collision_94kanvh", 12), // 6I
            new KeyValuePair<string, int>("collision_94kanvi", 13), // 7I
            new KeyValuePair<string, int>("collision_94kanvj", 14), // 8I
            new KeyValuePair<string, int>("collision_94kanvk", 15), // 9I
            new KeyValuePair<string, int>("collision_94kanvl", 16), // PW
            new KeyValuePair<string, int>("collision_94kanvn", 17), // SW
            new KeyValuePair<string, int>("collision_94kanvo", 18), // LW
            new KeyValuePair<string, int>("collision_94kanvq", 19)  // P
        };

        private static readonly List<string> ShotPowerTypes = new List<string>()
        {
            "collision_9b95m6d", // Normal
            "collision_8508fe1", // Power
            "collision_g6wlm9", // Punch
            "collision_9r0tisc", // Approach
            "collision_lne5sx", // Long Putt
            "collision_80yyqnp", // Putt
            "collision_958446q" // Short Putt
        };

        private static readonly List<string> models = new List<string>()
        {
            "prop_golf_bag_01",
            "prop_golf_bag_01b",
            "prop_golf_bag_01c",
            "prop_golf_ball",
            "prop_golf_ball_p2",
            "prop_golf_ball_p3",
            "prop_golf_ball_p4",
            "prop_golf_ball_tee",
            "prop_golf_driver",
            "prop_golf_iron_01",
            "prop_golf_marker_01",
            "prop_golf_pitcher_01",
            "prop_golf_putter_01",
            "prop_golf_tee",
            "prop_golf_wood_01"
        };

        /*

    func_1711(uParam0, joaat("prop_golf_putter_01"));
	func_1711(uParam0, joaat("prop_golf_ball"));
	func_1711(uParam0, joaat("prop_golf_pitcher_01"));
	func_1711(uParam0, joaat("prop_golf_wood_01"));
	func_1711(uParam0, joaat("prop_golf_iron_01"));
	func_1711(uParam0, joaat("caddy"));
	func_1711(uParam0, joaat("prop_golf_bag_01b"));
	func_1711(uParam0, joaat("prop_golfflag"));
	func_1711(uParam0, joaat("prop_golf_tee"));
	func_1711(uParam0, joaat("prop_golf_marker_01"));
         
            prop_golf_bag_01        =   886428669
            prop_golf_bag_01b       =   -344128923
            prop_golf_bag_01c       =   -37837080
            prop_golf_ball          =   -1358020705
            prop_golf_ball_p2       =   1616526761
            prop_golf_ball_p3       =   -717871261
            prop_golf_ball_p4       =   -980219875
            prop_golf_ball_tee      =   -1243214768
            prop_golf_driver        =   -2141023172
            prop_golf_iron_01       =   334347537
            prop_golf_marker_01     =   -1124612472
            prop_golf_pitcher_01    =   1933637837
            prop_golf_putter_01     =   1750479612
            prop_golf_tee           =   -1315457772
            prop_golf_wood_01       =   1705580940
        */


        public Gfx()
        {
            RequestAdditionalText("SP_GOLF", 3);
            Scale = new Scaleform("GOLF");
            Scale2 = new Scaleform("GOLF_FLOATING_UI");
            RegisterCommand("preview", new Action<int, List<object>, string>(async (int source, List<object> args, string rawCommand) =>
            {
                DestroyAllCams(false);
                if (!HasAnimDictLoaded("mini@golfhole_preview"))
                {
                    RequestAnimDict("mini@golfhole_preview");
                    while (!HasAnimDictLoaded("mini@golfhole_preview"))
                    {
                        await Delay(0);
                    }
                }
                DoScreenFadeOut(500);
                await Delay(500);

                Debug.WriteLine("loaded");
                int cam = CreateCamera(964613260, false);
                SetCamActive(cam, true);
                RenderScriptCams(true, false, 0, true, false);
                DoScreenFadeIn(500);
                if (PlayCamAnim(cam, $"hole_0{currentHole}_cam", "mini@golfhole_preview", -1317.17f, 60.494f, 53.56f, 0.0f, 0.0f, 0.0f, false, 2))
                {
                    Debug.WriteLine("true : " + IsCamPlayingAnim(cam, $"hole_0{currentHole}_cam", "mini@golfhole_preview").ToString());
                }
                else
                {
                    Debug.WriteLine("false : " + IsCamPlayingAnim(cam, $"hole_0{currentHole}_cam", "mini@golfhole_preview").ToString());
                }
                while (GetCamAnimCurrentPhase(cam) != 1f)
                {
                    await Delay(0);
                }
                Debug.WriteLine("Done playing");
                await Delay(1000);
                DoScreenFadeOut(500);
                await Delay(1000);
                DestroyAllCams(false);
                RenderScriptCams(false, false, 0, false, false);
                DoScreenFadeIn(500);

            }), false);

            RegisterCommand("idle", new Action<int, List<object>, string>(async (int source, List<object> args, string rawCommand) =>
            {
                #region player animation/tasks
                var holeCoords = holes[currentHole - 1].HoleCoords;
                var offset = GetOffsetFromEntityInWorldCoords(ball, 0.2f, -0.6f, 0f);
                float dx = offset.X - holeCoords.X;
                float dy = offset.Y - holeCoords.Y;
                float heading = GetHeadingFromVector_2d(dx, dy);

                if (!HasAnimDictLoaded("mini@golfai"))
                {
                    RequestAnimDict("mini@golfai");
                    while (!HasAnimDictLoaded("mini@golfai"))
                    {
                        await Delay(0);
                    }
                }
                int output = 0;
                ClearPedTasks(PlayerPedId());
                OpenSequenceTask(ref output);

                string anim = "";
                string prop = "";
                Vector3 clubOff = new Vector3(0f, 0f, 0f);
                switch (currentClubIndex)
                {
                    case 0:
                        prop = "prop_golf_driver";
                        anim = "wood_idle_a";
                        clubOff = new Vector3(clubOff.X + 0f, clubOff.Y + 0f, clubOff.Z + 0f);
                        break;
                    case 1:
                    case 2:
                        prop = "prop_golf_wood_01";
                        anim = "wood_idle_a";
                        clubOff = new Vector3(clubOff.X + 0f, clubOff.Y + 0f, clubOff.Z + 0f);
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                        prop = "prop_golf_iron_01";
                        anim = "iron_idle_a";
                        clubOff = new Vector3(clubOff.X + 0f, clubOff.Y + 0f, clubOff.Z + 0f);
                        break;
                    case 10:
                    case 11:
                    case 12:
                        prop = "prop_golf_pitcher_01";
                        anim = "wedge_idle_a";
                        clubOff = new Vector3(clubOff.X + 0f, clubOff.Y + 0f, clubOff.Z + 0f);
                        break;
                    case 13:
                        prop = "prop_golf_putter_01";
                        anim = "putt_idle_a";
                        clubOff = new Vector3(clubOff.X + 0f, clubOff.Y + 0f, clubOff.Z + 0f);
                        break;
                    default:
                        break;
                }

                if (currentClubObj != 0)
                {
                    int tmpClub = currentClubObj;
                    DeleteObject(ref tmpClub);
                    currentClubObj = 0;
                }
                int club = CreateObject(GetHashKey(prop), offset.X, offset.Y, offset.Z, true, false, false);
                currentClubObj = club;
                //int club = CreateObject(GetHashKey("prop_golf_putter_01"), offset.X, offset.Y, offset.Z, true, false, false);

                AttachEntityToEntity(club, PlayerPedId(), GetPedBoneIndex(PlayerPedId(), 28422), 0f, 0f, 0f, clubOff.X, clubOff.Y, clubOff.Z, false, false, false, false, 2, true);
                //TaskFollowNavMeshToCoord(0, offset.X, offset.Y, offset.Z, 1f, -1, 0.25f, false, heading + 90f);
                //TaskPlayAnim(0, "mini@golfai", "putt_approach_no_ball", 2.0f, -4.0f, -1, 0, 0, false, false, false);
                //TaskPlayAnim(0, "mini@golfai", "putt_approach_no_ball", 4f, -4f, -1, 2, 0.999f, false, false, false);
                TaskPlayAnim(0, "mini@golfai", anim, 2.0f, -4.0f, -1, 0, 0, false, false, false);
                //TaskPlayAnim(0, "mini@golfai", anim, 4f, -4f, -1, 1, 0.999f, false, false, false);
                TaskPlayAnim(0, "mini@golfai", "putt_intro", 4f, -4f, -1, 0, 0f, false, false, false);
                TaskPlayAnim(0, "mini@golfai", "putt_action", 4f, -4f, -1, 0, 0f, false, false, false);
                TaskPlayAnim(0, "mini@golfai", "putt_react_nuetral_01", 4f, -4f, -1, 0, 0f, false, false, false);
                TaskPlayAnim(0, "mini@golfai", anim, 4f, -4f, -1, 1, 0.999f, false, false, false);

                TaskPerformSequence(PlayerPedId(), CloseSequenceTask(output));

                ClearSequenceTask(ref output);
                #endregion

            }), false);

            RunSetup();
        }

        private async void RunSetup()
        {
            foreach (string model in models)
            {
                uint m = (uint)GetHashKey(model);
                if (IsModelValid(m))
                {
                    if (!HasModelLoaded(m))
                    {
                        RequestModel(m);
                        while (!HasModelLoaded(m))
                        {
                            await Delay(0);
                        }
                    }
                }
            }

            while (!Scale.IsLoaded || !HasAdditionalTextLoaded(3) || !Scale2.IsLoaded)
            {
                await Delay(0);
            }

            if (!HasStreamedTextureDictLoaded("GolfPutting"))
            {
                RequestStreamedTextureDict("GolfPutting", false);
                while (!HasStreamedTextureDictLoaded("GolfPutting"))
                {
                    await Delay(0);
                }
            }

            await PrepareTee();

            SetupPuttingMode();
            ToggleFlags(true);

            SwingMeterStuff();

            CoursePar();
            SetScoreboardTitle();
            SetPlayerList();

            int blip = AddBlipForCoord(holes[currentHole - 1].HoleCoords.X, holes[currentHole - 1].HoleCoords.Y, holes[currentHole - 1].HoleCoords.Z);
            SetBlipSprite(blip, hole_flag_blip_sprite);

            Tick += OnTick;
        }

        private void SwingMeterStuff()
        {
            Scale.CallFunction("SWING_METER_SET_MARKER", true, 0.5f, true, 0.0f);
            Scale.CallFunction("SWING_METER_TRANSITION_IN");
            Scale.CallFunction("SWING_METER_POSITION", 0.6f, 0.6f, 0.5f);
            Scale.CallFunction("SWING_METER_SET_APEX_MARKER", true, 0.5f, true, 0.0f);
            Scale.CallFunction("SWING_METER_SET_TARGET", 0.2f, 0.8f);
            Scale.CallFunction("SWING_METER_SET_TARGET", 0.2f, 0.8f);

            //Scale2.CallFunction("SET_DISTANCE", true, 0.5f, 0.6f, "test", "test2", "test3");

            BeginScaleformMovieMethod(Scale2.Handle, "SET_SWING_DISTANCE");
            ScaleformAddTextLabel("PREVIEW_DIST");
            BeginTextCommandScaleformString("DIST");
            AddTextComponentInteger(holes[currentHole - 1].Distance);
            EndTextCommandScaleformString();
            PushScaleformMovieMethodParameterString("0");
            EndScaleformMovieMethod();

            BeginScaleformMovieMethod(Scale2.Handle, "SET_STRENGTH");
            ScaleformAddTextLabel("PREVIEW_PCT");
            BeginTextCommandScaleformString("STRENGTH_PER");
            AddTextComponentInteger(94);
            EndTextCommandScaleformString();
            PushScaleformMovieMethodParameterString("0");
            EndScaleformMovieMethod();

            BeginScaleformMovieMethod(Scale2.Handle, "SET_HEIGHT");
            ScaleformAddTextLabel("PREVIEW_HT");
            BeginTextCommandScaleformString("DIST_SHORT");
            AddTextComponentInteger(80);
            EndTextCommandScaleformString();
            PushScaleformMovieMethodParameterString("0");
            EndScaleformMovieMethod();

            BeginScaleformMovieMethod(Scale2.Handle, "SET_PIN_DISTANCE");
            ScaleformAddTextLabel("PREVIEW_PIN");
            //ScaleformAddTextLabel("CARRY_DIST");
            BeginTextCommandScaleformString("DIST");
            AddTextComponentInteger(holes[currentHole - 1].Distance);
            EndTextCommandScaleformString();
            PushScaleformMovieMethodParameterString("0");
            EndScaleformMovieMethod();

            //Scale2.CallFunction("COLLAPSE", true);

            //Scale2.CallFunction("SET_STRENGTH", "Strength", "100%", 0);
            //Scale2.CallFunction("SET_SWING_DISTANCE", "Distance", "205 yds", 0);
            //Scale2.CallFunction("SET_PIN_DISTANCE", "To Pin", "265 yds", 0);
            //Scale2.CallFunction("SET_HEIGHT", "Height", "-2 ft", 1);
        }

        private async Task OnTick()
        {

            N_0x312342e1a4874f3f(holes[currentHole].HoleCoords.X, holes[currentHole].HoleCoords.Y, holes[currentHole].HoleCoords.Z, holes[currentHole].TeeCoords.X, holes[currentHole].TeeCoords.Y, holes[currentHole].TeeCoords.Z, 1f, 1f, false);
            N_0xa51c4b86b71652ae(true);
            N_0x2485d34e50a22e84(holes[currentHole].HoleCoords.X, holes[currentHole].HoleCoords.Y, holes[currentHole].HoleCoords.Z);
            //int one = 0;
            //int two = 0;
            //int three = 0;
            //N_0x632b2940c67f4ea9(Scale.Handle, ref one, ref two, ref three);
            //Debug.WriteLine($"{one} : {two} : {three}");
            //N_0xa356990e161c9e65(true);
            //N_0x1c4fc5752bcd8e48(-1120.569f, 222.185f, 64.814f, -0.712f, 0.7f, 0f, 14.92f, 24.48f, -0.63f, 42f, 20f, 56.974f, 0.08f);
            //N_0x5ce62918f8d703c7(255, 0, 0, 64, 255, 255, 255, 5, 255, 255, 0, 64);
            //N_0x12995f2e53ffa601((int)holes[currentHole].HoleCoords.X, (int)holes[currentHole].HoleCoords.Y, (int)holes[currentHole].HoleCoords.Z, (int)holes[currentHole].TeeCoords.X, (int)holes[currentHole].TeeCoords.Y, (int)holes[currentHole].TeeCoords.Z, (int)holes[currentHole].HoleCoords.X, (int)holes[currentHole].HoleCoords.Y, (int)holes[currentHole].HoleCoords.Z, (int)holes[currentHole].TeeCoords.X, (int)holes[currentHole].TeeCoords.Y, (int)holes[currentHole].TeeCoords.Z);
            //N_0x312342e1a4874f3f()
            N_0x2485d34e50a22e84(0.025f, 0.3f, 0.025f);
            N_0x12995f2e53ffa601(255, 255, 255, 100, 255, 255, 255, 100, 255, 255, 255, 100);
            N_0x9cfdd90b2b844bf7(1f, 1f, 1f, 1f, 0.3f);


            #region controls
            if (Game.IsControlPressed(0, Control.ReplayShowhotkey)) // k
            {
                SetDisplay(31); // scoreboard
            }
            else
            {
                SetDisplay(15); // normal
            }
            SetDisplay(15);
            Scale2.Render2D();

            if (Game.IsControlJustPressed(0, Control.ReplayFOVIncrease))
            {
                currentClubIndex++;
                if (currentClubIndex > GolfClubsData.Count - 1)
                {
                    currentClubIndex = 0;
                }
                Game.PlaySound("HIGHLIGHT_NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }
            else if (Game.IsControlJustPressed(0, Control.ReplayFOVDecrease))
            {
                currentClubIndex--;
                if (currentClubIndex < 0)
                {
                    currentClubIndex = GolfClubsData.Count - 1;
                }
                Game.PlaySound("HIGHLIGHT_NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }

            if (Game.IsControlJustPressed(0, Control.ReplayCameraUp))
            {
                currentShotTypeIndex++;
                if (currentShotTypeIndex > ShotPowerTypes.Count - 1)
                {
                    currentShotTypeIndex = 0;
                }
                Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }
            else if (Game.IsControlJustPressed(0, Control.ReplayCameraDown))
            {
                currentShotTypeIndex--;
                if (currentShotTypeIndex < 0)
                {
                    currentShotTypeIndex = ShotPowerTypes.Count - 1;
                }
                Game.PlaySound("NAV_LEFT_RIGHT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }
            #endregion

            SetHoleDisplay();
            ManageMinimap();
            SetSwingDisplay();
            CoursePar();
            SetScoreboardTitle();
            DrawHoleMarker();


            Scale.Render2D();


            if (ReturnTwo(0) == 1)
                await Delay(0);
        }

        private async Task PrepareTee()
        {
            #region request models
            if (!HasModelLoaded(golfball_model_hash))
            {
                RequestModel(golfball_model_hash);
                while (!HasModelLoaded(golfball_model_hash)) { await Delay(0); }
            }
            if (!HasModelLoaded(golfball_tee_model_hash))
            {
                RequestModel(golfball_tee_model_hash);
                while (!HasModelLoaded(golfball_tee_model_hash)) { await Delay(0); }
            }
            #endregion

            #region create objects and delete potential existing ones
            if (DoesObjectOfTypeExistAtCoords(holes[currentHole - 1].TeeCoords.X, holes[currentHole - 1].TeeCoords.Y, holes[currentHole - 1].TeeCoords.Z, 0.05f, golfball_tee_model_hash, false))
            {
                int oldTee = GetClosestObjectOfType(holes[currentHole - 1].TeeCoords.X, holes[currentHole - 1].TeeCoords.Y, holes[currentHole - 1].TeeCoords.Z, 0.1f, golfball_tee_model_hash, false, false, false);
                DeleteObject(ref oldTee);
            }

            if (DoesObjectOfTypeExistAtCoords(holes[currentHole - 1].TeeCoords.X, holes[currentHole - 1].TeeCoords.Y, holes[currentHole - 1].TeeCoords.Z, 0.05f, golfball_model_hash, false))
            {
                int oldBallObj = GetClosestObjectOfType(holes[currentHole - 1].TeeCoords.X, holes[currentHole - 1].TeeCoords.Y, holes[currentHole - 1].TeeCoords.Z, 0.1f, golfball_model_hash, false, false, false);
                DeleteObject(ref oldBallObj);
            }

            int tee = CreateObjectNoOffset(golfball_tee_model_hash, holes[currentHole - 1].TeeCoords.X, holes[currentHole - 1].TeeCoords.Y, holes[currentHole - 1].TeeCoords.Z, true, false, true);
            Debug.WriteLine("Tee created, id: " + tee.ToString());

            ball = CreateObjectNoOffset(golfball_model_hash, holes[currentHole - 1].TeeCoords.X, holes[currentHole - 1].TeeCoords.Y, holes[currentHole - 1].TeeCoords.Z + 0.05f, true, false, true);
            #endregion

            #region create a blip for the ball
            int ball_blip = AddBlipForEntity(ball);
            Debug.WriteLine("Ball object ID: " + ball.ToString());
            Debug.WriteLine("Ball blip ID: " + ball_blip.ToString());
            SetBlipSprite(ball_blip, 143);
            SetBlipScale(ball_blip, 0.5f);
            SetBlipColour(ball_blip, 28 + 13);
            #endregion


            #region shooting the ball by setting velocity or applying force to entity (velocity for initial shot, force for spin effect)
            //SetEntityNoCollisionEntity(tee, ball, false);

            //await Delay(1000);

            //float dx = holes[currentHole - 1].TeeCoords.X - holes[currentHole - 1].HoleCoords.X;
            //float dy = holes[currentHole - 1].TeeCoords.Y - holes[currentHole - 1].HoleCoords.Y;


            //float dx = Game.PlayerPed.Position.X - GetEntityCoords(ball, true).X;
            //float dy = Game.PlayerPed.Position.Y - GetEntityCoords(ball, true).Y;
            //float heading = GetHeadingFromVector_2d(dx, dy);
            //var a = GetOffsetFromEntityGivenWorldCoords(ball, GameplayCamera.Position.X, GameplayCamera.Position.Y, GameplayCamera.Position.Z);
            //SetEntityRotation(ball, heading, heading, heading, 0, false);
            //Debug.WriteLine("Heading: " + heading.ToString());
            //SetEntityHeading(PlayerPedId(), -heading);
            //SetEntityHeading(ball, -heading);
            //SetEntityVelocity(ball, -a.X, -a.Y, 10f);
            //CreateCamWithParams()
            //ApplyForceToEntity(ball, 0, -100f, 0f, 0f, 0f, 0f, 0f, 0, false, false, false, false, true);
            #endregion

        }

        private void ToggleFlags(bool toggle)
        {
            if (toggle)
            {
                if (!IsIplActive("GolfFlags"))
                {
                    RequestIpl("GolfFlags");
                }
            }
            else
            {
                if (IsIplActive("GolfFlags"))
                {
                    RemoveIpl("GolfFlags");
                }
            }



        }

        private void DrawHoleMarker()
        {
            var coords = holes[currentHole - 1].HoleCoords;
            DrawMarker(3, coords.X, coords.Y, coords.Z + 0.3f, 0f, 0f, 0f, 0f, 90f, 0f, 0.2f, 0.2f, -0.2f, 255, 255, 255, 255, false, true, 2, false, "GolfPutting", "PuttingMarker", false);
        }

        private void ManageMinimap()
        {
            SetRadarZoom(holes[currentHole - 1].MapZoom);
            N_0x71bdb63dbaf8da59(currentHole);
            LockMinimapPosition(holes[currentHole - 1].MapCenterCoords.X, holes[currentHole - 1].MapCenterCoords.Y);
            LockMinimapAngle(holes[currentHole - 1].MapAngle);
            ToggleStealthRadar(false);
            SetRadarBigmapEnabled(false, false);
        }

        private void SetupPuttingMode()
        {
            /*
             
            Btw, this natives is rendering the golf grid
            Citizen.InvokeNative(0xA356990E161C9E65, 1)
            Citizen.InvokeNative(0x1C4FC5752BCD8E48, -1289.969, 83.574, 54.183, -1.0, 0.004, 0.005, 19.01, 20.0, -0.63, 42.0, 20.0, 54.5, 0.09)
            Citizen.InvokeNative(0x5CE62918F8D703C7, 255, 0, 0, 64, 255, 255, 255, 5, 255, 255, 0, 64)
            https://i.imgur.com/5dg954F.png https://i.imgur.com/7HN2b3p.png

             */

            N_0xa356990e161c9e65(true); // toggle on/off
            N_0x1c4fc5752bcd8e48(
                holes[currentHole - 1].HoleCoords.X,    // x coord
                holes[currentHole - 1].HoleCoords.Y,    // y coord
                holes[currentHole - 1].HoleCoords.Z,    // z coord
                -1f,                                    // amount of grid divider lines (top/bottom) (leave at -1.0)
                0.85f,                                  // rotation / heading (0.0 - 1.0)
                0f,                                     // offset (top/bottom) (?)
                15f,                                    // grid width
                15f,                                    // grid length
                -1f,                                    // amount of grid divider lines (left/right) (leave at -1.0)
                20f,                                    // zoom / scale > = smaller (more) squares, < = bigger (less) squares
                40f,                                    // glow intensity / opacity
                holes[currentHole - 1].HoleCoords.Z,    // ground z coord?
                0.2f                                    // heigh difference color margin (0.2 is good)
                );
            N_0x5ce62918f8d703c7(
                255,
                0,
                0,
                64,
                255,
                255,
                255,
                5,
                255,    // red
                255,    // green
                0,      // blue
                64      // alpha
                );
        }

        /// <summary>
        /// Get the ped headshot, and clear all headshots in case they run out.
        /// </summary>
        /// <param name="ped"></param>
        /// <returns></returns>
        private async Task<string> GetPedHead(int ped)
        {
            int handle = RegisterPedheadshot(ped);
            if (handle < 10)
            {
                for (int i = 0; i < 255; i++)
                {
                    UnregisterPedheadshot(i);
                }
                handle = RegisterPedheadshot(ped);
            }
            while (!IsPedheadshotReady(handle) || !IsPedheadshotValid(handle))
            {
                await Delay(0);
            }
            return GetPedheadshotTxdString(handle);
        }

        private async void SetPlayerList()
        {
            for (int i = 0; i < 255; i++)
            {
                UnregisterPedheadshot(i);
            }
            int row = 0;
            foreach (Player p in new PlayerList())
            {
                string head = await GetPedHead(p.Character.Handle);
                SetPlayercardSlot(row, 2, p.Name, "", head, 28 + p.Handle, 36);
                if (NetworkIsPlayerTalking(p.Handle))
                {
                    SetPlayercardHeadset(row, HeadsetIcon.ACTIVE);
                }
                else if (NetworkIsPlayerMutedByMe(p.Handle))
                {
                    SetPlayercardHeadset(row, HeadsetIcon.MUTED);
                }
                else
                {
                    SetPlayercardHeadset(row, HeadsetIcon.NONE);
                }
                SetScoreboardSlot(row, 2, p.Name, "crew", "", 28 + p.Handle, 36, "5", "4", "3", "4", "4", "3", "4", "5", "4");
                row++;
            }
        }

        /// <summary>
        /// Sets te hole information to be displayed in the bottom left of the screen.
        /// </summary>
        /// <param name="hole"></param>
        /// <param name="par"></param>
        /// <param name="distance"></param>
        public static void SetHoleDisplay()
        {
            string _hole = GetLabelText("GOLF_HOLE_NUM").Replace("~1~", currentHole.ToString());
            string _par = GetLabelText("GOLF_PAR_NUM").Replace("~1~", holes[currentHole - 1].Par.ToString());
            string _dist = GetLabelText("DIST").Replace("~1~", holes[currentHole - 1].Distance.ToString());

            Scale.CallFunction("SET_HOLE_DISPLAY", _hole, _par, _dist);
        }

        /// <summary>
        /// Set the display type. This type determines which elements will be visible.
        /// </summary>
        /// <param name="state"></param>
        public static void SetDisplay(int state = 15)
        {
            Scale.CallFunction("SET_DISPLAY", state);
            /*
                -1 = all
                 0 = nothing
                 1 = map
                 2 = top right
                 3 = top right + map
                 8 = player list
                 9 = player list + map
                10 = player list + top right
                11 = player list + map + top right
                16 = scoreboard
                17 = scoreboard + map
                18 = scoreboard + top right
                19 = scoreboard + top right + map
                24 = scoreboard + player list
                25 = scoreboard + player list + map
                26 = scoreboard + player list + top right
                27 = scoreboard + player list + top right + map
            */
        }

        public static void SetPlayercardSlot(int id, int state, string name, string crewTag, string pedHeadshot, int color, int score)
        {
            Scale.CallFunction("SET_PLAYERCARD_SLOT",
                id,
                state,
                name,
                "|*+" + crewTag,
                pedHeadshot, // txd
                pedHeadshot, // txn
                color, // ball color
                score,
                score > 36 ? 6 : 18 // score color
                );
        }

        public enum HeadsetIcon
        {
            NONE = 0,
            INACTIVE = 1,
            ACTIVE = 2,
            MUTED = 3
        }

        public static void SetPlayercardHeadset(int id, HeadsetIcon icon)
        {
            Scale.CallFunction("SET_PLAYERCARD_HEADSET", id, (int)icon);
        }

        public static void SetScoreboardSlot(int row, int state, string name, string crew, string unused, int color, int score, string hole1, string hole2, string hole3, string hole4, string hole5, string hole6, string hole7, string hole8, string hole9)
        {
            Scale.CallFunction("SET_SCOREBOARD_SLOT", row, state, name, crew, unused, color, score, score > 36 ? 6 : 18, hole1, hole2, hole3, hole4, hole5, hole6, hole7, hole8, hole9);
        }

        private void CoursePar()
        {
            BeginScaleformMovieMethod(Scale.Handle, "COURSE_PAR");
            ScaleformAddTextLabel("PAR_5");
            ScaleformAddTextLabel("PAR_4");
            ScaleformAddTextLabel("PAR_3");
            ScaleformAddTextLabel("PAR_4");
            ScaleformAddTextLabel("PAR_4");
            ScaleformAddTextLabel("PAR_3");
            ScaleformAddTextLabel("PAR_4");
            ScaleformAddTextLabel("PAR_5");
            ScaleformAddTextLabel("PAR_4");
            ScaleformAddTextLabel("PAR_TOTAL");
            EndScaleformMovieMethod();
        }

        private void SetScoreboardTitle()
        {
            BeginScaleformMovieMethod(Scale.Handle, "SET_SCOREBOARD_TITLE");
            ScaleformAddTextLabel("TITLE_STANDING");
            ScaleformAddTextLabel("HOLE_ALLCAPS");
            ScaleformAddTextLabel("PAR_ALLCAPS");
            ScaleformAddTextLabel("SCORE_ALLCAPS");
            ScaleformAddTextLabel("SCORE_HOLEINONE");
            ScaleformAddTextLabel("SCORE_BELOW_PAR");
            ScaleformAddTextLabel("SCORE_ABOVE_PAR");
            ScaleformAddTextLabel("HOLE_1");
            ScaleformAddTextLabel("HOLE_2");
            ScaleformAddTextLabel("HOLE_3");
            ScaleformAddTextLabel("HOLE_4");
            ScaleformAddTextLabel("HOLE_5");
            ScaleformAddTextLabel("HOLE_6");
            ScaleformAddTextLabel("HOLE_7");
            ScaleformAddTextLabel("HOLE_8");
            ScaleformAddTextLabel("HOLE_9");
            EndScaleformMovieMethod();
        }

        public static void ScaleformAddTextLabel(string label)
        {
            BeginTextCommandScaleformString(label);
            EndTextCommandScaleformString();
        }

        public static void SetSwingDisplay()
        {
            BeginScaleformMovieMethod(Scale.Handle, "SET_SWING_DISPLAY");
            PushScaleformMovieMethodParameterInt(47);   // state
            ScaleformAddTextLabel("collision_hu79du");  // lie text label (green/fairway/etc)
            PushScaleformMovieMethodParameterInt(4);    // lieEnum

            BeginTextCommandScaleformString("GOLF_WIND_PLUS");
            AddTextComponentInteger(3);                         // wind force
            EndTextCommandScaleformString();

            PushScaleformMovieMethodParameterFloat(35.0f);                                  // wind direction
            ScaleformAddTextLabel(GolfClubsData[currentClubIndex].Key);                     // club label
            PushScaleformMovieMethodParameterInt(GolfClubsData[currentClubIndex].Value);    // club icon
            ScaleformAddTextLabel(ShotPowerTypes[currentShotTypeIndex]);                    // swing type label
            PushScaleformMovieMethodParameterBool(true);                                    // swing type changeable arrows
            ScaleformAddTextLabel("GOLF_SPIN");                                             // spin label
            PushScaleformMovieMethodParameterInt(20);                                       // spin force
            PushScaleformMovieMethodParameterFloat(218f);                                   // spin direction

            BeginTextCommandScaleformString("SHOT_NUM");
            AddTextComponentInteger(currentShot);                     // shot index
            EndTextCommandScaleformString();
        }
    }
}
