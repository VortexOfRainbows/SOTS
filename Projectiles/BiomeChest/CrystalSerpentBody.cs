using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs.MinionBuffs;

namespace SOTS.Projectiles.BiomeChest
{
	public class CrystalSerpentBody : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}
		public sealed override void SetDefaults()
        {
            Projectile.width = 50;
			Projectile.height = 50;
			Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
			Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.friendly = false;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
            Projectile.hide = true;
            Projectile.netImportant = true;
            Projectile.minionSlots = 1f;
		}
        public bool runOnce = true;
        public sealed override void AI()
        {
            Player player = Main.player[Projectile.owner];
            int ownedCounter = 0;
            for (int i = 0; i < Main.projectile.Length; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<CrystalSerpentHead>() && proj.whoAmI != Projectile.whoAmI)
                {
                    ownedCounter++;
                }
            }
            if (ownedCounter != 1)
            {
                if (player.whoAmI == Main.myPlayer)
                {
                    player.SpawnMinionOnCursor(Projectile.GetSource_FromThis(), player.whoAmI, ModContent.ProjectileType<CrystalSerpentHead>(), Projectile.originalDamage, Projectile.knockBack);
                }
            }
            Projectile.Center = player.Center;
            if (!player.active && ownedCounter != 1)
            {
                Projectile.active = false;
            }
            if (player.HasBuff(ModContent.BuffType<StarlightSerpent>()))
            {
                Projectile.timeLeft = 2;
            }
            else if(Main.myPlayer == Projectile.owner)
                Projectile.Kill();
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}