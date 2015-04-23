using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mathematics;
using System;
using MathNet.Numerics.LinearAlgebra.Double;
using Matrix = Mathematics.Matrix;

namespace Models
{
    public class InterpolationCurve : BezierCurve
    {
        private int _increment = 1;
        private BezierCurveC2 _bezierCurve;

        public InterpolationCurve(Vector4 position)
            : base(position, ModelType.InterpolationCurve)
        {
            ControlPointsEnabled = false;
            ChordParametrizationEnabled = true;
            CalculateBSplineCurve();
        }

        public InterpolationCurve(IEnumerable<Point> points, Vector4 position, bool chordParametrizationEnabled = true) :
            base(points, position, ModelType.InterpolationCurve)
        {
            ControlPointsEnabled = false;
            ChordParametrizationEnabled = chordParametrizationEnabled;
            CalculateBSplineCurve();
        }

        protected override void RecreateStructure(int number = 0)
        {
            Vertices.Clear();
            for (int i = 0; i < Children.Count(); i++)
            {
                var vector = Children[i].GetCurrentPositionWithoutMineTransformations(this);
                Vertices.Add(vector);
                Children[i].SetParentIndex(this, i);
            }
            CalculateBSplineCurve();
        }

        public override void PropagateTransformation(ParametricGeometricModel geometricModel = null)
        {
            base.PropagateTransformation(geometricModel);
            CalculateBSplineCurve();
        }

        public override void UpdateModel()
        {
            CalculateBSplineCurve();
        }

        public override void UpdateVertex(int number)
        {
            base.UpdateVertex(number);
            CalculateBSplineCurve();
        }

        private void CalculateBSplineCurve()
        {
            var points = Children.Cast<Point>().ToList();
            var degree = points.Count - 1;
            if (points.Count > 4)
                degree = 3;
            if (degree < 2)
            {
                _bezierCurve = new BezierCurveC2(points, SpacePosition) { ControlPointsEnabled = false, ChainEnabled = false };
                return;
            }
            points.Insert(0, new Point(points[0].GetCurrentPosition()));
            points.Insert(points.Count - 1, new Point(points[points.Count - 1].GetCurrentPosition()));
            var knots = new double[points.Count + 3 + 1];
            if (ChordParametrizationEnabled)
            {
                var distances = new double[points.Count - 1];
                for (int i = 0; i < points.Count - 1; i++)
                    distances[i] =
                        Math.Abs(Vector4.Distance3(points[i + 1].GetCurrentPosition(), points[i].GetCurrentPosition()));
                var sum = distances.Sum();
                var step = 1.0 / (knots.Length - 1);
                for (int i = 0; i < knots.Length; i++)
                {
                    if (i < (knots.Length - points.Count + 2) / 2 + 1)
                        knots[i] = i * step;
                    else if (i >= (knots.Length + points.Count - 2) / 2 - 1)
                        knots[i] = i * step;
                    else
                        knots[i] = knots[i - 1] + distances[i - (knots.Length - points.Count + 2) / 2] / sum
                            * (points.Count - 3) * step;
                }
            }
            else
            {
                var step = 1.0 / (knots.Length - 1);
                for (int i = 0; i < knots.Length; i++)
                    knots[i] = i * step;
            }

            var taus = new double[points.Count - 2];
            for (int i = 0; i < points.Count - 2; i++)
                taus[i] = knots[i + (knots.Length - points.Count + 2) / 2];

            var deBoorsPoints = FindDeBoorsPoints(knots, 3, taus, points);
            _bezierCurve = new BezierCurveC2(deBoorsPoints, SpacePosition)
            {
                ControlPointsEnabled = false,
                ChainEnabled = false,
                CurveParent = this
            };
        }

