using System;
using System.Net.Http.Formatting;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;
using uConstants = Umbraco.Core.Constants;

namespace Endzone.UCop
{
    [PluginController(Constants.AreaName)]
    [Tree(Constants.ApplicationAlias, Constants.TreeAlias, Constants.PluginName)]
    public class EndzoneTreeController : TreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            if (id == uConstants.System.Root.ToInvariantString())
            {
                var nodes = new TreeNodeCollection();
                var item = CreateTreeNode("structure", id, queryStrings, "Site structure", "icon-truck", false);
                nodes.Add(item);
                return nodes;
            }

            throw new NotSupportedException("We can only render the root level nodes");
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            //menu.DefaultMenuAlias = ActionAudit.Instance.Alias;
            //menu.Items.Add<ActionNew>("Create");
            return menu;
        }

    }
}
