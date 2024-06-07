using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.ML;
using Microsoft.Extensions.ML;
using MLModel_ConsoleApp1;

namespace Pc4ia.Controllers
{
    public class MLController : Controller
    {
        private readonly ILogger<MLController> _logger;
        private readonly PredictionEnginePool<MLModel.ModelInput, MLModel.ModelOutput> _predictionEnginePool;

        public MLController(ILogger<MLController> logger,
            PredictionEnginePool<MLModel.ModelInput, MLModel.ModelOutput> predictionEnginePool)
        {
            _logger = logger;
            _predictionEnginePool = predictionEnginePool;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Predict(string sentimiento)
        {
            MLModel.ModelInput modelInput = new MLModel.ModelInput()
            {
                Comentario = sentimiento
            };

            MLModel.ModelOutput prediction = _predictionEnginePool.Predict(modelInput);
            ViewData["Sentimiento"] = prediction.PredictedLabel;
            ViewData["Score"] = prediction.Score[1];

            if (prediction.PredictedLabel == 1)
            {
                ViewData["Mensaje"] = "Este es un comentario positivo.";
            }
            else
            {
                ViewData["Mensaje"] = "Este es un comentario negativo.";
            }

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}
