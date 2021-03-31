using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items
{	[AutoloadEquip(EquipType.Shield)]
	public class BulwarkOfTheAncients : ModItem
	{	int Probe = -1;
		int Probe2 = -1;	
		int Probe3 = -1;
		int Probe4 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bulwark Of The Ancients");
			Tooltip.SetDefault("Grants immunity to knockback and fire blocks\nGrants immunity to most debuffs\nIncreases life regen by 1, void regen by 2, reduces damage taken by 7%, and increases crit chance by 4%\nSurrounds you with 4 orbital projectiles\nGrants permanent hunter and dangersense effects\nProjectiles disabled when hidden");
		}
		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
            item.width = 42;     
            item.height = 46;   
            item.value = Item.sellPrice(0, 25, 0, 0);
            item.rare = 8;
			item.accessory = true;
			item.defense = 6;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "OlympianAegis", 1);
			recipe.AddIngredient(null, "ChiseledBarrier", 1);
			recipe.AddIngredient(ItemID.AnkhShield, 1);
			recipe.AddIngredient(null, "SunlightAmulet", 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			//modPlayer.PushBack = true; //removing this effect
			
			//Grants immunity to knockback and fire blocks, Grants immunity to most debuffs
			player.noKnockback = true;
			player.buffImmune[BuffID.Burning] = true; 
            player.buffImmune[BuffID.BrokenArmor] = true; 
            player.buffImmune[BuffID.Weak] = true; 
            player.buffImmune[BuffID.Bleeding] = true; 
            player.buffImmune[BuffID.Poisoned] = true; 
            player.buffImmune[BuffID.Slow] = true; 
            player.buffImmune[BuffID.Confused] = true; 
            player.buffImmune[BuffID.Silenced] = true; 
            player.buffImmune[BuffID.Cursed] = true; 
            player.buffImmune[BuffID.Darkness] = true; 
            player.buffImmune[BuffID.Chilled] = true; 
			
			//Increases life regen by 1, void regen by 2, reduces damage taken by 7%, and increases crit chance by 4%
			voidPlayer.voidRegen += 0.2f;
			player.lifeRegen += 1;
			player.endurance += 0.07f;
			player.meleeCrit += 4;
			player.rangedCrit += 4;
			player.magicCrit += 4;
			player.thrownCrit += 4;
			
			//Grants permanent hunter and dangerSense effect
			player.detectCreature = true;
			player.dangerSense = true;
			
			//Surrounds you with 4 orbital projectiles
			if(Main.myPlayer == player.whoAmI && !hideVisual)
			{
				if (Probe == -1)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 4); 
				}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("Rainbolt") || Main.projectile[Probe].owner != player.whoAmI || Main.projectile[Probe].ai[0] != 4)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 4);
				}
				Main.projectile[Probe].timeLeft = 6;

				if (Probe2 == -1)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 5);
				}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != mod.ProjectileType("Rainbolt") || Main.projectile[Probe2].owner != player.whoAmI || Main.projectile[Probe2].ai[0] != 5)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Rainbolt"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 5);
				}
				Main.projectile[Probe2].timeLeft = 6;

				if (Probe3 == -1)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 4);
				}
				if (!Main.projectile[Probe3].active || Main.projectile[Probe3].type != mod.ProjectileType("TinyPlanetTear") || Main.projectile[Probe3].owner != player.whoAmI || Main.projectile[Probe3].ai[0] != 4)
				{
					Probe3 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 4);
				}
				Main.projectile[Probe3].timeLeft = 6;

				if (Probe4 == -1)
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 5);
				}
				if (!Main.projectile[Probe4].active || Main.projectile[Probe4].type != mod.ProjectileType("TinyPlanetTear") || Main.projectile[Probe4].owner != player.whoAmI || Main.projectile[Probe4].ai[0] != 5)
				{
					Probe4 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("TinyPlanetTear"), (int)(item.damage * (1f + (player.magicDamage - 1f) + (player.allDamage - 1f))), 0, player.whoAmI, 5);
				}
				Main.projectile[Probe4].timeLeft = 6;
			}
		}
	}
}