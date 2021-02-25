using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SOTS.Items.IceStuff
{
	public class ShatterBlade : ModItem
	{
		int counterResetter = 0;
		int counter = 0;
		int broken = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shatter Blade");
			Tooltip.SetDefault("Shatters on critical strike, surrounding you with 3 ice shards\nRepairs itself after shattering\n'Don't worry, it's not a durability system'");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/IceStuff/ShatterBlade");
			Texture2D texture2 = mod.GetTexture("Items/IceStuff/ShatterFix");
			if(broken == 0)
				spriteBatch.Draw(texture, position, frame, drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			if(broken == 1)
			{
				spriteBatch.Draw(texture2, position, new Rectangle(0, 46 * counter + 1, 42, 45), drawColor, 0, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/IceStuff/ShatterBlade");
			Texture2D texture2 = mod.GetTexture("Items/IceStuff/ShatterFix");
			if (broken == 0)
				spriteBatch.Draw(texture, item.Center - Main.screenPosition, new Rectangle(0, 0, 42, 42), lightColor, 0, new Vector2(17,17), scale, SpriteEffects.None, 0f);
			if (broken == 1)
			{
				spriteBatch.Draw(texture2, item.Center - Main.screenPosition, new Rectangle(0, 46 * counter + 1, 42, 45), lightColor, 0, new Vector2(17, 17), scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetDefaults()
		{
			item.damage = 15;
			item.melee = true;
			item.width = 42;
			item.height = 42;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.knockBack = 3f;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = 2;
			item.UseSound = null;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ShatterFix");
			item.shootSpeed = 1;
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (broken == 1 || modPlayer.brokenFrigidSword >= 1)
			{
				item.useStyle = 4;
				item.noMelee = true;
				item.noUseGraphic = true;
			}
			else
			{
				item.useStyle = 1;
				item.noMelee = false;
				item.noUseGraphic = false;
			}
		}
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{ 
			counterResetter++;
			if (counterResetter > 180 && counter > 0)
			{
				counter--;
				counterResetter = 0;
			}
			if (broken == 1)
			{
				item.useStyle = 4;
				item.noMelee = true;
				item.noUseGraphic = true;
			}
			else
			{
				item.useStyle = 1;
				item.noMelee = false;
				item.noUseGraphic = false;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (broken == 1)
			{
				position += new Vector2(10 * player.direction, -10);
				speedX = 0;
				speedY = 0;
				counterResetter = 0;
				counter++;
				if (counter >= 10)
				{
					broken = 0;
					counter = 0;
				}
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, counter);
				return false;
			}
			Main.PlaySound(2, (int)(position.X), (int)(position.Y), 1, 1f);
			return false;
		}
		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (crit && player.whoAmI == Main.myPlayer)
			{
				for(int i = 0; i < 4; i++)
				{
					Vector2 circularSpeed = new Vector2(0, 12).RotatedBy(MathHelper.ToRadians(i * 90));
					int calc = damage + modPlayer.bonusShardDamage;		
					if (calc <= 0) calc = 1;
					Projectile.NewProjectile(player.Center.X, player.Center.Y, circularSpeed.X, circularSpeed.Y, mod.ProjectileType("ShatterShard"), calc, 3f, player.whoAmI);
				}
				broken = 1;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FrigidBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}