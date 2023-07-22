using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Fragments;
using Microsoft.Xna.Framework;
using SOTS.Items.Conduit;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Void
{
	public class SoulHeart : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Color color = new Color(110, 100, 130, 0);
			for (int k = 0; k < 8; k++)
			{
				Vector2 offset = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 45));
				ItemHelpers.DrawInInventoryBobbing(spriteBatch, Item, position + offset, frame, color * 1.2f * (1f - (Item.alpha / 255f)), scale, 0.25f);
			}
			for (int k = 0; k < 10; k++)
			{
				Vector2 offset = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * -2 + k * 36));
				ItemHelpers.DrawInInventoryBobbing(spriteBatch, Item, position + offset, frame, Color.Lerp(color, Color.Black, 0.8f) * 0.75f * (1f - (Item.alpha / 255f)), scale, 0.25f);
			}
			ItemHelpers.DrawInInventoryBobbing(spriteBatch, Item, position, frame, Color.White, scale, 0.25f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Color color = new Color(110, 100, 130, 0);
			for (int k = 0; k < 8; k++)
			{
				Vector2 offset = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 45));
				ItemHelpers.DrawInWorldBobbing(spriteBatch, Item, offset, color * 1.2f * (1f - (Item.alpha / 255f)), ref rotation, ref scale, 0.25f);
			}
			for (int k = 0; k < 10; k++)
			{
				Vector2 offset = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * -2 + k * 36));
				ItemHelpers.DrawInWorldBobbing(spriteBatch, Item, offset, Color.Lerp(color, Color.Black, 0.8f) * 0.75f * (1f - (Item.alpha / 255f)), ref rotation, ref scale, 0.25f);
			}
			ItemHelpers.DrawInWorldBobbing(spriteBatch, Item, Vector2.Zero, Color.White, ref rotation, ref scale, 0.25f);
			return false;
		}
        public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 32;
			Item.useAnimation = 12;
			Item.useTime = 12;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.value = 0;
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = 999;
			Item.autoReuse = false;
			Item.consumable = true;
			ItemID.Sets.ItemNoGravity[Item.type] = false;
		}
		public override bool? UseItem(Player player)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.NPCDeath39, player.Center);
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (voidPlayer.voidSoul < 1)
			{
				voidPlayer.voidMeterMax += 50;
				VoidPlayer.VoidEffect(player, 50);
				voidPlayer.voidSoul += 1;
				return true;
			}
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.LifeCrystal, 1).AddIngredient(ModContent.ItemType<SkipSoul>(), 50).AddTile(TileID.DemonAltar).Register();
		}
	}
}