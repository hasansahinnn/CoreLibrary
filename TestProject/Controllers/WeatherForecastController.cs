using Autofac;
using Business.ValidationRules.FluentValidation;
using Core;
using Core.Application.Interfaces;
using Core.AutoFac;
using Core.Caching;
using Core.DataSecurity;
using Core.Models;
using Core.Validation;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDataModel _testModel;
        private readonly ITestModelValidator _service;
        //private readonly IAsyncRepository<UserEntity> _userEntityRepository;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDataModel testModel, ITestModelValidator service)
        {
            _logger = logger;
            _testModel = testModel;
            _service = service;
            //_userEntityRepository = userEntityRepository;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Summaries;
        }

        [HttpGet("AutofacEndpoint")]
        public int AutofacEndpoint()
        {
            var testModel = AutoFacManager.Resolve<IDataModel>();
            return testModel.GetData();

            // return _testModel.GetData();
        }

        [HttpGet("CacheEndpoint")]
        [ResponseCache(Duration = 5,Location =ResponseCacheLocation.Client)]
        public string CacheEndpoint()
        {
            var  _memoryCache = Core.AutoFac.ServiceTool.ServiceProvider.GetService(typeof(IMemoryCache));
            var _cacheManager = AutoFacManager.Resolve<ICacheManager>();

            return DateTime.Now.ToString();
        }

        [HttpPost("FluentValidator")]
        public List<string> FluentValidator(DataModel model)
        {
            _service.Check(model);
            model.GetData();
            var (isValid, errors) = ValidationTool.Validate<DataModelValidator, DataModel>(model);
            if (!isValid)
                return errors;
            return new List<string> { "OK"};
        }

        [HttpPost("CryptData")]
        public async Task<IActionResult> CryptData(UserEntity userEntity)
        {
            return Ok();
        }
        [HttpPost("DeCryptData")]
        public string DeCryptData([FromBody] request cry)
        {
            return CryptoManager.DecrypteAES(cry.cryptedText);
        }

        [HttpGet("LogEndpoint")]
        public IActionResult LogEndpoint()
        {
            Log.Debug("debug");
            Log.Error("Error");
            _logger.LogInformation("LogInformation");
            return NotFound("not founded data");
        }
    }
    public class request
    {
        public string cryptedText { get; set; }
    }

}
