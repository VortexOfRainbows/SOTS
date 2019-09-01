using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;


namespace SOTS.Items.Blood
{
	public class LifeDevourer: ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Life Devourer");
			Tooltip.SetDefault("Drains your life to generate blood which can be used for crafting\nIncreases max void by 20, void damage by 8%, and boosts void regen speed by 1\nWhen at extremely low life, it will remove soul fragments instead");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 4));
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 44;
			item.value = 100000;
			item.rare = 10;
			item.maxStack = 1;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 20;
			voidPlayer.voidDamage += 0.08f;
			voidPlayer.voidRegen += 0.1f;
						
						
						
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			modPlayer.bloodDecay = false;
			
			player.lifeRegen -= 5;
			timer++;
			if(timer >= (350 + player.statLifeMax) - player.statLife && player.statLife > 20)
			{
				player.statLife -= 3;
				if(player.statLife < 20)
				{
					player.statLife = 20;
				}
				player.QuickSpawnItem(mod.ItemType("BloodEssence"), 1);	
				timer = 0;
			}
			
			if(player.statLife < 3)
			{
			player.lifeRegen = 0;
				if(timer >= 120 && player.statLifeMax2 > 1 && modPlayer.soulAmount > -100)
				{
					timer = 0;
					modPlayer.soulAmount--;
					player.QuickSpawnItem(mod.ItemType("SoulFragment"), 1);	
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "RedPowerChamber", 4);
			recipe.AddIngredient(null, "SteelBar", 4);
			recipe.AddIngredient(null, "BrassBar", 4);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}