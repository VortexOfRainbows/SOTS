using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{	[AutoloadEquip(EquipType.Shield)]
	public class ShardGuard : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shard Guard");
			Tooltip.SetDefault("Puts a shell around the owner when below 50% life that reduces damage by 25%\nSurrounds you with a blizzard of artifact probes\nGetting hit surrounds you with ice shards\nIncreases max life by 10");
		}
		public override void SetDefaults()
		{
			item.damage = 36;
			item.summon = true;
			item.maxStack = 1;
            item.width = 38;     
            item.height = 34;   
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = ItemRarityID.Lime;
			item.defense = 1;
			item.accessory = true;
		}
		int[] Probes = { -1, -1, -1, -1, -1, -1, -1, -1 };
		public void ProbesGen(Player player)
		{
			int type = mod.ProjectileType("BlizzardProbe"); 
			if (player.whoAmI == Main.myPlayer)
			{
				for (int i = 0; i < Probes.Length; i++)
				{
					if (Probes[i] == -1)
					{
						Probes[i] = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, (int)(item.damage * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, i, i * 15);
					}
					if (!Main.projectile[Probes[i]].active || Main.projectile[Probes[i]].type != type || Main.projectile[Probes[i]].ai[0] != i)
					{
						Probes[i] = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, type, (int)(item.damage * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, i, i * 15);
					}
					Main.projectile[Probes[i]].timeLeft = 6;
				}
			}
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int rand = Main.rand.Next(10);
			if (rand >= 0 && rand <= 1) //0,1,2 20%
				modPlayer.shardOnHit += 3;
			if (rand >= 2 && rand <= 4) //3,4 30%
				modPlayer.shardOnHit += 4;
			if (rand >= 5) //5,6,7,8,9 50%
				modPlayer.shardOnHit += 5;

			modPlayer.bonusShardDamage += (int)(item.damage * (1 + (player.minionDamage - 1f) + (player.allDamage - 1f)));
			player.statLifeMax2 += 10;

			if ((double)player.statLife <= (double)player.statLifeMax * 0.5)
			{
				player.AddBuff(BuffID.IceBarrier, 600, true);
			}

			ProbesGen(player);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DissolvingAurora", 1);
			recipe.AddIngredient(ItemID.FrozenTurtleShell, 1);
			recipe.AddIngredient(null, "PermafrostMedallion", 1);
			recipe.AddIngredient(null, "ShatterHeartShield", 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}