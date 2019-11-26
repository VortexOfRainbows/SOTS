using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;


namespace SOTS.Items.Celestial
{
	public class PlasmaCutterButOnAChain : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Cutter but on a Chain");
			Tooltip.SetDefault("'This is utmost wonderful idea'");
		}
		public override void SetDefaults()
		{
            item.damage = 110;
            item.melee = true;  
            item.width = 28;
            item.height = 20;  
            item.useTime = 25; 
            item.useAnimation = 25;
            item.useStyle = 5;    
            item.knockBack = 6f;
            item.value = Item.sellPrice(0, 8, 50, 0);
            item.rare = 8;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("PlasmaCutter"); 
            item.shootSpeed = 0f;
			item.channel = true;
			item.axe = 200;
            item.noUseGraphic = true; 
            item.noMelee = true;
			Item.staff[item.type] = true; 
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			
				bool summon = true;
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if(proj.active && proj.type == item.shoot && Main.player[proj.owner] == player)
					{
						summon = false;
					}
				}
			if(player.altFunctionUse != 2)
			{
				item.UseSound = SoundID.Item22;
				if(summon)
				{
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, (float)Math.Atan2(speedY, speedX) + 90f, 0);
				}
			}
              return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 15);
			recipe.AddIngredient(3098, 1); //butchers chainsaw
			recipe.AddIngredient(ItemID.Chain, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SanguiteBar", 60);
			recipe.AddIngredient(ItemID.Chain, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}