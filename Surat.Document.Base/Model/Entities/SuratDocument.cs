using Surat.Common.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surat.Document.Base.Model.Entities
{
    public class SuratDocument : AuditableEntityBase<long>
    {
        [Index("IX_Document_StoreId_GroupId", 1)]
        public int DocumentStoreId { get; set; }
        [Index("IX_Document_StoreId_GroupId", 2)]
        public int DocumentGroupId { get; set; }
        [Required]
        public int ContentTypeId { get; set; }
        [MaxLength(100)]
        public string FileName { get; set; }
        [Required]
        public int FileTypeId { get; set; }
        [Required]
        public double FileSize { get; set; }
        public bool IsDocumentSet { get; set; }
        public long SetParentDocumentId { get; set; }
        public bool IsDocumentShortCut { get; set; }
        public long ShortcutDocumentId { get; set; }
        public bool IsTemplateDocument { get; set; }
        [MaxLength(200)]
        public string Notes { get; set; }
        [MaxLength(200)]
        public string Keywords { get; set; }
        [Required]
        public int DocumentOwnerUserId { get; set; }
        [MaxLength(50)][Index("IX_Document_RelatedObjectType_Id", 1)]
        public string RelatedObjectType { get; set; }
        [Index("IX_Document_RelatedObjectType_Id", 2)]
        public long RelatedObjectId { get; set; }
    }
}
