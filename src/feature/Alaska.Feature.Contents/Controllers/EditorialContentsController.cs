using Alaska.Feature.Contents.Abstractions;
using Alaska.Feature.Contents.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Alaska.Feature.Contents.Controllers
{
    [Route("alaska/api/contents/[action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class EditorialContentsController : Controller
    {
        private readonly IContentService _contentsService;

        public EditorialContentsController(IContentService contentsService)
        {
            _contentsService = contentsService ?? throw new ArgumentException(nameof(IContentService));
        }

        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Page), 200)]
        public IActionResult GetPage(string id, string culture)
        {
            var result = _contentsService.GetPage(id, CultureInfo.CreateSpecificCulture(culture));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Component), 200)]
        public IActionResult GetComponent(string id, string culture)
        {
            var result = _contentsService.GetComponent(id, CultureInfo.CreateSpecificCulture(culture));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Site), 200)]
        public IActionResult GetSite(string route, string culture)
        {
            var result = _contentsService.GetSite(route, CultureInfo.CreateSpecificCulture(culture));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(Page), 200)]
        public IActionResult GetLabelPage(string pageId, string culture)
        {
            var result = _contentsService.GetLabelPage(pageId, CultureInfo.CreateSpecificCulture(culture));
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
