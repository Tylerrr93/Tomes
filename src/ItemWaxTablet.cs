using Vintagestory.GameContent;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Tomes
{
    public class ItemWaxTablet : ItemBook
    {
        // This code is going to be responsible for replacing the unlocked variant of a written item with a locked version of it
        public override void OnHeldInteractStart(
            ItemSlot slot,
            EntityAgent byEntity,
            BlockSelection blockSel,
            EntitySelection entitySel,
            bool firstEvent,
            ref EnumHandHandling handling)
        {
            // Check if the book is signed
            if (isSigned(slot))
            {
                
                // Make the book not signed
                slot.Itemstack.Attributes.RemoveAttribute("signedby");

            }

            base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
        }
    }
}
