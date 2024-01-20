using Vintagestory.API.Common;

namespace Tomes
{
    public class TomesModSystem : ModSystem
    {

        public override void Start(ICoreAPI api)
        {
            base.Start(api);
            api.RegisterItemClass("customitembook", typeof(CustomItemBook));
            api.RegisterItemClass("itemwaxtablet", typeof (ItemWaxTablet));
        }
    }
}
