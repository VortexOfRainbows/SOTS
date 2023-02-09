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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/HyperdriveGlow").Value;
			Color color = new Color(80, 80, 80, 0);
			for (int k = 0; k < 4; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Chaos/HyperdriveGlow").Value;
			Color color = new Color(80, 80, 80, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.1f;
				float y = Main.rand.Next(-10, 11) * 0.1f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("The second charge of a Crusher has a 33% chance to not consume void\nThe fourth charge of Crushers no longer consumes void\nExtends the range of Crushers by 1\nIncreases attack speed by 25% and melee damage by 5%\nReduces void cost by 10%");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 30;   
            Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer.ModPlayer(player).attackSpeedMod += 0.25f;
			player.GetDamage(DamageClass.Melee) += 0.05f;
			vPlayer.voidCost -= 0.1f;
			vPlayer.CrushResistor = true;
			vPlayer.CrushCapacitor = true;
			vPlayer.BonusCrushRangeMax++;
			vPlayer.BonusCrushRangeMin++;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PhaseBar>(), 20).AddIngredient(ModContent.ItemType<CircuitBoard>(), 1).AddIngredient(ModContent.ItemType<VibrancyModule>(), 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}