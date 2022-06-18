namespace Greed_V2.Game.Casting{
   class Gem : Actor
   {
      // summary: sets the values of the rock
      public Gem()
      {
         SetText("*");
         SetColor(GenerateColor());
      }

   }
}