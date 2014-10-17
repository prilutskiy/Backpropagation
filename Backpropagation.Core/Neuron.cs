using System;
using System.Linq;

namespace Backpropagation.Core
{
    /// <summary>
    /// Class represents neuron
    /// </summary>
    public class Neuron
    {
        #region Static members
        static Random r = new Random();
        #endregion
        #region Private members
        private Func<Double[], Double[], Double> ActivationFunction { get; set; }
        private Double _output;
        private readonly int _inputCount;
        private void InitWeightFactors()
        {
            for (int i = 0; i < WeightFactors.Count(); i++)
            {
                WeightFactors[i] = ((double)r.Next(-5, 5)) / 10.0;
            }
        }
        #endregion

        /// <summary>
        /// Default ctor for creating neuron instance
        /// </summary>
        /// <param name="inputCount">Count of neuon inputs</param>
        /// <param name="activationFunc">Neuron activation function</param>
        /// <param name="trainSpeed">Neuron training speed</param>
        public Neuron(int inputCount, Func<Double[], Double[], Double> activationFunc, double trainSpeed = 1)
        {
            _inputCount = inputCount;

            WeightFactors = new double[inputCount];
            ActivationFunction = activationFunc;
            InitWeightFactors();
            //TODO: Add function for setting train speed
            TrainSpeed = trainSpeed;
        }
        /// <summary>
        /// Adjusts neuron weight factors according to error signal set
        /// </summary>
        public void AdjustWeights()
        {
            for (int i = 0; i < _inputCount; i++)
            {
                WeightFactors[i] += TrainSpeed*ErrorSignal*Inputs[i]*_output*(1 - _output);
            }
        }
        /// <summary>
        /// Initializes neuron input values
        /// </summary>
        /// <param name="inputValues"></param>
        public void InitInputValues(Double[] inputValues)
        {
            Inputs = inputValues;
        }
        /// <summary>
        /// Neuron inputs
        /// </summary>
        public Double[] Inputs { get; private set; }
        /// <summary>
        /// Neuron error signal
        /// </summary>
        public Double ErrorSignal { get; set; }
        /// <summary>
        /// Neuron weight factors
        /// </summary>
        public Double[] WeightFactors { get; set; }
        /// <summary>
        /// Neuron training speed
        /// </summary>
        public Double TrainSpeed { get; private set; }
        /// <summary>
        /// Neuron output signal
        /// </summary>
        virtual public Double Output
        {
            get 
            {
                _output = ActivationFunction(Inputs, WeightFactors);
                return _output;
            }
            set { _output = value; }
        }
    }
}
