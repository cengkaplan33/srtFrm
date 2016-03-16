using System;
using Surat.Common.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Surat.Base.Model.Entities
{
	public class Bank : AuditableEntityBase<int>
	{
		[Required(ErrorMessage="Code Alanı Gereklidir.")]
		[MaxLength(15)]
		public string Code {get;set;}

		[Required(ErrorMessage="Name Alanı Gereklidir.")]
		[MaxLength(40)]
		public string Name {get;set;}

	}
}
