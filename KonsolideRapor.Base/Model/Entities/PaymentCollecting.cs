using System;
using Surat.Common.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Surat.Base.Model.Entities
{
    public class PaymentCollecting : AuditableEntityBase<int>
	{
		[Required(ErrorMessage="Code Alanı Gereklidir.")]
		[MaxLength(100)]
		public string Code {get;set;}

		[Required(ErrorMessage="Name Alanı Gereklidir.")]
		[MaxLength(200)]
		public string Name {get;set;}

        public bool IsPayment { get; set; }

        public bool IsCollection { get; set; }

	}
}
