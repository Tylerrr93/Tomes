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

            // Check the y1 blocks to the north, east, south, & west for clearance 
            if (!IsClear(world, blockPos.NorthCopy())) return false;
            if (!IsClear(world, blockPos.EastCopy())) return false;
            if (!IsClear(world, blockPos.SouthCopy())) return false;
            if (!IsClear(world, blockPos.WestCopy())) return false;
            // Check of y1 diagonal blocks for clearance
            if (!IsClear(world, blockPos.AddCopy(1, 0, 1))) return false;
            if (!IsClear(world, blockPos.AddCopy(1, 0, -1))) return false;
            if (!IsClear(world, blockPos.AddCopy(-1, 0, 1))) return false;
            if (!IsClear(world, blockPos.AddCopy(-1, 0, -1))) return false;
            // Check the y1 south-south block (the paper loader) for clearance based on orientation
            string variant = Variant["side"] as string;
            if (variant == "north") {
                if (!IsClear(world, blockPos.AddCopy(0, 0, 2))) return false;
            } else
            if (variant == "east") {
                if (!IsClear(world, blockPos.AddCopy(-2, 0, 0))) return false;
            } else
            if (variant == "south") {
                if (!IsClear(world, blockPos.AddCopy(0, 0, -2))) return false;
            } else
            if (variant == "west") {
                if (!IsClear(world, blockPos.AddCopy(2, 0, 0))) return false;
            }
            // Clearence for Y2 block (screw handle)
            if (!IsClear(world, blockPos.AddCopy(0, 1, 0))) return false;
            // Clearence for Y2 east/west blocks:
            if (variant == "north") {
                if (!IsClear(world, blockPos.AddCopy(1, 1, 0))) return false;
                if (!IsClear(world, blockPos.AddCopy(-1, 1, 0))) return false;
            } else
            if (variant == "east") {
                if (!IsClear(world, blockPos.AddCopy(0, 1, 1))) return false;
                if (!IsClear(world, blockPos.AddCopy(0, 1, -1))) return false;
            } else
            if (variant == "south") {
                if (!IsClear(world, blockPos.AddCopy(-1, 1, 0))) return false;
                if (!IsClear(world, blockPos.AddCopy(1, 1, 0))) return false;
            } else
            if (variant == "west") {
                if (!IsClear(world, blockPos.AddCopy(0, 1, 1))) return false;
                if (!IsClear(world, blockPos.AddCopy(0, 1, -1))) return false;
            }
            // Clearence for Y3 center block (top frame)
            if (!IsClear(world, blockPos.AddCopy(0, 2, 0))) return false;
            // Clearence for Y2 east/west blocks:
            if (variant == "north") {
                if (!IsClear(world, blockPos.AddCopy(1, 2, 0))) return false;
                if (!IsClear(world, blockPos.AddCopy(-1, 2, 0))) return false;
            } else
            if (variant == "east") {
                if (!IsClear(world, blockPos.AddCopy(0, 2, 1))) return false;
                if (!IsClear(world, blockPos.AddCopy(0, 2, -1))) return false;
            } else
            if (variant == "south") {
                if (!IsClear(world, blockPos.AddCopy(-1, 2, 0))) return false;
                if (!IsClear(world, blockPos.AddCopy(1, 2, 0))) return false;
            } else
            if (variant == "west") {
                if (!IsClear(world, blockPos.AddCopy(0, 2, 1))) return false;
                if (!IsClear(world, blockPos.AddCopy(0, 2, -1))) return false;
            }

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

            Block toPlaceBlock;

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
                //Y1Southwest
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southwest-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, 1));
                //Y1Southeast
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southeast-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, 1));
                //Y1South
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1south-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, 1));
                //Y1Southsouth
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southsouth-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, 2));
                //Y2
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 1, 0));
                //Y2West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 1, 0));
                //Y2East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 1, 0));
                //Y3
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 2, 0));
                //Y3West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 2, 0));
                //Y3East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 2, 0));

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
                //Y1Southwest
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southwest-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, -1));
                //Y1Southeast
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southeast-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, 1));
                //Y1South
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1south-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, 0));
                //Y1Southsouth
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southsouth-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-2, 0, 0));
                //Y2
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 1, 0));
                //Y2West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 1, -1));
                //Y2East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 1, 1));
                //Y3
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 2, 0));
                //Y3West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 2, -1));
                //Y3East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 2, 1));

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
                //Y1Southwest
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southwest-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, -1));
                //Y1Southeast
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southeast-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 0, -1));
                //Y1South
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1south-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, -1));
                //Y1Southsouth
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southsouth-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 0, -2));
                //Y2
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 1, 0));
                //Y2West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 1, 0));
                //Y2East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 1, 0));
                //Y3
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 2, 0));
                //Y3West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 2, 0));
                //Y3East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(-1, 2, 0));

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
                //Y1Southwest
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southwest-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, 1));
                //Y1Southeast
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southeast-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, -1));
                //Y1South
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1south-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(1, 0, 0));
                //Y1Southsouth
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy1southsouth-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(2, 0, 0));
                //Y2
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 1, 0));
                //Y2West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 1, 1));
                //Y2East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy2east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 1, -1));
                //Y3
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 2, 0));
                //Y3West
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3west-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 2, 1));
                //Y3East
                toPlaceBlock = world.GetBlock(new AssetLocation("tomes:gutenbergy3east-" + variant));
                world.BlockAccessor.SetBlock(toPlaceBlock.BlockId, pos.AddCopy(0, 2, -1));

            }

        }

        public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
        {

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
                //Y1Southwest
                Block y1Southwest = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, 1));
                if (y1Southwest.Code.Path == "gutenbergy1southwest-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, 1));
                }
                //Y1Southeast
                Block y1Southeast = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, 1));
                if (y1Southeast.Code.Path == "gutenbergy1southeast-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, 1));
                }
                //Y1South
                Block y1South = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, 1));
                if (y1South.Code.Path == "gutenbergy1south-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, 1));
                }
                //Y1Southsouth
                Block y1Southsouth = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, 2));
                if (y1Southsouth.Code.Path == "gutenbergy1southsouth-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, 2));
                }
                //Y2
                Block y2 = world.BlockAccessor.GetBlock(pos.AddCopy(0, 1, 0));
                if (y2.Code.Path == "gutenbergy2-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 1, 0));
                }
                //Y2West
                Block y2West = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 1, 0));
                if (y2West.Code.Path == "gutenbergy2west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 1, 0));
                }
                //Y2East
                Block y2East = world.BlockAccessor.GetBlock(pos.AddCopy(1, 1, 0));
                if (y2East.Code.Path == "gutenbergy2east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 1, 0));
                }
                //Y3
                Block y3 = world.BlockAccessor.GetBlock(pos.AddCopy(0, 2, 0));
                if (y3.Code.Path == "gutenbergy3-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 2, 0));
                }
                //Y3West
                Block y3West = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 2, 0));
                if (y3West.Code.Path == "gutenbergy3west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 2, 0));
                }
                //Y3East
                Block y3East = world.BlockAccessor.GetBlock(pos.AddCopy(1, 2, 0));
                if (y3East.Code.Path == "gutenbergy3east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 2, 0));
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
                //Y1Southwest
                Block y1Southwest = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, -1));
                if (y1Southwest.Code.Path == "gutenbergy1southwest-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, -1));
                }
                //Y1Southeast
                Block y1Southeast = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, 1));
                if (y1Southeast.Code.Path == "gutenbergy1southeast-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, 1));
                }
                //Y1South
                Block y1South = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, 0));
                if (y1South.Code.Path == "gutenbergy1south-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, 0));
                }
                //Y1Southsouth
                Block y1Southsouth = world.BlockAccessor.GetBlock(pos.AddCopy(-2, 0, 0));
                if (y1Southsouth.Code.Path == "gutenbergy1southsouth-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-2, 0, 0));
                }
                //Y2
                Block y2 = world.BlockAccessor.GetBlock(pos.AddCopy(0, 1, 0));
                if (y2.Code.Path == "gutenbergy2-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 1, 0));
                }
                //Y2West
                Block y2West = world.BlockAccessor.GetBlock(pos.AddCopy(0, 1, -1));
                if (y2West.Code.Path == "gutenbergy2west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 1, -1));
                }
                //Y2East
                Block y2East = world.BlockAccessor.GetBlock(pos.AddCopy(0, 1, 1));
                if (y2East.Code.Path == "gutenbergy2east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 1, 1));
                }
                //Y3
                Block y3 = world.BlockAccessor.GetBlock(pos.AddCopy(0, 2, 0));
                if (y3.Code.Path == "gutenbergy3-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 2, 0));
                }
                //Y3West
                Block y3West = world.BlockAccessor.GetBlock(pos.AddCopy(0, 2, -1));
                if (y3West.Code.Path == "gutenbergy3west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 2, -1));
                }
                //Y3East
                Block y3East = world.BlockAccessor.GetBlock(pos.AddCopy(0, 2, 1));
                if (y3East.Code.Path == "gutenbergy3east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 2, 1));
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
                //Y1Southwest
                Block y1Southwest = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, -1));
                if (y1Southwest.Code.Path == "gutenbergy1southwest-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, -1));
                }
                //Y1Southeast
                Block y1Southeast = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 0, -1));
                if (y1Southeast.Code.Path == "gutenbergy1southeast-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 0, -1));
                }
                //Y1South
                Block y1South = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, -1));
                if (y1South.Code.Path == "gutenbergy1south-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, -1));
                }
                //Y1Southsouth
                Block y1Southsouth = world.BlockAccessor.GetBlock(pos.AddCopy(0, 0, -2));
                if (y1Southsouth.Code.Path == "gutenbergy1southsouth-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 0, -2));
                }
                //Y2
                Block y2 = world.BlockAccessor.GetBlock(pos.AddCopy(0, 1, 0));
                if (y2.Code.Path == "gutenbergy2-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 1, 0));
                }
                //Y2West
                Block y2West = world.BlockAccessor.GetBlock(pos.AddCopy(1, 1, 0));
                if (y2West.Code.Path == "gutenbergy2west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 1, 0));
                }
                //Y2East
                Block y2East = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 1, 0));
                if (y2East.Code.Path == "gutenbergy2east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 1, 0));
                }
                //Y3
                Block y3 = world.BlockAccessor.GetBlock(pos.AddCopy(0, 2, 0));
                if (y3.Code.Path == "gutenbergy3-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 2, 0));
                }
                //Y3West
                Block y3West = world.BlockAccessor.GetBlock(pos.AddCopy(1, 2, 0));
                if (y3West.Code.Path == "gutenbergy3west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 2, 0));
                }
                //Y3East
                Block y3East = world.BlockAccessor.GetBlock(pos.AddCopy(-1, 2, 0));
                if (y3East.Code.Path == "gutenbergy3east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(-1, 2, 0));
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
                //Y1Southwest
                Block y1Southwest = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, 1));
                if (y1Southwest.Code.Path == "gutenbergy1southwest-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, 1));
                }
                //Y1Southeast
                Block y1Southeast = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, -1));
                if (y1Southeast.Code.Path == "gutenbergy1southeast-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, -1));
                }
                //Y1South
                Block y1South = world.BlockAccessor.GetBlock(pos.AddCopy(1, 0, 0));
                if (y1South.Code.Path == "gutenbergy1south-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(1, 0, 0));
                }
                //Y1Southsouth
                Block y1Southsouth = world.BlockAccessor.GetBlock(pos.AddCopy(2, 0, 0));
                if (y1Southsouth.Code.Path == "gutenbergy1southsouth-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(2, 0, 0));
                }
                //Y2
                Block y2 = world.BlockAccessor.GetBlock(pos.AddCopy(0, 1, 0));
                if (y2.Code.Path == "gutenbergy2-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 1, 0));
                }
                //Y2West
                Block y2West = world.BlockAccessor.GetBlock(pos.AddCopy(0, 1, 1));
                if (y2West.Code.Path == "gutenbergy2west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 1, 1));
                }
                //Y2East
                Block y2East = world.BlockAccessor.GetBlock(pos.AddCopy(0, 1, -1));
                if (y2East.Code.Path == "gutenbergy2east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 1, -1));
                }
                //Y3
                Block y3 = world.BlockAccessor.GetBlock(pos.AddCopy(0, 2, 0));
                if (y3.Code.Path == "gutenbergy3-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 2, 0));
                }
                //Y3West
                Block y3West = world.BlockAccessor.GetBlock(pos.AddCopy(0, 2, 1));
                if (y3West.Code.Path == "gutenbergy3west-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 2, 1));
                }
                //Y3East
                Block y3East = world.BlockAccessor.GetBlock(pos.AddCopy(0, 2, -1));
                if (y3East.Code.Path == "gutenbergy3east-" + variant)
                {
                    world.BlockAccessor.SetBlock(0, pos.AddCopy(0, 2, -1));
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
