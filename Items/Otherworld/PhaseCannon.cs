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
            item.damage = 27; 
            item.ranged = true;  
            item.width = 52;   
            item.height = 26; 
            item.useTime = 90; 
            item.useAnimation = 90;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 3, 25, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item92;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("FriendlyOtherworldlyBall");
			item.shootSpeed = 10; //not important
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Otherworld/PhaseCannonGlow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -2;
				item.GetGlobalItem<ItemUseGlow>().glowOffsetY = 1;
			}

		}
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-2, 1);
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/PhaseCannonGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
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
					index = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<OtherworldlyTracer>(), item.damage, item.knockBack, player.whoAmI, 1000, -1);
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
