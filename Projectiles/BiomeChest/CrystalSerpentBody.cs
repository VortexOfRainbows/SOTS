using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Buffs.MinionBuffs;
using SOTS.Items.ChestItems;

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
            if (runOnce)
            {
                runOnce = false;
                return;
            }
            int type = ModContent.ProjectileType<CrystalSerpentHead>();
            int ownedCount = player.ownedProjectileCounts[type];
            if (ownedCount < 1 && player.whoAmI == Main.myPlayer)
            {
                player.SpawnMinionOnCursor(Projectile.GetSource_FromThis(), player.whoAmI, type, Projectile.originalDamage, Projectile.knockBack);
                player.ownedProjectileCounts[type] = 1;
            }
            Projectile.Center = player.Center;
            if ((player.HasBuff(ModContent.BuffType<StarlightSerpent>()) || Main.myPlayer != Projectile.owner) && player.active)
            {
                Projectile.timeLeft = 2;
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}