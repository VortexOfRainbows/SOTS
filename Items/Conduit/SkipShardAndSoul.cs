using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Pyramid;
using SOTS.Void;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static SOTS.ItemHelpers;

namespace SOTS.Items.Conduit
{
	public class SkipSoul : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Color color = new Color(110, 100, 130, 0);
			for (int k = 0; k < 8; k++)
			{
				Vector2 offset = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 45));
				Main.spriteBatch.Draw(texture, position + offset, frame, color * 1.2f * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			for (int k = 0; k < 10; k++)
			{
				Vector2 offset = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * -2 + k * 36));
				Main.spriteBatch.Draw(texture, position + offset, frame, Color.Lerp(color, Color.Black, 0.8f) * 0.75f * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, position, frame, Color.White * 1f, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Color color = new Color(110, 100, 130, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 8; k++)
			{
				Vector2 offset = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 45));
				Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + offset, null, color * 1.2f * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			for (int k = 0; k < 10; k++)
			{
				Vector2 offset = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * -2 + k * 36));
				Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + offset, null, Color.Lerp(color, Color.Black, 0.8f) * 0.75f * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, Color.White * 1f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = true;
			this.SetResearchCost(200);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 24;
			Item.maxStack = 9999;
			Item.rare = ModContent.RarityType<AnomalyRarity>();
			Item.value = Item.sellPrice(0, 0, 75, 0);
		}
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
		}
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Vector3 vColor = ColorHelpers.VoidAnomaly.ToVector3() * 0.2f;
			Lighting.AddLight(Item.position, vColor);
		}
	}
	public class SkipShard : ModItem
	{
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Color alphaColor = ColorHelpers.VoidAnomaly;
			alphaColor.A = 0;
			for (int k = 0; k < 6; k++)
			{
				Vector2 offset = new Vector2(3f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 60));
				ItemHelpers.DrawInInventoryBobbing(spriteBatch, Item, position + offset, frame, alphaColor, scale, 1f, 0.75f);
			}
			ItemHelpers.DrawInInventoryBobbing(spriteBatch, Item, position, frame, ColorHelpers.VoidAnomaly * 1.5f, scale, 1f, 0.75f);
			return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Color alpha2Color = ColorHelpers.VoidAnomaly;
			alpha2Color.A = 0;
			for (int k = 0; k < 6; k++)
			{
				Vector2 offset = new Vector2(3f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 60));
				ItemHelpers.DrawInWorldBobbing(spriteBatch, Item, offset, alpha2Color, ref rotation, ref scale, 1f, 0.75f);
			}
			ItemHelpers.DrawInWorldBobbing(spriteBatch, Item, Vector2.Zero, ColorHelpers.VoidAnomaly * 1.5f, ref rotation, ref scale, 1f, 0.75f);
            return false;
        }
        public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = true;
			this.SetResearchCost(200);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 30;
			Item.maxStack = 9999;
			Item.rare = ModContent.RarityType<AnomalyRarity>();
			Item.value = Item.sellPrice(0, 0, 37, 50);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
        public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Vector3 vColor = ColorHelpers.VoidAnomaly.ToVector3() * 0.34f;
			Lighting.AddLight(Item.position, vColor);
		}
    }
}