﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Reflection;
using Grpc.Core;
using Grpc.Domain;
using Ketchup.Core.Utilities;
using Ketchup.Gateway.Internal;
using Ketchup.Gateway.Internal.Implementation;
using Ketchup.Grpc.Internal.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ketchup.Gateway.Controllers
{
    [ApiController]
    [Route("api/")]
    public class ServicesController : Controller
    {
        private readonly IGrpcClientProvider _clientProvider;
        private readonly IGatewayProvider _gatewayProvider;

        public ServicesController(IGrpcClientProvider clientProvider, IGatewayProvider gatewayProvider)
        {
            this._clientProvider = clientProvider;
            _gatewayProvider = gatewayProvider;
        }

        [HttpPost("{server}/{service}")]
        public async Task<object> ExecuteService(string server, string service, [FromBody] Dictionary<string, object> inputBody)
        {
            _gatewayProvider.MapClients.TryGetValue(service, out var value);

            var client = await _clientProvider.GetClientAsync(server, value);

            var descriptor =
                _gatewayProvider.MethodDescriptors.FirstOrDefault(item => item.Name == service);

            var methodModel = new GrpcGatewayMethod()
            {
                Type = descriptor?.InputType.ClrType,
                Request = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(inputBody), descriptor?.InputType.ClrType)
            };

            var method = _gatewayProvider.MapClients[service].GetMethod(service,
                new Type[] { methodModel.Type,
                    typeof(Metadata),
                    typeof(global::System.DateTime),
                    typeof(global::System.Threading.CancellationToken) });

            var result = method?.Invoke(client,
                new object[] { methodModel.Request, null, null, default(global::System.Threading.CancellationToken) });

            return result;
        }
    }
}
