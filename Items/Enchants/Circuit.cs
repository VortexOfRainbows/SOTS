using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Enchants
{
	public class Circuit : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic XIV : Circuit");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 6));
			
			Tooltip.SetDefault("S");
		}
		public override void SetDefaults()
		{
			item.damage = 50;  //gun damage
            item.thrown = true;   //its a gun so set this to true
            item.width = 32;     //gun image width
            item.height = 32;   //gun image  height
            item.useTime = 1;  //how fast 
            item.useAnimation = 2;
            item.useStyle = 1;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 1000000000;
            item.rare = 9;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Circuit"); 
            item.shootSpeed = 0.1f;
			item.noUseGraphic = true;
			item.expert = true;
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"SMaterial", 1);
			recipe.AddIngredient(null,"SteelEnergySword", 1);
			recipe.AddIngredient(null,"MicroKnife", 1);
			//recipe.AddIngredient(null,"SolarKunai", 1);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"TheHardCore", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
          public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
					player.AddBuff(mod.BuffType("Needle"), 300);
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");		
                if(modPlayer.needle == true)
				{
			  return false;
				}
				else
				{
			modPlayer.needle = true;
					
			  return true;
				}
					
          }  
	}
}