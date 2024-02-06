
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Tomes
{
    public class BlockEntityGutenbergPress : BlockEntityContainer
    {
        MeshData meshMovable;
        MeshData meshTypecast;

        // For referencing multiple selection boxes or blocks
        // Frisket comes from southsouth block
        public enum EnumGutenbergPressSection
        {
            Frisket
        }

        InventoryGeneric inv;
        public override InventoryBase Inventory => inv;
        public override string InventoryClassName => "gutenbergpress";
        public ItemSlot FrisketSlot0 => inv[0];
        public ItemSlot FrisketSlot1 => inv[1];

        public BlockEntityGutenbergPress()
        {
            inv = new InventoryGeneric(2, "gutenbergpress-0", null, null);
        }

        BlockGutenbergPress ownBlock;
        ICoreClientAPI capi;

        AssetLocation typecastSound = new AssetLocation("game", "sounds/effect/crusher-impact1");

        public int typecastAdded;

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
            this.ownBlock = Block as BlockGutenbergPress;
            capi = api as ICoreClientAPI;

            if (ownBlock != null)
            {
                // This calls the top half of the gutenberg press to be tessalated into the model at initialization 
                Shape shape = Shape.TryGet(api, "tomes:shapes/gutenbergpress/part-movable.json");

                if (api.Side == EnumAppSide.Client)
                {
                    capi.Tesselator.TesselateShape(ownBlock, shape, out meshMovable, new Vec3f(0, ownBlock.Shape.rotateY, 0));
                }

            }
        }

        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
        {
            bool skip = base.OnTesselation(mesher, tessThreadTesselator);

            if (!skip) mesher.AddMeshData(meshMovable);

            mesher.AddMeshData(meshTypecast);

            return false;
        }

        public bool OnBlockInteractStart(IPlayer byPlayer, BlockSelection blockSel, EnumGutenbergPressSection section)
        {   

            if (section == EnumGutenbergPressSection.Frisket)
            {
                return InteractFrisket(byPlayer, blockSel);
            }

            return false;

        }

        private bool InteractFrisket(IPlayer byPlayer, BlockSelection blockSel)
        {
            ItemSlot handslot = byPlayer.InventoryManager.ActiveHotbarSlot;
            ItemStack handStack = handslot.Itemstack;

            // Check if the player's hand slot is empty and the frisket slot is not empty
            if (handslot.Empty && !FrisketSlot0.Empty)
            {
                // Give the itemstack in the frisket slot to the player
                if (!byPlayer.InventoryManager.TryGiveItemstack(FrisketSlot0.Itemstack, true))
                {
                    // If giving fails, spawn the item entity in the world
                    Api.World.SpawnItemEntity(FrisketSlot0.Itemstack, Pos.ToVec3d().Add(0.5, 0.5, 0.5));
                }

                Api.World.PlaySoundAt(typecastSound, Pos.X + 0.5, Pos.Y, Pos.Z + 0.5, byPlayer);

                // Clear the frisket slot and update the block entity
                typecastAdded = 0;
                FrisketSlot0.Itemstack = null;
                MarkDirty(true);
                meshTypecast?.Clear();
            }
            // Check if the player's hand slot contains an item and it's a parchment
            else if (handStack != null && handStack.Collectible.Code.Path == "paper-parchment" && FrisketSlot0.Empty)
            {
                // Try putting the item from the player's hand slot into the frisket slot
                bool moved = handslot.TryPutInto(Api.World, FrisketSlot0, 1) > 0;

                // If the item is successfully moved, update slots, mesh, and play the typecast sound
                if (moved)
                {
                    typecastAdded = 1;
                    handslot.MarkDirty();
                    MarkDirty(true);
                    generateMeshTypecast();
                    Api.World.PlaySoundAt(typecastSound, Pos.X + 0.5, Pos.Y, Pos.Z + 0.5, byPlayer);
                }
            }

            return true;
        }

        private void generateMeshTypecast() {

        Console.WriteLine("generateMeshTypecast is attempting to run");
        Console.WriteLine("capi is equal to: " + capi);
        Console.WriteLine ("typecastAdded is currently:" + typecastAdded);

        // Checks to make sure capi is not null, will crash if it is. 
        if (typecastAdded == 0 || capi == null) return;
        Console.WriteLine("generateMeshTypecast passes for typecast not being empty and capi not being null");

        // Load the typecast mesh and set it as meshSource
        Shape shapeTypecast = Shape.TryGet(Api, "tomes:shapes/gutenbergpress/part-typecast.json");
        var meshSource = shapeTypecast;

        // If loaded sucessfully...
        if (meshSource != null) {

            MeshData meshTempTypecast;
            capi.Tesselator.TesselateShape(ownBlock, meshSource, out meshTempTypecast, new Vec3f(0, ownBlock.Shape.rotateY, 0));
            meshTypecast = meshTempTypecast.Clone();
            Console.WriteLine("meshTempTypecast cloned to meshTypecast...generateMeshTypecast is complete");

        }

        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
        {
            base.FromTreeAttributes(tree, worldForResolving);

            typecastAdded = tree.GetInt("typecastAdded");
            Console.WriteLine("FromTreeAttributes is running...Typecast value: " + typecastAdded);

            if (worldForResolving.Side == EnumAppSide.Client && typecastAdded == 1) {
                    Console.WriteLine("generateMeshTypecast will run from FromTreeAttributes");
                    generateMeshTypecast();
            }
                
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetInt("typecastAdded", typecastAdded);
            Console.WriteLine("ToTreeAttributes has been ran");
        }

    }
}

