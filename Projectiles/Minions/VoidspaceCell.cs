using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Projectiles.Celestial;
using System.Collections.Generic;
using System;
using SOTS.Buffs;
using SOTS.Helpers;

namespace SOTS.Projectiles.Minions
{
    public class VoidspaceCell : ModProjectile
    {	
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 40; 
            Projectile.hostile = false; 
            Projectile.friendly = false; 
            Projectile.ignoreWater = true;  
			Projectile.timeLeft = Projectile.SentryLifeTime;
			Projectile.penetrate = -1;
            Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.sentry = true;
			Projectile.DamageType = DamageClass.Summon;
		}
		private bool IsVoidSpaceLantern => Type != ModContent.ProjectileType<AncientSteelLantern>();
        protected virtual float Radius => 320f + SOTSPlayer.ApplyDamageClassModWithGeneric(Main.player[Projectile.owner], DamageClass.Summon, 80);
		protected virtual Color FlameColor => new Color(75, 255, 30, 0);
        protected List<FireParticle> particleList = new List<FireParticle>();
        protected float counter = 0;
        protected bool chains = true;
        protected Vector2 StartingPosition;
        public void CatalogueParticlePositions()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
                particleList[i].AlternateUpdate();
				if (!particleList[i].active)
				{
					FireParticle temp = particleList[particleList.Count - 1];
					particleList[particleList.Count - 1] = particleList[i];
					particleList[i] = temp;
					particleList.RemoveAt(particleList.Count - 1);
				}
			}
		}
		public override bool PreAI()
		{
			Player player = Main.player[Projectile.owner];
            if (StartingPosition == Vector2.Zero)
            {
				Projectile.Center = new Vector2((int)Projectile.Center.X / 16 + 0.5f, (int)Projectile.Center.Y / 16 + 0.5f) * 16;
                StartingPosition = Projectile.Center;
                player.UpdateMaxTurrets();
            }
			CatalogueParticlePositions();
			Vector2 rotational = new Vector2(0, -3f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
			rotational.X *= Main.rand.NextFloat(0.25f);
			rotational.Y *= Main.rand.NextFloat(0.25f, .75f);
			particleList.Add(new FireParticle(new Vector2(0, 4) - rotational * 0.5f * Main.rand.NextFloat(), rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.9f, 1.1f)));
			int sides = 12;
			float rad = MathHelper.TwoPi / sides;
			for(int i = 0; i < sides; i++)
			{
				Vector2 circular = new Vector2(Radius, 0).RotatedBy(i * rad + Main.rand.NextFloat(-rad, rad) * 0.5f);
                int i2 = (int)(circular.X + Projectile.Center.X) / 16;
                int j2 = (int)(circular.Y + Projectile.Center.Y) / 16;
                bool disable = false;
                if (!WorldGen.InWorld(i2, j2, 20) || Main.tile[i2, j2].HasTile && Main.tileSolidTop[Main.tile[i2, j2].TileType] == false && Main.tileSolid[Main.tile[i2, j2].TileType] == true)
                    disable = true;
                if (!disable)
                {
                    for (int j = 0; j < Main.maxProjectiles; j++)
                    {
                        Projectile other = Main.projectile[j];
                        if (other.active && other.ModProjectile is VoidspaceCell cell && other.owner == Projectile.owner && j != Projectile.whoAmI)
                        {
                            Vector2 distanceBetween = Projectile.Center + circular - other.Center;
                            if (distanceBetween.LengthSquared() < cell.Radius * cell.Radius)
                            {
                                disable = true;
                                break;
                            }
                        }
                    }
                }
                if (!disable)
				{
                    rotational = -circular.SNormalize() * Main.rand.NextFloat();
                    particleList.Add(new FireParticle(circular, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.775f, 0.825f)));
                }
            }
			return base.PreAI();
		}
		public override void PostDraw(Color lightColor)
        {
            if (Main.LocalPlayer.Distance(Projectile.Center) < Radius + 4)
                Main.SceneMetrics.HasCampfire = true;
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
                Color color = Projectile.GetAlpha(FlameColor) * (0.1f + 0.4f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-.5f, .5f);
					float y = Main.rand.NextFloat(-.5f, .5f);
					Main.spriteBatch.Draw(texture, Projectile.Center + drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.15f, SpriteEffects.None, 0f);
				}
			}
			texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(0, texture.Height - 10) - Main.screenPosition, new Rectangle(0, texture.Height - 10, texture.Width, 10), lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            texture = ModContent.Request<Texture2D>("SOTS/Assets/FlailBloom").Value;
            drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.EntitySpriteDraw(texture, Projectile.Center + new Vector2(0, 2) - Main.screenPosition, null, FlameColor * 0.75f, Projectile.rotation, drawOrigin, Projectile.scale * 1.25f, SpriteEffects.None, 0f);
        }
        public void DrawChain(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D texture = !IsVoidSpaceLantern ? ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/AncientSteelChain").Value : ModContent.Request<Texture2D>("SOTS/Projectiles/Minions/VoidspaceAuraChain").Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Color color = Color.White;
			float widthMult = !IsVoidSpaceLantern ? 1f : 0.9f;
			float height = 16;
			float timer = SOTSWorld.GlobalCounter;
			int maxLength = IsVoidSpaceLantern ? 20 : 12;
			for (int j2 = 1; j2 < maxLength; j2++)
			{
				Tile tile2 = Framing.GetTileSafely(i, j - j2);
				if ((tile2.HasTile && Main.tileSolid[tile2.TileType] && !Main.tileSolidTop[tile2.TileType]) || !WorldGen.InWorld(i, j - j2, 27))
				{
					maxLength = j2;
					break;
				}
			}
			maxLength++;
			Vector2 previous = Projectile.Center;
			for (int z = 0; z < maxLength; z++)
			{
				float dynamicMult = MathF.Sin(MathHelper.Pi * (z + 1) / maxLength);
				Vector2 dynamicAddition = new Vector2(6f / maxLength * z * 0.4f + 0.5f, 0).RotatedBy(MathHelper.ToRadians(z * 24 + timer)) * dynamicMult;
				Vector2 pos = Projectile.Center;
				pos.Y -= z * 16;
				pos += dynamicAddition;
				if (z != 0)
                {
                    Vector2 rotateTo = pos - previous;
                    float lengthTo = rotateTo.Length();
                    float stretch = lengthTo / height * 1.00275f;
                    if (z == 0)
                        stretch = 1f;
                    float alphaScale = (32f - z * 1.575f) / 20f;
					Vector2 scaleVector2 = new Vector2(widthMult, stretch);
					Main.spriteBatch.Draw(texture, previous - Main.screenPosition, null, Lighting.GetColor((int)previous.X / 16, (int)previous.Y / 16, color) * alphaScale, rotateTo.ToRotation() + MathHelper.PiOver2, origin, scaleVector2, SpriteEffects.None, 0f);
				}
				previous = pos;
			}
		}
		public override bool PreDraw(ref Color lightColor)
        {
            if (chains)
                DrawChain((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, Main.spriteBatch);
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
        }
        public override void AI()
		{
			Player player = Main.player[Projectile.owner];
            Lighting.AddLight(Projectile.Center, Color.Lerp(Color.White, FlameColor, 0.45f).ToVector3() * Radius / 200f);
			if(!Projectile.Center.Equals(StartingPosition))
				chains = false;
			Projectile.ai[0]--;
			if (Projectile.ai[0] <= 0)
            {
                if (Main.myPlayer == Projectile.owner)
                {
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC target = Main.npc[i];
						Vector2 toNPC = target.Center - Projectile.Center;
						if (toNPC.Length() < Radius + 4f && target.CanBeChasedBy(Projectile))
						{
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center, toNPC, ModContent.ProjectileType<VoidspaceExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner, i, Type);
						}
					}
				}
				Projectile.ai[0] = 30;
			}
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player target = Main.player[i];
				if(target.Distance(Projectile.Center) < Radius + 4f && target.active)
				{
					if(IsVoidSpaceLantern)
						target.AddBuff(ModContent.BuffType<AuraBoost>(), 6, false);
                }
            }
		}
	}
	public class AncientSteelLantern : VoidspaceCell
	{
        public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.width = 28;
			Projectile.height = 38;
        }
		protected override float Radius => 96f + SOTSPlayer.ApplyDamageClassModWithGeneric(Main.player[Projectile.owner], DamageClass.Summon, 80);
        protected override Color FlameColor
		{
			get
			{
				Color c = ColorHelper.InfernoColorGradient(0.25f);
				c.A = 0;
				return c;
			}
		}
    }
}