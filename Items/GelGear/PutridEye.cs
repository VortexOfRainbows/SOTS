using Microsoft.Xna.Framework;
using Terraria;
using SOTS.Void;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.GelGear
{
	public class PutridEye : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Eye");
			Tooltip.SetDefault("'You want deathray? We have deathray'");
		}
		Vector2 toPos = new Vector2(7.5f, 0);
		Vector2 CurrentPos = new Vector2(0, 0);
		int waitTime = 0;
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/GelGear/PutridEyeEmpty");
			Vector2 origin2 = new Vector2(5, 5);
			if (CurrentPos.X != toPos.X || CurrentPos.Y != toPos.Y)
			{
				Vector2 to = toPos - CurrentPos;
				to.Normalize();
				to *= 0.15f;
				CurrentPos += to;
				if ((toPos - CurrentPos).Length() < 0.2f)
				{
					CurrentPos = new Vector2(toPos.X, toPos.Y);
				}
			}
			else if (waitTime < 0)
			{
				waitTime = 30;
				toPos = toPos.RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
			}
			else
				waitTime--;
			Texture2D texture2 = mod.GetTexture("Items/GelGear/PutridPupil");
			spriteBatch.Draw(texture, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture2, position + new Vector2(25 * scale, 17 * scale) + CurrentPos * scale, null, drawColor, 0, origin2, scale, SpriteEffects.None, 1f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/GelGear/PutridEyeEmpty");
			Vector2 origin = new Vector2(23, 23);
			Vector2 origin2 = new Vector2(5, 5);
			if (CurrentPos.X != toPos.X || CurrentPos.Y != toPos.Y)
			{
				Vector2 to = toPos - CurrentPos;
				to.Normalize();
				to *= 0.15f;
				CurrentPos += to;
				if ((toPos - CurrentPos).Length() < 0.2f)
				{
					CurrentPos = new Vector2(toPos.X, toPos.Y);
				}
			}
			else if (waitTime < 0)
			{
				waitTime = 30;
				toPos = toPos.RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
			}
			else
				waitTime--;
			Texture2D texture2 = mod.GetTexture("Items/GelGear/PutridPupil");
			spriteBatch.Draw(texture, item.Center - Main.screenPosition, null, lightColor, 0, origin, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture2, item.position + new Vector2(25 * scale, 17 * scale) - Main.screenPosition + CurrentPos * scale, null, lightColor, 0, origin2, scale, SpriteEffects.None, 1f);
			return false;
		}
		public override void SafeSetDefaults()
		{
            item.damage = 30;  
            item.magic = true;  
            item.width = 46;    
            item.height = 46;   
            item.useTime = 40;
			item.useAnimation = 40;
            item.useStyle = 5;    
            item.knockBack = 2.25f;
            item.value = Item.sellPrice(0, 1, 80, 0);
            item.rare = 4;
			//item.UseSound = SoundID.Item15;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("PutridEye"); 
            item.shootSpeed = 1;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.channel = true;
			item.expert = true;
		}
		public override bool BeforeDrainMana(Player player)
		{
			return false;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 20;
		}
		public override float UseTimeMultiplier(Player player)
		{
			return 1f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			bool summon = true;
			for (int l = 0; l < Main.projectile.Length; l++)
			{
				Projectile proj = Main.projectile[l];
				if (proj.active && proj.type == item.shoot && Main.player[proj.owner] == player)
				{
					summon = false;
				}
			}
			if (player.altFunctionUse != 2)
			{
				//item.UseSound = SoundID.Item22;
				if (summon)
				{
					//Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
					return true;
				}
			}
			return false;
		}
	}
}
