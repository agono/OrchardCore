using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrchardCore.Security;
using OrchardCore.Security.Permissions;

namespace OrchardCore.Settings
{
    public class Permissions : IPermissionProvider
    {
        public static readonly Permission ManageSettings = new Permission("ManageSettings", "Manage settings");

        // This permission is not exposed, it's just used for the APIs to generate/check custom ones
        public static readonly Permission ManageGroupSettings = new Permission("ManageResourceSettings", "Manage settings", new[] { ManageSettings });

        public Task<IEnumerable<Permission>> GetPermissionsAsync()
        {
            return Task.FromResult(new[] { ManageSettings }.AsEnumerable());
        }

        public IEnumerable<PermissionStereotype> GetDefaultStereotypes()
        {
            return new[]
            {
                new PermissionStereotype
                {
                    Name = BuiltInRole.Administrator,
                    Permissions = new[] { ManageSettings }
                }
            };
        }
    }
}
