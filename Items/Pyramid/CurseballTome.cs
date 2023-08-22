using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Pyramid;
using Terraria.DataStructures;

namespace SOTS.Items.Pyramid
{
	public class CurseballTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 20; 
            Item.DamageType = DamageClass.Magic; 
            Item.width = 32;   
            Item.height = 34;   
            Item.useTime = 6;   
            Item.useAnimation = 28;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;  
            Item.knockBack = 2.5f;
            Item.value = Item.sellPrice(0, 2, 25, 0);
            Item.rare = ItemRarityID.Pink;
            Item.UseSound = SoundID.Item8;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<GasBlast>(); 
            Item.shootSpeed = 7f;
			Item.mana = 17;
			Item.reuseDelay = 22;
		}
		int counter = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			counter++;
			int modN = counter % 5 - 2;
			float scaleOff = 0.25f * modN;
			Projectile.NewProjectile(source, position.X, position.Y, velocity.X + Main.rand.NextFloat(-1f, 1f), velocity.Y + Main.rand.NextFloat(-1f, 1f), type, damage, knockback, player.whoAmI, 0, scaleOff + Main.rand.NextFloat(-0.15f, 0.15f));
			return false;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<CursedMatter>(6).AddIngredient<RoyalRubyShard>(10).AddTile(TileID.Anvils).Register();
		}
		/*
		int index1 = -1;
		int index2 = -1;
		int index3 = -1;
		int index4 = -1;
		int rot = 0;
		bool inInventory = false;
		public override bool CanUseItem(Player player)
		{
			if(inInventory)
				return true;
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			inInventory = true;
			rot++;
			if(index1 != -1)
			{
				Projectile proj = Main.projectile[index1];
				if(proj.type == Item.shoot)
				{
					Vector2 rotatePos = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(rot));
					proj.position.X = rotatePos.X + player.Center.X - proj.width/2;
					proj.position.Y = rotatePos.Y + player.Center.Y - proj.height/2;
				}
			}
			if(index2 != -1)
			{
				Projectile proj = Main.projectile[index2];
				if(proj.type == Item.shoot)
				{
					Vector2 rotatePos = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(rot + 90));
					proj.position.X = rotatePos.X + player.Center.X - proj.width/2;
					proj.position.Y = rotatePos.Y + player.Center.Y - proj.height/2;
				}
			}
			if(index3 != -1)
			{
				Projectile proj = Main.projectile[index3];
				if(proj.type == Item.shoot)
				{
					Vector2 rotatePos = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(rot + 180));
					proj.position.X = rotatePos.X + player.Center.X - proj.width/2;
					proj.position.Y = rotatePos.Y + player.Center.Y - proj.height/2;
				}
			}
			if(index4 != -1)
			{
				Projectile proj = Main.projectile[index4];
				if(proj.type == Item.shoot)
				{
					Vector2 rotatePos = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(rot + 270));
					proj.position.X = rotatePos.X + player.Center.X - proj.width/2;
					proj.position.Y = rotatePos.Y + player.Center.Y - proj.height/2;
				}
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			inInventory = false;
			if(index1 != -1 && Main.projectile[index1].type == type && Main.projectile[index1].friendly && Main.projectile[index1].active)
				Main.projectile[index1].velocity = new Vector2(speedX, speedY);
			
				index1 = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
						
			if(index2 != -1 && Main.projectile[index2].type == type && Main.projectile[index2].friendly && Main.projectile[index2].active)
				Main.projectile[index2].velocity = new Vector2(speedX, speedY);
			
				index2 = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
			
			if(index3 != -1 && Main.projectile[index3].type == type && Main.projectile[index3].friendly && Main.projectile[index3].active)
				Main.projectile[index3].velocity = new Vector2(speedX, speedY);
			
				index3 = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
						
			if(index4 != -1 && Main.projectile[index4].type == type && Main.projectile[index4].friendly && Main.projectile[index4].active)
				Main.projectile[index4].velocity = new Vector2(speedX, speedY);
			
				index4 = Projectile.NewProjectile(position.X, position.Y, 0, 0, type, damage, knockBack, player.whoAmI);
			return false; 
		}
		*/
	}
}
