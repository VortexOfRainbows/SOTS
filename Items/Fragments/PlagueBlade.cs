using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Fragments
{
	public class PlagueBlade : ModItem
	{ 	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plague Blade");
			Tooltip.SetDefault("Plagues enemies on hit, making them plague other nearby enemies\nLess effective against enemies with debuff immunities");
		}
		public override void SetDefaults()
		{
            item.damage = 15;  
            item.melee = true; 
            item.width = 34;    
            item.height = 32;  
            item.useTime = 14;
            item.useAnimation = 14;
            item.useStyle = 1;   
            item.autoReuse = true; 
			item.useTurn = true;
            item.knockBack = 1.95f;
			item.value = Item.sellPrice(0, 0, 80, 0);
            item.rare = 3;
            item.UseSound = SoundID.Item1;
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if(Main.rand.Next(3) == 0)
			{
				target.AddBuff(mod.BuffType("Plague"), 360, false);
				for(int j = 0; j < 360; j += 10)
				{
					Vector2 circularLocation = new Vector2(target.width/2 - 12, 0).RotatedBy(MathHelper.ToRadians(j));
					int num1 = Dust.NewDust(new Vector2(target.Center.X + circularLocation.X - 4, target.Center.Y + circularLocation.Y - 4), 4, 4, 36);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.65f;
					circularLocation.Normalize(); 
					Main.dust[num1].velocity = circularLocation * 12.5f;
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FragmentOfNature", 6);
			recipe.AddIngredient(null, "SporeClub", 1);
			recipe.AddIngredient(ItemID.JungleSpores, 6);
			recipe.AddIngredient(ItemID.Stinger, 6);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
