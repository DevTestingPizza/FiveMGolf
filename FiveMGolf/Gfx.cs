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
        private static int currentHole = 2;
        private static int currentPar = 4;
        private static int currentDistance = 436;
        private static int currentClubIndex = 0;
        private static int currentShotTypeIndex = 0;

        public Gfx()
        {
            RequestAdditionalText("SP_GOLF", 3);
            Scale = new Scaleform("GOLF");
            RunSetup();
        }

        private async void RunSetup()
        {
            while (!Scale.IsLoaded || !HasAdditionalTextLoaded(3))
            {
                await Delay(0);
            }

            CoursePar();
            SetScoreboardTitle();
            SetPlayerList();

            int blip = AddBlipForCoord(-1321.98f, 158.93f, 57.8f);
            SetBlipSprite(blip, 358);
            SetSwingDisplay();

            Tick += OnTick;
        }

        private async Task OnTick()
        {


            if (Game.IsControlPressed(0, Control.ReplayShowhotkey)) // k
            {
                SetDisplay(31); // scoreboard
            }
            else
            {
                SetDisplay(15); // normal
            }

            SetHoleDisplay(currentHole, currentPar, currentDistance);
            SetRadarZoom(850);
            N_0x71bdb63dbaf8da59(2);
            LockMinimapPosition(-1220.0f, 240.0f);
            LockMinimapAngle(90);
            ToggleStealthRadar(false);
            SetRadarBigmapEnabled(false, false);

            Scale.Render2D();



            if (ReturnTwo(0) == 1)
                await Delay(0);
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
        public static void SetHoleDisplay(int hole, int par, int distance)
        {
            string _hole = GetLabelText("GOLF_HOLE_NUM").Replace("~1~", hole.ToString());
            string _par = GetLabelText("GOLF_PAR_NUM").Replace("~1~", par.ToString());
            string _dist = GetLabelText("DIST").Replace("~1~", distance.ToString());

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

            PushScaleformMovieMethodParameterFloat(35.0f);  // wind direction
            ScaleformAddTextLabel("collision_hmhne2");      // club label
            PushScaleformMovieMethodParameterInt(1);        // club icon
            ScaleformAddTextLabel("collision_9b95m6d");     // swing type label
            PushScaleformMovieMethodParameterBool(true);    // swing type changeable arrows
            ScaleformAddTextLabel("GOLF_SPIN");             // spin label
            PushScaleformMovieMethodParameterInt(20);       // spin force
            PushScaleformMovieMethodParameterFloat(218f);   // spin direction

            BeginTextCommandScaleformString("SHOT_NUM");
            AddTextComponentInteger(1);                     // shot index
            EndTextCommandScaleformString();
        }
    }
}
