﻿using Catalog.API.Data.Interfaces;
using Catalog.API.Entities;
using Catalog.API.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContext : ICatalogContext
    {
        public CatalogContext(ICatalogDatabaseSettings catalogDatabaseSettings)
        {
            var client = new MongoClient(catalogDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(catalogDatabaseSettings.DatabaseName);
            Products = database.GetCollection<Product>(catalogDatabaseSettings.CollectionName);
            CatalogContextSeed.SeedData(Products);
        }
        public IMongoCollection<Product> Products { get; set; }
    }
}
