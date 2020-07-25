using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SERP.Core.Entities.Context
{
    public static class DbContextDisponse
    {
        //public static bool IsDisposed(this DbContext dbContext)
        //{
        //    var result = true;

        //    var typeDbContext = typeof(SERPContext);
        //    var typeInternalContext = typeDbContext.Assembly.GetType("SERP.Core.Entities");

        //    var fi_InternalContext = typeDbContext.GetField("_internalContext", BindingFlags.NonPublic | BindingFlags.Instance);
        //    var pi_IsDisposed = typeInternalContext.GetProperty("IsDisposed");

        //    var ic = fi_InternalContext.GetValue(dbContext);

        //    if (ic != null)
        //    {
        //        result = (bool)pi_IsDisposed.GetValue(ic);
        //    }

        //    return result;
        //}
    }
}
