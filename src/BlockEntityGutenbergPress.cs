
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace Tomes
{
    public class BlockEntityGutenbergPress : BlockEntityContainer
    {
        InventoryGeneric inv;
        public override InventoryBase Inventory => inv;
        public override string InventoryClassName => "gutenbergpress";
        public ItemSlot TraySlot0 => inv[0];
        public ItemSlot TraySlot1 => inv[1];

        public BlockEntityGutenbergPress()
        {
            inv = new InventoryGeneric(2, "gutenbergpress-0", null, null);
        }

        public enum EnumGutenbergPressSection
        {
            //Tray is currently the y1 south block
            //Frisket is currently the y1 "southsouth" block
            Tray,
            Frisket
        }

        MeshData meshMovable;
        MeshData meshTypecast;
        MeshData meshTypecastInked;

        BlockGutenbergPress ownBlock;
        ICoreClientAPI capi;
        
        AssetLocation typecastSound = new AssetLocation("game", "sounds/effect/crusher-impact1");
        
        public bool typecastAdded;
        public bool typecastBareVisible;
        public bool typecastIsInked;
        public bool typecastInkedVisible;

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
            this.ownBlock = Block as BlockGutenbergPress;
            capi = api as ICoreClientAPI;

            if (ownBlock != null)
            {    
                if (api.Side == EnumAppSide.Client)
                {
                    Shape shape = Shape.TryGet(api, "tomes:shapes/gutenbergpress/part-movable.json");
                    capi.Tesselator.TesselateShape(ownBlock, shape, out meshMovable, new Vec3f(0, ownBlock.Shape.rotateY, 0));
                    Shape shapeTypecast = Shape.TryGet(Api, "tomes:shapes/gutenbergpress/part-typecast.json");
                    capi.Tesselator.TesselateShape(ownBlock, shapeTypecast, out meshTypecast, new Vec3f(0, ownBlock.Shape.rotateY, 0));
                    Shape shapeTypecastInked = Shape.TryGet(Api, "tomes:shapes/gutenbergpress/part-typecast-inked.json");
                    capi.Tesselator.TesselateShape(ownBlock, shapeTypecastInked, out meshTypecastInked, new Vec3f(0, ownBlock.Shape.rotateY, 0));
                }
            }
        }

        public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
        {
            bool skip = base.OnTesselation(mesher, tessThreadTesselator);
            // This adds the metal part of the press
            if (!skip) mesher.AddMeshData(meshMovable);
            // If typecast bare (not inked) model is supposed to be visible, add the typecast mesh
            if (typecastBareVisible == true) {
                mesher.AddMeshData(meshTypecast);
            }
            if (typecastInkedVisible == true) {
                mesher.AddMeshData(meshTypecastInked);
            }
            return false;
        }

        public bool OnBlockInteractStart(IPlayer byPlayer, BlockSelection blockSel, EnumGutenbergPressSection section)
        {   
            if (section == EnumGutenbergPressSection.Tray)
            {
                return InteractTray(byPlayer, blockSel);
            }
            return false;
        }

        private bool InteractTray(IPlayer byPlayer, BlockSelection blockSel)
        {
            ItemSlot handslot = byPlayer.InventoryManager.ActiveHotbarSlot;
            ItemStack handStack = handslot.Itemstack;

            // Check if the player's hand slot is empty and the tray slot is not empty
            if (handslot.Empty && !TraySlot0.Empty)
            {
                // If typecast is inked already, prevent typecast from being pulled out
                if (typecastIsInked == true) {
                    return true;
                }

                // Give the itemstack in the tray slot to the player
                if (!byPlayer.InventoryManager.TryGiveItemstack(TraySlot0.Itemstack, true))
                {
                    // If giving fails, spawn the item entity in the world
                    Api.World.SpawnItemEntity(TraySlot0.Itemstack, Pos.ToVec3d().Add(0.5, 0.5, 0.5));
                }
                // Set the typecast mesh to clear, clear the tray slot, and update the block entity
                typecastAdded = false;
                typecastBareVisible = false;
                TraySlot0.Itemstack = null;
                MarkDirty(true);
                Api.World.PlaySoundAt(typecastSound, Pos.X + 0.5, Pos.Y, Pos.Z + 0.5, byPlayer);
            }

            // If the typecast has been added, check if the player is holding a stick (to be replaced with ink beaters)
            if (!handslot.Empty && typecastAdded == true && handStack.Collectible.Code.Path == "stick")
            {
                // Check to prevent double inkings
                if (typecastIsInked == true) {
                    return true;
                }

                // Dummy logic for inking: disable bare typecast mesh then set the inked one to true and to true to appear, finally play sound
                typecastBareVisible = false;
                typecastInkedVisible = true;
                typecastIsInked = true;
                MarkDirty(true);
                Api.World.PlaySoundAt(typecastSound, Pos.X + 0.5, Pos.Y, Pos.Z + 0.5, byPlayer);
            }

            // Check if the player's hand slot contains an item and it's a parchment
            // !!!THIS WILL NEED TO BE CHANGED TO BE A FILLED TYPECAST ONCE TYPECASTS ARE SORTED OUT!!!
            else if (handStack != null && handStack.Collectible.Code.Path == "paper-parchment" && TraySlot0.Empty)
            {
                // If typecast is inked already, return as the typecast is already added
                if (typecastIsInked == true) {
                    return true;
                }
                
                // Try putting the item from the player's hand slot into the tray slot
                bool moved = handslot.TryPutInto(Api.World, TraySlot0, 1) > 0;
                // If the item is successfully moved, update slots, mesh, and play the typecast sound
                if (moved)
                {
                    typecastAdded = true;
                    typecastBareVisible = true;
                    handslot.MarkDirty();
                    MarkDirty(true);
                    Api.World.PlaySoundAt(typecastSound, Pos.X + 0.5, Pos.Y, Pos.Z + 0.5, byPlayer);
                }
            }

            return true;
        }

        public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
        {
            base.FromTreeAttributes(tree, worldForResolving);
            typecastAdded = tree.GetBool("typecastAdded");
            typecastBareVisible = tree.GetBool("typecastBareVisible");
            typecastIsInked = tree.GetBool("typecastIsInked");
            typecastInkedVisible = tree.GetBool("typecastInkedVisible");
            Console.WriteLine("FromTreeAttributes has been ran...Typecast value: " + typecastAdded + "\nTypecast-bare Visible: " + typecastBareVisible);  
        }

        public override void ToTreeAttributes(ITreeAttribute tree)
        {
            base.ToTreeAttributes(tree);
            tree.SetBool("typecastAdded", typecastAdded);
            tree.SetBool("typecastBareVisible", typecastBareVisible);
            tree.SetBool("typecastIsInked", typecastIsInked);
            tree.SetBool("typecastInkedVisible", typecastInkedVisible);
            Console.WriteLine("ToTreeAttributes has been ran...Typecast added: " + typecastAdded + "\nTypecast-bare Visible: " + typecastBareVisible);
        }

    }
}