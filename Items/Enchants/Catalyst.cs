using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Enchants
{
	public class Catalyst : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic XXV : Catalyst");
			Tooltip.SetDefault("Does nothing by itself, it is merely fuel\nSome minions might start firing your own weapons for you\nE");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(9, 4));
		}
		public override void SetDefaults()
		{
      
            item.width = 14;     
            item.height = 26;   
            item.value = 1000000000;
            item.rare = 5;
			item.expert = true;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"EMaterial", 1);
			recipe.AddIngredient(null,"SummonMaterial", 1);
			recipe.AddIngredient(null,"RollofDuctTape", 1);
			recipe.AddIngredient(null,"MegaMelon", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"TheHardCore", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.Catalyst = true;
			if(modPlayer.lostSoul)
			{
			player.thrownDamage += 1.15f;
			player.minionDamage += 0.15f;
			player.meleeDamage += 1.15f;
			player.magicDamage += 1.15f;
			player.rangedDamage += 1.15f;
				
			}
			
				  
		}
		
	}
}
