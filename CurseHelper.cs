using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Text;
using SOTS.NPCs.Boss.Curse;
using SOTS.Projectiles.Pyramid;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SOTS
{
    public static class CurseHelper
    {
		public static void DrawPlayerFoam(SpriteBatch spriteBatch, Player player)
		{
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			List<CurseFoam> foamList = modPlayer.foamParticleList1;
			List<int> slots = new List<int>();
			int startAt = 2;
			if (SOTS.Config.lowFidelityMode)
				startAt = 1;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				bool validType = proj.type == ModContent.ProjectileType<GasBlast>() || proj.type == ModContent.ProjectileType<Projectiles.Minions.CursedBlade>() || proj.type == ModContent.ProjectileType<Projectiles.Pyramid.BrachialLance>();
				if (validType && proj.active && proj.owner == player.whoAmI)
				{
					slots.Add(i);
					List<CurseFoam> list = getFoamList(player, proj);
					DrawFoam(list, startAt, spriteBatch);
				}
			}
			DrawFoam(foamList, startAt, spriteBatch);
			startAt--;
			for (int i = 0; i < slots.Count; i++)
			{
				Projectile proj = Main.projectile[slots[i]];
				List<CurseFoam> list = getFoamList(player, proj);
				DrawFoam(list, startAt, spriteBatch);
			}
			DrawFoam(foamList, startAt, spriteBatch);
			startAt--;
			if (startAt >= 0)
			{
				for (int i = 0; i < slots.Count; i++)
				{
					Projectile proj = Main.projectile[slots[i]];
					List<CurseFoam> list = getFoamList(player, proj);
					DrawFoam(list, startAt, spriteBatch);
				}
				DrawFoam(foamList, startAt, spriteBatch);
			}
		}
		public static List<CurseFoam> getFoamList(Player player, Projectile proj)
		{
			if (proj.type == ModContent.ProjectileType<GasBlast>() && proj.active && proj.owner == player.whoAmI)
			{
				GasBlast ring = proj.ModProjectile as GasBlast;
				return ring.foamParticleList1;
			}
			if (proj.type == ModContent.ProjectileType<Projectiles.Minions.CursedBlade>() && proj.active && proj.owner == player.whoAmI)
			{
				Projectiles.Minions.CursedBlade ring = proj.ModProjectile as Projectiles.Minions.CursedBlade;
				return ring.foamParticleList1;
			}
			if (proj.type == ModContent.ProjectileType<BrachialLance>() && proj.active && proj.owner == player.whoAmI)
			{
				BrachialLance ring = proj.ModProjectile as BrachialLance;
				return ring.foamParticleList1;
			}
			return null;
		}
		public static void DrawFoam(List<CurseFoam> dustList, int layer, SpriteBatch spriteBatch)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Assets/PlayerCurseFoam");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 6);
			for (int i = 0; i < dustList.Count; i++)
			{
				int shade = 255 - (int)(dustList[i].counter * 4f);
				if (shade < 0)
					shade = 0;
				Color color = new Color(shade + dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation, shade - dustList[i].dustColorVariation);
				if (layer != 1 || !SOTS.Config.lowFidelityMode)
					color = Lighting.GetColor((int)dustList[i].position.X / 16, (int)dustList[i].position.Y / 16, color);
				float reduction = shade / 255f;
				if (layer == 2)
				{
					Color first = new Color((int)(111 * reduction), (int)(80 * reduction), (int)(154 * reduction));
					Color second = new Color((int)(76 * reduction), (int)(58 * reduction), (int)(101 * reduction));
					color = Color.Lerp(first, second, 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(ColorHelpers.soulColorCounter * 2)));
				}
				Vector2 drawPos = dustList[i].position - Main.screenPosition;
				Rectangle frame = new Rectangle(0, texture.Height / 3 * layer, texture.Width, texture.Width);
				float scale = layer == 0 ? 1.5f : 2.0f;
				spriteBatch.Draw(texture, drawPos + new Vector2(0, 0), frame, color, dustList[i].rotation, drawOrigin, dustList[i].scale * scale, SpriteEffects.None, 0);
			}
		}
		public class ColoredFireParticle
		{
			public Color color;
			public Vector2 position;
			public Vector2 velocity;
			public float rotation;
			public float nextRotation;
			public float mult;
			public ColoredFireParticle()
			{
				position = Vector2.Zero;
				velocity = Vector2.Zero;
				rotation = 0;
				nextRotation = 0;
				scale = 1;
				mult = Main.rand.NextFloat(0.9f, 1.1f);
			}
			public ColoredFireParticle(Vector2 position, Vector2 velocity, float rotation, float nextRotation, float scale, Color color)
			{
				this.position = position;
				this.velocity = velocity;
				this.rotation = rotation;
				this.nextRotation = nextRotation;
				this.scale = scale;
				this.color = color;
				mult = Main.rand.NextFloat(0.9f, 1.1f);
			}
			public float counter = 0;
			public float scale;
			public bool active = true;
			public void Update()
			{
				counter++;
				float veloMult = 0.6f + 0.4f * counter / 15f;
				if (veloMult > 1)
					veloMult = 1f;
				position += velocity * veloMult;
				for (int i = 0; i < 1 + (int)(Main.rand.NextFloat(1f) * mult); i++)
				{
					velocity.Y *= 0.98f;
					velocity.X *= 0.98f;
					scale *= 0.95f;
				}
				if (counter < 31f)
					rotation += nextRotation / 30f;
				if (scale <= 0.05f)
					active = false;
			}
		}
	}
	public static class ParticleHelper
    {
        public static Texture2D WaterParticleTexture;
        public static Texture2D WaterParticleOutline;
        public static List<WaterParticle> waterParticles;
		public static int MemoryCounter = 0;
		public static void Load()
		{
			waterParticles = new List<WaterParticle>();
			WaterParticleTexture = ModContent.Request<Texture2D>("SOTS/Assets/WaterParticle", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            WaterParticleOutline = ModContent.Request<Texture2D>("SOTS/Assets/WaterParticleOutline", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        }
        public static void ReInstantiateList()
        {
            List<WaterParticle> temp = new List<WaterParticle>();
            for (int i = 0; i < waterParticles.Count; i++)
            {
                if (waterParticles[i].active && waterParticles[i] != null)
                    temp.Add(waterParticles[i]);
            }
            waterParticles = new List<WaterParticle>();
            for (int i = 0; i < temp.Count; i++)
            {
                waterParticles.Add(temp[i]);
            }
        }
        public static void Update()
		{
            for (int i = 0; i < waterParticles.Count; i++)
            {
                WaterParticle particle = waterParticles[i];
                particle.Update();
                if (!particle.active)
                {
                    waterParticles.RemoveAt(i);
                    i--;
                }
            }
            MemoryCounter++;
            if (MemoryCounter >= 900)
            {
                MemoryCounter = 0;
                ReInstantiateList();
            }
        }
		public static void DrawWaterParticles(bool outline = false)
        {
            if (waterParticles == null)
				return;
			Vector2 origin = new Vector2(16, 16);
			for(int i = 0; i < waterParticles.Count; i++)
			{
				WaterParticle particle = waterParticles[i];
				if(particle.active)
				{
					if(outline)
					{
						for(int k = 0; k < 4; k++)
                        {
                            Vector2 circular = new Vector2(3f * particle.scale * 0.5f, 0).RotatedBy(MathHelper.PiOver2 * k);
                            Main.spriteBatch.Draw(WaterParticleOutline, particle.position - Main.screenPosition + circular, null, Color.White, 0f, origin, particle.scale * 0.5f, SpriteEffects.None, 0f);
                        }
                    }
                    else
                        Main.spriteBatch.Draw(WaterParticleTexture, particle.position - Main.screenPosition, null, Color.White, 0f, origin, particle.scale * 0.5f, SpriteEffects.None, 0f);
                }
			}
		}
    }
    public class WaterParticle
    {
		public static void NewWaterParticle(Vector2 position, Vector2 velocity, float scale)
		{
			ParticleHelper.waterParticles.Add(new WaterParticle(position, velocity, scale));
		}
        public Vector2 position;
        public Vector2 velocity;
        public WaterParticle(Vector2 position, Vector2 velocity, float scale)
        {
            this.position = position;
            this.velocity = velocity;
            this.scale = scale;
        }
        public float counter = 0;
        public float scale;
        public bool active = true;
        public void Update()
        {
            counter++;
            float veloMult = 1f;
            position += velocity * veloMult;
            velocity *= 1.0f - 0.025f * scale - 0.06f;
            scale *= 0.9535f;
            scale -= 0.0175f;
            if (scale <= 0.00f)
                active = false;
        }
    }
}