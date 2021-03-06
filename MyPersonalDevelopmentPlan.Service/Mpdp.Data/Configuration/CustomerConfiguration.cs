﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mpdp.Entities;

namespace Mpdp.Data.Configuration
{
  public class CustomerConfiguration : EntityBaseConfiguration<Customer>
  {
    public CustomerConfiguration()
    {
      Property(u => u.FirstName).IsRequired().HasMaxLength(100);
      Property(u => u.LastName).IsRequired().HasMaxLength(100);
      Property(c => c.UserName).IsRequired().HasMaxLength(25);
      Property(u => u.Password).IsRequired().HasMaxLength(25);
      Property(c => c.Mobile).HasMaxLength(10);
      Property(c => c.Email).IsRequired().HasMaxLength(200);
      Property(c => c.DateOfBirth).IsRequired();
    }
  }
}
