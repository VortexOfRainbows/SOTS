using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
namespace SOTS.Items.OreItems
{
	public class PlatinumScythe : VoidItem
	{ 	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Scythe");
			Tooltip.SetDefault("Critical hits permanently curse enemies\nThis effect can stack up to 9 times, and deals 120% damage");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 19;  
            item.melee = true; 
            item.width = 40;    
            item.height = 40;  
            item.useTime = 21;
            item.useAnimation = 21;
            item.useStyle = 1;   
            item.autoReuse = true; 
			item.useTurn = true;
            item.knockBack = 1.55f;
			item.value = Item.sellPrice(0, 0, 35, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item71;
			item.crit = 11;
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if(crit)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("PlatinumCurse"), (int)(damage * 1.2f) + 1, 0, player.whoAmI, target.whoAmI, 0f);
			}
		}
		public override void GetVoid(Player player)
		{
			voidMana = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PlatinumBar, 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
