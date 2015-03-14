using System;

namespace Backpropagation.Core
{
    public interface INeuralImage
    {
        int ClassId { get; set; }
        String ClassName { get; }
        double[] Values { get; set; }
    }
}