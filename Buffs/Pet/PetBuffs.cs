using SOTS.Items.Master;
using SOTS.Projectiles;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Buffs.Pet
{
    public class BeepBoop : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            bool unused = false;
            player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<Boopy>());
        }
    }
    public class PurpleBalloon : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
			Main.lightPet[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            modPlayer.PurpleBalloon = true;
            player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = true;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<LuckyPurpleBalloon>()] > 0)
				petProjectileNotSpawned = false;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
				Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, ModContent.ProjectileType<LuckyPurpleBalloon>(), 0, 0f, player.whoAmI, 0f, 0f);
        }
    }
}