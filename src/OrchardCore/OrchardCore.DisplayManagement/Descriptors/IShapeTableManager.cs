using System.Threading.Tasks;

namespace OrchardCore.DisplayManagement.Descriptors;

public interface IShapeTableManager
{
    Task<ShapeTable> GetShapeTableAsync(string themeId);
}
