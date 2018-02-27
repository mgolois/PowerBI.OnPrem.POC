using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBi.OnPrem.Core
{
    public class MoveItemsRequest
    {
        public string[] CatalogItemPaths { get; set; }
        public string TargetPath { get; set; }
    }
}
