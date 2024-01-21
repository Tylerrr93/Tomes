using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Tomes
{
    public class BlockEntityGutenbergPress : BlockEntity
    {
        BlockGutenbergPress ownBlock;
        ICoreClientAPI capi;
        MeshData meshMovable;

        public override void Initialize(ICoreAPI api)
        {
            base.Initialize(api);
            this.ownBlock = Block as BlockGutenbergPress;
            capi = api as ICoreClientAPI;

            if (ownBlock != null)
            {
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

            return false;
        }
    }
}
