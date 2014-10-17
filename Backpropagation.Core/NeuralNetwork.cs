using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backpropagation.Core
{
    /// <summary>
    /// Class represents artificial neural network
    /// </summary>
    [Serializable]
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
            var sigm = Math.Pow(Math.Pow(Math.E, -sum) + 1.0, -1);
            return sigm;
        }
        #endregion

        #region Private members
        private int LayerCount;
        private Boolean CheckResult(INeuralImage img, Double[] values)
        {
            bool result = true;
            for (int i = 0; i < values.Count(); i++)
            {
                if (i == img.ClassId)
                {
                    result &= (int)values[img.ClassId] == 1;
                }
                else
                {
                    result &= (int)values[i] == 0;
                }
            }
            return result;
        }
        private NeuralLayer GetInputNeuralLayer()
        {
            return NeuralLayers.First();
        }
        private NeuralLayer GetOutputNeuralLayer()
        {
            return NeuralLayers.Last();
        }
        private Double[] RoundValues(Double[] values)
        {
            var rounded = new double[values.Count()];
            for (int i = 0; i < rounded.Count(); i++)
                rounded[i] = Math.Round(values[i]);
            return rounded;
        }

        private Double[] GetOuputRule(INeuralImage img)
        {
            var rule = new double[OutputCount];
            rule[img.ClassId] = 1.0;
            return rule;
        }
        /// <summary>
        /// Initializes neural layers
        /// </summary>
        private void InitNeuralLayers()
        {
            NeuralLayers = new List<NeuralLayer>();
            if (ActivationFunction == null) throw new NullReferenceException("Activation function is not defined");
            for (int i = 0; i < LayerCount; i++)
            {
                var l = new NeuralLayer(InputCount, i == LayerCount - 1 ? OutputCount : InputCount, ActivationFunction);
                NeuralLayers.Add(l);
            }
        }
        /// <summary>
        /// Neuron activation function
        /// </summary>
        private Func<Double[], Double[], Double> ActivationFunction { get; set; }
        /// <summary>
        /// Adjusts weight factors for all neurons on each layer
        /// </summary>
        private void AdjustNeuronsWeights()
        {
            foreach (var neuralLayer in NeuralLayers)
                foreach (var neuron in neuralLayer.Neurons)
                {
                    neuron.AdjustWeights();
                    neuron.ErrorSignal = default(double);
                }
        }
        /// <summary>
        /// Sets error signal for each neuron
        /// </summary>
        /// <param name="outputDeviation"></param>
        private void SetNeuronErrorSignals(double[] outputDeviation)
        {
            var outputLayer = GetOutputNeuralLayer();
            int x = 0;
            foreach (var neuron in outputLayer.Neurons)//устанавливаем коэф-ты ошибки нейронов в выходном слое, основываясь на полученных выходных значениях
                neuron.ErrorSignal = outputDeviation[x++];
            for (int nc = NeuralLayers.Count - 1; nc > 0; nc--)//проходим по всем слоям, начиная с выходного
            {
                var layer = NeuralLayers.ToList()[nc];//текущий слой
                var subLayer = NeuralLayers.ToList()[nc - 1]; // предыдущий слой
                for (int i = 0; i < layer.Neurons.Count; i++)//проходим по всем нейронам в выбранном слое
                {
                    for (int j = 0; j < subLayer.Neurons.Count; j++)//проходим по всем нейронам в предыдущем слое
                    {
                        var neuron = layer.Neurons.ToList()[i]; //нейрон в основном слое
                        var subNeuron = subLayer.Neurons.ToList()[j]; // нейрон в подслое
                        subNeuron.ErrorSignal += neuron.ErrorSignal * neuron.WeightFactors[j]; //устанавливаем коэф. ошибки нейрона в подслое, основываясь на коэф.-те ошибки нейрона в основном слое
                    }
                }
            }
        }
        #endregion
        /// <summary>
        /// Default ctor for creating neural network instance
        /// </summary>
        /// <param name="inputCount">Inputs count of ANN</param>
        /// <param name="outputCount">Outputs count of ANN</param>
        /// <param name="layerCount">Layers count of ANN</param>
        /// <param name="activationFunction">Neuron activation function</param>
        public NeuralNetwork(int inputCount, int outputCount, Func<Double[], Double[], Double> activationFunction, int layerCount = 2)
        {
            if (layerCount < 2) throw new ArgumentException("Layer count cannot be less than 2.");
            InputCount = inputCount;
            OutputCount = outputCount;
            LayerCount = layerCount;
            this.ActivationFunction = activationFunction;
        }
        [Obsolete("This constructor is obsolete. Use parametrized one instead", true)]
        public NeuralNetwork()
        {
            
        }
        /// <summary>
        /// Count of inputs in ANN
        /// </summary>
        public Int32 InputCount { get; set; }
        /// <summary>
        /// Count of outputs in ANN
        /// </summary>
        public Int32 OutputCount { get; set; }
        /// <summary>
        /// Set of neural layers
        /// </summary>
        public List<NeuralLayer> NeuralLayers { get; set; }
        /// <summary>
        /// Gets output sequence produced by ANN
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public Double[] GetNetworkOutput(INeuralImage image)
        {
            if (NeuralLayers == null) throw new NullReferenceException("Neural layers are not initialized");
            //input layer
            var inputLayer = GetInputNeuralLayer();
            inputLayer.InitInputValues(image.Values);
            var tempOutValues = inputLayer.GetLayerOutput();
            //all hidden layers
            for (int i = 1; i < NeuralLayers.Count() - 1; i ++)
            {
                var hiddenLayer = NeuralLayers.ToList()[i];
                hiddenLayer.InitInputValues(tempOutValues);
                tempOutValues = hiddenLayer.GetLayerOutput();
            }
            //output layer
            var outputLayer = GetOutputNeuralLayer();
            outputLayer.InitInputValues(tempOutValues);
            var resultValues = outputLayer.GetLayerOutput();

            return resultValues;
        }

        /// <summary>
        /// Trains the ANN
        /// </summary>
        /// <param name="imgs">Set of images</param>
        /// <param name="iterationsLimit">Limit of iterations to complete training; -1 - unlimited</param>
        /// <param name="trainCallBack">Training callback function</param>
        /// <returns></returns>
        public int TrainNetwork(ICollection<INeuralImage> imgs, int iterationsLimit = -1 ,Action<int> trainCallBack = null)
        {
            InitNeuralLayers();
            int iterations = 0;
            bool noErrors;
            do
            {
                noErrors = true;
                foreach (var img in imgs)
                {
                    var recResult = GetNetworkOutput(img);
                    var outputRule = GetOuputRule(img);
                    var roundedOutput = RoundValues(recResult);
                    var outputDeviation = new double[recResult.Count()];
                    if (!CheckResult(img, roundedOutput)) noErrors = false;
                    for (int i = 0; i < recResult.Count(); i++)
                        outputDeviation[i] = outputRule[i] - roundedOutput[i];
                    SetNeuronErrorSignals(outputDeviation);//вычислить коэф. ошибки для каждого нейрона каждого слоя (начиная с выходного слоя
                    AdjustNeuronsWeights(); //корректируем весовые коэф-ты начиная со входного слоя
                    iterations++;
                    if (trainCallBack != null)
                        trainCallBack(iterations);
                    if (iterationsLimit > 0 && iterations >= iterationsLimit) return iterations;
                }
            } while (noErrors != true);
            return iterations;
        }
        /// <summary>
        /// Recognizes specified image. Returns null if cannot recognize
        /// </summary>
        /// <param name="img">Image to recognize</param>
        /// <returns>Recognized class id</returns>
        public int? GetClassIdOrDefault(INeuralImage img)
        {
            var values = RoundValues(GetNetworkOutput(img));
            for (int i = 0; i < values.Count(); i++)
            {
                if ((int)values[i] == 1) return i;
            }
            return null;
        }
    }
}