        private List<Point> FindDeBoorsPoints(double[] knots, int degree, double[] taus, List<Point> points)
        {
            var size = points.Count - 2;
            //var interpolationMatrix = new Matrix(size, size);
            //for (int i = 0; i < size; i++)
            //    for (int j = 0; j < size; j++)
            //        interpolationMatrix[i, j] = Splines.CalculateNSplineValues(knots, j + 2, degree, taus[i]);
            //var factor = 1.25;
            //interpolationMatrix[0, 0] *= factor;
            //interpolationMatrix[size - 1, size - 1] *= factor;
            //interpolationMatrix = interpolationMatrix.Invert();
            var interpolationMatrix = new Matrix(size, degree);
            for (int i = 0; i < size; i++)
                for (int j = 0; j < degree; j++)
                {
                    var k = j + i - 1;
                    if (k < 0 || k >= size) continue;
                    interpolationMatrix[i, j] = Splines.CalculateNSplineValues(knots, k + 2, degree, taus[i]);
                }
            var factor = 1.25;
            interpolationMatrix[0, degree / 2] *= factor;
            interpolationMatrix[size - 1, degree / 2] *= factor;

            var resultsMatrix = new Matrix(size, 3);
            for (int i = 0; i < size; i++)
            {
                var position = points[i + 1].GetCurrentPosition();
                resultsMatrix[i, 0] = position.X;
                resultsMatrix[i, 1] = position.Y;
                resultsMatrix[i, 2] = position.Z;
            }
            //var deBoorsMatrix = interpolationMatrix * resultsMatrix;
            var deBoorsMatrix = CalculateSystemOfEquationsInLinearTime(interpolationMatrix, resultsMatrix);
            var deBoorsPoints = new List<Point>();
            deBoorsPoints.Add(new Point(new Vector4(deBoorsMatrix[0, 0], deBoorsMatrix[0, 1], deBoorsMatrix[0, 2])));
            for (int i = 0; i < size; i++)
                deBoorsPoints.Add(new Point(new Vector4(deBoorsMatrix[i, 0], deBoorsMatrix[i, 1], deBoorsMatrix[i, 2])));
            deBoorsPoints.Add(new Point(new Vector4(deBoorsMatrix[size - 1, 0], deBoorsMatrix[size - 1, 1],
                deBoorsMatrix[size - 1, 2])));

            return deBoorsPoints;
        }

        private Matrix CalculateSystemOfEquationsInLinearTime(Matrix matrix, Matrix result)
        {
            var arguments = new Matrix(result.rows, result.cols);
            var alphas = new double[result.rows];
            var betas = new double[result.rows];
            alphas[0] = matrix[0, 1];
            for (int i = 1; i < result.rows; i++)
            {
                betas[i] = matrix[i, 0] / alphas[i - 1];
                alphas[i] = matrix[i, 1] - betas[i] * matrix[i - 1, 2];
            }
            var g = new double[result.rows, 3];
            g[0, 0] = result[0, 0];
            g[0, 1] = result[0, 1];
            g[0, 2] = result[0, 2];
            for (int i = 1; i < result.rows; i++)
            {
                g[i, 0] = result[i, 0] - betas[i] * g[i - 1, 0];
                g[i, 1] = result[i, 1] - betas[i] * g[i - 1, 1];
                g[i, 2] = result[i, 2] - betas[i] * g[i - 1, 2];
            }
            arguments[result.rows - 1, 0] = g[result.rows - 1, 0] / alphas[result.rows - 1];
            arguments[result.rows - 1, 1] = g[result.rows - 1, 1] / alphas[result.rows - 1];
            arguments[result.rows - 1, 2] = g[result.rows - 1, 2] / alphas[result.rows - 1];
            for (int i = result.rows - 2; i >= 0; i--)
            {
                arguments[i, 0] = (g[i, 0] - matrix[i, 2] * arguments[i + 1, 0]) / alphas[i];
                arguments[i, 1] = (g[i, 1] - matrix[i, 2] * arguments[i + 1, 1]) / alphas[i];
                arguments[i, 2] = (g[i, 2] - matrix[i, 2] * arguments[i + 1, 2]) / alphas[i];
            }
            return arguments;
        }

        public override void Draw(Graphics graphics, Matrix currentProjectionMatrix)
        {
            base.Draw(graphics, currentProjectionMatrix);
            _bezierCurve.Draw(graphics, currentProjectionMatrix);
        }

        public override void DrawStereoscopy(Graphics graphics, Matrix leftMatrix, Matrix rightMatrix, bool additiveColorBlending = false)
        {
            base.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
            _bezierCurve.DrawStereoscopy(graphics, leftMatrix, rightMatrix, additiveColorBlending);
        }

        protected override void DrawModel(Graphics graphics, Matrix currentProjectionMatrix, Color color) { }

        protected override void DrawWithAdditiveBlending(Bitmap bitmap, Graphics graphics,
            Matrix currentProjectionMatrix, Color color) { }

        protected override void CreateEdges() { }

        public override string ToString()
        {
            if (CustomName != null)
                return "Interpolation Curve <" + CustomName + ">";
            return "Interpolation Curve <" + _increment++ + ">";
        }

        public bool ChordParametrizationEnabled { get; set; }
    }
}
