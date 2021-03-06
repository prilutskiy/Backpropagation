﻿using System;
using System.Collections.Generic;

namespace Backpropagation.Core
{
    /// <summary>
    ///     Class represents artificial neural network layer
    /// </summary>
    [Serializable]
    public class NeuralLayer
    {
        #region Private members

        private readonly int _inputCount;
        private readonly int _outputCount;
        private Double[] Inputs;

        private void GenerateNeurons(int outputCount, Func<Double[], Double[], Double> activationFunc)
        {
            Neurons = new List<Neuron>();
            for (var i = 0; i < outputCount; i++)
            {
                var n = new Neuron(_inputCount, activationFunc);
                n.InitInputValues(Inputs);
                Neurons.Add(n);
            }
        }

        #endregion

        #region Public members

        /// <summary>
        ///     Default ctor for creating ANN layer of neurons
        /// </summary>
        /// <param name="inputCount">Network inputs count</param>
        /// <param name="outputCount">Network outputs count</param>
        /// <param name="activationFunc">Neuron activation function</param>
        public NeuralLayer(int inputCount, int outputCount, Func<Double[], Double[], Double> activationFunc)
        {
            _inputCount = inputCount;
            _outputCount = outputCount;

            GenerateNeurons(_outputCount, activationFunc);
        }

        [Obsolete("This constructor is obsolete. Use parametrized one instead", true)]
        public NeuralLayer()
        {
        }

        /// <summary>
        ///     Initializes input values
        /// </summary>
        /// <param name="inputValues"></param>
        public void InitInputValues(Double[] inputValues)
        {
            Inputs = inputValues;
        }

        /// <summary>
        ///     Set of neurons on this layer
        /// </summary>
        public List<Neuron> Neurons { get; set; }

        /// <summary>
        ///     Get output values of the layer
        /// </summary>
        /// <returns></returns>
        public Double[] GetLayerOutput()
        {
            var arr = new Double[_outputCount];
            var i = 0;
            foreach (var neuron in Neurons)
            {
                neuron.InitInputValues(Inputs);
                arr[i++] = neuron.Output;
            }
            return arr;
        }

        #endregion
    }
}