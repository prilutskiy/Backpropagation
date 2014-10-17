using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpropagation.Core
{

    public interface INeuralImage
    {
        int Class { get; set; }
        double[] Values { get; set; }
    }
}
