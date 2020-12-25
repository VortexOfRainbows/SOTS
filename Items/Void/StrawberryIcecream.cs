using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;

namespace SOTS.Items.Void
{
	public class StrawberryIcecream : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Strawberry Icecream");
			Tooltip.SetDefault("Automatically consumed when void is low\n50% chance to refill 10 void and surround you with an ice shard\n50% chance to refill 6 void and surround you with 2 ice shards");
		}
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 7, 50);
			item.rare = 1;
			item.maxStack = 999;

			item.useStyle = 2;
			item.useTime = 15;
			item.useAnimation = 15;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
		}
		public override bool UseItem(Player player)
		{
			return true;
		}
		public void RefillEffect(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");

			int rand = Main.rand.Next(2);
			if (rand == 0)
			{
				voidPlayer.voidMeter += 10;
				VoidPlayer.VoidEffect(player, 10);
				Vector2 circularSpeed = new Vector2(0, -12);
				int calc = 10 + modPlayer.bonusShardDamage;
				if (calc <= 0) calc = 1;
					Projectile.NewProjectile(player.Center.X, player.Center.Y, circularSpeed.X, circularSpeed.Y, mod.ProjectileType("ShatterShard"), calc, 3f, player.whoAmI);
				
			}
			else
			{
				voidPlayer.voidMeter += 6;
				VoidPlayer.VoidEffect(player, 6);
				for (int i = 0; i < 2; i++)
				{
					Vector2 circularSpeed = new Vector2(12, 0).RotatedBy(MathHelper.ToRadians(i * 180));
					int calc = 10 + modPlayer.bonusShardDamage;
					if (calc <= 0) calc = 1;
						Projectile.NewProjectile(player.Center.X, player.Center.Y, circularSpeed.X, circularSpeed.Y, mod.ProjectileType("ShatterShard"), calc, 3f, player.whoAmI);
				}
			}
		}
		public override bool ConsumeItem(Player player)
		{
			return true;
		}
		public override void OnConsumeItem(Player player)
		{
			RefillEffect(player);
			base.OnConsumeItem(player);
		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			while(voidPlayer.voidMeter < (voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls) / 10 && voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls > 40)
			{
				RefillEffect(player);
				item.stack--;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Curgeon", 1);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}