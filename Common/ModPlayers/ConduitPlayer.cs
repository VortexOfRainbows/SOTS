using Microsoft.Xna.Framework;
using SOTS.Buffs.ConduitBoosts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SOTS.Common.ModPlayers
{
    public class ConduitPlayer : ModPlayer
    {
        public override void SaveData(TagCompound tag)
        {
            tag["Power1"] = NaturePower;
            tag["Power2"] = EarthPower;
            tag["Power3"] = PermafrostPower;
            tag["Power4"] = OtherworldPower;
            tag["Power5"] = TidePower;
            tag["Power6"] = EvilPower;
            tag["Power7"] = InfernoPower;
            tag["Power8"] = ChaosPower;
        }
        public override void LoadData(TagCompound tag)
        {
            NaturePower = tag.GetInt("Power1");
            EarthPower = tag.GetInt("Power2");
            PermafrostPower = tag.GetInt("Power3");
            OtherworldPower = tag.GetInt("Power4");
            TidePower = tag.GetInt("Power5");
            EvilPower = tag.GetInt("Power6");
            InfernoPower = tag.GetInt("Power7");
            ChaosPower = tag.GetInt("Power8");
        }
        public const float ChargeTime = 90f;
        public static ConduitPlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<ConduitPlayer>();
        }
        public bool NatureBoosted => NaturePower >= ChargeTime;
        public bool EarthBoosted => EarthPower >= ChargeTime;
        public bool PermafrostBoosted => PermafrostPower >= ChargeTime;
        public bool OtherworldBoosted => OtherworldPower >= ChargeTime;
        public bool TideBoosted => TidePower >= ChargeTime;
        public bool EvilBoosted => EvilPower >= ChargeTime;
        public bool InfernoBoosted => InfernoPower >= ChargeTime;
        public bool ChaosBoosted => ChaosPower >= ChargeTime;
        public int NaturePower = 0;
        public int EarthPower = 0;
        public int PermafrostPower = 0;
        public int OtherworldPower = 0;
        public int TidePower = 0;
        public int EvilPower = 0;
        public int InfernoPower = 0;
        public int ChaosPower = 0;
        public override void ResetEffects()
        {
            IteratePower(ref NaturePower, ColorHelpers.NatureColor);
            IteratePower(ref EarthPower, ColorHelpers.EarthColor);
            IteratePower(ref PermafrostPower, ColorHelpers.PermafrostColor);
            IteratePower(ref OtherworldPower, ColorHelpers.PurpleOtherworldColor);
            IteratePower(ref TidePower, ColorHelpers.TideColor);
            IteratePower(ref EvilPower, ColorHelpers.RedEvilColor);
            IteratePower(ref InfernoPower, ColorHelpers.Inferno1);
            IteratePower(ref ChaosPower, ColorHelpers.ChaosPink);

            if(NatureBoosted || EarthBoosted || PermafrostBoosted || OtherworldBoosted || TideBoosted || EvilBoosted || InfernoBoosted || ChaosBoosted)
            {
                Player.AddBuff(ModContent.BuffType<Attuned>(), 300);
            }
        }
        private void IteratePower(ref int Power, Color color)
        {
            if(Power > 0)
            {
                if (Power < ChargeTime)
                {
                    Power++;
                    if(Power >= ChargeTime)
                    {
                        SOTSUtils.PlaySound(SoundID.Item30, Player.Center, 0.75f, 0.25f);
                        for (int i = 0; i < 24; i++)
                        {
                            Dust dust = Dust.NewDustDirect(Player.Center, 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>(), 0, 0, 0, color, 1.1f);
                            dust.scale *= 1.4f;
                            dust.velocity *= 0.6f;
                            dust.velocity += new Vector2(0, 9f).RotatedBy(MathHelper.TwoPi * i / 24f) / dust.scale;
                            dust.fadeIn = 0.1f;
                            dust.noGravity = true;
                        }
                    }
                }
                else
                {
                    Power = (int)ChargeTime;
                }
            }
        }
    }
}