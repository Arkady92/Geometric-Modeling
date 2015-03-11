using System.Drawing;
using Mathematics;

namespace Models
{
    public abstract class GeometricModel
    {
        public ModelType Type;

        public Matrix CurrentOperationsMatrix;

        protected GeometricModel(ModelType type)
        {
            Type = type;
            CurrentOperationsMatrix = OperationsMatrices.Identity();
        }

        public abstract void Draw(Graphics graphics, /*Matrix currentOperationsMatrix,*/ Matrix currentProjectionMatrix);

        public abstract void UpdateModel();
    }
}
