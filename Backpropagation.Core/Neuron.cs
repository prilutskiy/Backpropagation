using System;
using System.Linq;

namespace Backpropagation.Core
{
    /// <summary>
    /// Class represents neuron
    /// </summary>
    public class Neuron
    {
        private Func<Double[], Double[], Double> ActivationFunction { get; set; }
        private Double _output;
        private readonly int _inputCount;
        static Random r = new Random();
        private void InitWeightFactors()
        {
            for (int i = 0; i < WeightFactors.Count(); i++)
            {
                WeightFactors[i] = ((double) r.Next(-5, 5))/10.0;
            }
        }

        /// <summary>
        /// Default ctor for creating neuron instance
        /// </summary>
        /// <param name="inputCount">Count of neuon inputs</param>
        /// <param name="activationFunc">Neuron activation function</param>
        public Neuron(int inputCount, Func<Double[], Double[], Double> activationFunc)
        {
            _inputCount = inputCount;

            WeightFactors = new double[inputCount];
            ActivationFunction = activationFunc;
            InitWeightFactors();
            //TODO: Add function for setting train speed
            TrainSpeed = 1;
        }

        public void AdjustWeights()
        {
            for (int i = 0; i < _inputCount; i++)
            {
                WeightFactors[i] += TrainSpeed*ErrorSignal*Inputs[i]*_output*(1 - _output);
            }
        }

        public void InitInputValues(Double[] inputValues)
        {
            Inputs = inputValues;
        }

        public Double[] Inputs { get; private set; }

        public Double ErrorSignal { get; set; }
        public Double[] WeightFactors { get; set; }
        public Double TrainSpeed { get; private set; }

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
