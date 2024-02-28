using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using static SOTS.ItemHelpers;

namespace SOTS.Items.Conduit
{
	public class InfinityPouch : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Color color = new Color(110, 100, 130, 0);
			for (int k = 0; k < 6; k++)
			{
				Vector2 offset = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 60));
				DrawInInventoryBobbing(spriteBatch, Item, position + offset, frame, color * 1.2f * (1f - (Item.alpha / 255f)), scale, 0.5f, 0.25f);
			}
			DrawInInventoryBobbing(spriteBatch, Item, position, frame, Color.White, scale, 0.5f, 0.5f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Color color = new Color(110, 100, 130, 0);
			for (int k = 0; k < 6; k++)
			{
				Vector2 offset = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 60));
				DrawInWorldBobbing(spriteBatch, Item, offset, color * 1.2f * (1f - (Item.alpha / 255f)), ref rotation, ref scale, 0.5f, 0.5f);
			}
			DrawInWorldBobbing(spriteBatch, Item, Vector2.Zero, Color.White, ref rotation, ref scale, 0.5f, 0.5f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 30;     
            Item.height = 38;   
            Item.value = Item.sellPrice(gold: 10);
			Item.rare = ModContent.RarityType<AnomalyRarity>();
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.SOTSPlayer().InfinityPouch = true;
        }
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Vector3 vColor = ColorHelpers.VoidAnomaly.ToVector3() * 0.5f;
			Lighting.AddLight(Item.position, vColor);
		}
	}
}

