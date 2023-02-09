using SOTS.Projectiles.Minions;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.MinionBuffs
{
	public class NatureSpiritAid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<NatureSpirit>()] > 0)
			{
				player.buffTime[buffIndex] = 6;
			}
		}
	}
	public class EarthenSpiritAid : ModBuff
    {
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex) 
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<EarthenSpirit>()] > 0) 
			{
				player.buffTime[buffIndex] = 6;
			}
		}
	}
	public class PermafrostSpiritAid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<PermafrostSpirit>()] > 0)
			{
				player.buffTime[buffIndex] = 6;
			}
		}
	}
	public class OtherworldlySpiritAid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<OtherworldlySpirit>()] > 0)
			{
				player.buffTime[buffIndex] = 6;
			}
		}
	}
	public class TidalSpiritAid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<TidalSpirit>()] > 0)
			{
				player.buffTime[buffIndex] = 6;
			}
		}
	}
	public class EvilSpiritAid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<EvilSpirit>()] > 0)
			{
				player.buffTime[buffIndex] = 6;
			}
		}
	}
	public class InfernoSpiritAid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<InfernoSpirit>()] > 0)
			{
				player.buffTime[buffIndex] = 6;
			}
		}
	}
	public class ChaosSpiritAid : ModBuff
	{
		public override void SetStaticDefaults()
		{
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<ChaosSpirit>()] > 0)
			{
				player.buffTime[buffIndex] = 6;
			}
		}
	}
}