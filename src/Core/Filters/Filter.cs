using System.Linq.Expressions;

namespace Cep.App.Core.Filters;

public class Filter<T>
{
    public bool Condition { get; set; }
    public Expression<Func<T, bool>> Expression { get; set; }
}