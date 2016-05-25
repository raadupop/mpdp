﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mpdp.Entities;

namespace Mpdp.Data.Configuration
{
  public class EntityBaseConfiguration<T> : EntityTypeConfiguration<T> where T : class, IEntityBase
  {
    public EntityBaseConfiguration()
    {
      HasKey(e => e.Id);
    }
  }
}
