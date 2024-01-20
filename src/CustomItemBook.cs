using Vintagestory.GameContent;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace Tomes
{
    public class CustomItemBook : ItemBook
    {
        // This code is going to be responsible for replacing the unlocked variant of a written item with a locked version of it
        private static void TryChangeVariant(ItemStack stack, ICoreAPI api, string variantName, string variantValue, bool saveAttributes = true)
        {
            if (stack?.Collectible?.Variant?.ContainsKey(variantName) == null) return;

            ITreeAttribute clonedAttributes = stack.Attributes.Clone();
            int size = stack.StackSize;
            ItemStack newStack = new();

            switch (stack.Collectible.ItemClass)
            {
                case EnumItemClass.Block:
                    newStack = new ItemStack(api.World.GetBlock(stack.Collectible.CodeWithVariant(variantName, variantValue)));
                    break;

                case EnumItemClass.Item:
                    newStack = new ItemStack(api.World.GetItem(stack.Collectible.CodeWithVariant(variantName, variantValue)));
                    break;
            }

            if (saveAttributes) newStack.Attributes = clonedAttributes;
            newStack.StackSize = size;

            stack.SetFrom(newStack);
        }

        public override void OnHeldIdle(ItemSlot slot, EntityAgent byEntity)
        {
            base.OnHeldIdle(slot, byEntity);

            // Check if the book is signed
            if (isSigned(slot))
            {
                // Perform your custom action here when the book is idle and signed
                // For example, you can update the variant to "locked" using TryChangeVariant method
                TryChangeVariant(slot.Itemstack, api, "type", "locked");

                // Optionally, you can print a debug message
                System.Console.WriteLine("CustomItemBook is idle and signed!");
            }
        }
    }
}
