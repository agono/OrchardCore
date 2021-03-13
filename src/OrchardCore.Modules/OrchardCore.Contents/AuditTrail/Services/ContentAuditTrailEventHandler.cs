using System.Threading.Tasks;
using OrchardCore.AuditTrail.Extensions;
using OrchardCore.AuditTrail.Models;
using OrchardCore.AuditTrail.Services;
using OrchardCore.AuditTrail.Services.Models;
using OrchardCore.ContentManagement;
using OrchardCore.Entities;
using OrchardCore.Modules;

namespace OrchardCore.Contents.AuditTrail.Services
{
    [RequireFeatures("OrchardCore.AuditTrail")]
    public class ContentAuditTrailEventHandler : AuditTrailEventHandlerBase
    {
        public override Task CreateAsync(AuditTrailCreateContext context)
        {
            var content = context.EventFilterKey == "content" ? context.EventData.Get<ContentItem>("ContentItem") : default;
            var auditTrailPart = content != null ? content.ContentItem.As<AuditTrailPart>() : default;

            if (auditTrailPart == null) return Task.CompletedTask;

            context.Comment = auditTrailPart.Comment;

            return Task.CompletedTask;
        }
    }
}
