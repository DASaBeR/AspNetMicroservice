using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Common
{
	public abstract class EntityBase
	{
		public int Id { get; protected set; }
		public Guid Guid { get; set; }
		public string CreatedBy { get; set; }
		public long CreatedDate { get; set; }
		public string LastModifiedBy { get; set; }
		public long? LastModifiedDate { get; set; }
	}
}
