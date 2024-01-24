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

            // Gutenberg printing press dummy blocks
            api.RegisterBlockClass("blockgutenbergy1north", typeof(BlockGutenbergY1North));
            api.RegisterBlockClass("blockgutenbergy1northeast", typeof(BlockGutenbergY1Northeast));
            api.RegisterBlockClass("blockgutenbergy1northwest", typeof(BlockGutenbergY1Northwest));
            api.RegisterBlockClass("blockgutenbergy1west", typeof(BlockGutenbergY1West));
            api.RegisterBlockClass("blockgutenbergy1east", typeof(BlockGutenbergY1East));
            api.RegisterBlockClass("blockgutenbergy1southwest", typeof(BlockGutenbergY1Southwest));
            api.RegisterBlockClass("blockgutenbergy1southeast", typeof(BlockGutenbergY1Southeast));
            api.RegisterBlockClass("blockgutenbergy1south", typeof(BlockGutenbergY1South)); 
            api.RegisterBlockClass("blockgutenbergy1southsouth", typeof(BlockGutenbergY1Southsouth));
            api.RegisterBlockClass("blockgutenbergy2", typeof(BlockGutenbergY2));
            api.RegisterBlockClass("blockgutenbergy2west", typeof(BlockGutenbergY2West));
            api.RegisterBlockClass("blockgutenbergy2east", typeof(BlockGutenbergY2East));
            api.RegisterBlockClass("blockgutenbergy3", typeof(BlockGutenbergY3));
            api.RegisterBlockClass("blockgutenbergy3west", typeof(BlockGutenbergY3West));
            api.RegisterBlockClass("blockgutenbergy3east", typeof(BlockGutenbergY3East));
            
        }
    }
}
