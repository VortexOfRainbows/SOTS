using Microsoft.Xna.Framework;
using SOTS.Buffs.Debuffs;
using SOTS.Dusts;
using SOTS.Helpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Common.ModPlayers
{
    public class AVPlayer : ModPlayer
    {
        public bool InVillage => Player.SOTSPlayer().AbandonedVillageBiome;
        public bool InUnderground => Player.ZoneRockLayerHeight || Player.ZoneDirtLayerHeight;
        private bool Moving => Player.velocity.X > 1 || Player.velocity.Y > 1 || Player.velocity.X < -1 || Player.velocity.Y < -1; //Basically, am I moving a sufficient amount?
        private static float VillageTransitionTime = 90f;
        private float VillageCounter = 0;
        public override void PostUpdateMiscEffects()
        {
            if (Player.HasBuff<LungRot>())
            {
                if (Moving)
                {
                    Player.lifeRegen = (int)(Player.lifeRegen * LungRot.LifeRegenWhileMoving);
                    Player.VoidPlayer().voidRegenSpeed += 0.4f;
                }
            }
            if (InVillage)
            {
                VillageCounter++;
                if (VillageCounter >= VillageTransitionTime)
                {
                    VillageCounter = VillageTransitionTime;
                    Player.AddBuff(ModContent.BuffType<LungRot>(), 6, true);
                }
                if (Main.myPlayer == Player.whoAmI)
                    SpawnAmbientParticles();
            }
            else if (VillageCounter > 0)
                VillageCounter--;
            else
                VillageCounter = 0;
        }
        public override void NaturalLifeRegen(ref float regen)
        {
            if (Player.HasBuff<LungRot>())
            {
                if (Moving)
                {
                    regen *= LungRot.LifeRegenWhileMoving;
                }
            }
        }
        private void SpawnAmbientParticles()
        {
            float percent = VillageCounter / VillageTransitionTime;
            float target = percent * 0.5f;
            if (Main.GraveyardVisualIntensity < target)
                Main.GraveyardVisualIntensity = target;
            //This is where dust spawning will go
            float w = Main.screenWidth;
            float h = Main.screenHeight;
            Vector2 center = Main.screenPosition + new Vector2(w * 0.5f, h * 0.5f);
            float targetParticleCount = 10 / Main.GameZoomTarget * (SOTS.Config.lowFidelityMode ? 0.5f : 1f) * percent;
            Vector2 windDirection = new Vector2(Main.windSpeedCurrent * (InUnderground ? 0.25f : 1f), 0) * 2.4f;
            for(int a = 0; a < targetParticleCount; a++)
            {
                Vector2 position = center + new Vector2(w * 0.5f * Main.rand.NextFloat(-1, 1f), h * 0.5f * Main.rand.NextFloat(-1, 1f)) / Main.GameZoomTarget;
                int i = (int)position.X / 16;
                int j = (int)position.Y / 16;
                Vector2? dustPosition = SOTSTile.GetWorldPositionOnTile(i, j, Main.rand.Next(4), position.X - i * 16, position.Y - j * 16, true);
                if (dustPosition != null)
                {
                    Vector2 awayFromBlockSurface = (dustPosition.Value - position).SNormalize() * Main.rand.NextFloat(1f);
                    PixelDust.Spawn(dustPosition.Value, 0, 0, windDirection + Main.rand.NextVector2Circular(0.5f, 0.5f) + awayFromBlockSurface, ColorHelper.AVDustColor * Main.rand.NextFloat(0.65f, 0.85f), 2).scale = 1;

                }
            }
            //Gore.NewGoreDirect(new EntitySource_Misc("SOTS:AVAmbience"), Player.Center, Vector2.Zero, Main.rand.Next(GoreID.AmbientFloorCloud1, GoreID.AmbientAirborneCloud3 + 1), 1);
        }
    }
}