﻿using Newtonsoft.Json;
using Nwd.Orders.Domain.Entities;
using Nwd.Orders.Infrastructure.Data.Interfaces;
using System.Reflection;

namespace Nwd.Orders.Infrastructure.Data.Seed
{
    internal class SeedDataReader
    {
        private readonly ICosmosDbContainerFactory _factory;

        public SeedDataReader(ICosmosDbContainerFactory factory)
        {
            _factory = factory;
        }
        internal async Task SeedAllAsync()
        {
            await SeedAsync<Customer>(CosmosDbContainer.CustomersContainerName);
            await SeedAsync<Product>(CosmosDbContainer.ProductsContainerName);
        }

        private async Task SeedAsync<T>(string containerName) where T : class
        {
            var data = ReadCustomerFromFile<List<T>>($"Nwd.Orders.Infrastructure.Data.Seed.{containerName}.json");
            var container = _factory.GetContainer(containerName);
            var containerIterator = await container._container.GetItemQueryIterator<T>().ReadNextAsync();
            if (containerIterator.Count == 0)
            {
                foreach (var item in data)
                {
                    await container._container.CreateItemAsync<T>(item);
                }
            }
        }
        private static T ReadCustomerFromFile<T>(string fileName) where T : class
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(fileName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string jsonFile = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(jsonFile);
            }

        }
    }
}