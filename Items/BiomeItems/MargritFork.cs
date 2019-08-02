using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	public class MargritFork : ModItem
	{ 	int damageBuff = 0;
		int swingActive = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Margrit Fork");
			Tooltip.SetDefault("Splits enemies in half");
		}
		public override void SetDefaults()
		{
            item.damage = 30;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 52;     //gun image width
            item.height = 52;   //gun image  height
            item.useTime = 15;  //how fast 
            item.useAnimation = 15;
            item.useStyle = 1;    

            item.knockBack = 1.55f;
            item.value = 110000;
            item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;

			
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if(target.boss == false)
			{	
				if(target.width > 12 && target.height > 12 && target.scale > 0.25f)
				{
					NPC.NewNPC((int)target.Center.X + target.width, (int)target.Center.Y, target.type);	
					NPC.NewNPC((int)target.Center.X - target.width, (int)target.Center.Y, target.type);	
					for(int i = 0; i < 200; i++)
					{
						NPC clone1 = Main.npc[i];
						if(clone1.Center.X == (int)target.Center.X + target.width && clone1.type == target.type || clone1.Center.X == (int)target.Center.X - target.width && clone1.type == target.type)
						{
							clone1.width = (int)(target.width * 0.75f);
							clone1.height = (int)(target.height * 0.75f);
							//clone2.width = (int)(target.width * 0.5f);
							//clone2.height = (int)(target.height * 0.5f);
							clone1.scale = target.scale * 0.75f;
							//clone2.scale = target.scale * 0.5f;
							clone1.lifeMax = (int)(target.lifeMax * 0.5f);
							clone1.life = (int)(target.life * 0.5f);
							//clone2.lifeMax = (int)(target.lifeMax * 0.5f);
						}
					}
					target.active = false;
				}
			}
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritCore", 1);
			recipe.AddIngredient(null, "ObsidianScale", 20);
			recipe.AddIngredient(3081, 20);
			recipe.AddIngredient(3086, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
