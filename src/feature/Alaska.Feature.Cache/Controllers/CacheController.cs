﻿using Alaska.Foundation.Core.Caching.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alaska.Feature.Cache.Controllers
{
    [Route("alaska/api/cache/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CacheController : Controller
    {
        private ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<ICacheInstance>))]
        public IActionResult GetCacheInstances()
        {
            return Ok(_cacheService.GetAllCaches().ToList());
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<string>))]
        public IActionResult GetCacheKeys([FromQuery]string cacheId)
        {
            return Ok(GetCache(cacheId).Keys.ToList());
        }

        [HttpGet]
        [Produces(typeof(ICacheItem))]
        public IActionResult GetCacheEntry([FromQuery]string cacheId, [FromQuery]string cacheKey)
        {
            return Ok(GetCache(cacheId).GetItem(cacheKey));
        }

        [HttpPost]
        [Produces(typeof(void))]
        public IActionResult RemoveCacheEntry([FromQuery]string cacheId, [FromQuery]string cacheKey)
        {
            GetCache(cacheId).Remove(cacheKey);
            return Ok();
        }

        [HttpPost]
        [Produces(typeof(void))]
        public IActionResult ClearCache([FromQuery]string cacheId)
        {
            GetCache(cacheId).Clear();
            return Ok();
        }

        private ICacheInstance GetCache(string cacheId)
        {
            var cache = _cacheService.GetCache(cacheId);
            return cache ?? throw new InvalidOperationException($"Cache {cacheId} not found");
        }
    }
}
