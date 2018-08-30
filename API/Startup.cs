using System;
using System.Collections.Generic;
using System.Linq;
using EthereumLibrary.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            var requiredVariables = new Dictionary<string, string>
            {
                {"IPFS client host", Configuration["IPFS:API:host"]},
                {"IPFS client port", Configuration["IPFS:API:port"]},
                {"IPFS client protocol", Configuration["IPFS:API:protocol"]},
                {"Ethereum RPC host", Configuration["Ethereum:RPCServer"]},
                {"Ethereum Transaction Gas", Configuration["Ethereum:Gas"]},
                {"Ethereum contract address", EnvironmentVariablesHelper.ContractAddress},
                {"Ethereum wallet address", EnvironmentVariablesHelper.WalletAddress}
            };
            requiredVariables.Where(item => string.IsNullOrEmpty(item.Value)).ToList().ForEach(item =>
                throw new ArgumentNullException(item.Key + " is required"));
        }
    }
}