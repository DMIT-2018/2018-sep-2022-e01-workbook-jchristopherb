using ChinookSystem.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespace
#endregion

namespace ChinookSystem.BLL
{
    public class TrackServices
    {
        #region Constructor for Context Dependency
        private readonly ChinookContext _context;

        internal TrackServices(ChinookContext context)
        {
            _context = context;
        }


        #endregion
    }
}
