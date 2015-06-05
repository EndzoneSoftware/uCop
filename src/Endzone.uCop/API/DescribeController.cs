using System.Linq;
using System.Web.Http;
using umbraco;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;

namespace Endzone.UCop.API
{
    [PluginController(Constants.AreaName)]
    public class DescribeController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public object DocumentTypes()
        {
            var all = Services.ContentTypeService.GetAllContentTypes().ToList();
            return from doc in all select new
            {
                docType = string.Format("{0} ({1})", doc.Name, doc.Alias),
                templates = from template in doc.AllowedTemplates select string.Format("{0} ({1})", template.Name, template.Alias),
                urls = from content in Services.ContentService.GetContentOfContentType(doc.Id)
                       select new
                       {
                           name = content.Name,
                           url = library.NiceUrl(content.Id),
                           trashed = content.Trashed,
                           published = content.Published
                        },
                dataType = from propGroup in doc.PropertyGroups
                           select new
                           {
                               name = propGroup.Name,
                               properties = from prop in propGroup.PropertyTypes select prop.Name
                           }
            };
        }
    }
}
