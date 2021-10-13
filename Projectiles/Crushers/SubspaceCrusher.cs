using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Projectiles.Celestial;

namespace SOTS.Projectiles.Crushers
{    
    public class SubspaceCrusher : CrusherProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Arm");
		}
        public override void SafeSetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			maxDamage = 0f;
			chargeTime = 60;
			minExplosions = 1;
			maxExplosions = 1;
			explosiveRange = 48;
			releaseTime = 10;
			accSpeed = 0.5f;
			initialExplosiveRange = 48;
			exponentReduction = 0.75f;
			minDamage = 1f;
			minTimeBeforeRelease = 10;
		}
        public override bool UseCustomExplosionEffect(float x, float y, float dist, float rotation, float chargePercent, int index)
        {
			int first = 0;
			for(int i = 1; i < storage.Count; i++)
			{
				Vector2 location = storage[i];
				int whoAmI = Projectile.NewProjectile(location, Vector2.Zero, ModContent.ProjectileType<PlasmaCrush>(), projectile.damage, projectile.knockBack, projectile.owner, first);
				if(i % 5 == 1)
				{
					Vector2 circular = new Vector2(Main.rand.NextFloat(3f, 7f), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					Projectile.NewProjectile(location, circular, ModContent.ProjectileType<PurgatoryGhost>(), projectile.damage, projectile.knockBack, projectile.owner);
				}
				first = whoAmI;
			}
			return true;
        }
        public override void ExplosionSound()
		{
			Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, 39, 1.25f, -0.5f);
		}
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(current.X);
			writer.Write(current.Y);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			current.X = reader.ReadSingle();
			current.Y = reader.ReadSingle();
        }
        bool runOnce = true;
		Vector2 current;
		List<Vector2> storage = new List<Vector2> { Vector2.Zero };
        public override bool CanCharge()
        {
			if(runOnce)
            {
				if(Main.myPlayer == projectile.owner)
				{
					current = Main.MouseWorld;
					projectile.netUpdate = true;
					storage.Add(current);
				}
				runOnce = false;
				return true;
			}
			if (Main.myPlayer == projectile.owner)
			{
				current = Main.MouseWorld;
				projectile.netUpdate = true;
			}
			Vector2 prev = storage[storage.Count - 1];
			if(current != null && Vector2.Distance(current, prev) > 24f)
            {
				storage.Add(current);
				return true;
            }
            return storage.Count > 60;
        }
        public override int ExplosionType()
        {
            return ModContent.ProjectileType<Webbing>();
        }
        public override Texture2D ArmTexture(int handNum, int direction)
        {
            return mod.GetTexture("Projectiles/Crushers/SubspaceClaw");
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce || storage.Count <= 1)
				return;
			Texture2D texture = mod.GetTexture("Projectiles/Crushers/SubspaceLine");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = storage[1];
			for (int k = 1; k < storage.Count; k++)
			{
				float scale = projectile.scale;
				scale *= 1.25f;
				if (storage[k] == Vector2.Zero)
				{
					return;
				}
				Vector2 drawPos = storage[k] - Main.screenPosition;
				Vector2 currentPos = storage[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / ((texture.Width + 8) * scale);
				for (int i = 0; i < max; i++)
				{
					Color color = Color.White;
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					Main.spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
				}
				previousPosition = currentPos;
			}
        }
    }
}
			