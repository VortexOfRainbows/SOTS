using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using Terraria.DataStructures;

namespace SOTS.Items.Earth
{
    public class SporeSprayer : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.damage = 11;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 30;
            Item.height = 60;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 0.5f;
            Item.value = Item.sellPrice(0, 0, 40, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item5;
            Item.autoReuse = true;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 8f;
            Item.useAmmo = AmmoID.Arrow; 
        }
        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            velocity.X += Main.rand.NextFloat(-1f, 1f); //Using the Terraria Main.rand library to generate a random decimal number from -1 to 1 (inclusive)
            velocity.Y += Main.rand.NextFloat(-1f, 1f); //These two methods serve to somewhat randomize the trajectory of the arrow projectile while keeping it mostly straight
            bool field = false;
            for (int j = -1; j <= 1; j += 2) //This runs the following for loop twice. Once for j = -1, and once for j = 1
            {
                for (int i = 0; i < 3; i++) //This repeats the following thrice, changing the way an angle is modified each time
                {
                    if (!Main.rand.NextBool(3)) //This functions provides a roughly 2/3 computer-generated chance for the following to activate. This is because it inverses a 1/X chance function.
                    {
                        Vector2 burstDirection = velocity.RotatedBy(MathHelper.ToRadians((7f + 35 * i) * j)); //Manipulate the velocity to angle the direction of the spore clouds
                        Projectile.NewProjectile(source, position, burstDirection, ModContent.ProjectileType<SporeCloudFriendly>(), (int)(damage * 0.66f), knockback, player.whoAmI);
                        //The above function runs the necessary code to generate the projectile in game. I did not make this.
                        field = true; //true if the item decides to generate spore clouds
                    }
                }
            }
            if(field) //play spore-cloud sound (which would only not happen if the extremely low odds of (1/3)^6 are met)
                SOTSUtils.PlaySound(SoundID.Item34, (int)position.X, (int)position.Y, 0.7f, -0.1f); //Helper method I made that calls related sound-producing functions
            return true; 
        }
    }
}
