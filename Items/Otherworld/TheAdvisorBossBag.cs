using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Advisor;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Otherworld.EpicWings;

namespace SOTS.Items.Otherworld
{
	public class TheAdvisorBossBag : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/TheAdvisorBossBagGlow").Value;
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/TheAdvisorBossBagGlow").Value;
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
				new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y + 2),
				null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override int BossBagNPC => ModContent.NPCType<TheAdvisorHead>();
		public override bool CanRightClick()
		{
			return true;
		}
		public override void OpenBossBag(Player player)
		{
			player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<TwilightGyroscope>());
			if(Main.rand.NextBool(3))
				player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<StarlightAlloy>(), Main.rand.Next(12, 19));
			else
				player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<SkywareKey>());

			if (Main.rand.NextBool(3))
				player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<OtherworldlyAlloy>(), Main.rand.Next(12, 19));
			else
				player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<MeteoriteKey>());

			if (Main.rand.NextBool(3))
				player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<HardlightAlloy>(), Main.rand.Next(12, 19));
			else
				player.QuickSpawnItem(player.GetSource_Loot(), ModContent.ItemType<StrangeKey>());
		}
	}
}