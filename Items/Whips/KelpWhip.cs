using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.WhipBuffs;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Whips
{
	public class KelpWhip : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
			// This method quickly sets the whip's properties.
			// Mouse over to see its parameters.
			Item.DefaultToWhip(ModContent.ProjectileType<KelpWhipProjectile>(), 13, 4, 4, 30);
			Item.shootSpeed = 4;
			Item.rare = ItemRarityID.Blue;
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
			Item.value = Item.sellPrice(0, 1, 0, 0);
		}
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float sizeDifference = Main.rand.NextFloat(0.85f, 1.15f);
			float randMult = Main.rand.NextFloat(0.25f, 1.5f);
			Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(-15 * randMult)), type, damage, knockback, player.whoAmI, ai1: 1 - sizeDifference);
			randMult = Main.rand.NextFloat(0.25f, 1.5f);
			Projectile.NewProjectile(source, position, velocity.RotatedBy(MathHelper.ToRadians(15 * randMult)), type, damage, knockback, player.whoAmI, ai1: sizeDifference - 1);
			return false;
        }
        public override bool MeleePrefix()
		{
			return true;
		}
	}
	public class KelpWhipProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.IsAWhip[Type] = true;
		}
		public override void SetDefaults()
		{
			Projectile.DefaultToWhip();
			Projectile.WhipSettings.Segments = 16;
			Projectile.WhipSettings.RangeMultiplier = 1.1f + Projectile.ai[1];
		}
        public override bool PreAI()
		{
			Projectile.WhipSettings.RangeMultiplier = 1.1f + Projectile.ai[1];
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);
			if(Timer > 18)
				for (int i = 0; i < list.Count - 1; i++)
				{
					if(Main.rand.NextBool((int)(70 - i * 2.4f)))
					{
						Vector2 pos = list[i];
						Dust dust = Dust.NewDustDirect(pos - new Vector2(6, 6), 4, 4, DustID.Grass);
						dust.velocity *= 0.5f;
						dust.noGravity = true;
						dust.scale *= 0.3f;
						dust.scale += 1.2f;
						dust.color = new Color(83, 113, 14) * 1.5f;
					}
				}
			return base.PreAI();
        }
        private float Timer
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<KelpWhipBuff>(), 240);
			Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
		}

		// This method draws a line between all points of the whip, in case there's empty space between the sprites.
		private void DrawLine(List<Vector2> list)
		{
			Texture2D texture = TextureAssets.FishingLine.Value;
			Rectangle frame = texture.Frame();
			Vector2 origin = new Vector2(frame.Width / 2, 2);

			Vector2 pos = list[0];
			for (int i = 0; i < list.Count - 1; i++)
			{
				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2;
				Color color = Lighting.GetColor(element.ToTileCoordinates(), new Color(83, 113, 14));
				Vector2 scale = new Vector2(1, (diff.Length() + 2) / frame.Height);

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

				pos += diff;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			List<Vector2> list = new List<Vector2>();
			Projectile.FillWhipControlPoints(Projectile, list);

			DrawLine(list);

			//Main.DrawWhip_WhipBland(Projectile, list);
			// The code below is for custom drawing.
			// If you don't want that, you can remove it all and instead call one of vanilla's DrawWhip methods, like above.
			// However, you must adhere to how they draw if you do.

			SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			Main.instance.LoadProjectile(Type);
			Texture2D texture = TextureAssets.Projectile[Type].Value;

			Vector2 pos = list[0];

			for (int i = 0; i < list.Count - 1; i++)
			{
				// These two values are set to suit this projectile's sprite, but won't necessarily work for your own.
				// You can change them if they don't!
				Rectangle frame = new Rectangle(0, 0, 18, 18);
				Vector2 origin = new Vector2(10, 8);
				float scale = 1.1f;

				// These statements determine what part of the spritesheet to draw for the current segment.
				// They can also be changed to suit your sprite.
				if (i == list.Count - 2)
				{
					scale = 0.9f;
					frame.Y = 48;
					frame.Height = 26;

					// For a more impactful look, this scales the tip of the whip up when fully extended, and down when curled up.
					Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
					float t = Timer / timeToFlyOut;
					scale *= MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
				}
				else if (i > 0)
				{
					scale = 0.7f;
					frame.Y = 20;
					frame.Height = 26;
				}

				Vector2 element = list[i];
				Vector2 diff = list[i + 1] - element;

				float rotation = diff.ToRotation() - MathHelper.PiOver2; // This projectile's sprite faces down, so PiOver2 is used to correct rotation.
				Color color = Lighting.GetColor(element.ToTileCoordinates());

				Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

				pos += diff;
			}
			return false;
		}
	}
}