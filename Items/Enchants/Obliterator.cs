using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;



namespace SOTS.Items.Enchants
{
	public class Obliterator  : ModItem
	{	int charge = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic XXIX : Obliterator");
			
			Tooltip.SetDefault("V");
		}
		public override void SetDefaults()
		{
			item.damage = 216;  //gun damage
            item.width = 60;     //gun image width
            item.height = 28;   //gun image  height
            item.useTime = 1;  //how fast 
            item.useAnimation = 2;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0f;
            item.value = 1000000000;
            item.rare = 9;
            item.UseSound = SoundID.Item6;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Obliteration"); 
            item.shootSpeed = 33f;
			item.expert = true;
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"VMaterial", 1);
			recipe.AddIngredient(null,"Geo", 1);
			recipe.AddIngredient(null,"TerraStar", 1);
			recipe.AddIngredient(null,"ChainGrenadier", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"TheHardCore", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
          public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
				 SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			  if(modPlayer.starBurst)
			{
            item.knockBack = 1f;
			}
			else
			{
            item.knockBack = 0f;	
			}
				charge++;
					player.AddBuff(mod.BuffType("FrozenThroughTime"), 10);
			  if(charge >= 120)
				{
					charge = 0;
					item.autoReuse = false;
					return true;
				}
					return false;
          }
		  public override void UpdateInventory(Player player)
			{
				
			
				if(charge >= 1)
				{
				item.autoReuse = true;
				}
				
				
		}  
	}
}