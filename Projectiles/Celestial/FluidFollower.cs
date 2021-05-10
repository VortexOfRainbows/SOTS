using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Graphics.Shaders;

namespace SOTS.Projectiles.Celestial
{    
    public class FluidFollower : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("my curse behind me");
		}
        public override void SetDefaults()
        {
			projectile.penetrate = -1;
			projectile.width = 34;
			projectile.height = 40;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.alpha = 0;
			projectile.timeLeft = 20;
			projectile.friendly = false;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			/*
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);

			Rectangle tempBodyFrame = player.bodyFrame;
			player.bodyFrame = modPlayer.storedFramesBody[0];

			int tempWingFrame = player.wingFrame;
			player.wingFrame = modPlayer.storedFramesWings[0];

			Rectangle tempLegsFrame = player.legFrame;
			player.legFrame = modPlayer.storedFramesLegs[0];

			int tempDirection = player.direction;
			player.direction = modPlayer.storedDirection[0];

			int tempUseAnimation = player.itemAnimation;
			player.itemAnimation = 0;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			int shader = GameShaders.Armor.GetShaderIdFromItemId(ItemID.BlackDye);
			GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
			Filters.Scene.Activate("BloodMoon", projectile.Center).GetShader().UseColor(0, 0, 0);
			Main.instance.DrawPlayer(player, projectile.Center - new Vector2(player.width / 2, player.height / 2), player.fullRotation, new Vector2(player.width/2, player.height/2), -1f);
			Filters.Scene.Deactivate("BloodMoon");

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

			player.bodyFrame = tempBodyFrame;
			player.wingFrame = tempWingFrame;
			player.legFrame = tempLegsFrame;
			player.direction = tempDirection;
			player.itemAnimation = tempUseAnimation;

			return false;
			*/
			return true;
        }
        public override void AI()
        {
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if(modPlayer.FluidCurse && player.active)
            {
				projectile.timeLeft = 60;
				projectile.Center = modPlayer.storedPositions[0];
            }
			projectile.alpha = 255 - (int)(projectile.timeLeft * 255f / 60f);
        }
    }
}
		
			