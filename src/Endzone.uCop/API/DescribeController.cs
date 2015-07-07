using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using umbraco;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Endzone.UCop.UmbracoExtensions;
namespace Endzone.UCop.API
{
    [PluginController(Constants.AreaName)]
    public class DescribeController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public object DocumentTypes()
        {            
            var all = Services.ContentTypeService.GetAllContentTypes().ToList();
            var dataTypeDefintions = Services.DataTypeService.GetAllDataTypeDefinitions();
            return from doc in all
                   let instances = Services.ContentService.GetPagedContentsOfContentType(doc.Id, 10, 1,"")
                   select new
            {
                docType = new
                {
                    name = doc.Name,
                    id = doc.Id,
                    parentId = doc.ParentId
                },
                templates = from template in doc.AllowedTemplates
                            select new
                            {
                                name = template.Name,
                                id = template.Id
                            },
                urls = new
                {
                    totalCount = instances.Count(),
                    sample = (from content in instances
                              select new
                              {
                                  path = GetUrlPath(content),
                                  trashed = content.Trashed,
                                  published = content.Published
                              }).Take(10)
                },
                dataType = from propGroup in doc.PropertyGroups
                           select new
                           {
                               name = propGroup.Name,
                               properties = from prop in propGroup.PropertyTypes
                                            select new
                                            {
                                                name = prop.Name,
                                                type = dataTypeDefintions.Where(d => d.Id == prop.DataTypeDefinitionId).Select(d => d.Name).FirstOrDefault()
                                            } 
                           }
            };
        }

        [HttpGet]
        public object Urls([FromUri]int contentTypeId)
        {
            return from content in Services.ContentService.GetContentOfContentType(contentTypeId)
                select new
                {
                    path = GetUrlPath(content),
                    trashed = content.Trashed,
                    published = content.Published
                };
        }

        private IEnumerable<UrlNode> GetUrlPath(IContent content)
        {
            var stack = new Stack<UrlNode>();
            while (content != null)
            {
                stack.Push(new UrlNode()
                {
                    url = library.NiceUrl(content.Id),
                    name = content.Name
                });
                content = content.Parent();
            }

            while (stack.Count > 0)
            {
                yield return stack.Pop();
            }
        }
    }

    internal class UrlNode
    {
        public string url { get; set; }
        public string name { get; set; }
    }
}
