using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Crushers;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Boss.Lux;
using SOTS.Void;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class LuxBag : ModItem
	{
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/LuxBag").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Color color;
			for (int k = 0; k < 12; k++)
			{
				Vector2 circular = new Vector2(3 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 30 + Main.GameUpdateCount * 1.2f));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 30));
				color.A = 0;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + circular, null, color * 0.4f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, Color.White * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
			Color color;
			for (int k = 0; k < 12; k++)
			{
				Vector2 circular = new Vector2(2 * scale, 0).RotatedBy(MathHelper.ToRadians(k * 30 + Main.GameUpdateCount * 1.2f));
				color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(k * 30));
				color.A = 0;
				Main.spriteBatch.Draw(texture, position + circular, frame, color * 0.4f, 0f, origin, scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, position, frame, Color.White * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = false;
			this.SetResearchCost(3);
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 34;
			Item.value = 0;
			Item.rare = ItemRarityID.LightPurple;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.expert = true;
		}
		public override int BossBagNPC => ModContent.NPCType<Lux>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<VoidAnomaly>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhaseOre>(), 1, 120, 180));
			itemLoot.Add(ItemDropRule.Common(ItemID.SoulofLight, 1, 10, 20));
		}
	}
}