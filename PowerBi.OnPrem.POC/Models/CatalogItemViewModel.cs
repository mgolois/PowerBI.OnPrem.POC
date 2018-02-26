using System;

namespace PowerBi.OnPrem.POC.Models
{
    public class CatalogItemViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsPersonalItem { get; set; }
        public bool IsSystemAdmin { get; set; }

        public string FolderName => Path?.Split('/')?[1];
        public bool IsPublicItem => FolderName?.Equals("Public", StringComparison.InvariantCultureIgnoreCase) ?? false;
        public bool CanDownload => true;
        public bool CanDelete => true;// IsPersonalItem || IsSystemAdmin;
        public bool CanMove => true; //IsPersonalItem;
        public bool CanClone => true; //IsSystemAdmin || IsPersonalItem;
        public string FileName => $"{Name}.pbix";

        public bool Visible { get; internal set; }
    }
}