﻿using System.Drawing;
namespace Mathematics
{
    public static class Parameters
    {
        public static int GridResolutionX = 30;
        public static int GridResolutionY = 10;
        public static int SurfaceGridResolutionX = 4;
        public static int SurfaceGridResolutionY = 4;
        public static int WorldPanelWidth;
        public static int WorldPanelHeight;
        public static double Illuminance = 2;
        public static double XAxisFactor = 200;
        public static double YAxisFactor = 100;
        public static double ZAxisFactor = 100;
        public static int PixelMaxSize = 80;
        public static float WorldPanelSizeFactor;
        public static double MouseInaccuracy = 5;
        public static double CursorMoveValue = 0.01;
        public const string DefaultFilePath = "Scene.mg1";
        public static double IntersectionAccuracy = 0.01;
    }
}
