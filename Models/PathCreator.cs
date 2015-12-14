using Mathematics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Models
{
    public class PathCreator
    {
        private List<BezierSurface> surfaces;
        private List<Vector4> pointsCloud;
        private Dictionary<Vector4, Tuple<double, double>> pointsParametrizations;
        private double[,] heightMap;
        //private double[,] envelopeMap;
        //private List<Vector4> envelopePoints;
        private const int mapSize = 150;
        private const double EPSILON = 0.0001;
        private int materialReserveSize = 2;
        private int sphereMillerRadius = 8;
        private int sphereSmallMillerRadius = 4;
        private int flatMillerRadius = 6;
        private int baseSize = 21;
        private int firstCutSize = 40;
        private List<GeometricModel> models;

        public PathCreator() { }

        public PathCreator(List<BezierSurface> surfaces, List<GeometricModel> models)
        {
            this.surfaces = surfaces;
            this.models = models;
            pointsCloud = new List<Vector4>();
            heightMap = new double[mapSize, mapSize];
            pointsParametrizations = new Dictionary<Vector4, Tuple<double, double>>();
            //envelopeMap = new double[mapSize, mapSize];
            //envelopePoints = new List<Vector4>();
            //GeneratePointsCloud(models);
            LoadPointSCloud();
        }

        private void LoadPointSCloud()
        {
            using (StreamReader reader = new StreamReader("../../../../PUSN/3CProcessingSimulation/3CProcessingSimulation/resources/paths/heightMap.txt"))
            {
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        heightMap[i, j] = double.Parse(reader.ReadLine(), CultureInfo.InvariantCulture) - 0.21;
                    }
                }
            }
        }

        private List<Vector4> LoadFrontHandPoints()
        {
            var result = new List<Vector4>();
            using (StreamReader reader = new StreamReader("../../../../PUSN/3CProcessingSimulation/3CProcessingSimulation/resources/paths/front.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var numbers = Regex.Split(line, " ");
                    var u = double.Parse(numbers[0], CultureInfo.InvariantCulture);
                    var v = double.Parse(numbers[1], CultureInfo.InvariantCulture);
                    var s = double.Parse(numbers[2], CultureInfo.InvariantCulture);
                    var t = double.Parse(numbers[3], CultureInfo.InvariantCulture);
                    result.Add(new Vector4(u, v, s, t));
                }
            }
            return result;
        }

        private List<Vector4> LoadBackHandPoints()
        {
            var result = new List<Vector4>();
            using (StreamReader reader = new StreamReader("../../../../PUSN/3CProcessingSimulation/3CProcessingSimulation/resources/paths/back.txt"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var numbers = Regex.Split(line, " ");
                    var u = double.Parse(numbers[0], CultureInfo.InvariantCulture);
                    var v = double.Parse(numbers[1], CultureInfo.InvariantCulture);
                    var s = double.Parse(numbers[2], CultureInfo.InvariantCulture);
                    var t = double.Parse(numbers[3], CultureInfo.InvariantCulture);
                    result.Add(new Vector4(u, v, s, t));
                }
            }
            return result;
        }

        private void GeneratePointsCloud(List<GeometricModel> models)
        {
            var minX = mapSize;
            var maxX = 0;
            var minY = mapSize;
            var maxY = 0;
            var maxZ = 0.0;
            foreach (var item in surfaces)
            {
                for (int patchNum = 0; patchNum < item.PatchesBreadthCount * item.PatchesLengthCount; patchNum++)
                {
                    for (double u = 0; u <= 1; u += 0.02)
                    {
                        for (double v = 0; v <= 1; v += 0.02)
                        {
                            var point = item.GetSurfacePoint(u, v, patchNum);
                            //models.Add(new Point(point));
                            pointsCloud.Add(point);
                            var x = (int)Math.Round((point.X + 1) * mapSize / 2);
                            var y = (int)Math.Round((point.Y + 1) * mapSize / 2);
                            var z = point.Z;
                            minX = (minX > x) ? x : minX;
                            maxX = (maxX < x) ? x : maxX;
                            minY = (minY > y) ? y : minY;
                            maxY = (maxY < y) ? y : maxY;
                            maxZ = (maxZ < z) ? z : maxZ;
                            if (heightMap[x, y] < z)
                            {
                                heightMap[x, y] = z;
                            }
                            if (Math.Abs(z) < EPSILON)
                            {
                                models.Add(new Point(point));
                            }

                        }
                    }
                }
            }
            using (StreamWriter outputFile = new StreamWriter("../../../../PUSN/3CProcessingSimulation/3CProcessingSimulation/resources/paths/heightMap.txt"))
            {
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        var result = (heightMap[i, j] + 0.2 > 0.5) ? 0.5 : heightMap[i, j] + 0.21;
                        outputFile.WriteLine(result.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        public void GenerateRoughPathes()
        {
            using (StreamWriter outputFile = new StreamWriter("../../../../PUSN/3CProcessingSimulation/3CProcessingSimulation/resources/paths/Rough.k16"))
            {
                Vector4 savePlace = new Vector4(-75.000, -100.000, 40.000);
                int counter = 3;
                int halfBorderSize = 75 + sphereMillerRadius;
                int halfSize = 75;
                int step = 8;

                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, savePlace.X, savePlace.Y, savePlace.Z));
                for (int i = 0; i < mapSize + step / 2; i += step)
                {
                    if (i >= mapSize) i = mapSize - 1;
                    outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, i - halfSize, -halfBorderSize, firstCutSize));
                    outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, i - halfSize, halfBorderSize, firstCutSize));

                    outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, i - halfSize, halfBorderSize, baseSize));
                    double lastMaxZ = 0;
                    int lastMaxZIndex = 0;
                    for (int j = mapSize - 1; j >= 0; j--)
                    {
                        double maxZ = 0;
                        for (int k = -sphereMillerRadius; k < sphereMillerRadius; k++)
                        {
                            for (int l = -sphereMillerRadius; l < sphereMillerRadius; l++)
                            {
                                if (i + k < 0) continue;
                                if (i + k >= mapSize) continue;
                                if (j + l < 0) continue;
                                if (j + l >= mapSize) continue;
                                if (heightMap[i + k, j + l] > maxZ)
                                    maxZ = heightMap[i + k, j + l];
                            }
                        }
                        if (maxZ == lastMaxZ)
                            continue;
                        if (j + 1 != lastMaxZIndex)
                            outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                                counter++, i - halfSize, j - halfSize + 1,
                                lastMaxZ * 100 + baseSize + ((lastMaxZ <= 0) ? 0 : materialReserveSize)));
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, i - halfSize, j - halfSize,
                            maxZ * 100 + baseSize + ((lastMaxZ <= 0.0) ? 0 : materialReserveSize)));
                        lastMaxZ = maxZ;
                        lastMaxZIndex = j;
                    }
                    outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, i - halfSize, -halfBorderSize, baseSize));
                }
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, savePlace.X, savePlace.Y, savePlace.Z));
            }
        }

        public void GenerateEnvelopePathes()
        {
            using (StreamWriter outputFile = new StreamWriter("../../../../PUSN/3CProcessingSimulation/3CProcessingSimulation/resources/paths/Envelope.f12"))
            {
                int counter = 3;
                int halfBorderSize = 75 + sphereMillerRadius;
                int halfSize = 75;
                Vector4 savePlace = new Vector4(-75.000, -100.000, 40.000);

                double step = 0.01;
                double borderValue = 0.1;
                var radius = (flatMillerRadius / (double)halfSize);

                var activeSurface = surfaces[0];
                // CORE TOP
                var coreTopEnvelopePoints = new List<Vector4>();
                int disabler = 1;
                for (double u = 0; u <= 0.98; u += step)
                {
                    if (u > 0.87 && u < 0.96)
                        continue;
                    if (u > 0.63 && u < 0.74)
                        continue;
                    if (u > 0.37 && u < 0.47)
                    {
                        if (disabler > 0)
                        {
                            disabler--;
                            continue;
                        }
                        else
                            disabler = 2;
                    }
                    double v = 0.875;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    coreTopEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }
                models.Add(new IntersectionCurve(coreTopEnvelopePoints, null, null, Vector4.Zero()) { Color = System.Drawing.Color.Red });

                // CORE BOTTOM LEFT
                var coreBottomLeftEnvelopePoints = new List<Vector4>();
                for (double u = 0.97; u >= 0.74; u -= step)
                {
                    if (u > 0.86 && u < 0.96)
                        continue;
                    double v = 0.375;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    coreBottomLeftEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }
                models.Add(new IntersectionCurve(coreBottomLeftEnvelopePoints, null, null, Vector4.Zero()) { Color = System.Drawing.Color.Red });

                step = 0.025;
                // CORE BOTTOM MIDDLE
                var coreBottomMiddleEnvelopePoints = new List<Vector4>();
                for (double u = 0.36; u >= 0.25; u -= step)
                {
                    double v = 0.375;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    coreBottomMiddleEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }
                models.Add(new IntersectionCurve(coreBottomMiddleEnvelopePoints, null, null, Vector4.Zero()) { Color = System.Drawing.Color.Red });

                step = 0.01;
                // CORE BOTTOM RIGHT
                var coreBottomRightEnvelopePoints = new List<Vector4>();
                for (double u = 0.03; u >= 0.0; u -= step)
                {
                    double v = 0.375;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    coreBottomRightEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }
                step = 0.05;
                for (double v = 0.5; v <= 0.72; v += step)
                {
                    double u = 0.01;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    coreBottomRightEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }
                models.Add(new IntersectionCurve(coreBottomRightEnvelopePoints, null, null, Vector4.Zero()) { Color = System.Drawing.Color.Red });


                activeSurface = surfaces[3];
                // FRONT HAND
                step = 0.03;
                var frontHandEnvelopePoints = new List<Vector4>();

                for (double u = 0.9; u >= 0.01; u -= step)
                {
                    if (u > 0.6 && u < 0.75)
                        continue;
                    double v = 0.25;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    frontHandEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }

                if (true)
                {
                    double vL = 0.25;
                    double vR = 0.75;
                    double uC = 0;
                    var surfacePointR = activeSurface.GetSurfacePoint(uC, vR);
                    surfacePointR.Z = 0;
                    var surfaceNormalY = new Vector4(0, -1, 0);
                    var millerPositionR = surfacePointR + surfaceNormalY * radius;
                    models.Add(new Point(millerPositionR));

                    var surfacePointL = activeSurface.GetSurfacePoint(uC, vL);
                    surfacePointL.Z = 0;
                    var millerPositionL = surfacePointL + surfaceNormalY * radius;
                    models.Add(new Point(millerPositionL));

                    var surfacePointLL = surfacePointL;
                    surfacePointLL.Z = 0;
                    var surfaceNormalLL = activeSurface.GetSurfaceNormal(uC, vL);
                    var millerPositionLL = surfacePointLL + surfaceNormalLL * radius * 2;
                    millerPositionLL.Y -= 0.07;
                    models.Add(new Point(millerPositionLL));

                    var surfacePointRR = surfacePointR;
                    surfacePointRR.Z = 0;
                    var surfaceNormalRR = activeSurface.GetSurfaceNormal(uC, vR);
                    var millerPositionRR = surfacePointRR + surfaceNormalRR * radius * 2;
                    millerPositionRR.Y -= 0.07;
                    models.Add(new Point(millerPositionRR));

                    frontHandEnvelopePoints.Add(millerPositionLL);
                    frontHandEnvelopePoints.Add(millerPositionL);
                    frontHandEnvelopePoints.Add(millerPositionR);
                    frontHandEnvelopePoints.Add(millerPositionRR);
                }

                for (double u = 0.01; u <= 0.81; u += step)
                {
                    if (u > 0.6 && u < 0.78)
                        continue;
                    double v = 0.75;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    frontHandEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }
                models.Add(new IntersectionCurve(frontHandEnvelopePoints, null, null, Vector4.Zero()) { Color = System.Drawing.Color.Red });


                activeSurface = surfaces[2];
                // BACK HAND
                var backHandEnvelopePoints = new List<Vector4>();
                for (double u = 0.78; u >= 0.01; u -= step)
                {
                    double v = 0.25;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    backHandEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }

                if (true)
                {
                    double vL = 0.25;
                    double vR = 0.75;
                    double uC = 0;
                    var surfacePointR = activeSurface.GetSurfacePoint(uC, vR);
                    surfacePointR.Z = 0;
                    var surfaceNormalY = new Vector4(0, -1, 0);
                    var millerPositionR = surfacePointR + surfaceNormalY * radius;
                    models.Add(new Point(millerPositionR));

                    var surfacePointL = activeSurface.GetSurfacePoint(uC, vL);
                    surfacePointL.Z = 0;
                    var millerPositionL = surfacePointL + surfaceNormalY * radius;
                    models.Add(new Point(millerPositionL));

                    var surfacePointLL = surfacePointL;
                    surfacePointLL.Z = 0;
                    var surfaceNormalLL = activeSurface.GetSurfaceNormal(uC, vL);
                    var millerPositionLL = surfacePointLL + surfaceNormalLL * radius * 1.8;
                    models.Add(new Point(millerPositionLL));

                    backHandEnvelopePoints.Add(millerPositionLL);
                    backHandEnvelopePoints.Add(millerPositionL);
                    backHandEnvelopePoints.Add(millerPositionR);
                }

                for (double u = 0.01; u <= 0.89; u += step)
                {
                    double v = 0.75;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    backHandEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }
                models.Add(new IntersectionCurve(backHandEnvelopePoints, null, null, Vector4.Zero()) { Color = System.Drawing.Color.Red });

                // DRILL
                step = 0.05;
                var drillEnvelopePoints = new List<Vector4>();
                activeSurface = surfaces[1];
                for (double v = 0.2; v <= 0.92; v += step)
                {
                    double u = 1.0;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    drillEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }

                step = 0.03;
                for (double u = 0.85; u >= 0.75; u -= step)
                {
                    double v = 0.99;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    drillEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }

                for (double u = 0.65; u <= 0.75; u += step)
                {
                    double v = 0.99;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    drillEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }

                step = 0.05;
                for (double v = 0.95; v >= 0.15; v -= step)
                {
                    double u = 0.5;
                    var surfacePoint = activeSurface.GetSurfacePoint(u, v);
                    var surfaceNormal = activeSurface.GetSurfaceNormal(u, v);
                    if (surfaceNormal.Z > borderValue)
                        continue;
                    surfacePoint.Z = 0;
                    surfaceNormal.Z = 0;
                    surfaceNormal.NormalizeSecond();
                    var millerPosition = surfacePoint + surfaceNormal * radius;
                    drillEnvelopePoints.Add(millerPosition);
                    models.Add(new Point(millerPosition));
                }
                models.Add(new IntersectionCurve(drillEnvelopePoints, null, null, Vector4.Zero()) { Color = System.Drawing.Color.Red });


                var fullEnvelope = new List<Vector4>();
                fullEnvelope.AddRange(coreTopEnvelopePoints);
                fullEnvelope.AddRange(drillEnvelopePoints);
                fullEnvelope.AddRange(coreBottomLeftEnvelopePoints);
                fullEnvelope.AddRange(frontHandEnvelopePoints);
                fullEnvelope.AddRange(coreBottomMiddleEnvelopePoints);
                fullEnvelope.AddRange(backHandEnvelopePoints);
                fullEnvelope.AddRange(coreBottomRightEnvelopePoints);

                models.Add(new IntersectionCurve(fullEnvelope, null, null, Vector4.Zero()) { Color = System.Drawing.Color.Green });

                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                   counter++, savePlace.X, savePlace.Y, savePlace.Z));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, 80, -100, savePlace.Z));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, 80, 30, savePlace.Z));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, 80, 30, baseSize));

                for (int i = 0; i < fullEnvelope.Count; i++)
                {
                    var millerRealPosition = fullEnvelope[i] * halfSize;
                    millerRealPosition.Z = millerRealPosition.Z + baseSize;

                    outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, millerRealPosition.X, millerRealPosition.Y, millerRealPosition.Z));
                }

                var millerLastRealPosition = fullEnvelope[fullEnvelope.Count - 1] * halfSize;
                millerLastRealPosition.Z = millerLastRealPosition.Z + baseSize;

                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, millerLastRealPosition.X, millerLastRealPosition.Y, savePlace.Z));


                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                counter++, savePlace.X, savePlace.Y, savePlace.Z));
            }
        }

        public void GenerateFlatPathes()
        {
            using (StreamWriter outputFile = new StreamWriter("../../../../PUSN/3CProcessingSimulation/3CProcessingSimulation/resources/paths/Flat.f12"))
            {
                Vector4 savePlace = new Vector4(-75.000, -100.000, 40.000);
                int counter = 3;
                int halfBorderSize = 75 + sphereMillerRadius;
                int halfSize = 75;
                int step = 10;

                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, savePlace.X, savePlace.Y, savePlace.Z));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, savePlace.X, savePlace.Y, baseSize));

                for (int i = 0; i < mapSize + step / 2; i += step)
                {
                    if (i >= mapSize) i = mapSize - 1;
                    outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, i - halfSize, -halfBorderSize, baseSize));
                    bool pathCreated = false;
                    for (int j = 0; j < mapSize; j++)
                    {
                        double maxZ = 0;
                        for (int k = -flatMillerRadius; k < flatMillerRadius; k++)
                        {
                            for (int l = -flatMillerRadius; l < flatMillerRadius; l++)
                            {
                                if (i + k < 0) continue;
                                if (i + k >= mapSize) continue;
                                if (j + l < 0) continue;
                                if (j + l >= mapSize) continue;
                                if (heightMap[i + k, j + l] > maxZ)
                                    maxZ = heightMap[i + k, j + l];
                            }
                        }
                        if (maxZ == 0.0)
                            continue;
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, i - halfSize, j - halfSize - 1 - flatMillerRadius / 2, baseSize));
                        pathCreated = true;
                        break;
                    }
                    if (!pathCreated)
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, i - halfSize, halfBorderSize, baseSize));
                    outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, i - halfSize, -halfBorderSize, baseSize));
                }

                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, halfBorderSize, -halfBorderSize, baseSize));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, halfBorderSize, halfBorderSize, baseSize));

                for (int i = mapSize - 1; i >= -step / 2; i -= step)
                {
                    if (i < 0) i = 0;
                    outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, i - halfSize, halfBorderSize, baseSize));
                    bool pathCreated = false;
                    for (int j = mapSize - 1; j >= 0; j--)
                    {
                        double maxZ = 0;
                        for (int k = -flatMillerRadius; k < flatMillerRadius; k++)
                        {
                            for (int l = -flatMillerRadius; l < flatMillerRadius; l++)
                            {
                                if (i + k < 0) continue;
                                if (i + k >= mapSize) continue;
                                if (j + l < 0) continue;
                                if (j + l >= mapSize) continue;
                                if (heightMap[i + k, j + l] > maxZ)
                                    maxZ = heightMap[i + k, j + l];
                            }
                        }
                        if (maxZ == 0.0)
                            continue;
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, i - halfSize, j - halfSize + 1 + flatMillerRadius / 2, baseSize));
                        pathCreated = true;
                        break;
                    }
                    if (!pathCreated)
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, i - halfSize, -halfBorderSize, baseSize));
                    outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                        counter++, i - halfSize, halfBorderSize, baseSize));
                }
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                counter++, savePlace.X, savePlace.Y, savePlace.Z));
            }
        }

        public void GenerateAccuratePathes()
        {
            var frontIntersectionParameters = LoadFrontHandPoints();
            var backIntersectionParameters = LoadBackHandPoints();
            var frontIntersectionPoints = frontIntersectionParameters.Select((x) => { return surfaces[0].GetSurfacePoint(x.X, x.Y); }).ToList();
            var backIntersectionPoints = backIntersectionParameters.Select((x) => surfaces[0].GetSurfacePoint(x.X, x.Y)).ToList();

            using (StreamWriter outputFile = new StreamWriter("../../../../PUSN/3CProcessingSimulation/3CProcessingSimulation/resources/paths/Accurate.k08"))
            {
                Vector4 savePlace = new Vector4(-75.000, -100.000, 40.000);
                int counter = 3;
                double stepU = 0.009;
                double stepV = 0.012;
                int halfSize = 75;
                var radius = (sphereSmallMillerRadius / (double)halfSize);
                var distanceFactor = Math.Sqrt(2) * 0.8;
                var radiusFactor = 1.2;

                // CORE
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, savePlace.X, savePlace.Y, savePlace.Z));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, 80, -100, savePlace.Z));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, 80, 80, savePlace.Z));

                var activeSurface = surfaces[0];
                bool forward = true;
                Vector4 lastMillerPosition = Vector4.Zero();
                for (double u = 0; u <= 0.975; u += stepU)
                {
                    for (double v = 0.875; v <= 1.375; v += stepV)
                    {
                        var vv = forward ? v : 1.375 - v + 0.875;
                        vv = (vv > 1) ? vv - 1 : vv;
                        var surfacePoint = activeSurface.GetSurfacePoint(u, vv);
                        if (surfacePoint.Z < 0)
                            continue;
                        var surfaceNormal = activeSurface.GetSurfaceNormal(u, vv);
                        var millerPosition = surfacePoint + surfaceNormal * radius;
                        var millerRealPosition = millerPosition * halfSize;
                        millerRealPosition.Z = millerRealPosition.Z - sphereSmallMillerRadius + baseSize;
                        if (millerRealPosition.Z < baseSize)
                            continue;
                        bool conflict = false;
                        foreach (var point in frontIntersectionPoints)
                        {
                            if (Vector4.Distance3(millerPosition, point) < radius * distanceFactor ||
                                Vector4.Distance3(new Vector4(millerPosition.X, millerPosition.Y + radius,
                                    millerPosition.Z), point) < radius * distanceFactor * radiusFactor)
                            {
                                conflict = true;
                                break;
                            }
                        }
                        if (conflict)
                            continue;
                        foreach (var point in backIntersectionPoints)
                        {
                            if (Vector4.Distance3(millerPosition, point) < radius * distanceFactor ||
                                Vector4.Distance3(new Vector4(millerPosition.X, millerPosition.Y + radius,
                                    millerPosition.Z), point) < radius * distanceFactor * radiusFactor)
                            {
                                conflict = true;
                                break;
                            }
                        }
                        if (conflict)
                            continue;
                        models.Add(new Point(millerPosition));
                        lastMillerPosition = millerRealPosition;
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, millerRealPosition.X, millerRealPosition.Y, millerRealPosition.Z));
                    }
                    forward = !forward;
                }

                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, lastMillerPosition.X, lastMillerPosition.Y, savePlace.Z));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, savePlace.X, savePlace.Y, savePlace.Z));

                distanceFactor = Math.Sqrt(2) * 0.9;
                radiusFactor = 1.1;
                // FRONT HAND
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, 0.0, savePlace.Y, savePlace.Z));

                stepU = 0.015;
                stepV = 0.025;
                activeSurface = surfaces[3];
                forward = true;
                for (double u = 0; u <= 1; u += stepU)
                {
                    for (double v = 0.75; v <= 1.25; v += stepV)
                    {
                        var vv = forward ? v : 1.25 - v + 0.75;
                        vv = (vv > 1) ? vv - 1 : vv;
                        var surfacePoint = activeSurface.GetSurfacePoint(u, vv);
                        if (surfacePoint.Z < 0)
                            continue;
                        var surfaceNormal = activeSurface.GetSurfaceNormal(u, vv);
                        var millerPosition = surfacePoint + surfaceNormal * radius;
                        var millerRealPosition = millerPosition * halfSize;
                        millerRealPosition.Z = millerRealPosition.Z - sphereSmallMillerRadius + baseSize;
                        if (millerRealPosition.Z < baseSize)
                            continue;
                        bool conflict = false;
                        foreach (var point in frontIntersectionPoints)
                        {
                            if (Vector4.Distance3(millerPosition, point) < radius * distanceFactor ||
                                Vector4.Distance3(new Vector4(millerPosition.X, millerPosition.Y - radius,
                                    millerPosition.Z), point) < radius * distanceFactor * radiusFactor)
                            {
                                conflict = true;
                                break;
                            }
                        }
                        if (conflict)
                            continue;
                        models.Add(new Point(millerPosition));
                        lastMillerPosition = millerRealPosition;
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, millerRealPosition.X, millerRealPosition.Y, millerRealPosition.Z));
                    }
                    forward = !forward;
                }

                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, lastMillerPosition.X, lastMillerPosition.Y, savePlace.Z));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, savePlace.X, savePlace.Y, savePlace.Z));

                // BACK HAND
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, 37.5, savePlace.Y, savePlace.Z));

                activeSurface = surfaces[2];
                forward = true;
                for (double u = 0; u <= 1; u += stepU)
                {
                    for (double v = 0.75; v <= 1.25; v += stepV)
                    {
                        var vv = forward ? v : 1.25 - v + 0.75;
                        vv = (vv > 1) ? vv - 1 : vv;
                        var surfacePoint = activeSurface.GetSurfacePoint(u, vv);
                        if (surfacePoint.Z < 0)
                            continue;
                        var surfaceNormal = activeSurface.GetSurfaceNormal(u, vv);
                        var millerPosition = surfacePoint + surfaceNormal * radius;
                        var millerRealPosition = millerPosition * halfSize;
                        millerRealPosition.Z = millerRealPosition.Z - sphereSmallMillerRadius + baseSize;
                        if (millerRealPosition.Z < baseSize)
                            continue;
                        bool conflict = false;
                        foreach (var point in backIntersectionPoints)
                        {
                            if (Vector4.Distance3(millerPosition, point) < radius * distanceFactor ||
                                Vector4.Distance3(new Vector4(millerPosition.X, millerPosition.Y - radius,
                                    millerPosition.Z), point) < radius * distanceFactor * radiusFactor)
                            {
                                conflict = true;
                                break;
                            }
                        }
                        if (conflict)
                            continue;
                        models.Add(new Point(millerPosition));
                        lastMillerPosition = millerRealPosition;
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, millerRealPosition.X, millerRealPosition.Y, millerRealPosition.Z));
                    }
                    forward = !forward;
                }

                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, lastMillerPosition.X, lastMillerPosition.Y, savePlace.Z));
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, savePlace.X, savePlace.Y, savePlace.Z));

                // DRILL

                stepU = 0.05;
                stepV = 0.04;
                activeSurface = surfaces[1];
                forward = true;
                bool first = true;
                for (double v = 0.2; v <= 1; v += stepV)
                {
                    for (double u = 0.249; u <= 0.498; u += stepU)
                    {
                        var uu = forward ? 0.498 - u : u - 0.249;
                        var surfacePoint = activeSurface.GetSurfacePoint(uu, v);
                        if (surfacePoint.Z < 0)
                            continue;
                        var surfaceNormal = activeSurface.GetSurfaceNormal(uu, v);
                        var millerPosition = surfacePoint + surfaceNormal * radius;
                        var millerRealPosition = millerPosition * halfSize;
                        millerRealPosition.Z = millerRealPosition.Z - sphereSmallMillerRadius + baseSize;
                        if (millerRealPosition.Z < baseSize)
                            continue;
                        models.Add(new Point(millerPosition));
                        lastMillerPosition = millerRealPosition;
                        if (first)
                        {
                            first = false;
                            outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                                counter++, millerRealPosition.X, millerRealPosition.Y, savePlace.Z));
                        }
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, millerRealPosition.X, millerRealPosition.Y, millerRealPosition.Z));
                    }
                    forward = !forward;
                }
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, lastMillerPosition.X, lastMillerPosition.Y, savePlace.Z));

                activeSurface = surfaces[1];
                forward = true;
                first = true;
                for (double v = 0.2; v <= 1; v += stepV)
                {
                    for (double u = 0.251; u <= 0.5; u += stepU)
                    {
                        var uu = forward ? u : 0.5 - u + 0.251;
                        var surfacePoint = activeSurface.GetSurfacePoint(uu, v);
                        if (surfacePoint.Z < 0)
                            continue;
                        var surfaceNormal = activeSurface.GetSurfaceNormal(uu, v);
                        var millerPosition = surfacePoint + surfaceNormal * radius;
                        var millerRealPosition = millerPosition * halfSize;
                        millerRealPosition.Z = millerRealPosition.Z - sphereSmallMillerRadius + baseSize;
                        if (millerRealPosition.Z < baseSize)
                            continue;
                        models.Add(new Point(millerPosition));
                        lastMillerPosition = millerRealPosition;
                        if (first)
                        {
                            first = false;
                            outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                                counter++, millerRealPosition.X, millerRealPosition.Y, savePlace.Z));
                        }
                        outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                            counter++, millerRealPosition.X, millerRealPosition.Y, millerRealPosition.Z));
                    }
                    forward = !forward;
                }
                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                    counter++, lastMillerPosition.X, lastMillerPosition.Y, savePlace.Z));

                outputFile.WriteLine(String.Format(CultureInfo.InvariantCulture, "N{0}G01X{1}Y{2}Z{3}",
                counter++, savePlace.X, savePlace.Y, savePlace.Z));
            }
        }
    }

    public struct Indexer
    {
        public int X;
        public int Y;

        public Indexer(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
