using Pokédex.Models;
using Realms;

namespace Pokédex.Services;

public class RealmService
{
    private readonly Realm _realm;

    public RealmService()
    {
        _realm = Realm.GetInstance();
    }

    public IEnumerable<ResourceListsModel> FindResourceLists()
    {
        return _realm.All<ResourceListsModel>();
    }

    public bool AddResourceLists(IEnumerable<ResourceListsModel> resourceLists)
    {
        foreach (ResourceListsModel resourceList in resourceLists)
        {
            _realm.Add<ResourceListsModel>
        }
        return _realm.Write<ResourceListsModel>();
    }
}
