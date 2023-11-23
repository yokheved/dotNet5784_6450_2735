
namespace DO;

public interface IDependency : ICrud<Dependency>
{
    IEnumerable<Dependency> ReadAll(Func<Dependency, bool>? filter = null);
}
