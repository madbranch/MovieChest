using System.Collections.Generic;

namespace MovieChest.LocalStorage;

public interface ILocalStore : IDictionary<string, string>
{
}