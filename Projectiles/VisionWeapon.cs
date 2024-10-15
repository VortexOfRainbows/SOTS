using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items;
using SOTS.Items.Conduit;
using SOTS.Items.Earth;
using SOTS.Items.Evil;
using SOTS.Items.Secrets;
using SOTS.Items.Temple;
using SOTS.NPCs.Boss.Curse;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Projectiles
{
	public class VisionWeapon : ModProjectile
	{
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vision Weapon");
		}
		public sealed override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 300;
			Projectile.netImportant = true;
		}
		public List<CurseFoam> particleList = new List<CurseFoam>();
		public void ResetFoamLists()
		{
			List<CurseFoam> temp = new List<CurseFoam>();
			for (int i = 0; i < particleList.Count; i++)
			{
				if (particleList[i].active && particleList[i] != null)
					temp.Add(particleList[i]);
			}
			particleList = new List<CurseFoam>();
			for (int i = 0; i < temp.Count; i++)
			{
				particleList.Add(temp[i]);
			}
		}
		public void catalogueParticles()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				CurseFoam particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particle = null;
					particleList.RemoveAt(i);
					i--;
				}
				else
				{
					particle.Update();
					if (!particle.active)
					{
						particle = null;
						particleList.RemoveAt(i);
						i--;
					}
				}
			}
		}
        public override void PostDraw(Color lightColor)
		{
			if(itemType > 0)
			{
				Player player = Main.player[Projectile.owner];
				Item item = player.HeldItem;
				Color color = item.GetAlpha(lightColor);
				Texture2D texture = Terraria.GameContent.TextureAssets.Item[itemType].Value;
				Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / frameCount / 2);
				Rectangle rectangleFrame = new Rectangle(0, texture.Height / frameCount * frame, texture.Width, texture.Height / frameCount);
				float scale = Projectile.scale;
				if(item.type == ModContent.ItemType<DreamLamp>())
                {
					scale = item.scale;
                }
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				SOTS.VisionShader.Parameters["progress"].SetValue(itemAlpha);
				SOTS.VisionShader.Parameters["lightColor"].SetValue(color.ToVector4());
				SOTS.VisionShader.Parameters["colorMod"].SetValue(GetColor().ToVector4());
				SOTS.VisionShader.Parameters["uImageSize0"].SetValue(new Vector2(texture.Width, texture.Height));
				SOTS.VisionShader.Parameters["uSourceRect"].SetValue(new Vector4(0, texture.Height / frameCount * frame, texture.Width, texture.Height / frameCount));
				SOTS.VisionShader.CurrentTechnique.Passes[0].Apply();
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, rectangleFrame, color, Projectile.rotation, drawOrigin, scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			}
			GetColor();
		}
		public void SpawnDust(Texture2D texture, int rate, float percent, float rotation)
		{
			int width = texture.Width;
			int height = texture.Height / frameCount;
			Color[] data = new Color[texture.Width * texture.Height];
			int startAt = width * height * frame;
			texture.GetData(data);
			Vector2 position;
			int localX = 0;
			int localY = 0;
			for (int i = startAt; i < startAt + width * height; i++)
			{
				localX++;
				if (localX > width)
				{
					localX -= width;
					localY++;
				}
				float percentX = localX / (float)width;
				float percentY = localY / (float)height;
				float length = new Vector2(percentX - 0.5f, percentY - 0.5f).Length();
				if (rate <= 1)
					rate = 1;
				if (data[i].A >= 255 && Main.rand.NextBool(rate) && Math.Abs(length - percent) <= 0.03f)
				{
					Vector2 offset = -new Vector2(width / 2, height / 2) + new Vector2(localX, localY);
					offset.X *= Projectile.spriteDirection;
					position = offset.RotatedBy(rotation);
					Vector2 velocity = offset.SafeNormalize(Vector2.Zero).RotatedBy(rotation) * 0.4f;
					particleList.Add(new CurseFoam(position, velocity, 1.7f, true));
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Dusts/CopyDust4");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 6);
			Color color2 = GetColor();
			color2.A = 0;
			for (int i = 0; i < particleList.Count; i++)
			{
				Vector2 drawPos = Projectile.Center + particleList[i].position - Main.screenPosition;
				Color color = Projectile.GetAlpha(color2) * (0.35f + 0.65f * particleList[i].scale);
				Main.spriteBatch.Draw(texture, drawPos, new Rectangle(0, 0, texture.Width, texture.Height / 3), color, particleList[i].rotation, drawOrigin, particleList[i].scale * 0.9f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public Color GetColor()
		{
			Player player = Main.player[Projectile.owner];
			return SOTSPlayer.VisionColor(player);
        }
		private int lastItem = -1;
		private float itemAlpha = 0;
		private int itemType = -1;
		private int ticksPerFrame = 0;
		private int frameCounter = 0;
		private int frameCount = 1;
		private int frame = 0;
		public void FindPosition()
		{
			Player player = Main.player[Projectile.owner];
			if (Main.myPlayer != Projectile.owner)
				Projectile.timeLeft = 20;
			Vector2 idlePosition = player.Center;
			idlePosition.X -= player.direction * 16f;
			Projectile.spriteDirection = player.direction;
			Projectile.ai[0]++;
			float sin = (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[0] * 1.75f)) * 4;
			idlePosition.Y += sin - 4;
			Projectile.Center = idlePosition;
			Projectile.rotation = (Projectile.oldPosition.X - Projectile.position.X) * -0.05f;
		}
		public static bool VisualActive(Item item)
		{
			return !item.IsAir && item.active && item.createTile == -1 && item.createWall == -1 && item.useStyle != ItemUseStyleID.None;
        }
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Item item = player.HeldItem;
			FindPosition();
			catalogueParticles();
			if (VisualActive(item))
			{
				DrawAnimation anim = Main.itemAnimations[item.type];
				if(anim != null)
                {
					frameCount = anim.FrameCount;
					ticksPerFrame = anim.TicksPerFrame;
                }
				else
				{
					frameCount = 1;
				}
				int type = item.type;
				bool isBlade = type == ModContent.ItemType<DigitalDaito>() || type == ModContent.ItemType<Pyrocide>() || type == ModContent.ItemType<ToothAche>() || type == ModContent.ItemType<Vertebraeker>() || type == ModContent.ItemType<Items.Invidia.VesperaNanDao>() || type == ItemID.Arkhalis
					|| type == ItemID.Terragrim || type == ModContent.ItemType<SkipScythe>() || type == ModContent.ItemType<Colossus>();
                if (item.useStyle == ItemUseStyleID.Swing || Item.staff[item.type] || isBlade)
                {
					Projectile.rotation += MathHelper.ToRadians(150) * Projectile.spriteDirection;
                }
				else if(item.useStyle == ItemUseStyleID.Shoot)
                {
					if(item.height > item.width) //bow type?
					{
						Projectile.spriteDirection *= -1;
						Projectile.rotation += MathHelper.ToRadians(15) * Projectile.spriteDirection;
					}
					else if(item.width >= item.height) //gun type?
					{
						Projectile.rotation += MathHelper.ToRadians(105) * Projectile.spriteDirection;
					}
                }
				itemType = item.type;
			}
			else
			{
				frameCount = 1;
				itemType = -1;
			}
			if (lastItem != itemType)
			{
				ResetFoamLists();
				lastItem = itemType;
				itemAlpha = 0;
			}
			if (itemType >= 0)
			{
				if (frameCount > 1)
				{
					frameCounter++;
					if (frameCounter > ticksPerFrame)
					{
						frame = (frame + 1) % frameCount;
						frameCounter -= ticksPerFrame;
					}
				}
				else
				{
					ticksPerFrame = 0;
					frame = 0;
					frameCounter = 0;
				}
				if (player.itemAnimation > 0)
				{
					itemAlpha = -0.24f - (!item.autoReuse ? 0.24f : 0);
				}
				else if (itemAlpha < 2)
				{
					if (Main.netMode != NetmodeID.Server)
					{
						SpawnDust(Terraria.GameContent.TextureAssets.Item[itemType].Value, 7, itemAlpha, Projectile.rotation);
					}
					itemAlpha += 0.03f;
				}
				if (itemAlpha > 2)
					itemAlpha = 2;
			}
			Lighting.AddLight(Projectile.Center, GetColor().ToVector3() * 0.3f);
		}
	}
}