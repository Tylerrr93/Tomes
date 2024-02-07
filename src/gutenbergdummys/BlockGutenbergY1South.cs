using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Tomes
{
    public class BlockGutenbergY1South : Block
    {

        public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
        {
            // Determine orientation of press, then determine block+entity location based on that
            BlockEntityGutenbergPress be = null;
            BlockGutenbergPress block = null;
            string variant = Variant["side"] as string;
            if (variant == "north") {
                be = world.BlockAccessor.GetBlockEntity(blockSel.Position.AddCopy(0, 0, -1)) as BlockEntityGutenbergPress;
                block = world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(0, 0, -1)) as BlockGutenbergPress;
            } else if (variant == "east") {
                be = world.BlockAccessor.GetBlockEntity(blockSel.Position.AddCopy(1, 0, 0)) as BlockEntityGutenbergPress;
                block = world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(1, 0, 0)) as BlockGutenbergPress;
            } else if (variant == "south") {
                be = world.BlockAccessor.GetBlockEntity(blockSel.Position.AddCopy(0, 0, 1)) as BlockEntityGutenbergPress;
                block = world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(0, 0, 1)) as BlockGutenbergPress;
            } else if (variant == "west") {
                be = world.BlockAccessor.GetBlockEntity(blockSel.Position.AddCopy(-1, 0, 0)) as BlockEntityGutenbergPress;
                block = world.BlockAccessor.GetBlock(blockSel.Position.AddCopy(-1, 0, 0)) as BlockGutenbergPress;
            }

            if (be != null)
            {
                // This calls the OnBlockInteractStart method of the Gutenberg block entity (if its found, it always should be)
                var handled = be.OnBlockInteractStart(byPlayer, blockSel, BlockEntityGutenbergPress.EnumGutenbergPressSection.Tray);
                return handled;
            }

            // If the block and block entity are not found, then just run default interact start behavior
            return base.OnBlockInteractStart(world, byPlayer, blockSel);
        }

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1)
        {
            // Find the main press block to run its OnBlockBroken
            // Determine the side of the block, or which direction the press structure is facing to run appropriate disassembly 
            string variant = Variant["side"] as string;
            
            if (variant == "north")
            {
                // Variant is north, find source block to run OnBlockBroken appropriately
                Block block = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, -1)) as BlockGutenbergPress;
                if (block != null) block.OnBlockBroken(world, pos.AddCopy(0, 0, -1), byPlayer, dropQuantityMultiplier);
            
            } else if (variant == "east") {
                // Variant is east
                Block block = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, 0)) as BlockGutenbergPress;
                if (block != null) block.OnBlockBroken(world, pos.AddCopy(1, 0, 0), byPlayer, dropQuantityMultiplier);
            
            } else if (variant == "south") {
                // Variant is south
                Block block = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, 1)) as BlockGutenbergPress;
                if (block != null) block.OnBlockBroken(world, pos.AddCopy(0, 0, 1), byPlayer, dropQuantityMultiplier);

            } else if (variant == "west") {
                //Variant is west
                Block block = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, 0)) as BlockGutenbergPress;
                if (block != null) block.OnBlockBroken(world, pos.AddCopy(-1, 0, 0), byPlayer, dropQuantityMultiplier);
            }

        } 

        //public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
        //{
            // This is telling creative mode that if you pick this block for your hotbar, actually just pick the main press block 
            // This is also telling survival mode to "mask" the top block as a gutenberg press main block when its being looked at
            
            //var block = world.BlockAccessor.GetBlock(pos.DownCopy()) as BlockGutenbergPress;
            //if (block != null) return block.OnPickBlock(world, pos.DownCopy());
            //return base.OnPickBlock(world, pos);
        //}

        // This (if it returns true) allows the player to select individual selection boxes within the block
        public override bool DoParticalSelection(IWorldAccessor world, BlockPos pos)
        {
            return true;
        }

    }
}
