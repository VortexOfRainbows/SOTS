using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.IceStuff
{
	[AutoloadEquip(EquipType.Head)]
	
	public class FrostArtifactHelmet : ModItem
	{	
		int Probe = -1;
		public override void SetDefaults()
		{

			item.width = 20;
			item.height = 24;

            item.value = Item.sellPrice(0, 6, 25, 0);
			item.rare = 7;
			item.defense = 11;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Artifact Helmet");
			Tooltip.SetDefault("15% increased melee and ranged damage\nA Frost Storm surrounds you, frostburning nearby enemies");
		}
		
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("FrostArtifactChestplate") && legs.type == mod.ItemType("FrostArtifactTrousers");
        }
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawOutlines = true;
		}	
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Nearby enemies and projectiles will have their velocities slowed";
			float minDist = 160;
			float dX = 0f;
			float dY = 0f;
			float distanceEnemy = 0;
			
			for(int j = 0; j < Main.npc.Length - 1; j++)
			{
				NPC target = Main.npc[j];
				if(!target.friendly && target.dontTakeDamage == false && target.active)
					{
						dX = target.Center.X - player.Center.X;
						dY = target.Center.Y - player.Center.Y;
						distanceEnemy = (float) Math.Sqrt((double)(dX * dX + dY * dY));
						if(distanceEnemy < minDist)
						{
							target.velocity.X *= 0.98f;
							target.velocity.Y *= 0.98f;
						}
					}	
			}
			
			for(int j = 0; j < Main.projectile.Length - 1; j++)
			{
				Projectile target = Main.projectile[j];
				if(!target.friendly && target.damage > 0 && target.active && target.hostile)
					{
						dX = target.Center.X - player.Center.X;
						dY = target.Center.Y - player.Center.Y;
						distanceEnemy = (float) Math.Sqrt((double)(dX * dX + dY * dY));
						if(distanceEnemy < minDist)
						{
							target.velocity.X *= 0.98f;
							target.velocity.Y *= 0.98f;
											
							if(Math.Abs(target.velocity.X) < 0.01f)
							target.Kill();
							
							if(Math.Abs(target.velocity.Y) < 0.01f)
							target.Kill(); 
						}
					}	
			}
		}
		public override void UpdateEquip(Player player)
		{
			for(int i = 0; i < 5; i++)
			{
				int num1 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 67);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 7.5f;
			}
				
				float minDist = 240;
				float dX = 0f;
				float dY = 0f;
				float distanceEnemy = 0;
				
					for(int j = 0; j < Main.npc.Length - 1; j++)
					{
						NPC target = Main.npc[j];
						if(!target.friendly && target.dontTakeDamage == false && target.active)
							{
								dX = target.Center.X - player.Center.X;
								dY = target.Center.Y - player.Center.Y;
								distanceEnemy = (float) Math.Sqrt((double)(dX * dX + dY * dY));
								if(distanceEnemy < minDist)
								{
									target.AddBuff(BuffID.Frostburn, 240, false);
								}
							}	
					}
				
			player.meleeDamage += 0.15f;
			player.rangedDamage += 0.15f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(684, 1);
			recipe.AddIngredient(null, "AbsoluteBar", 16);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}

	}
}