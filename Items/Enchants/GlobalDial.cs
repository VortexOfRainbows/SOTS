using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;



namespace SOTS.Items.Enchants
{
	public class GlobalDial  : ModItem
	{	int stateOfFreeze = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Relic XXIII : Global Dial");
			
			Tooltip.SetDefault("Freezes all projectiles and npcs\nUses health as ammo\nDoesn't work when health is below 100\nRight click to use a offensive projectile");
		}
		public override void SetDefaults()
		{
			item.damage = 100;  //gun damage
            item.width = 46;     //gun image width
            item.height = 46;   //gun image  height
            item.useTime = 4;  //how fast 
            item.useAnimation = 8;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0f;
            item.value = 1000000000;
            item.rare = 9;
            item.UseSound = SoundID.Item6;
            item.autoReuse = true;
			item.expert = true;
			item.shoot = mod.ProjectileType("PlanetaryBeam");
			item.shootSpeed = 2;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"MargritClock", 1);
			recipe.AddIngredient(null,"SpectreWarpCore", 1);
			recipe.AddIngredient(null,"PlanetaryScepter", 1);
			recipe.AddIngredient(null,"ReanimationMaterial", 12);
			recipe.AddIngredient(null,"AntimaterialMandible", 15);
			recipe.AddIngredient(null,"CoreOfStatus", 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
				if(player.altFunctionUse == 2)
				{
				return true;
				}
				else
				{
				return false;
				}
			
			
		}
		 public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool CanUseItem(Player player)
		{
			if(player.statLife > 100)
			{
				
			player.statLife--;
			for(int i = 0; i < 200; i++)
				{
						NPC npc = Main.npc[i];
					if(!npc.friendly && npc.active)
					{
								npc.velocity.X *= 0.01f;
								npc.velocity.Y *= 0.01f;
					}
				}
				for(int j = 0; j < 1000; j++)
				{
						Projectile projectile = Main.projectile[j];
					if(projectile.hostile && projectile.active)
					{
							projectile.velocity.X *= 0.01f;
							projectile.velocity.Y *= 0.01f;
					}
				}
			}
            return base.CanUseItem(player);
		}  
	}
}