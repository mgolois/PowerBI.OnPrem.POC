using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerBi.OnPrem.Core
{
    public class CatalogItem
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public bool Hidden { get; set; }
        public long Size { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ParentFolderId { get; set; }
        public string ContentType { get; set; }
        public string Content { get; set; }
        public bool IsFavorite { get; set; }
    }
}
