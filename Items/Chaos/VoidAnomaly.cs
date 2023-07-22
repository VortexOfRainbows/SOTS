using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;

namespace SOTS.Items.Chaos
{
	public class VoidAnomaly : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Color color = new Color(110, 100, 130, 0);
			for (int k = 0; k < 8; k++)
			{
				Vector2 offset = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 45));
				ItemHelpers.DrawInInventoryBobbing(spriteBatch, Item, position + offset, frame, color * 1.2f * (1f - (Item.alpha / 255f)), scale, 0.5f);
			}
			for (int k = 0; k < 10; k++)
			{
				Vector2 offset = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * -2 + k * 36));
				ItemHelpers.DrawInInventoryBobbing(spriteBatch, Item, position + offset, frame, Color.Lerp(color, Color.Black, 0.8f) * 0.75f * (1f - (Item.alpha / 255f)), scale, 0.5f);
			}
			ItemHelpers.DrawInInventoryBobbing(spriteBatch, Item, position, frame, Color.White, scale, 0.5f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Color color = new Color(110, 100, 130, 0);
			for (int k = 0; k < 8; k++)
			{
				Vector2 offset = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 45));
				ItemHelpers.DrawInWorldBobbing(spriteBatch, Item, offset, color * 1.2f * (1f - (Item.alpha / 255f)), ref rotation, ref scale, 0.5f);
			}
			for (int k = 0; k < 10; k++)
			{
				Vector2 offset = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * -2 + k * 36));
				ItemHelpers.DrawInWorldBobbing(spriteBatch, Item, offset, Color.Lerp(color, Color.Black, 0.8f) * 0.75f * (1f - (Item.alpha / 255f)), ref rotation, ref scale, 0.5f);
			}
			ItemHelpers.DrawInWorldBobbing(spriteBatch, Item, Vector2.Zero, Color.White, ref rotation, ref scale, 0.5f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 44;     
            Item.height = 26;   
            Item.value = Item.sellPrice(platinum: 1);
            Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.bonusVoidGain += 1f;
			vPlayer.voidRegenSpeed += 0.05f;
			//modPlayer.VMincubator = true;
			modPlayer.VoidAnomaly = true;
		}
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Vector3 vColor = ColorHelpers.VoidAnomaly.ToVector3() * 0.5f;
			Lighting.AddLight(Item.position, vColor);
		}
	}
}

