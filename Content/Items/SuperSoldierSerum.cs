using BestFriend.Common.Players;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace BestFriend.Content.Items {
  public class SuperSoldierSerum : ModItem {
    public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/SuperSoldierSerum";

    public override void SetStaticDefaults() {
      CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults() {
      Item.useStyle = ItemUseStyleID.EatFood;
      Item.consumable = true;
      Item.potion = true;
      Item.useTurn = true;
      Item.maxStack = 1;
      Item.width = 34;
      Item.height = 38;
      Item.value = 75000;
      Item.rare = ItemRarityID.Purple;
    }

    public override bool CanUseItem(Player player) {
      // Any mod that changes statLifeMax to be greater than 500 is broken and needs to fix their code.
      // This check also prevents this item from being used before vanilla health upgrades are maxed out.
      return !player.GetModPlayer<BestFriendAbilitiesPlayer>().has_serum;
    }

    public override bool? UseItem(Player player) {
      if (player.GetModPlayer<BestFriendAbilitiesPlayer>().has_serum) return null;

      if (Main.myPlayer == player.whoAmI) {
        player.UseHealthMaxIncreasingItem(100);

        var mod_player = player.GetModPlayer<BestFriendAbilitiesPlayer>();
        mod_player.has_serum = true;
        return true;
      }

      return null;
    }

    public override void AddRecipes() {
      var recipe = CreateRecipe();
      recipe.AddIngredient(ItemID.Vitamins);
      recipe.AddIngredient(ItemID.Bezoar);
      recipe.AddIngredient(ItemID.AvengerEmblem);
      recipe.AddIngredient(ItemID.LifeFruit);
      recipe.AddIngredient(ItemID.SoulofMight, 5);
      recipe.AddIngredient(ItemID.SoulofSight, 5);
      recipe.AddTile(TileID.Bottles);
      recipe.Register();

      recipe = CreateRecipe();
      recipe.AddIngredient(ItemID.Vitamins);
      recipe.AddIngredient(ItemID.Bezoar);
      recipe.AddIngredient(ItemID.AvengerEmblem);
      recipe.AddIngredient(ItemID.LifeFruit);
      recipe.AddIngredient(ItemID.SoulofMight, 5);
      recipe.AddIngredient(ItemID.SoulofSight, 5);
      recipe.AddTile(TileID.Bottles);
      recipe.Register();
    }
  }
}