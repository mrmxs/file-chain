using System;
using System.IO;
using System.Reflection;

namespace EthereumLibrary.Helper
{
    public static class ResourceHelper
    {
        private const string SmartContractResourcePath = "EthereumLibrary.SmartContract.compiled";
        
        public static string Get(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resource = $"{SmartContractResourcePath}.{resourceName}";

            try
            {
                using (var stream = assembly.GetManifestResourceStream(resource))
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                throw new Exception($"Resource not found: {resource}");
            }
            
        }
    }
}