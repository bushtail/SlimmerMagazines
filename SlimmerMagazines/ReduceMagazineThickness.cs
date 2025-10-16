using JetBrains.Annotations;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Servers;

namespace SlimmerMagazines;

[UsedImplicitly]
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class ReduceMagazineThickness(ISptLogger<ReduceMagazineThickness> logger, DatabaseServer databaseServer) : IOnLoad
{
    private const string Parent = "5448bc234bdc2d3c308b4569";
    private Dictionary<MongoId, TemplateItem>? _itemsDb;
    
    public Task OnLoad()
    {
        logger.Info("[SM] Putting magazines on a diet...");
        var mags = 0;
        _itemsDb = databaseServer.GetTables().Templates.Items;

        foreach (var item in _itemsDb.Where(item => item.Value.Parent.Equals(Parent)))
        {
            if (item.Value.Properties is null) continue;
            
            if (item.Value.Properties.Width == 2)
                item.Value.Properties.Width = 1;

            if (item.Value.Properties.Height == 3)
                item.Value.Properties.Height = 2;
            
            mags++;
        }
        
        logger.Info($"[SM] Slimmed down {mags} magazines!");
        return Task.CompletedTask;
    }
}