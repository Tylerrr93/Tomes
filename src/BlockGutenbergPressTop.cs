using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Tomes
{
    public class BlockGutenbergPressTop : Block
    {

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1)
        {
            // This just tells the presses second block that if its the one broken, run the onbroken behavior for the main press 
            // block which is to remove this block and drop/break the main one
            var block = world.BlockAccessor.GetBlock(pos.DownCopy()) as BlockGutenbergPress;
            if (block != null) block.OnBlockBroken(world, pos.DownCopy(), byPlayer, dropQuantityMultiplier);
        }

        public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
        {
            // This is telling creative mode that if you pick this block for your hotbar, actually just pick the main press block 
            var block = world.BlockAccessor.GetBlock(pos.DownCopy()) as BlockGutenbergPress;
            if (block != null) return block.OnPickBlock(world, pos.DownCopy());
            return base.OnPickBlock(world, pos);
        }

    }
}
