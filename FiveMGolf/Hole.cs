using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace FiveMGolf
{
    public class Hole
    {
        public int Id { get; set; }
        public int Par { get; set; }
        public int Distance { get; set; }
        public int MapZoom { get; set; }
        public int MapAngle { get; set; }
        public Vector3 MapCenterCoords { get; set; }
        public Vector3 SpawnCoords { get; set; }
        public Vector3 HoleCoords { get; set; }
        public Vector3 TeeCoords { get; set; }

        public Hole(int id, int par, int distance, int mapZoom, int mapAngle, Vector3 mapCenter, Vector3 spawn, Vector3 holeCoords, Vector3 teeCoords)
        {
            Id = id;
            Par = par;
            Distance = distance;
            MapZoom = mapZoom;
            MapAngle = mapAngle;
            SpawnCoords = spawn;
            TeeCoords = teeCoords;
            HoleCoords = holeCoords;
            MapCenterCoords = mapCenter;
        }

    }
}
