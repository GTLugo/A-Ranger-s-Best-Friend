using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace BestFriend.Content;

public class BestFriendRecipes : ModSystem {
  private static RecipeGroup DemoniteBarRecipeGroup { get; set; }
  public static int DemoniteBarRecipeGroupId { get; private set; }

  public override void Unload() {
    DemoniteBarRecipeGroup = null;
  }

  public override void AddRecipeGroups() {
    DemoniteBarRecipeGroup = new RecipeGroup(
      () =>
        $"{Language.GetTextValue("LegacyMisc.37")} {Lang.GetItemNameValue(ItemID.DemoniteBar)}",
      ItemID.DemoniteBar,
      ItemID.CrimtaneBar
    );
    DemoniteBarRecipeGroupId = RecipeGroup.RegisterGroup(nameof(ItemID.DemoniteBar), DemoniteBarRecipeGroup);
  }
}