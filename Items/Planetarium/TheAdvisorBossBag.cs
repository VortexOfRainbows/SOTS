using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Advisor;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Planetarium.EpicWings;
using Terraria.GameContent.ItemDropRules;

namespace SOTS.Items.Planetarium
{
	public class TheAdvisorBossBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.BossBag[Type] = true;
			ItemID.Sets.PreHardmodeLikeBossBag[Type] = true;
			this.SetResearchCost(3);
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.value = 0;
			Item.rare = ItemRarityID.LightPurple;
			Item.expert = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/TheAdvisorBossBagGlow").Value;
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				if (k == 0)
				{
					x = 0;
					y = 0;
				}
				Main.spriteBatch.Draw(texture,
				new Vector2(position.X + x, position.Y + y),
				null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/TheAdvisorBossBagGlow").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				if (k == 0)
				{
					x = 0;
					y = 0;
				}
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override bool CanRightClick()
		{
			return true;
		}
		public override void ModifyItemLoot(ItemLoot itemLoot)
		{
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<TwilightGyroscope>()));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<OtherworldlyAlloy>(), 3, 12, 18))
				.OnFailedRoll(ItemDropRule.Common(ModContent.ItemType<MeteoriteKey>(), 1, 1, 1));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<StarlightAlloy>(), 3, 12, 18))
				.OnFailedRoll(ItemDropRule.Common(ModContent.ItemType<SkywareKey>(), 1, 1, 1));
			itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<HardlightAlloy>(), 3, 12, 18))
				.OnFailedRoll(ItemDropRule.Common(ModContent.ItemType<StrangeKey>(), 1, 1, 1));
			itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<TheAdvisorHead>()));
		}
	}
}