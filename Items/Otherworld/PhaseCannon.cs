using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Otherworld;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	public class PhaseCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Cannon");
			Tooltip.SetDefault("Fires a supercharged ball of plasma that can travel through walls");
		}
		public override void SetDefaults()
		{
            Item.damage = 27; 
            Item.ranged = true;  
            Item.width = 52;   
            Item.height = 26; 
            Item.useTime = 90; 
            Item.useAnimation = 90;
            Item.useStyle = 5;    
            Item.noMelee = true;
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 3, 25, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item92;
            Item.autoReuse = true;
            Item.shoot = mod.ProjectileType("FriendlyOtherworldlyBall");
			Item.shootSpeed = 10; //not important
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Otherworld/PhaseCannonGlow").Value;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -2;
				Item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 1;
			}

		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 1);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/PhaseCannonGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[Item.type].Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void HoldItem(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			ref int index = ref modPlayer.phaseCannonIndex;
			if (index >= 0)
			{
				Projectile proj = Main.projectile[index];
				if(proj.alpha > 0)
					proj.alpha -= 6;
			}
			if (index == -1)
			{
				Vector2 mouse = Main.MouseWorld;
				if (player.whoAmI == Main.myPlayer)
				{
					index = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<OtherworldlyTracer>(), Item.damage, Item.knockBack, player.whoAmI, 1000, -1);
				}
			}
			else if (index < -1)
			{
				index++;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			ref int index = ref modPlayer.phaseCannonIndex;
			if (index < 0)
			{
				return false;
			}
			Projectile proj = Main.projectile[index];
			proj.ai[1] = -3;
			index = -31;
			Vector2 mouse = Main.MouseWorld;
			Vector2 distTo = mouse - position;
			distTo /= 30f;
			Projectile.NewProjectile(position.X, position.Y, distTo.X, distTo.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}
