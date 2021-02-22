using Consul;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calamus.Consul
{
	/// <summary>
	/// Consul服务注册 Host Service
	/// </summary>
    public class AgentServiceRegistrationHostedService : IHostedService
    {
        private readonly IConsulClient _consulClient;
        private readonly AgentServiceRegistration _serviceRegistration;
		private readonly ILogger<AgentServiceRegistrationHostedService> _logger;
		public AgentServiceRegistrationHostedService(IConsulClient consulClient, AgentServiceRegistration serviceRegistration, ILogger<AgentServiceRegistrationHostedService> logger)
		{
			_consulClient = consulClient;
			_serviceRegistration = serviceRegistration;
			_logger = logger;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("程序启动，注册Consul服务...");
			await _consulClient.Agent.ServiceRegister(_serviceRegistration, cancellationToken);
		}

		public async Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("程序停止，移除Consul服务...");
			await _consulClient.Agent.ServiceDeregister(_serviceRegistration.ID);
		}
	}
}
