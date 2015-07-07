using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseModelDefinitions;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Core.Services;
using Umbraco.Core.Persistence.UnitOfWork;

namespace Endzone.UCop.UmbracoExtensions
{
    public static class ContentServiceExtensions
    {
        /// <summary>
        /// Extension method to get paged results of Contents from ContentService
        /// </summary>
        /// <param name="contentService"></param>
        /// <param name="id"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="totalRecords"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IEnumerable<IContent> GetPagedContentsOfContentType(this IContentService contentService, int id, int pageSize, int pageNumber, out int totalRecords, string orderBy, Direction orderDirection = Direction.Ascending)
        {
            IDatabaseUnitOfWorkProvider uowProvider = new PetaPocoUnitOfWorkProvider();
            var repositoryFactory = new RepositoryFactory();
            using (var repository = repositoryFactory.CreateContentRepository(uowProvider.GetUnitOfWork()))
            {
                var query = Query<IContent>.Builder.Where(x => x.ContentTypeId == id);
                var contents = repository.GetPagedResultsByQuery(query,pageNumber, pageSize, out totalRecords, orderBy, orderDirection);
                return contents;
            }
        }
    }

    public class UowProviderHelper
    {

    }
}
