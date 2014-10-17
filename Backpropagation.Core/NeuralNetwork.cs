using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpropagation.Core
{
    /// <summary>
    /// Class represents artificial neural network
    /// </summary>
    public class NeuralNetwork
    {
        #region Static members
        public static Double SigmoidalActivationFunction(Double[] inputValues, Double[] weights)
        {
            if (inputValues == null) throw new ArgumentException("Input values is not defined");
            var count = inputValues.Count();
            Double sum = 0;
            for (int i = 0; i < count; i++)
                sum += inputValues[i]*weights[i];
            //TODO: Inline this!
            var e = Math.E;
            var pow = Math.Pow(e, -sum);
            var s = pow + 1.0;
            var sigm = Math.Pow(s, -1);
            //
            return sigm;
        }
        #endregion

        private int LayerCount;
        public NeuralNetwork(int inputCount, int outputCount, int layerCount = 2)
        {
            if (layerCount < 2) throw new ArgumentException("Layer count cannot be less than 2.");
            InputCount = inputCount;
            OutputCount = outputCount;
            LayerCount = layerCount;
        }

        public Func<Double[], Double[], Double> ActivationFunction { get; set; }
        public Int32 InputCount { get; private set; }
        public Int32 OutputCount { get; private set; }
        public ICollection<NeuralLayer> NeuralLayers { get; set; }

        public void InitNeuralLayers()
        {
            NeuralLayers = new List<NeuralLayer>();
            if (ActivationFunction == null) throw new NullReferenceException("Activation function is not defined");
            for (int i = 0; i < LayerCount; i ++)
            {
                var l = new NeuralLayer(InputCount, i == LayerCount-1 ? OutputCount : InputCount, ActivationFunction);
                NeuralLayers.Add(l);
            }
        }

        public Double[] GetNetworkOutput(INeuralImage image)
        {
            if (NeuralLayers == null) throw new NullReferenceException("Neural layers are not initialized");
            var inputLayer = GetInputNeuralLayer();
            inputLayer.InitInputValues(image.Values);
            var tempOutValues = inputLayer.GetLayerOutput();

            var outputLayer = GetOutputNeuralLayer();
            outputLayer.InitInputValues(tempOutValues);
            var resultValues = outputLayer.GetLayerOutput();

            return resultValues;
        }
        public NeuralLayer GetInputNeuralLayer()
        {
            return NeuralLayers.First();
        }

        public NeuralLayer GetOutputNeuralLayer()
        {
            return NeuralLayers.Last();
        }
    }
}
