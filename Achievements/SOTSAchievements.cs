using SOTS.Items;
using SOTS.Items.Conduit;
using SOTS.Items.Earth;
using SOTS.Items.Gems;
using SOTS.Items.Invidia;
using SOTS.Items.Pyramid;
using SOTS.Items.Secrets;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Boss.Advisor;
using SOTS.NPCs.Boss.Curse;
using SOTS.NPCs.Boss.Glowmoth;
using SOTS.NPCs.Boss.Lux;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using Steamworks;
using Terraria.Achievements;
using Terraria.GameContent.Achievements;
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
        Achievement.SetCategory(AchievementCategory.Slayer);
        AddNPCKilledCondition(ModContent.NPCType<Glowmoth>());
    }
}
public class PinkyKill : SOTSAchievement
{
    public override int Index => 1;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Slayer);
        AddNPCKilledCondition(ModContent.NPCType<PutridPinkyPhase2>());
    }
}
public class CurseKill : SOTSAchievement
{
    public override int Index => 2;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Slayer);
        AddNPCKilledCondition(ModContent.NPCType<PharaohsCurse>());
    }
}
public class AdvisorKill : SOTSAchievement
{
    public override int Index => 3;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Slayer);
        AddNPCKilledCondition(ModContent.NPCType<TheAdvisorHead>());
    }
}
public class PolarisKill : SOTSAchievement
{
    public override int Index => 4;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Slayer);
        AddNPCKilledCondition(ModContent.NPCType<NewPolaris>());
    }
}
public class LuxKill : SOTSAchievement
{
    public override int Index => 5;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Slayer);
        AddNPCKilledCondition(ModContent.NPCType<Lux>());
    }
}
public class SubspaceKill : SOTSAchievement
{
    public override int Index => 6;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Slayer);
        AddNPCKilledCondition(ModContent.NPCType<SubspaceSerpentHead>());
    }
}
public class ChallengerRings : SOTSAchievement
{
    public override int Index => 7;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Collector);
        AddItemCraftCondition(ModContent.ItemType<ChallengerRing>());
    }
}
public class ArchaeologistTalk : SOTSAchievement
{
    public CustomFlagCondition ArchaeologistTalkedToCondition { get; private set; }
    public override int Index => 8;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Explorer);
        ArchaeologistTalkedToCondition = AddCondition();
    }
}
public class TidalConstructSuicide : SOTSAchievement
{
    public CustomFlagCondition TidalConstructSuicideCondition { get; private set; }
    public override int Index => 9;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        TidalConstructSuicideCondition = AddCondition();
    }
}
public class Sirius : SOTSAchievement
{
    public override int Index => 10;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Collector);
        AddItemPickupCondition(ModContent.ItemType<PhotonGeyser>());
    }
}
public class RefractingCrystalCollect : SOTSAchievement
{
    public override int Index => 11;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Collector);
        AddItemPickupCondition(ModContent.ItemType<RefractingCrystal>());
    }
}
public class VoidDeathHappening : SOTSAchievement
{
    public CustomFlagCondition VoidDeathCondition { get; private set; } //Die while void shock or recovery
    public override int Index => 12;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Explorer);
        VoidDeathCondition = AddCondition();
    }
}
public class IntoThePyramid : SOTSAchievement
{
    public CustomFlagCondition EnterPyramidCondition { get; private set; } //Pharaoh's Curse debuff after boss2
    public override int Index => 13;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Explorer);
        EnterPyramidCondition = AddCondition();
    }
}
public class VivaleBaguette : SOTSAchievement
{
    public CustomFlagCondition LongBaguetteCondition { get; private set; } //Baguette length is greater than or equal to 20
    public override int Index => 14;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        LongBaguetteCondition = AddCondition();
    }
}
public class ColossusObtain : SOTSAchievement
{
    public override int Index => 15;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Collector);
        AddItemPickupCondition(ModContent.ItemType<Colossus>());
    }
}
public class OneHundredPercentSuperChungus : SOTSAchievement
{
    public override int Index => 16;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Collector);
        AddManyItemPickupCondition([ModContent.ItemType<BlinkBlade>(), ModContent.ItemType<MrEepy>(), ModContent.ItemType<FlowerCrown>()]);
    }
}
public class FourthDimension : SOTSAchievement
{
    public CustomFlagCondition FourTesseractsAtOnce { get; private set; }
    public override int Index => 17;
    public override void SetStaticDefaults()
    {
        Achievement.SetCategory(AchievementCategory.Challenger);
        FourTesseractsAtOnce = AddCondition();
    }
}