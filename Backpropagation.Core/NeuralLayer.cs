using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpropagation.Core
{
    /// <summary>
    /// Class represents artificial neural network layer
    /// </summary>
    public class NeuralLayer
    {
        private readonly int _inputCount;
        private readonly int _outputCount;
        private Double[] Inputs;
        public NeuralLayer(int inputCount, int outputCount, Func<Double[], Double[], Double> activationFunc)
        {
            _inputCount = inputCount;
            _outputCount = outputCount;

            GenerateNeurons(_outputCount, activationFunc);
        }

        public void InitInputValues(Double[] inputValues)
        {
            Inputs = inputValues;
        }
        private void GenerateNeurons(int outputCount, Func<Double[], Double[], Double> activationFunc)
        {
            Neurons = new List<Neuron>();
            for (int i = 0; i < outputCount; i++)
            {
                var n = new Neuron(_inputCount, activationFunc);
                n.InitInputValues(Inputs);
                Neurons.Add(n);
            }
        }

        public ICollection<Neuron> Neurons { get; set; }

        public Double[] GetLayerOutput()
        {
            var arr = new Double[_outputCount];
            int i = 0;
            foreach (var neuron in Neurons)
            {
                neuron.InitInputValues(Inputs);
                arr[i++] = neuron.Output;
            }
            return arr;
        }
    }
}
