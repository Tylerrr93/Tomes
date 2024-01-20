using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Tomes
{
    public class BlockGutenbergPress : Block
    {
        public override bool CanPlaceBlock(
            IWorldAccessor world,
            IPlayer byPlayer,
            BlockSelection blockSel,
            ref string failureCode)
        {
            // Check if the block can be placed by ensuring surrounding spaces are clear
            BlockPos blockPos = blockSel.Position;

            // Check the blocks to the north, east, south, west for clearance 
            if (!IsClear(world, blockPos.NorthCopy())) return false;
            if (!IsClear(world, blockPos.EastCopy())) return false;
            if (!IsClear(world, blockPos.SouthCopy())) return false;
            if (!IsClear(world, blockPos.WestCopy())) return false;

            return true;
        }

        private bool IsClear(IWorldAccessor world, BlockPos pos)
        {
            // Check if the specified block position is clear (e.g., air or a replaceable block)
            Block block = world.BlockAccessor.GetBlock(pos);
            return block.IsReplacableBy(this);
        }

        public override void OnBlockBroken(
            IWorldAccessor world,
            BlockPos pos,
            IPlayer byPlayer,
            float dropQuantityMultiplier = 1f)
        {
            // Handle the removal of the additional top block
            Block upBlock = world.BlockAccessor.GetBlock(pos.UpCopy());
            if (upBlock.Code.Path == "gutenbergpresstop-" + Variant["side"])
            {
                world.BlockAccessor.SetBlock(0, pos.UpCopy());
            }

            base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
        }
    }
}
