﻿using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
	public class OrderContext : DbContext
	{
		public OrderContext(DbContextOptions<OrderContext> options) : base(options)
		{
		}

		public DbSet<Order> Orders { get; set; }

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			foreach (var entry in ChangeTracker.Entries<EntityBase>())
			{
				switch (entry.State)
				{
					case EntityState.Modified:
						entry.Entity.LastModifiedDate = DateTimeOffset.Now.ToUnixTimeSeconds();
						entry.Entity.LastModifiedBy = "SaBeR";
						break;
					case EntityState.Added:
						entry.Entity.Guid = Guid.NewGuid();
						entry.Entity.CreatedDate = DateTimeOffset.Now.ToUnixTimeSeconds();
						entry.Entity.CreatedBy = "SaBeR";
						break;
				}

			}
			return base.SaveChangesAsync(cancellationToken);
		}

	}
}
