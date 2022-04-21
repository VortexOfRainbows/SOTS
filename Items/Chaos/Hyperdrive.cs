using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Crushers;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Chaos
{
	public class Hyperdrive : ModItem
	{
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/HyperdriveGlow");
			Color color = new Color(80, 80, 80, 0);
			for (int k = 0; k < 4; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Chaos/HyperdriveGlow");
			Color color = new Color(80, 80, 80, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y + 2), null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("The second charge of a Crusher has a 33% chance to not consume void\nThe fourth charge of Crushers no longer consumes void\nExtends the range of Crushers by 1\nIncreases attack speed by 25% and melee damage by 5%\nReduces void cost by 10%");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 28;     
            item.height = 30;   
            item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer.ModPlayer(player).attackSpeedMod += 0.25f;
			player.meleeDamage += 0.05f;
			vPlayer.voidCost -= 0.1f;
			vPlayer.CrushResistor = true;
			vPlayer.CrushCapacitor = true;
			vPlayer.BonusCrushRangeMax++;
			vPlayer.BonusCrushRangeMin++;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PhaseBar>(), 20);
			recipe.AddIngredient(ModContent.ItemType<CircuitBoard>(), 1);
			recipe.AddIngredient(ModContent.ItemType<VibrancyModule>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}