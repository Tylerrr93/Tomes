using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Client;

namespace Tomes
{
    public class BlockGutenbergPress : Block
    {
        public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref string failureCode)
        {
            // Check if the block can be placed by ensuring surrounding spaces are clear
            BlockPos blockPos = blockSel.Position;

            // Check the blocks to the north, east, south, west for clearance 
            if (!IsClear(world, blockPos.NorthCopy())) return false;
            if (!IsClear(world, blockPos.EastCopy())) return false;
            if (!IsClear(world, blockPos.SouthCopy())) return false;
            if (!IsClear(world, blockPos.WestCopy())) return false;

            // Check of ground level diagonal blocks for clearance
            if (!IsClear(world, blockPos.AddCopy(1, 0, 1))) return false;
            if (!IsClear(world, blockPos.AddCopy(1, 0, -1))) return false;
            if (!IsClear(world, blockPos.AddCopy(-1, 0, 1))) return false;
            if (!IsClear(world, blockPos.AddCopy(-1, 0, -1))) return false;

            return true;
        }

        private bool IsClear(IWorldAccessor world, BlockPos pos)
        {
            // Check if the specified block position is clear (e.g., air or a replaceable block)
            Block block = world.BlockAccessor.GetBlock(pos);
            return block.IsReplacableBy(this);
        }

        public override void OnBlockPlaced(IWorldAccessor world, BlockPos pos, ItemStack byItemStack = null)
        {
            base.OnBlockPlaced(world, pos, byItemStack);

            // Here for fruitpress reference
            Block toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergpresstop-" + Variant["side"]));
            world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.UpCopy());

            // Get the placed press variant to determine multiblock locations (n/e/s/w)
            string variant = Variant["side"] as string;
            if (variant == "north")
            {
                // Variant is north, add blocks appropriately
                //Y1North
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1north-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, -1));
                //Y1Northeast
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1northeast-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, -1));
                //Y1Northwest
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1northwest-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, -1));
                //Y1West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, 0));
                //Y1East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, 0));

            } else if (variant == "east") {
                // Variant is east
                //Y1North
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1north-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, 0));
                //Y1Northeast
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1northeast-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, 1));
                //Y1Northwest
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1northwest-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, -1));
                //Y1West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, -1));
                //Y1East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, 1));

            } else if (variant == "south") {
                // Variant is south
                //Y1North
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1north-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, 1));
                //Y1Northeast
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1northeast-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, 1));
                //Y1Northwest
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1northwest-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, 1));
                //Y1West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, 0));
                //Y1East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, 0));

            } else if (variant == "west") {
                // Variant is west
                //Y1North
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1north-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, 0));
                //Y1Northeast
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1northeast-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, -1));
                //Y1Northwest
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1northwest-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, 1));
                //Y1West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, 1));
                //Y1East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, -1));
            }

        }

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
        {
            // Handle the removal of the additional top block
            Block upBlock = world.BlockAccessor.GetBlock(pos.UpCopy());
            if (upBlock.Code.Path == "gutenbergpresstop-" + Variant["side"])
            {
                world.BlockAccessor.SetBlock(0, pos.UpCopy());
            }

            // Get the placed press variant to determine removal of multiblocks
            string variant = Variant["side"] as string;
            
            if (variant == "north")
            {
                // Variant is north, remove blocks appropriately
                //Y1North
                Block y1North = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, -1));
                if (y1North.Code.Path == "gutenbergy1north-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, -1));
                }
                //Y1Northeast
                Block y1Northeast = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, -1));
                if (y1Northeast.Code.Path == "gutenbergy1northeast-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, -1));
                }
                //Y1Northwest
                Block y1Northwest = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, -1));
                if (y1Northwest.Code.Path == "gutenbergy1northwest-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, -1));
                }
                //Y1West
                Block y1West = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, 0));
                if (y1West.Code.Path == "gutenbergy1west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, 0));
                }
                //Y1East
                Block y1East = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, 0));
                if (y1East.Code.Path == "gutenbergy1east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, 0));
                }
            
            } else if (variant == "east") {
                // Variant is east
                //Y1North
                Block y1North = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, 0));
                if (y1North.Code.Path == "gutenbergy1north-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, 0));
                }
                //Y1Northeast
                Block y1Northeast = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, 1));
                if (y1Northeast.Code.Path == "gutenbergy1northeast-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, 1));
                }
                //Y1Northwest
                Block y1Northwest = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, -1));
                if (y1Northwest.Code.Path == "gutenbergy1northwest-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, -1));
                }
                //Y1West
                Block y1West = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, -1));
                if (y1West.Code.Path == "gutenbergy1west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, -1));
                }
                //Y1East
                Block y1East = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, 1));
                if (y1East.Code.Path == "gutenbergy1east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, 1));
                }
                
            
            } else if (variant == "south") {
                // Variant is south
                //Y1North
                Block y1North = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, 1));
                if (y1North.Code.Path == "gutenbergy1north-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, 1));
                }
                //Y1Northeast
                Block y1Northeast = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, 1));
                if (y1Northeast.Code.Path == "gutenbergy1northeast-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, 1));
                }
                //Y1Northwest
                Block y1Northwest = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, 1));
                if (y1Northwest.Code.Path == "gutenbergy1northwest-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, 1));
                }
                //Y1West
                Block y1West = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, 0));
                if (y1West.Code.Path == "gutenbergy1west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, 0));
                }
                //Y1East
                Block y1East = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, 0));
                if (y1East.Code.Path == "gutenbergy1east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, 0));
                }

            } else if (variant == "west") {
                // Variant is west
                //Y1 North
                Block y1North = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, 0));
                if (y1North.Code.Path == "gutenbergy1north-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, 0));
                }
                //Y1Northeast
                Block y1Northeast = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, -1));
                if (y1Northeast.Code.Path == "gutenbergy1northeast-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, -1));
                }
                //Y1Northwest
                Block y1Northwest = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, 1));
                if (y1Northwest.Code.Path == "gutenbergy1northwest-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, 1));
                }
                //Y1West
                Block y1West = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, 1));
                if (y1West.Code.Path == "gutenbergy1west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, 1));
                }
                //Y1East
                Block y1East = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, -1));
                if (y1East.Code.Path == "gutenbergy1east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, -1));
                }
            }

            base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
        }

        //If returns true, allows you to select individual selection boxes in the block
        //public override bool DoParticalSelection(IWorldAccessor world, BlockPos pos)
        //{
            //return true;
        //}

    }
}
