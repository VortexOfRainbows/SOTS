using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Fragments
{
	public class FragmentOfEvil : ModItem
	{
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fragment of Evil");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 26;
            item.value = Item.sellPrice(0, 0, 0, 50);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 999;
		}
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			Texture2D texture = WorldGen.crimson ? mod.GetTexture("Items/Fragments/FragmentOfEvil") : mod.GetTexture("Items/Fragments/FragmentOfEvilAlt");
			spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = WorldGen.crimson ? mod.GetTexture("Items/Fragments/FragmentOfEvil") : mod.GetTexture("Items/Fragments/FragmentOfEvilAlt");
			spriteBatch.Draw(texture, item.Center - Main.screenPosition, null, lightColor, 0f, new Vector2(texture.Width/2, texture.Height/2), scale, SpriteEffects.None, 0f);
			return false;
        }
    }
}