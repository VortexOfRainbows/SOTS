using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class MargritBlaster : ModItem
	{	int ammo = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Blaster");
			Tooltip.SetDefault("Right click to load ammo");
		}
		public override void SetDefaults()
		{
            item.damage = 27;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 36;     //gun image width
            item.height = 24;   //gun image  height
            item.useTime = 3;  //how fast 
            item.useAnimation = 3;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 110000;
            item.rare = 6;
            item.UseSound = SoundID.Item11;
            item.autoReuse = false;
            item.shoot = 10; 
            item.shootSpeed = 12;
			item.useAmmo = AmmoID.Bullet;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
				if(player.altFunctionUse == 2)
				{
				if(ammo < (int)(6 * modPlayer.reloadBoost))
				{
				ammo = (int)(6 * modPlayer.reloadBoost);
				}
				player.HealEffect(ammo);
				return false;
				}
				else
				{
				return true;
				}
			
			
		}
		 public override bool AltFunctionUse(Player player)
        {
            return true;
        }
		public override bool ConsumeAmmo(Player player)
		{
			if(player.altFunctionUse == 2 || item.useAnimation < 2)
			{
			return false;
			}
			return true;
		}
 
        public override bool CanUseItem(Player player)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
            if (player.altFunctionUse == 2)     //2 is right click
            {
				
				
                item.useTime = 60;
                item.useAnimation = 60;
				
 
 
 
            }
            else
            {
				if(ammo != 0)
				{
                item.useTime = 3;
                item.useAnimation = 3;
				ammo--;
				player.HealEffect(ammo);
				
				}
				else
				{
                item.useTime = 1;
                item.useAnimation = 1;
				player.HealEffect(ammo);
				}
			
			}
            return base.CanUseItem(player);
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddIngredient(null, "ObsidianScale", 12);
			recipe.AddIngredient(3081, 12);
			recipe.AddIngredient(3086, 36);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
