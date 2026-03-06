using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services
{
    public class AuditLogService
    {
        private readonly IAuditLog _auditLog;

        public AuditLogService(IAuditLog auditLog)
        {
            _auditLog = auditLog;
        }

        public void SaveAuditLog()
        {
            _auditLog.Save();
        }
    }
}
