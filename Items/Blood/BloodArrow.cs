using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Blood
{
	public class BloodArrow : VoidItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Arrow");
			Tooltip.SetDefault("Pierces and lifesteals\nAutomatically refuels using 4 void\nDecays while in the inventory");
		}public override void SafeSetDefaults()
		{
			item.damage = 11;
			item.ranged = true;
			item.width = 18;
			item.height = 38;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 0.15f;
			item.value = 4;
			item.rare = 10;
			item.shoot = mod.ProjectileType("BloodArrow");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 2.25f;                  //The speed of the projectile
			item.ammo = AmmoID.Arrow;   
            item.UseSound = SoundID.Item23;
		}
		public override void UpdateInventory(Player player)
		{
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
				SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
				if(modPlayer.bloodDecay)
				{
				timer += (int)(item.stack * 0.1f);
				}
				if(timer >= 999)
				{
					timer = 0;
					item.stack--;
				}
			
				if(item.stack < 6)
				{
						voidPlayer.voidMeter -= 4;
						item.stack += 10;
				}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BloodEssence", 2);
			recipe.AddIngredient(null, "RedPowerChamber", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 55);
			recipe.AddRecipe();
			
		}
	}
}