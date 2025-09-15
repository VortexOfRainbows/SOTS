using SOTS.NPCs.Boss.Glowmoth;
using Terraria.Achievements;
using Terraria.ModLoader;

namespace SOTS.Achievements;
public abstract class SOTSAchievement : ModAchievement
{
    public override string TextureName => "SOTS/Achievements/AllAchievements";
    public override int Index => 0;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
    }
    //public override IEnumerable<Position> GetModdedConstraints()
    //{
    //    yield return new After(ModContent.GetInstance<ManyExampleWormsKilled>());
    //}
    public override void OnCompleted(Achievement achievement)
    {
        // Make some fireworks
        // int fireworkProjectile = ProjectileID.RocketFireworksBoxRed + Main.rand.Next(4);
        // Projectile.NewProjectile(Main.LocalPlayer.GetSource_FromThis(), Main.LocalPlayer.Top, new Vector2(Main.rand.NextFloat(-2, 2), -Main.rand.NextFloat(2f, 4f)).RotatedByRandom(0.3f), fireworkProjectile, 0, 0, Main.myPlayer);
    }
}
public class GlowmothKill : SOTSAchievement
{
    public override int Index => 0;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        AddNPCKilledCondition(ModContent.NPCType<Glowmoth>());
    }
}