using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace SOTS.Projectiles.Base
{
	public class BlastDoorProj : ModProjectile
	{
		public override void SetDefaults() //Do you enjoy how all my net sycning is done via projectiles?
		{
			Projectile.alpha = 255;
			Projectile.timeLeft = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.netImportant = true;
			Projectile.width = 26;
			Projectile.height = 36;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		public override void AI()
		{
			Projectile.alpha = 255;
			Projectile.Kill();
		}
		public override void Kill(int timeLeft)
		{
			if(Projectile.ai[0] == 1)
				Terraria.Audio.SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, SoundLoader.GetSoundSlot(Mod, "Sounds/Items/BlastDoorClose"), 1f, 0.0f);
			else
				Terraria.Audio.SoundEngine.PlaySound(SoundLoader.CustomSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, SoundLoader.GetSoundSlot(Mod, "Sounds/Items/BlastDoorOpen"), 1f, 0.0f);
		}
	}
}
		
			