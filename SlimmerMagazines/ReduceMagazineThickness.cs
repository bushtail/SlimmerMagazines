using JetBrains.Annotations;
using SPTarkov.Common.Models.Logging;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Spt.Tables;

namespace SlimmerMagazines;

[Injectable(TypePriority = OnLoadOrder.TraderRegistration - 1), UsedImplicitly]
public class ReduceMagazineThickness(ISptLogger<ReduceMagazineThickness> logger, TemplateTable templateTable) : IOnLoad
{
    private const string Parent = "5448bc234bdc2d3c308b4569";
    private Dictionary<MongoId, TemplateItem>? _itemsDb;
    
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        logger.Info("[SM] Putting magazines on a diet...");
        var mags = 0;
        _itemsDb = templateTable.Items;

        foreach (var item in _itemsDb.Where(item => item.Value.Parent.Equals(Parent)))
        {
            if (item.Value.Properties is null) continue;

            if (item.Value.Properties.Width == 2 && item.Value.Properties.Height > 1)
            {
                item.Value.Properties.Width = 1;
                item.Value.Properties.ExtraSizeDown = 0;
                mags++;
            } 
            else if (item.Value.Properties.Height == 3 && item.Value.Properties.Width == 1)
            {
                item.Value.Properties.Height = 2;
                item.Value.Properties.ExtraSizeDown = 0;
                mags++;
            }
        }
        
        logger.Info($"[SM] Slimmed down {mags} magazines!");
        return Task.CompletedTask;
    }
}