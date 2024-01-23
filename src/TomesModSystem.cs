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
            api.RegisterBlockEntityClass("blockentitygutenbergpress", typeof(BlockEntityGutenbergPress));
            api.RegisterBlockClass("blockgutenbergpress", typeof(BlockGutenbergPress));
            api.RegisterBlockClass("blockgutenbergpresstop", typeof(BlockGutenbergPressTop));

            // Gutenberg printing press dummy blocks
            api.RegisterBlockClass("blockgutenbergy1north", typeof(BlockGutenbergY1North));
            api.RegisterBlockClass("blockgutenbergy1northeast", typeof(BlockGutenbergY1Northeast));
            api.RegisterBlockClass("blockgutenbergy1northwest", typeof(BlockGutenbergY1Northwest));
            api.RegisterBlockClass("blockgutenbergy1west", typeof(BlockGutenbergY1West));
            api.RegisterBlockClass("blockgutenbergy1east", typeof(BlockGutenbergY1East));
            
        }
    }
}
